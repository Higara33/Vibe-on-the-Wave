using FunctionEmailApi.Services.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Text.RegularExpressions;

namespace FunctionEmailApi.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _settings;

        public EmailService(IOptions<EmailSettings> options)
        {
            _settings = options.Value;
        }

        // Простая отправка текстового письма
        public async Task SendEmailAsync(string to, string subject, string body)
        {
            await SendEmailAsync(to, subject, htmlBody: body, plainTextBody: body, isHtml: false);
        }

        // Отправка письма с выбором HTML или текстового формата
        public async Task SendEmailAsync(string to, string subject, string body, bool isHtml)
        {
            if (isHtml)
            {
                // Генерация текстовой версии из HTML
                var plain = Regex.Replace(body, "<.*?>", string.Empty);
                await SendEmailAsync(to, subject, htmlBody: body, plainTextBody: plain, isHtml: true);
            }
            else
            {
                await SendEmailAsync(to, subject, htmlBody: body, plainTextBody: body, isHtml: false);
            }
        }

        // Основной метод отправки: HTML + текст
        public async Task SendEmailAsync(string to, string subject, string htmlBody, string plainTextBody, bool isHtml)
        {
            var message = new MimeMessage();

            // From: отображаемое имя и адрес
            message.From.Add(new MailboxAddress("Vibe on the Wave", _settings.User));
            // Reply-To: куда отвечать
            message.ReplyTo.Add(new MailboxAddress("Vibe Support", _settings.User));
            // To: получатель
            message.To.Add(MailboxAddress.Parse(to));
            message.Subject = subject;

            // List-Unsubscribe: ссылка для отписки
            message.Headers.Add("List-Unsubscribe", $"<mailto:unsubscribe@vibeofthewave.com>");

            var builder = new BodyBuilder
            {
                HtmlBody = isHtml ? htmlBody : null,
                TextBody = plainTextBody
            };

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
