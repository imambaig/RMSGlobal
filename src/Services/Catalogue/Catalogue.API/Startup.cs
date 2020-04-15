using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//using System.Web.Http.Dependencies;
using Catalogue.API.Graphql;
using GraphiQl;
using GraphQL.Http;
using GraphQL;
using GraphQL.Types;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Catalogue.API
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {

            // Register DepedencyResolver; this will be used when a GraphQL type needs to resolve a dependency
            services.AddSingleton<IServiceProvider>(c => new DefaultServiceProvider());
            services.AddSingleton<VehiclesQuery>();
            services.AddSingleton<VehicleMutation>();
            services.AddSingleton<VehicleType>();
            services.AddSingleton<BuyerVehiclePriceType>();

            //services.AddMvc(option => option.EnableEndpointRouting = false);
            services.AddControllers().AddNewtonsoftJson();
            services.AddHttpContextAccessor();
            services.AddScoped<ISchema, VehicleSchema>();
            services.AddScoped<IDocumentExecuter, DocumentExecuter>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseCors(options => options.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
            // app.UseAuthentication();

            app.UseGraphiql("/graphiql", options =>
            {
                options.GraphQlEndpoint = "/graphql";
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });
            //app.UseGraphiQl("/graphql");

            //app.UseMvc();
            /*app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                });
            });*/
        }
    }
}
