using Microsoft.AspNetCore.Identity.UI.Services;
using MimeKit;
using System.Text.Encodings.Web;
using MimeKit;
//using MailKit.Net.Smtp;
using System.Net.Mail;
using System.Net;

namespace Presentation.Extensions
{
    public class EmailSenderExtensions : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            MailAddress to = new MailAddress(email);
            MailAddress from = new MailAddress("fatbardhd_@outlook.com");
            MailMessage message = new MailMessage(from, to);
            message.Subject = subject;
            message.Body = htmlMessage;
            SmtpClient client = new SmtpClient("smtp-mail.outlook.com", 587)
            {
                Credentials = new NetworkCredential("fatbardhd_@outlook.com", "Bardhi123"),
                EnableSsl = true
                // specify whether your host accepts SSL connections
            };
            // code in brackets above needed if authentication required
            try
            {
                client.Send(message);
            }
            catch (SmtpException ex)
            {
                Console.WriteLine(ex.ToString());
            }
            //var emailToSend = new MimeMessage();
            //emailToSend.From.Add(MailboxAddress.Parse("fatbardhd_@outlook.com"));
            //emailToSend.To.Add(MailboxAddress.Parse(email));
            //emailToSend.Subject = subject;
            //emailToSend.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = htmlMessage };

            ////send email
            //using (var emailClient = new SmtpClient())
            //{
            //    emailClient.Connect("smtp-mail.outlook.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
            //    emailClient.Authenticate("fatbardhd_@outlook.com", "bardhi123");
            //    emailClient.Send(emailToSend);
            //    emailClient.Disconnect(true);
            //}

            return Task.CompletedTask;
        }
    }

}