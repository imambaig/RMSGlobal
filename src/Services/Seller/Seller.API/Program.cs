using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RMSGlobal.BuildingBlocks.IntegrationEventLogEF;
using Seller.API.Infrastructure;
using Seller.Persistence;
using System;

namespace Seller.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host =CreateHostBuilder(args).Build();

            using(var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var intcontext = services.GetRequiredService<IntegrationEventLogContext>();
                    intcontext.Database.Migrate();

                    var context = services.GetRequiredService<DataContext>();
                    context.Database.Migrate();
                    Seed.SeedData(context);

                    var salesSessioncontext = services.GetRequiredService<DataContext>();
                   
                    var env = services.GetService<Microsoft.AspNetCore.Hosting.IHostingEnvironment>();
                    var settings = services.GetService<IOptions<SellerSettings>>();
                    var logger = services.GetService<ILogger<SalesSessionContextSeed>>();

                    new SalesSessionContextSeed()
                    .SeedAsync(salesSessioncontext, env, settings, logger)
                    .Wait();


                }
                catch(Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occured during migration");
                }
            }
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
