using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using Serilog;
using Serilog.Events;

namespace OcelotApiGateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
             CreateHostBuilder(args).Build().Run();
            
        }        
        
        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            IHostBuilder builder = Host.CreateDefaultBuilder(args);
            builder.ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.ConfigureServices(s => s.AddSingleton(builder));
                webBuilder.ConfigureAppConfiguration(ic => ic.AddJsonFile("configuration.json", optional: false, reloadOnChange: true));
                webBuilder.UseStartup<Startup>();
                webBuilder.UseUrls("http://localhost:9001");
                webBuilder.UseSerilog((_, config) =>
                 {
                     config
                         .MinimumLevel.Information()
                         .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                         .Enrich.FromLogContext()
                         .WriteTo.File(@"Logs\log.txt", rollingInterval: RollingInterval.Day);
                 });
            });
            
            return builder;
        }

    }
}
        