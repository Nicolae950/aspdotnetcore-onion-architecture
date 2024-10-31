using Domain.Entities;
using Domain.Models;
using Infrastructure.Database.Configurations;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Database;

public class ApplicationDbContext : DbContext
{
    public DbSet<Account> Accounts { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Report> Reports { get; set; }

    public readonly IHttpContextAccessor _httpContextAccessor;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IHttpContextAccessor httpContextAccessor)
        : base(options)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        new AccountEntityConfiguration().Configure(modelBuilder.Entity<Account>());
        new TransactionEntityConfiguration().Configure(modelBuilder.Entity<Transaction>());
        
        //new UserEntityConfiguration().Configure(modelBuilder.Entity<User>());
        
        //modelBuilder.ApplyConfigurationsFromAssembly(typeof(AccountEntityConfiguration).Assembly);
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var req = _httpContextAccessor.HttpContext.Request.Path.Value.Trim().Split('/');
        var id = req.Length > 4 ? Guid.Parse(req[4].ToUpper()) : Guid.Empty;

        foreach (var account in ChangeTracker.Entries<Account>().AsEnumerable())
        {
            if (account.State == EntityState.Added)
            {
                account.Entity.CreatedBy = account.Entity.Id;
            }
        }

        foreach (var baseEntity in ChangeTracker.Entries<BaseEntity>().AsEnumerable())
        {
            if (baseEntity.State == EntityState.Added)
            {
                baseEntity.Entity.CreatedAt = DateTimeOffset.Now;
                if (baseEntity.Entity.CreatedBy == Guid.Empty)
                    baseEntity.Entity.CreatedBy = id;
            }
        }

        foreach (var auditableEntity in ChangeTracker.Entries<AuditableEntity>().AsEnumerable())
        {
            if (auditableEntity.State == EntityState.Modified)
            {
                auditableEntity.Entity.LastModifiedAt = DateTimeOffset.Now;
                auditableEntity.Entity.LastModifiedBy = id;
            }
        }

        foreach (var fullAuditableEntity in ChangeTracker.Entries<FullAuditableEntity>().AsEnumerable())
        {
            if (fullAuditableEntity.Entity.IsDeleted == true && fullAuditableEntity.State == EntityState.Modified)
            {
                fullAuditableEntity.Entity.DeletedAt = DateTimeOffset.Now;
                fullAuditableEntity.Entity.DeletedBy = id;
            }
        }
        return base.SaveChangesAsync(cancellationToken);
    }
}
