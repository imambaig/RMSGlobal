﻿using Identity.Domain;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Persistence
{
    public class Seed
    {
        public static async Task SeedData(DataContext context, UserManager<AppUser> userManager)
        {
            if(!userManager.Users.Any())
            {
                var users = new List<AppUser>
                {
                    new AppUser
                    {
                        DisplayName="Imam",
                        UserName="imam",
                        Email="imam@test.com"
                    },
                    new AppUser
                    {
                        DisplayName="Amar",
                        UserName="amar",
                        Email="amar@test.com"
                    },
                    new AppUser
                    {
                        DisplayName="Sunil",
                        UserName="sunil",
                        Email="sunil@test.com"
                    }
                };
                foreach (var user in users)
                {
                    await userManager.CreateAsync(user,"Pa$$w0rd");
                }

            }

            if (!context.DirectSales.Any())
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
}
