using Application.Helpers;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Interfaces;
using Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services;

public class AccountService(IAccountRepository accountRepository, ITransactionRepository transactionRepository) : IAccountService
{
    public async Task<IEnumerable<Account>> GetAllAccountsAsync(Guid id)
    {
        return await accountRepository.GetAllAccountsAsync(id);
    }

    public async Task<Account> AddAccountAsync(Account account)
    {
        var newAcc = await accountRepository.CreateAsync(account);

        await accountRepository.SaveAsync();
            
        return newAcc;
    }

    public async Task<Account> UpdateAccountAsync(Account account)
    {
        var accountToUpdate = await accountRepository.GetByIdAsync(account.Id);

        accountToUpdate.ChangeAccountName(account.FirstName, account.LastName);

        await accountRepository.UpdateAsync(accountToUpdate);
        await accountRepository.SaveAsync();

        return accountToUpdate;
    }

    public async Task InactivateAccountAsync(Guid id)
    {
        var account = await accountRepository.GetByIdAsync(id);
        account.CheckAccountStatus();
        await accountRepository.InactivateEntityAsync(account);
        
        await accountRepository.SaveAsync();
    }

    public async Task DeleteAccountAsync(Guid id)
    {
        var account = await accountRepository.GetByIdAsync(id);
        if (account.CheckDeleteBalance())
            await accountRepository.SoftDeleteAsync(account.Id, account);

        await accountRepository.SaveAsync();
    }

    

    public async Task<Account> GetAccountDetailsAsync(Guid? id)
    {
        return await accountRepository.GetAccountAsync(id);
    }

    public async Task<Account> GetAccount(Guid? id)
    {
        return await accountRepository.GetByIdAsync(id);
    }
}

