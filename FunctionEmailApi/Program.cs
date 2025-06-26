using FunctionEmailApi.Services;
using FunctionEmailApi.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    // 1) ������ ������������ �� local.settings.json / Azure App Settings
    .ConfigureAppConfiguration((context, config) =>
    {
        config
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();
    })

    // 2) �������� ������ Functions isolated worker
    .ConfigureFunctionsWorkerDefaults()

    // 3) ������������ ��� EmailService � ���������
    .ConfigureServices((context, services) =>
    {
        // ������ ������ EmailSettings �� ������������ � ����� EmailSettings
        services.Configure<EmailService.EmailSettings>(
            context.Configuration.GetSection("EmailSettings"));
        // � ��������� EmailService �� ����������
        services.AddTransient<IEmailService, EmailService>();
    })

    .Build();

// ��������� ���� Azure Functions
host.Run();