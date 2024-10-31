using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Database.Configurations;

public class UserEntityConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        //builder
        //    .HasMany(a => a.Accounts)
        //    .WithOne(a => a.User)
        //    .HasForeignKey(a => a.UserId)
        //    .HasPrincipalKey(u => u.Id);
    }
}
