using FunctionEmailApi.Services.Interfaces;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Options;
using System.Net;
using System.Text.Json;

namespace FunctionEmailApi
{
    public class SendMailFunction
    {
        private readonly IEmailService _email;
        public SendMailFunction(IEmailService email) => _email = email;

        [Function("SendMail")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "options", "post")] HttpRequestData req)
        {
            // ���������� Origin ��� CORS
            var origin = req.Headers.TryGetValues("Origin", out var vals)
                         ? vals.FirstOrDefault()
                         : "*";

            // ��������� preflight (OPTIONS)
            if (req.Method.Equals("OPTIONS", StringComparison.OrdinalIgnoreCase))
            {
                var preflight = req.CreateResponse(HttpStatusCode.NoContent);
                preflight.Headers.Add("Access-Control-Allow-Origin", origin);
                preflight.Headers.Add("Access-Control-Allow-Methods", "POST,OPTIONS");
                preflight.Headers.Add("Access-Control-Allow-Headers", "Content-Type");
                return preflight;
            }

            HttpResponseData response;
            try
            {
                // ��������� �� ���� ������� � ������
                string json;
                using (var sr = new StreamReader(req.Body))
                {
                    json = await sr.ReadToEndAsync();
                }

                if (string.IsNullOrWhiteSpace(json))
                    throw new ArgumentException("Payload is empty");

                // ������������� DTO �� JSON
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var dto = JsonSerializer.Deserialize<MailDto>(json, options)
                          ?? throw new ArgumentException("Cannot parse JSON payload");

                if (string.IsNullOrWhiteSpace(dto.To))
                    throw new ArgumentException("Recipient (To) cannot be null or empty");

                // ���������� ������
                await _email.SendEmailAsync(dto.To, dto.Subject, dto.Body, dto.IsHtml);

                // �������� �����
                response = req.CreateResponse(HttpStatusCode.OK);
                await response.WriteStringAsync("OK");
            }
            catch (Exception ex)
            {
                // � ������ ������ ���������� 400 � ����� ������
                response = req.CreateResponse(HttpStatusCode.BadRequest);
                await response.WriteStringAsync(ex.Message);
            }

            // ������ ��������� CORS-��������� � �����
            response.Headers.Add("Access-Control-Allow-Origin", origin);
            return response;
        }

        private record MailDto(string To, string Subject, string Body, bool IsHtml);
    }
}