using DatabaseLayer;
using Experimental.System.Messaging;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RepositoryLayer.Services
{
    public class UserRL : IUserRL
    {
        private IConfiguration Configuration { get; }
        private SqlConnection sqlConnection;
        string ConnectionString;
        public UserRL(IConfiguration configuration)
        {
            ConnectionString = configuration.GetConnectionString("BookStore_DB");
            // this.Configuration = configuration;
        }



        public UserModel RegisterUser(UserModel user)
        {
            SqlConnection connection = new SqlConnection(ConnectionString);
            try
            {
                using (connection)
                {
                    SqlCommand cmd = new SqlCommand("spUserRegistration", connection);


                    cmd.CommandType = CommandType.StoredProcedure;


                    cmd.Parameters.AddWithValue("@FullName", user.FullName);
                    cmd.Parameters.AddWithValue("@Email", user.Email);
                    cmd.Parameters.AddWithValue("@Password", user.Password);
                    cmd.Parameters.AddWithValue("@PhoneNo", user.PhoneNo);
                    connection.Open();
                    cmd.ExecuteNonQuery();

                    return null;
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }
        public LoginModel LoginUser(string Email, string Password)
        {
            SqlConnection connection = new SqlConnection(ConnectionString);
            try
            {
                using (connection)
                {
                    LoginModel user = new LoginModel();
                    SqlCommand cmd = new SqlCommand("spLoginUser", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Email", Email);
                    cmd.Parameters.AddWithValue("@Password", Password);
                    connection.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    if (rdr.HasRows)
                    {
                        int UserId = 0;
                        // UserLogin user = new UserLogin();
                        while (rdr.Read())
                        {
                            user.Email = Convert.ToString(rdr["Email"]);
                            user.Password = Convert.ToString(rdr["Password"]);
                            UserId = Convert.ToInt32(rdr["UserId"]);
                        }
                        // connection.Close();
                        user.Token = (string)this.GenerateJWTToken(Email, UserId);
                        return user;
                    }
                    else
                    {
                        connection.Close();
                        return null;
                    }


                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                connection.Close();
            }
        }

        private string GenerateJWTToken(string Email, int UserId)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes("THIS_IS_MY_KEY_TO_GENERATE_TOKEN");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("Email", Email),
                    new Claim("UserId",UserId.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(1),

                SigningCredentials =
                new SigningCredentials(
                    new SymmetricSecurityKey(tokenKey),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public string ForgotPassword(string Email)
        {
            SqlConnection connection = new SqlConnection(ConnectionString);
            try
            {
                using (connection)
                {
                    LoginModel user = new LoginModel();
                    SqlCommand cmd = new SqlCommand("spForgotPassword", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Email", Email);

                    connection.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    if (rdr.HasRows)
                    {
                        int UserId = 0;
                        // UserLogin user = new UserLogin();
                        while (rdr.Read())
                        {
                            Email = Convert.ToString(rdr["Email"]);

                            UserId = Convert.ToInt32(rdr["UserId"]);
                        }
                        // connection.Close();
                        var token = (string)this.GenerateJWTToken(Email, UserId);
                        new EmailService().Sender(token);
                        return token;
                    }

                    else
                    {
                        connection.Close();
                        return null;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
               connection.Close();
            }


        }

        public bool ResetPassword(string Email, string Password, string ConfirmPassword)
        {
            SqlConnection connection = new SqlConnection(ConnectionString);
            try
            {
                if (Password.Equals(ConfirmPassword))
                {
                    using (connection)
                    {
                        LoginModel user = new LoginModel();
                        SqlCommand cmd = new SqlCommand("spResetPassword", connection);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Email", Email);
                        cmd.Parameters.AddWithValue("@Password", ConfirmPassword);

                        connection.Open();
                        return true;
                    }
                }
                
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                connection.Close();
            }
           
        }
        //private void msmqQueue_ReceiveCompleted(object sender, ReceiveCompletedEventArgs e)
        //{
        //    try
        //    {
        //        MessageQueue queue = (MessageQueue)sender;
        //        Message msg = queue.EndReceive(e.AsyncResult);
        //        EmailService.SendMail(e.Message.ToString(), GenerateToken(e.Message.ToString()));
        //        queue.BeginReceive();
        //    }
        //    catch (MessageQueueException ex)
        //    {
        //        if (ex.MessageQueueErrorCode ==
        //            MessageQueueErrorCode.AccessDenied)
        //        {
        //            Console.WriteLine("Access is denied. " +
        //                "Queue might be a system queue.");
        //        }
        //        // Handle other sources of MessageQueueException.
        //    }

        //}
        ////GENERATE TOKEN WITH EMAIL
        //public string GenerateToken(string Email)
        //{
        //    if (Email == null)
        //    {
        //        return null;
        //    }
        //    var tokenHandler = new JwtSecurityTokenHandler();
        //    var tokenKey = Encoding.ASCII.GetBytes("THIS_IS_MY_KEY_TO_GENERATE_TOKEN");
        //    var tokenDescriptor = new SecurityTokenDescriptor
        //    {
        //        Subject = new ClaimsIdentity(new Claim[]
        //        {

        //              new Claim("Email",Email)
        //        }),
        //        Expires = DateTime.UtcNow.AddHours(1),
        //        SigningCredentials =
        //        new SigningCredentials(
        //            new SymmetricSecurityKey(tokenKey),
        //            SecurityAlgorithms.HmacSha256Signature)
        //    };
        //    var token = tokenHandler.CreateToken(tokenDescriptor);
        //    return tokenHandler.WriteToken(token);
        //}


    } 
}

