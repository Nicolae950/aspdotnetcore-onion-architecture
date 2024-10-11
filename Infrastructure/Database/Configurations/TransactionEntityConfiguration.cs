using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Database.Configurations;

public class TransactionEntityConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.Property(t => t.SourceAccountId)
            .IsRequired();

        builder.Property(t => t.OperationType)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(t => t.Amount)
            .IsRequired()
            .HasColumnType("money");

        builder.Property(t => t.StateOfTransaction)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(t => t.Description)
            .HasMaxLength(100);

    }
}
