using Identity.Application.Interfaces;
using Identity.Domain;
using Identity.Persistence;
using IdentityModel.Client;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Indentity.API.IntegrationTests
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Remove the app's ApplicationDbContext registration.
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                        typeof(DbContextOptions<DataContext>));

                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                // Add a database context using an in-memory 
                // database for testing.
                services.AddDbContext<DataContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDbForTesting");
                });

                // Register test services
                services.AddScoped<IUserAccessor, TestUserAccessor>();
                //services.AddScoped<ICurrentUserService, TestCurrentUserService>();
                //services.AddScoped<IDateTime, TestDateTimeService>();
                //services.AddScoped<IIdentityService, TestIdentityService>();

                // Build the service provider
                var sp = services.BuildServiceProvider();

                // Create a scope to obtain a reference to the database
                // context (ApplicationDbContext).
                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var context = scopedServices.GetRequiredService<DataContext>();
                    var logger = scopedServices.GetRequiredService<ILogger<CustomWebApplicationFactory<TStartup>>>();

                    // Ensure the database is created.
                    context.Database.EnsureCreated();

                    try
                    {
                        // Seed the database with test data.
                        SeedSampleData(context);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "An error occurred seeding the database with test messages. Error: {Message}", ex.Message);
                    }
                }
            })
                .UseEnvironment("Test");
        }

        public HttpClient GetAnonymousClient()
        {
            return CreateClient();
        }

        public async Task<HttpClient> GetAuthenticatedClientAsync()
        {
            return await GetAuthenticatedClientAsync("jason@clean-architecture", "CA!");
        }

        public async Task<HttpClient> GetAuthenticatedClientAsync(string userName, string password)
        {
            var client = CreateClient();

            var token = await GetAccessTokenAsync(client, userName, password);

            client.SetBearerToken(token);

            return client;
        }

        private async Task<string> GetAccessTokenAsync(HttpClient client, string userName, string password)
        {
            var disco = await client.GetDiscoveryDocumentAsync();

            if (disco.IsError)
            {
                throw new Exception(disco.Error);
            }

            var response = await client.RequestPasswordTokenAsync(new PasswordTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = "CA.IntegrationTests",
                ClientSecret = "secret",

                Scope = "CA.WebUIAPI openid profile",
                UserName = userName,
                Password = password
            });

            if (response.IsError)
            {
                throw new Exception(response.Error);
            }

            return response.AccessToken;
        }

        public static void SeedSampleData(DataContext context)
        {

            var directSales = new List<DirectSale>
                {
                    new DirectSale  { Name="DirectSale1", EndDate=DateTime.Now.AddDays(1), DirectSaleType="DS"},
                    new DirectSale  { Name="DirectSale2", EndDate=DateTime.Now.AddDays(2), DirectSaleType="CO"},
                    new DirectSale  { Name="DirectSale3", EndDate=DateTime.Now.AddDays(3), DirectSaleType="DS"},
                    new DirectSale  { Name="DirectSale4", EndDate=DateTime.Now.AddDays(4), DirectSaleType="DS"},
                    new DirectSale  { Name="DirectSale5", EndDate=DateTime.Now.AddDays(5), DirectSaleType="CO"},
                    new DirectSale  { Name="DirectSale6", EndDate=DateTime.Now.AddDays(6), DirectSaleType="CO"},
                    new DirectSale  { Name="DirectSale7", EndDate=DateTime.Now.AddDays(7), DirectSaleType="DS"},
                    new DirectSale  { Name="DirectSale8", EndDate=DateTime.Now.AddDays(8), DirectSaleType="CO"},
                    new DirectSale  { Name="DirectSale9", EndDate=DateTime.Now.AddDays(9), DirectSaleType="DS"},
                    new DirectSale  { Name="DirectSale10", EndDate=DateTime.Now.AddDays(10), DirectSaleType="CO"},
                    new DirectSale  { Name="DirectSale11", EndDate=DateTime.Now.AddDays(11), DirectSaleType="DS"},

                };
            context.DirectSales.AddRange(directSales);
            context.SaveChanges();
        }
    }
}
