using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Seller.Domain.Aggregates.SalesSessionAggregate;
using System;

namespace Seller.Persistence.EntityConfigurations
{
    public class SalesSessionStepEntityTypeConfiguration:IEntityTypeConfiguration<SalesSessionStep>
    {
        public void Configure(EntityTypeBuilder<SalesSessionStep> salesSessionStepConfiguration)
        {
            salesSessionStepConfiguration.ToTable("SalesSessionStep");
            salesSessionStepConfiguration.Property<Guid>("Id")
                       .ValueGeneratedOnAdd()
                       .HasColumnType("TEXT");
            salesSessionStepConfiguration.Ignore(b => b.DomainEvents);
            salesSessionStepConfiguration.Property<int>("StepNumber").IsRequired(true);
            salesSessionStepConfiguration.Property<DateTime?>("StartDate").IsRequired(false);
            salesSessionStepConfiguration.Property<int>("SaleTypeId").IsRequired(true);
        }

       
    }
}
