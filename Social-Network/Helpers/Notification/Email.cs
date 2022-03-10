using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MimeKit;
using Social_Network.Data;
using MailKit;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace Social_Network.Helpers.Notification
{
    public class Email : IEmailSender
    {
        public static string ukrNet_pswd = "qW2L0YlsMLv2DmSu";

        public async Task SendEmailAsync(
            string email,
            string subject,
            string message)
        {
            var emailMessage = new MimeMessage();


            emailMessage.From.Add(new MailboxAddress("Администрация сайта", "danilblago@ukr.net"));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.ukr.net", 2525, true);
                await client.AuthenticateAsync("danilblago@ukr.net", ukrNet_pswd);
                await client.SendAsync(emailMessage);

                await client.DisconnectAsync(true);
            }
        }
    }
}
