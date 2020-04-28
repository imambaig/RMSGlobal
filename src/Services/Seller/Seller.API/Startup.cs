using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Seller.Persistence;
using MediatR;
using Seller.Application.DirectSales;
using FluentValidation.AspNetCore;
using Seller.API.Middleware;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using HealthChecks.UI.Client;
using System.Text;
using Newtonsoft.Json;
using RMSGlobal.BuildingBlocks.EventBus.Abstractions;
using Autofac;
using RMSGlobal.BuildingBlocks.EventBus;
using RMSGlobal.BuildingBlocks.EventBusServiceBus;
using RMSGlobal.BuildingBlocks.EventBusRabbitMQ;
using Microsoft.AspNetCore.Http;
using System.Data.Common;
using RMSGlobal.BuildingBlocks.IntegrationEventLogEF.Services;
using Microsoft.Azure.ServiceBus;
using RabbitMQ.Client;
using Seller.Application.IntegrationEvents;
using RMSGlobal.BuildingBlocks.IntegrationEventLogEF;
using System.Reflection;
using Seller.Application.Behaviours;
using Seller.Domain.SeedWork;
using Seller.Persistence.DomainEvents;

namespace Seller.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<DataContext>(opt =>
            {
                opt.UseSqlite(Configuration.GetConnectionString("DefaultConnection"));
            });

            //services.AddDbContext<SalesSessionContext>(opt =>
            //{
            //    opt.UseSqlite(Configuration.GetConnectionString("DefaultConnection"));
            //});


            services.AddCors(Options =>
            {
                Options.AddPolicy("CorsPolicy", policy =>
                {
                    policy.AllowAnyHeader().AllowAnyMethod().WithExposedHeaders("WWW-Authenticate").WithOrigins("http://localhost:3000").AllowCredentials();
                });
            });
            services.AddMediatR(typeof(List.Handler).Assembly);
            services.AddControllers()
                    .AddFluentValidation(cfg=>
                    {
                        cfg.RegisterValidatorsFromAssemblyContaining<Create>();
                    });
            services.AddHealthChecks().AddDbContextCheck<DataContext>();
            /* services.AddDbContext<IntegrationEventLogContext>(options =>
             {
                 options.UseSqlServer(Configuration["ConnectionString"],
                                      sqlServerOptionsAction: sqlOptions =>
                                      {
                                          sqlOptions.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name);
                                          //Configuring Connection Resiliency: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency 
                                          sqlOptions.EnableRetryOnFailure(maxRetryCount: 10, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                                      });
             });*/
            services.AddDbContext<IntegrationEventLogContext>(options =>
            {
                options.UseSqlite(Configuration.GetConnectionString("DefaultConnection"),
                                      sqliteOptionsAction: sqlOptions =>
                                      {
                                          sqlOptions.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name);
                                          ////Configuring Connection Resiliency: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency 
                                          //sqlOptions.EnableRetryOnFailure(maxRetryCount: 10, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                                      });
            });

            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(TransactionBehaviour<,>));
            services.AddScoped(typeof(IDomainEventDispatcher), typeof(DomainEventDispatcher));
            services.AddCustomIntegrations(Configuration)
                    .AddCustomConfiguration(Configuration)
                    .AddEventBus(Configuration);

            ////configure autofac

            //var container = new ContainerBuilder();
            //container.Populate(services);

            //container.RegisterModule(new MediatorModule());
            //container.RegisterModule(new ApplicationModule(Configuration.GetConnectionString("DefaultConnection")));

            //return new AutofacServiceProvider(container.Build());
            //services.AddAutofac(container.Build());
           // var serviceProvider = services.BuildServiceProvider();
            //var serviceScopeFactory = serviceProvider.GetRequiredService<IServiceScopeFactory>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<ErrorHandlingMiddleware>();
            if (env.IsDevelopment())
            {
                //app.UseDeveloperExceptionPage();
            }
            /*app.UseHealthChecks("/health", new HealthCheckOptions()
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });*/
            app.UseHealthChecks("/health", new HealthCheckOptions
            {
                ResponseWriter = async (context, report) =>
                {
                    context.Response.ContentType = "application/json; charset=utf-8";
                    var bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(report));
                    await context.Response.Body.WriteAsync(bytes);
                }
            });
            //app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors("CorsPolicy");

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

    }

    static class CustomExtensionsMethods
    {
        public static IServiceCollection AddCustomConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions();
            services.Configure<SellerSettings>(configuration);
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var problemDetails = new ValidationProblemDetails(context.ModelState)
                    {
                        Instance = context.HttpContext.Request.Path,
                        Status = StatusCodes.Status400BadRequest,
                        Detail = "Please refer to the errors property for additional details."
                    };

                    return new BadRequestObjectResult(problemDetails)
                    {
                        ContentTypes = { "application/problem+json", "application/problem+xml" }
                    };
                };
            });

            return services;
        }
        public static IServiceCollection AddCustomIntegrations(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
           // services.AddTransient<IIdentityService, IdentityService>();
            services.AddTransient<Func<DbConnection, IIntegrationEventLogService>>(
                sp => (DbConnection c) => new IntegrationEventLogService(c));

            services.AddTransient<ISellerIntegrationEventService, SellerIntegrationEventService>();

            if (configuration.GetValue<bool>("AzureServiceBusEnabled"))
            {
                services.AddSingleton<IServiceBusPersisterConnection>(sp =>
                {
                    var logger = sp.GetRequiredService<ILogger<DefaultServiceBusPersisterConnection>>();

                    var serviceBusConnectionString = configuration["EventBusConnection"];
                    var serviceBusConnection = new ServiceBusConnectionStringBuilder(serviceBusConnectionString);

                    return new DefaultServiceBusPersisterConnection(serviceBusConnection, logger);
                });
            }
            else
            {
                services.AddSingleton<IRabbitMQPersistentConnection>(sp =>
                {
                    var logger = sp.GetRequiredService<ILogger<DefaultRabbitMQPersistentConnection>>();


                    var factory = new ConnectionFactory()
                    {
                        HostName = configuration["EventBusConnection"]
                    };

                    if (!string.IsNullOrEmpty(configuration["EventBusUserName"]))
                    {
                        factory.UserName = configuration["EventBusUserName"];
                    }

                    if (!string.IsNullOrEmpty(configuration["EventBusPassword"]))
                    {
                        factory.Password = configuration["EventBusPassword"];
                    }

                    var retryCount = 5;
                    if (!string.IsNullOrEmpty(configuration["EventBusRetryCount"]))
                    {
                        retryCount = int.Parse(configuration["EventBusRetryCount"]);
                    }

                    return new DefaultRabbitMQPersistentConnection(factory, logger, retryCount);
                });
            }

            return services;
        }
               
        public static IServiceCollection AddEventBus(this IServiceCollection services, IConfiguration configuration)
        {
            var subscriptionClientName = configuration["SubscriptionClientName"];

            if (configuration.GetValue<bool>("AzureServiceBusEnabled"))
            {
                services.AddSingleton<IEventBus, EventBusServiceBus>(sp =>
                {
                    var serviceBusPersisterConnection = sp.GetRequiredService<IServiceBusPersisterConnection>();
                    var iLifetimeScope = sp.GetRequiredService<ILifetimeScope>();
                    var logger = sp.GetRequiredService<ILogger<EventBusServiceBus>>();
                    var eventBusSubcriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();

                    return new EventBusServiceBus(serviceBusPersisterConnection, logger,
                        eventBusSubcriptionsManager, subscriptionClientName, iLifetimeScope);
                });
            }
            else
            {
                services.AddSingleton<IEventBus, EventBusRabbitMQ>(sp =>
                {
                    var rabbitMQPersistentConnection = sp.GetRequiredService<IRabbitMQPersistentConnection>();
                   // var iLifetimeScope = sp.GetRequiredService<ILifetimeScope>();
                    var logger = sp.GetRequiredService<ILogger<EventBusRabbitMQ>>();
                    var eventBusSubcriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();
                    var serviceScopeFactory = sp.GetRequiredService<IServiceScopeFactory>();
                    var retryCount = 5;
                    if (!string.IsNullOrEmpty(configuration["EventBusRetryCount"]))
                    {
                        retryCount = int.Parse(configuration["EventBusRetryCount"]);
                    }

                    return new EventBusRabbitMQ(rabbitMQPersistentConnection, logger, eventBusSubcriptionsManager, subscriptionClientName, retryCount, serviceScopeFactory);
                });
            }

            services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();

            return services;
        }
    }
}
