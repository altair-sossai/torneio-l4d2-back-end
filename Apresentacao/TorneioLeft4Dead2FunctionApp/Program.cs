using Microsoft.Extensions.Hosting;
using TorneioLeft4Dead2.DependencyInjection;

namespace TorneioLeft4Dead2FunctionApp
{
    public class Program
    {
        public static void Main()
        {
            var host = new HostBuilder()
                .ConfigureFunctionsWorkerDefaults()
                .ConfigureServices(services => services.AddApp())
                .Build();

            host.Run();
        }
    }
}