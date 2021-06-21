using Bug_Tracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace Bug_Tracker.BL
{
    public class GmailService
    {
        public GmailService()
        {
            SetUp();
        }

        private const string SENDER_EMAIL = "bug.tracker.for.you@gmail.com";
        private const string SENDER_EMAIL_PASSWORD = "finalProjectMITT2021";
        private const string SENDER_NAME = "noreply@bugtracker";

        private SmtpClient Client;
        private MailAddress FromEmail;

        private void SetUp()
        {
            this.Client = new SmtpClient()
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential()
                {
                    UserName = SENDER_EMAIL,
                    Password = SENDER_EMAIL_PASSWORD
                }
            };
            this.FromEmail = new MailAddress(SENDER_EMAIL, SENDER_NAME);
        }

        public void Send(string address, string name, string subject, string message)
        {
            MailAddress toEmail = new MailAddress(address, name);
            MailMessage mailMessage = new MailMessage()
            {
                From = this.FromEmail,
                Subject = subject,
                Body = message
            };
            mailMessage.To.Add(toEmail);

            try
            {
                this.Client.Send(mailMessage);
            }
            catch (Exception ex)
            {
                throw new Exception("Third party software - Gmail - Exception \n" + ex.Message);
            }
        }
    }
}