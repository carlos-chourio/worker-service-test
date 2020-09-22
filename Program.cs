using Microsoft.Extensions.Hosting;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace SampleWorker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var builder = Host.CreateDefaultBuilder(args);
            builder.ConfigureLogging((hostingContext, loggingBuilder) =>
            {
                loggingBuilder.AddFile("Logs/SampleWorker-{Date}.log");
            });
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) {
                builder.UseSystemd();
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {
                builder.UseWindowsService();
            }
            return builder.ConfigureServices((hostContext, services) =>
                   {
                       services.AddHostedService<Worker>();
                   });
        }
    }
}
