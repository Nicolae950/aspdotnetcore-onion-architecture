using Application.DTOs;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Helpers;

public static class TransactionHelper
{
    public static async Task RejectedTransactionBalanceUpdate(Transaction transaction, IAccountRepository accountRepository)
    {
        var sAccount = await accountRepository.GetById(transaction.SourceAccountId);
        var dAccount = await accountRepository.GetById(transaction.DestinationAccountId);

        sAccount.ChangeBalance(sAccount.Balance + transaction.Amount);
        dAccount.ChangeBalance(dAccount.Balance - transaction.Amount);

        await accountRepository.Update(sAccount);
        await accountRepository.Update(dAccount);
    }

    public static async Task ExecuteUpdateAccountsBalance(Transaction transaction, IAccountRepository accountRepository)
    {
        var sAccount = await accountRepository.GetById(transaction.SourceAccountId);
        var dAccount = await accountRepository.GetById(transaction.DestinationAccountId);

        await accountRepository.ExecuteUpdate(sAccount.Id, 
            sAccount,
            a => a.Id == sAccount.Id,
            u => u.SetProperty(a => a.Balance, sAccount.Balance + transaction.Amount));

        await accountRepository.ExecuteUpdate(dAccount.Id,
            dAccount,
            a => a.Id == dAccount.Id,
            u => u.SetProperty(a => a.Balance, dAccount.Balance - transaction.Amount));
    }
}
