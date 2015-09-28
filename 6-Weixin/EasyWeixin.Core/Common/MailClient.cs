using System;
using System.Configuration;
using System.Net;
using System.Net.Mail;

namespace EasyWeixin.Core.Common
{
    public static class MailClient
    {
        private static readonly SmtpClient Client;

        static MailClient()
        {
            Client = new SmtpClient
            {
                Host = ConfigurationManager.AppSettings["SmtpServer"],
                Port = Convert.ToInt32(ConfigurationManager.AppSettings["SmtpPort"]),
                DeliveryMethod = SmtpDeliveryMethod.Network
            };
            Client.UseDefaultCredentials = false;
            Client.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["SmtpUser"], ConfigurationManager.AppSettings["SmtpPass"]);
        }

        private static bool SendMessage(string from, string to, string subject, string body)
        {
            MailMessage mm = null;
            bool isSent = false;
            try
            {
                // Create our message
                mm = new MailMessage(from, to, subject, body);
                mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

                // Send it
                Client.Send(mm);
                isSent = true;
            }
            // Catch any errors, these should be logged and dealt with later
            catch (Exception ex)
            {
                // If you wish to log email errors,
                // add it here...
                var exMsg = ex.Message;
            }
            finally
            {
                mm.Dispose();
            }

            return isSent;
        }

        public static bool SendWelcome(string email)
        {
            string body = "Put welcome email content here...";

            return SendMessage(ConfigurationManager.AppSettings["adminEmail"], email, "Welcome message", body);
        }

        public static bool SendLostPassword(string email, string password)
        {
            string body = "Your password is: " + password;

            return SendMessage(ConfigurationManager.AppSettings["adminEmail"], email, "Lost Password", body);
        }
    }
}