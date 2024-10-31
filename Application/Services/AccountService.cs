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

public class AccountService : IAccountService
{
    private readonly IAccountRepository _accountRepository;
    private readonly ITransactionRepository _transactionRepository;

    private readonly AccountHelper accountHelper;
    public AccountService(IAccountRepository accountRepository, ITransactionRepository transactionRepository)
    {
        _accountRepository = accountRepository;
        _transactionRepository = transactionRepository;
        accountHelper = new AccountHelper();
    }

    public async Task<IEnumerable<Account>> GetAllAccountsAsync(Guid id)
    {
        return await _accountRepository.GetAllAccountsAsync(id);
    }

    public async Task<Account> AddAccountAsync(Account account)
    {
        var newAcc = await _accountRepository.CreateAsync(account);

        await _accountRepository.SaveAsync();
            
        return newAcc;
    }

    public async Task<Account> UpdateAccountAsync(Account account)
    {
        var accountToUpdate = await _accountRepository.GetByIdAsync(account.Id);

        accountToUpdate.ChangeAccountName(account.FirstName, account.LastName);

        await _accountRepository.UpdateAsync(accountToUpdate);
        await _accountRepository.SaveAsync();

        return accountToUpdate;
    }

    public async Task InactivateAccountAsync(Guid id)
    {
        var account = await _accountRepository.GetByIdAsync(id);
        account.CheckAccountStatus();
        await _accountRepository.InactivateEntityAsync(account);
        
        await _accountRepository.SaveAsync();
    }

    public async Task DeleteAccountAsync(Guid id)
    {
        var account = await _accountRepository.GetByIdAsync(id);
        if (account.CheckDeleteBalance())
            await _accountRepository.SoftDeleteAsync(account.Id, account);
        //await accountHelper.DeleteOwnTransactions(account.Id, _transactionRepository);

        await _accountRepository.SaveAsync();
    }

    

    public async Task<Account> GetAccountDetailsAsync(Guid? id)
    {
        return await _accountRepository.GetAccountAsync(id);
    }

    public async Task<Account> GetAccount(Guid? id)
    {
        return await _accountRepository.GetByIdAsync(id);
    }
}

