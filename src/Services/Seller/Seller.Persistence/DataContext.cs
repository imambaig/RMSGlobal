using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Seller.Domain;
using System;
using System.Data;
using System.Threading.Tasks;

namespace Seller.Persistence
{
    // dotnet ef migrations add InitialCreate -p Seller.Persistence/ -s Seller.API/
    // dotnet ef migrations add SeedVehicles -p seller.Persistence/ -s seller.API/
    // dotnet ef migrations add "DirectSaleEntityAdded" -p Seller.Persistence/ -s Seller.API/

    public class DataContext : DbContext
    {

        private IDbContextTransaction _currentTransaction;
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public IDbContextTransaction GetCurrentTransaction => _currentTransaction;

        public async Task BeginTransactionAsync()
        {
            _currentTransaction = _currentTransaction ?? await Database.BeginTransactionAsync(IsolationLevel.ReadCommitted);
        }

        public async Task CommitTransactionAsync()
        {
            try
            {
                await SaveChangesAsync();
                _currentTransaction?.Commit();
            }
            catch
            {
                RollbackTransaction();
                throw;
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                _currentTransaction?.Rollback();
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

        public DbSet<Vehicle> Vehicles { get; set; }

        public DbSet<DirectSale> DirectSales { get; set; }

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
