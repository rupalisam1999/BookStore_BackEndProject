using BusinessLayer.Interfaces;
using DatabaseLayer;
using RepositoryLayer.Interfaces;
using RepositoryLayer.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Services
{
    public class UserBL : IUserBL
    {
        IUserRL userRL;
        public UserBL(IUserRL userRL)
        {
            this.userRL = userRL;
        }

        public UserModel RegisterUser(UserModel user)
        {
            try
            {
                return this.userRL.RegisterUser(user);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        public LoginModel LoginUser(string Email, string Password)
        {
            try
            {
                return this.userRL.LoginUser(Email, Password);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public string ForgotPassword(string Email)
        {
            try
            {
                return this.userRL.ForgotPassword(Email);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public bool ResetPassword(string Email, string Password, string ConfirmPassword)
        {
            try
            {
                return this.userRL.ResetPassword(Email,Password,ConfirmPassword);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}
