﻿using Domain.Entities;
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

    public async Task<IEnumerable<Account>> GetAllAccounts()
    {
        var accounts = await _dbSet
            .Where(a => a.Status != AccountStatus.Inactive && a.IsDeleted == false)
            .ToListAsync();
        return accounts;
    }

    public async Task<Account> GetAccountWithTransactions(Guid? id)
    {
        var account = await _dbSet
            .Include(a => a.TransactionsAsSource
                .Where(t => t.IsDeleted == false)
                )
            .Include(a => a.TransactionsAsDestination
                .Where(t => t.IsDeleted == false)
                )
            .Where(a => a.Id == id)
            .AsNoTracking()
            .FirstAsync();
        return account;
    }

    public async Task InactivateEntity(Account account)
    {
        account.GhangeAccountStatus(AccountStatus.Inactive);
        await base.Update(account);
    }

    public async Task ActivateAccount(Account account)
    {
        account.GhangeAccountStatus(AccountStatus.Active);
        await base.Update(account);
    }
}

