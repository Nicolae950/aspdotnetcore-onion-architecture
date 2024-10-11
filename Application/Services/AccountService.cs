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
        
    public async Task<Account> AddAccount(Account account)
    {
        var newAcc = await _accountRepository.Create(account);

        await _accountRepository.Save();
            
        return newAcc;
    }

    public async Task<Account> UpdateAccount(Account account)
    {
        var accountToUpdate = await _accountRepository.GetById(account.Id);

        accountToUpdate.ChangeAccountName(account.FirstName, account.LastName);

        await _accountRepository.Update(accountToUpdate);
        await _accountRepository.Save();

        return accountToUpdate;
    }

    public async Task InactivateAccount(Guid id)
    {
        var account = await _accountRepository.GetById(id);
        
        await _accountRepository.InactivateEntity(account);
        
        await _accountRepository.Save();
    }

    public async Task DeleteAccount(Guid id)
    {
        var account = await _accountRepository.GetById(id);

        await _accountRepository.SoftDelete(account.Id, account);
        //await accountHelper.DeleteOwnTransactions(account.Id, _transactionRepository);

        await _accountRepository.Save();
    }

    public async Task<Account> GetAccountDetails(Guid? id)
    {
        return await _accountRepository.GetAccountWithTransactions(id);
    }
}

