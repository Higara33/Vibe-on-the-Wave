using MyLandingPage.Services.Interfaces;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Security;

namespace MyLandingPage.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _settings;

        public EmailService(IOptions<EmailSettings> options)
        {
            _settings = options.Value;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            await SendEmailAsync(to, subject, body, isHtml: false);
        }

        public async Task SendEmailAsync(string to, string subject, string body, bool isHtml)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Сайт", _settings.User));
            message.To.Add(MailboxAddress.Parse(to));
            message.Subject = subject;

            var builder = new BodyBuilder();
            if (isHtml)
            {
                builder.HtmlBody = body;
                // Создаем текстовую версию из HTML для клиентов, не поддерживающих HTML
                builder.TextBody = System.Text.RegularExpressions.Regex.Replace(body, "<.*?>", "");
            }
            else
            {
                builder.TextBody = body;
            }
            
            message.Body = builder.ToMessageBody();

            using var client = new SmtpClient();
            await client.ConnectAsync(_settings.Host, _settings.Port, SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(_settings.User, _settings.Password);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }

        public async Task SendEmailAsync(string to, string subject, string htmlBody, string plainTextBody, bool isHtml)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Сайт", _settings.User));
            message.To.Add(MailboxAddress.Parse(to));
            message.Subject = subject;

            var builder = new BodyBuilder();
            if (isHtml)
            {
                builder.HtmlBody = htmlBody;
                builder.TextBody = plainTextBody;
            }
            else
            {
                builder.TextBody = htmlBody; // В случае если isHtml = false, используем htmlBody как текст
            }
            
            message.Body = builder.ToMessageBody();

            using var client = new SmtpClient();
            await client.ConnectAsync(_settings.Host, _settings.Port, SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(_settings.User, _settings.Password);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }

        public class EmailSettings
        {
            public string Host { get; set; } = string.Empty;
            public int Port { get; set; }
            public string User { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
        }
    }
}
