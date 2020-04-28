using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using System;
using System.Collections.Generic;
using System.Text;

namespace RMSGlobal.BuildingBlocks.IntegrationEventLogEF
{
    public class IntegrationEventLogContext : DbContext
    {
        //static LoggerFactory object
     //   public static readonly ILoggerFactory loggerFactory = new LoggerFactory(new[] {
     //new ConsoleLoggerProvider((_, __) => true, true)});

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseLoggerFactory(loggerFactory)  //tie-up DbContext with LoggerFactory object
        //        .EnableSensitiveDataLogging();
        //}

        public IntegrationEventLogContext(DbContextOptions<IntegrationEventLogContext> options) : base(options)
        {
            
        }

        public DbSet<IntegrationEventLogEntry> IntegrationEventLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {          
            builder.Entity<IntegrationEventLogEntry>(ConfigureIntegrationEventLogEntry);
        }

        void ConfigureIntegrationEventLogEntry(EntityTypeBuilder<IntegrationEventLogEntry> builder)
        {
            builder.ToTable("IntegrationEventLog");

            builder.HasKey(e => e.EventId);

            builder.Property(e => e.EventId)
                .IsRequired();

            builder.Property(e => e.Content)
                .IsRequired();

            builder.Property(e => e.CreationTime)
                .IsRequired();

            builder.Property(e => e.State)
                .IsRequired();

            builder.Property(e => e.TimesSent)
                .IsRequired();

            builder.Property(e => e.EventTypeName)
                .IsRequired();

        }
    }
}
