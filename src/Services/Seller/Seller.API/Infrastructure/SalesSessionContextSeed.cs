using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using Seller.API.Extensions;
using Seller.Domain.Aggregates.SalesSessionAggregate;
using Seller.Persistence;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Seller.API.Infrastructure
{
    public class SalesSessionContextSeed
    {
        public async Task SeedAsync(DataContext context, IHostingEnvironment env, IOptions<SellerSettings> settings, ILogger<SalesSessionContextSeed> logger)
        {
            var useCustomizationData = settings.Value
                .UseCustomizationData;
            var contentRootPath = env.ContentRootPath;
            
           
            context.Database.Migrate();

           /* RelationalDatabaseCreator databaseCreator =(RelationalDatabaseCreator)context.Database.GetService<IDatabaseCreator>();
            databaseCreator.CreateTables();
            if (!context.SessionStatus.Any())
            {
                context.SessionStatus.AddRange(useCustomizationData
                                        ? GetSessionStatusFromFile(contentRootPath, logger)
                                        : GetPredefinedSessionStatus());
            }*/


            await context.SaveChangesAsync();

            /*var policy = CreatePolicy(logger, nameof(SalesSessionContextSeed));

            await policy.ExecuteAsync(async () =>
            {

                var useCustomizationData = settings.Value
                .UseCustomizationData;

                var contentRootPath = env.ContentRootPath;


                using (context)
                {
                    context.Database.Migrate();

                    if (!context.CardTypes.Any())
                    {
                        context.CardTypes.AddRange(useCustomizationData
                                                ? GetCardTypesFromFile(contentRootPath, logger)
                                                : GetPredefinedCardTypes());

                        await context.SaveChangesAsync();
                    }

                    if (!context.SessionStatus.Any())
                    {
                        context.SessionStatus.AddRange(useCustomizationData
                                                ? GetSessionStatusFromFile(contentRootPath, logger)
                                                : GetPredefinedSessionStatus());
                    }

                    await context.SaveChangesAsync();
                }
            });*/
        }

        private IEnumerable<SessionStatus> GetSessionStatusFromFile(string contentRootPath, ILogger<SalesSessionContextSeed> log)
        {
            string csvFileSessionStatus = Path.Combine(contentRootPath, "Setup", "SessionStatus.csv");

            if (!File.Exists(csvFileSessionStatus))
            {
                return GetPredefinedSessionStatus();
            }

            string[] csvheaders;
            try
            {
                string[] requiredHeaders = { "SessionStatus" };
                csvheaders = GetHeaders(requiredHeaders, csvFileSessionStatus);
            }
            catch (Exception ex)
            {
                log.LogError(ex.Message);
                return GetPredefinedSessionStatus();
            }

            int id = 1;
            return File.ReadAllLines(csvFileSessionStatus)
                                        .Skip(1) // skip header row
                                        .SelectTry(x => CreateSessionStatus(x, ref id))
                                        .OnCaughtException(ex => { log.LogError(ex.Message); return null; })
                                        .Where(x => x != null);
        }

        private SessionStatus CreateSessionStatus(string value, ref int id)
        {
            if (String.IsNullOrEmpty(value))
            {
                throw new Exception("SessionStatus is null or empty");
            }

            return new SessionStatus(id++, value.Trim('"').Trim().ToLowerInvariant());
        }

        private IEnumerable<SessionStatus> GetPredefinedSessionStatus()
        {
            return new List<SessionStatus>()
            {
                SessionStatus.Draft,
                SessionStatus.Active,
                SessionStatus.AwaitingDecision,
                SessionStatus.Finished,
                SessionStatus.Completed,
                SessionStatus.Cancelled
            };
        }

        private string[] GetHeaders(string[] requiredHeaders, string csvfile)
        {
            string[] csvheaders = File.ReadLines(csvfile).First().ToLowerInvariant().Split(',');

            if (csvheaders.Count() != requiredHeaders.Count())
            {
                throw new Exception($"requiredHeader count '{ requiredHeaders.Count()}' is different then read header '{csvheaders.Count()}'");
            }

            foreach (var requiredHeader in requiredHeaders)
            {
                if (!csvheaders.Contains(requiredHeader))
                {
                    throw new Exception($"does not contain required header '{requiredHeader}'");
                }
            }

            return csvheaders;
        }

        //private Policy CreatePolicy(ILogger<SalesSessionContextSeed> logger, string prefix, int retries = 3)
        //{
        //    return Policy.Handle<SqlException>().
        //        WaitAndRetryAsync(
        //            retryCount: retries,
        //            sleepDurationProvider: retry => TimeSpan.FromSeconds(5),
        //            onRetry: (exception, timeSpan, retry, ctx) =>
        //            {
        //                logger.LogTrace($"[{prefix}] Exception {exception.GetType().Name} with message ${exception.Message} detected on attempt {retry} of {retries}");
        //            }
        //        );
        //}
    }
}
