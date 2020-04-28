using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Seller.Domain.Aggregates.SalesSessionAggregate;
using System;

namespace Seller.Persistence.EntityConfigurations
{
    public class SalesSessionEntityTypeConfiguration : IEntityTypeConfiguration<SalesSession>
    {
        public void Configure(EntityTypeBuilder<SalesSession> salesSessionConfiguration)
        {
            salesSessionConfiguration.ToTable("SalesSession");
            salesSessionConfiguration.Property<Guid>("Id")
                       .ValueGeneratedOnAdd()
                       .HasColumnType("TEXT");
            salesSessionConfiguration.Ignore(b => b.DomainEvents);
            salesSessionConfiguration.Property<string>("Name").IsRequired(true);
            salesSessionConfiguration.Property<int>("SessionStatusId").IsRequired(true);

            var navigation = salesSessionConfiguration.Metadata.FindNavigation(nameof(SalesSession.SalesSessionSteps));

            navigation.SetPropertyAccessMode(PropertyAccessMode.Field);

            salesSessionConfiguration.HasOne(o => o.SessionStatus)
                .WithMany()
                .HasForeignKey("SessionStatusId");
        }
    }
}