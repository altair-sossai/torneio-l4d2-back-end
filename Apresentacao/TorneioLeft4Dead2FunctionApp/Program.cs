using System.Globalization;
using System.Threading;
using Microsoft.Extensions.Hosting;
using TorneioLeft4Dead2.DependencyInjection;

namespace TorneioLeft4Dead2FunctionApp
{
    public class Program
    {
        public static void Main()
        {
            var cultureInfo = new CultureInfo("pt-BR");
            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;

            var host = new HostBuilder()
                .ConfigureFunctionsWorkerDefaults()
                .ConfigureServices(services => services.AddApp())
                .Build();

            host.Run();
        }
    }
}