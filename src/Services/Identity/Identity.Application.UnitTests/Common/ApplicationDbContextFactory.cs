using Identity.Domain;
using Identity.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Identity.Application.UnitTests.Common
{
    public static class ApplicationDbContextFactory
    {
        public static DataContext Create()
        {
            var options = new DbContextOptionsBuilder<DataContext>();
            options.UseInMemoryDatabase("InMemoryDbForTesting");
            //var operationalStoreOptions = Options.Create(
            //    new OperationalStoreOptions
            //    {
            //        DeviceFlowCodes = new TableConfiguration("DeviceCodes"),
            //        PersistedGrants = new TableConfiguration("PersistedGrants")
            //    });

            //var dateTimeMock = new Mock<IDateTime>();
            //dateTimeMock.Setup(m => m.Now)
            //    .Returns(new DateTime(3001, 1, 1));

            //var currentUserServiceMock = new Mock<ICurrentUserService>();
            //currentUserServiceMock.Setup(m => m.UserId)
            //    .Returns("00000000-0000-0000-0000-000000000000");

            var context = new DataContext(
                options.Options);

            context.Database.EnsureCreated();

            SeedSampleData(context);

            return context;
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

        public static void Destroy(DataContext context)
        {
            context.Database.EnsureDeleted();

            context.Dispose();
        }
    }
}
