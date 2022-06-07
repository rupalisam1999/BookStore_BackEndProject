using DatabaseLayer;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Interfaces
{
   public interface IUserBL
    {
        public UserModel RegisterUser(UserModel user);
        public LoginModel LoginUser(string Email, string Password);
        public string ForgotPassword(string Email);
        public bool ResetPassword(string Email, string Password, string ConfirmPassword);
    }
}
