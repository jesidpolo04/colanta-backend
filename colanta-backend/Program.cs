using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;

namespace colanta_backend
{
    public class Program
    {
        public static void  Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        } 

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.ConfigureLogging(logging =>
                    {
                        logging.ClearProviders();
                    });
                    webBuilder.UseNLog();
                });
    }
}
