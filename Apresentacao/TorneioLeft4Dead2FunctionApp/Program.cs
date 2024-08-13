using System.Globalization;
using System.Threading;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TorneioLeft4Dead2.DependencyInjection;
using TorneioLeft4Dead2FunctionApp.Json;
using JsonOptions = Microsoft.AspNetCore.Mvc.JsonOptions;

var cultureInfo = new CultureInfo("pt-BR");

CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
Thread.CurrentThread.CurrentCulture = cultureInfo;
Thread.CurrentThread.CurrentUICulture = cultureInfo;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices(services =>
    {
        services.Configure<KestrelServerOptions>(options => { options.AllowSynchronousIO = true; });
        services.Configure<IISServerOptions>(options => { options.AllowSynchronousIO = true; });
        services.Configure<JsonOptions>(options => options.UseAppOptions());
        services.AddApp();
    })
    .ConfigureAppConfiguration((_, config) => { config.AddJsonFile("host.json", true); })
    .Build();

await host.RunAsync();