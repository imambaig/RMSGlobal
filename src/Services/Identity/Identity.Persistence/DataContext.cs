using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Identity.Domain;
using System;

namespace Identity.Persistence
{
    // dotnet ef migrations add InitialCreate -p Identity.Persistence/ -s Identity.API/
    // dotnet ef migrations add SeedVehicles -p Identity.Persistence/ -s Identity.API/
    // dotnet ef migrations add "DirectSaleEntityAdded" -p Identity.Persistence/ -s Identity.API/

    public class DataContext : IdentityDbContext<AppUser>
    {
        public DataContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Vehicle> Vehicles { get; set; }

        public DbSet<DirectSale> DirectSales { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Vehicle>()
                .HasData(
                    new Vehicle { Id = 1, VIN = "VIN1VIN1VIN1VIN1", Registration = "Registration1" },
                    new Vehicle { Id = 2, VIN = "VIN2VIN2VIN2VIN2", Registration = "Registration2" },
                    new Vehicle { Id = 3, VIN = "VIN3VIN3VIN3VIN3", Registration = "Registration3" }
                );
        }
    }
}
