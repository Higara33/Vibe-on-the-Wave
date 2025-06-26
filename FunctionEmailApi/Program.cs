using FunctionEmailApi.Services;
using FunctionEmailApi.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    // 1) Сборка конфигурации из local.settings.json / Azure App Settings
    .ConfigureAppConfiguration((context, config) =>
    {
        config
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();
    })

    // 2) Включаем модель Functions isolated worker
    .ConfigureFunctionsWorkerDefaults()

    // 3) Регистрируем ваш EmailService и настройки
    .ConfigureServices((context, services) =>
    {
        // биндим секцию EmailSettings из конфигурации в класс EmailSettings
        services.Configure<EmailService.EmailSettings>(
            context.Configuration.GetSection("EmailSettings"));
        // и добавляем EmailService по интерфейсу
        services.AddTransient<IEmailService, EmailService>();
    })

    .Build();

// Запускаем хост Azure Functions
host.Run();