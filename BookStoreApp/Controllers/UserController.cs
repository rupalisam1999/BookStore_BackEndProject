using BusinessLayer.Interfaces;
using DatabaseLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        IUserBL userBL;
        public UserController(IUserBL userBL)
        {
            this.userBL = userBL;
        }
        [HttpPost("Register")]
        public IActionResult RegisterUser(UserModel user)
        {
            try
            {
                UserModel userData = this.userBL.RegisterUser(user);
                if (userData != null)
                {
                    return this.Ok(new { success = true, message = "Registration is successfull", res = userData });
                }
                return this.Ok(new { success = true, message = "User Already Exists" });
            }
            catch (Exception ex)
            {
                return this.Ok(new { success = false, message = ex.Message });
            }

        }
        [HttpPost("Login/{Email}/{Password}")]

        public IActionResult LoginUser(string Email, string Password)
        {
            try
            {
                var login = this.userBL.LoginUser(Email, Password);
                if (login != null)
                {
                    return this.Ok(new { Success = true, message = "Login Successful", token = login });
                }
                else
                {
                    return this.Ok(new { Success = false, message = "Invalid User Please enter valid email and password." });
                }
            }
            catch (Exception ex)
            {
                return this.BadRequest(new { success = false, message = ex.Message });

            }
        }
        [Authorize]
        [HttpPost("ForgotPassword/{Email}")]
        public IActionResult ForgotPassword(string Email)
        {
            try
            {
                var forgotPasswordToken = this.userBL.ForgotPassword(Email);
                if (forgotPasswordToken != null)
                    return this.Ok(new { success = true, message = $"Token generated.Please check your email" });
                else
                    return this.Ok(new { success = false, message = $"email not sent" });

            }
            catch (Exception e)
            {
                return BadRequest(new { success = false, e.Message });
            }
        }
        [Authorize]
        [HttpPost("ResetPassword/{Password}/{ConfirmPassword}")]
        public IActionResult ResetPassword(string Password,string ConfirmPassword)
        {
            try
            {
                var Email = User.FindFirst("Email").Value.ToString();
                if (this.userBL.ResetPassword(Email, Password, ConfirmPassword))
                {
                    return this.Ok(new { success = true, message = $"Password change Successfully" });
                }
                else
                {
                    return this.Ok(new { success = false, message = $"Password not set successfully" });
                }

            }
            catch (Exception e)
            {
                return BadRequest(new { success = false, e.Message });
            }
        }
    }
}
