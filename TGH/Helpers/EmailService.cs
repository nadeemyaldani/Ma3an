using System;
using System.Net;
using System.Net.Mail;

namespace TGH.Helpers
{
    public static class EmailService
    {
        private static readonly string _HOST = "smtp.gmail.com";
        private static readonly int _PORT = 587;
        private static readonly NetworkCredential _networkCredential = new NetworkCredential("contactcenter@together.support", "Support@11111");
        private static readonly MailAddress _mailAddress = new MailAddress("contactcenter@together.support", "MA3AN معا");
        public static void SendEmail(Email email)
        {
            try
            {
                MailMessage message = new MailMessage();
                SmtpClient smtp = new SmtpClient();
                message.From = _mailAddress;
                message.To.Add(new MailAddress(email.EmailAddress));
                message.Subject = email.Subject;
                message.IsBodyHtml = email.IsHtml;
                message.Body = email.Body;
                smtp.Port = _PORT;
                smtp.Host = _HOST;
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = _networkCredential;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Send(message);
            }
            catch (Exception ex)
            {

            }
        }
    }

    public class Email
    {
        public string EmailAddress { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool IsHtml { get; set; }
    }
    public class SupportEmail : Email
    {
        public string UserId { get; set; }
    }
}
