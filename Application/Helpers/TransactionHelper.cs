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
    public static async Task RejectedTransactionBalanceUpdateAsync(Transaction transaction, IAccountRepository accountRepository)
    {
        var sAccount = await accountRepository.GetByIdAsync(transaction.SourceAccountId);
        var dAccount = await accountRepository.GetByIdAsync(transaction.DestinationAccountId);

        sAccount.ReverseTransfer(transaction);
        dAccount.ReverseTransfer(transaction);

        await accountRepository.UpdateAsync(sAccount);
        await accountRepository.UpdateAsync(dAccount);
    }

    public static async Task ExecuteUpdateAccountsBalanceAsync(Transaction transaction, IAccountRepository accountRepository)
    {
        var sAccount = await accountRepository.GetByIdAsync(transaction.SourceAccountId);
        var dAccount = await accountRepository.GetByIdAsync(transaction.DestinationAccountId);

        await accountRepository.ExecuteUpdateAsync(sAccount.Id, 
            sAccount,
            a => a.Id == sAccount.Id,
            u => u.SetProperty(a => a.Balance, sAccount.Balance + transaction.Amount));

        await accountRepository.ExecuteUpdateAsync(dAccount.Id,
            dAccount,
            a => a.Id == dAccount.Id,
            u => u.SetProperty(a => a.Balance, dAccount.Balance - transaction.Amount));
    }
}
