using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionEmailApi.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string body);
        Task SendEmailAsync(string to, string subject, string body, bool isHtml);
        Task SendEmailAsync(string to, string subject, string htmlBody, string plainTextBody, bool isHtml);
    }
}
