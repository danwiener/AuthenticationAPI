using Authentication.Models;
using System.Net.Mail;

namespace Authentication.Services
{
    public class MailService
    {
        private static readonly string smtpClient = "localhost";
        private static readonly int smtpPort = 1025;
        private static readonly string smtpEmail = "test@gmail.com";
        private static readonly string smtpName = "Testing";

        public static async void SendPasswordResetMailAsync(ResetToken token)
        {
            SmtpClient client = new(smtpClient)
            {
                Port = smtpPort,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = true,
                EnableSsl = false
            };

            MailMessage email = new()
            {
                From = new MailAddress(smtpEmail, smtpName),
                Subject = "Reset your password!",
                Body = $"Click <a href=\"http://localhost:3000/reset/{token.Token}\">here</a> to reset your password!",
                IsBodyHtml = true
            };

            email.To.Add(new MailAddress(token.Email));

            await client.SendMailAsync(email);

            email.Dispose();
        }
    }
}
