using Authentication.Models;
using MimeKit.Text;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using Authentication.DTO;

namespace Authentication.Services
{
    public class MailService
    {
        public static async void SendPasswordResetMailAsync(ResetToken token)
        {

			var email = new MimeMessage();
			email.From.Add(MailboxAddress.Parse("DanWienerFantasyFootball@gmail.com"));
			email.To.Add(MailboxAddress.Parse($"{token.Email}"));
			email.Subject = "Reset your password!";
			email.Body = new TextPart(TextFormat.Html) { Text = $"Copy & paste the following code into the app to reset your password: { token.Token}"};

            using var smtp = new SmtpClient();
            smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate("DanWienerFantasyFootball@gmail.com", "wjimsgiwqbkqfacz");
            smtp.Send(email);
            smtp.Disconnect(true);
        }

		public static async void RegisterEmailSend(RegisterDTO dto)
		{

			var email = new MimeMessage();
			email.From.Add(MailboxAddress.Parse("DanWienerFantasyFootball@gmail.com"));
			email.To.Add(MailboxAddress.Parse($"{dto.Email}"));
			email.Subject = "Welcome to Button Hook Fantasy Football";
			email.Body = new TextPart(TextFormat.Html) { Text = $"Username {dto.Username} has been registered for Button Hook Fantasy Football; Welcome to start playing now!" };

			using var smtp = new SmtpClient();
			smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
			smtp.Authenticate("DanWienerFantasyFootball@gmail.com", "wjimsgiwqbkqfacz");
			smtp.Send(email);
			smtp.Disconnect(true);
		}
	}
}



