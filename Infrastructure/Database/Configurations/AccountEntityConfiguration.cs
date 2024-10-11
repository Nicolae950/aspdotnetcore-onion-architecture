using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Database.Configurations;

public class AccountEntityConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.Property(acc => acc.Id)
            .IsRequired();

        builder.Property(acc => acc.FirstName)
            .IsRequired()
            .HasMaxLength(30);

        builder.Property(acc => acc.LastName)
            .IsRequired()
            .HasMaxLength(30);

        builder.Property(acc => acc.Balance)
            .IsRequired()
            .HasColumnType("money")
            .HasDefaultValue(0);

        builder.Property(acc => acc.CreatedAt)
            .IsRequired();

        builder.Property(acc => acc.Status)
            .IsRequired()
            .HasMaxLength(30)
            .HasConversion<string>();

        // Constraints
        builder
            .HasMany(a => a.TransactionsAsSource)
            .WithOne(t => t.SourceAccount)
            .HasForeignKey(t => t.SourceAccountId)
            .HasPrincipalKey(a => a.Id);

        builder
            .HasMany(a => a.TransactionsAsDestination)
            .WithOne(t => t.DestinationAccount)
            .HasForeignKey(t => t.DestinationAccountId)
            .HasPrincipalKey(a => a.Id);
    }
}
