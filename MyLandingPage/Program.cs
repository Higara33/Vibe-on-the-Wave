using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MyLandingPage;
using System;
using System.Globalization;
using System.Linq;
using System.Net.Http;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");

// 1. HttpClient для обращения к серверным API (при необходимости)
builder.Services.AddScoped(sp =>
{
    var apiBase = builder.HostEnvironment.IsDevelopment()
        ? "http://localhost:7012/"
        : builder.HostEnvironment.BaseAddress;
    return new HttpClient { BaseAddress = new Uri(apiBase) };
});

// 3. Локализация (Resources)
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

// 4. Настройка RequestLocalizationOptions из конфигурации
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supported = builder.Configuration.GetSection("Localization:SupportedCultures").Get<string[]>();
    var supportedCultures = supported?.Select(x => new CultureInfo(x)).ToList()
                             ?? new List<CultureInfo> { new CultureInfo("ru") };

    var defaultCulture = builder.Configuration.GetValue<string>("Localization:DefaultCulture") ?? "ru";

    options.DefaultRequestCulture = new RequestCulture(defaultCulture);
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
});

var host = builder.Build();

// 5. Установка культуры по умолчанию (если локализация настроена)
var locOptions = host.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value;
var defaultCI = locOptions.DefaultRequestCulture.Culture;
CultureInfo.DefaultThreadCurrentCulture = defaultCI;
CultureInfo.DefaultThreadCurrentUICulture = defaultCI;

await host.RunAsync();