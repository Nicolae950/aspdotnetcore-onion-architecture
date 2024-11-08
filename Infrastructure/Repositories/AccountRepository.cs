using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories;

public class AccountRepository : FullAuditableRepository<Account>, IAccountRepository
{
    public AccountRepository(ApplicationDbContext context) 
        : base(context)
    { }

    public async Task<IEnumerable<Account>> GetAllAvailableAsync()
    {
        var accounts = await _dbSet
            .Where(a => a.Status != AccountStatus.Inactive && a.IsDeleted == false)
            .AsNoTracking()
            .ToListAsync();
        return accounts;
    }

    public async Task<IEnumerable<Account>> GetAllAccountsAsync(Guid id)
    {
        var accounts = await _dbSet
            .Where(a => a.Status != AccountStatus.Inactive && a.IsDeleted == false && a.UserId == id)
            .AsNoTracking()
            .ToListAsync();
        return accounts;
    }

    public async Task<Account> GetAccountAsync(Guid? id)
    {
        var account = await _dbSet
            .Where(a => a.Id == id)
            .AsNoTracking()
            .FirstAsync();
        return account;
    }

    public async Task InactivateEntityAsync(Account account)
    {
        account.InactivateAccount();
        await base.UpdateAsync(account);
    }
}

