using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Seller.Domain.Aggregates.SalesSessionAggregate;
using System;

namespace Seller.Persistence.EntityConfigurations
{
    public class SessionStatusEntityTypeConfiguration : IEntityTypeConfiguration< SessionStatus>
    {
        public void Configure(EntityTypeBuilder<SessionStatus>  SessionStatusConfiguration)
        {
             SessionStatusConfiguration.ToTable("SessionStatus");

             SessionStatusConfiguration.HasKey(o => o.Id);

             SessionStatusConfiguration.Property(o => o.Id)
                .HasDefaultValue(1)
                .ValueGeneratedNever()
                .IsRequired();

             SessionStatusConfiguration.Property(o => o.Name)
                .HasMaxLength(200)
                .IsRequired();
        }
    }
}
