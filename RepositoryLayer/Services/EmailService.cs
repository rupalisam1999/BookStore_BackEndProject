using Experimental.System.Messaging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace RepositoryLayer.Services
{
   public class EmailService
    {
        MessageQueue messageQueue = new MessageQueue();
        public void Sender(string token)
        {
            //path msmq server
            messageQueue.Path = @".\private$\Tokens";
            try
            {
                //checking path exist or not
                if (!MessageQueue.Exists(messageQueue.Path))
                {
                    //path is not there then create path
                    MessageQueue.Create(messageQueue.Path);
                }
                //Delegates for sending email
                messageQueue.Formatter = new XmlMessageFormatter(new Type[] { typeof(string) });
                messageQueue.ReceiveCompleted += MessageQueue_ReceiveCompleted;
                messageQueue.Send(token);
                messageQueue.BeginReceive();
                messageQueue.Close();
            }
            catch (Exception)
            {
                throw;
            }
        }
        //public static void SendMail(string Email, string token)
        //{
        //    using (SmtpClient client = new SmtpClient("smtp.gmail.com", 587))
        //    {
        //        client.EnableSsl = true;
        //        client.DeliveryMethod = SmtpDeliveryMethod.Network;
        //        client.UseDefaultCredentials = true;
        //        client.Credentials = new NetworkCredential("rupsam0607@gmail.com", "Rups123#");

        //        MailMessage msgObj = new MailMessage();
        //        msgObj.To.Add(Email);
        //        msgObj.From = new MailAddress("rupsam0607@gmail.com");
        //        msgObj.Subject = "Password Reset Link";
        //        msgObj.Body = $"http://localhost:4200/reset-password/{token}";
        //        client.Send(msgObj);


        //    }
        //}
        private void MessageQueue_ReceiveCompleted(object sender, ReceiveCompletedEventArgs e)
        {
            var message = messageQueue.EndReceive(e.AsyncResult);
            string token = message.Body.ToString();
            try
            {
                MailMessage mailMessage = new MailMessage();
                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    EnableSsl = true,
                    Credentials = new NetworkCredential("rupsam0607@gmail.com", "Rups123#")
                };
                mailMessage.From = new MailAddress("rupsam0607@gmail.com");
                mailMessage.To.Add(new MailAddress("rupsam0607@gmail.com"));
                mailMessage.Body = token;
                mailMessage.Subject = "Forget Password link";
                smtpClient.Send(mailMessage);
            }
            catch (Exception)
            {
                messageQueue.BeginReceive();
            }
        }
    }
}

