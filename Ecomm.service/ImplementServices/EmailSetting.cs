using Ecomm.core.Entities.EmailSetting;
using Ecomm.service.InterfaceServices;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecomm.service.ImplementServices
{
    public class EmailSetting : IEmailSetting
    {
        private readonly EmailConfig emailconfig;

        public EmailSetting(IOptions<EmailConfig> options)
        {
            this.emailconfig = options.Value;
        }
        public async Task SendEmailAsync(string sub, string body, string to)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(emailconfig.EmailSender));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject= sub;
            email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = body };

            var client = new SmtpClient();
            await client.ConnectAsync(emailconfig.Host, int.Parse(emailconfig.Port));
            await client.AuthenticateAsync(emailconfig.EmailSender, emailconfig.EmailPassword);
            await client.SendAsync(email);
            await client.DisconnectAsync(true);


        }
    }
}
