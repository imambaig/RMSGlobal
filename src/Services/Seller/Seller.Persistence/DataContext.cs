using Microsoft.EntityFrameworkCore;
using Seller.Domain;
using System;

namespace Seller.Persistence
{
    // dotnet ef migrations add InitialCreate -p Seller.Persistence/ -s Seller.API/
    // dotnet ef migrations add SeedVehicles -p seller.Persistence/ -s seller.API/
    public class DataContext:DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Vehicle> Vehicles { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Vehicle>()
                .HasData(
                    new Vehicle { Id = 1, VIN = "VIN1VIN1VIN1VIN1", Registration = "Registration1" },
                    new Vehicle { Id = 2, VIN = "VIN2VIN2VIN2VIN2", Registration = "Registration2" },
                    new Vehicle { Id = 3, VIN = "VIN3VIN3VIN3VIN3", Registration = "Registration3" }
                );
        }
    }
}
