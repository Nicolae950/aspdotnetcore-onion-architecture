using Application.DTOs;
using Application.Helpers;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Interfaces;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services;

public class TransactionService : ITransactionService
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IAccountRepository _accountRepository;
    public TransactionService(ITransactionRepository transactionRepository, IAccountRepository accountRepository)
    {
        _transactionRepository = transactionRepository;
        _accountRepository = accountRepository;
    }

    public async Task<IEnumerable<Transaction>> GetAllTransactionsForAccount(Guid accountId, FilterDTO filter)
    {
        var transactions = await _transactionRepository.GetAllTransactionsForAccount(accountId);

        var filteredTransactions = await filter.GetFilter(transactions).ToListAsync();
        return filteredTransactions;
    }

    public async Task<Transaction> GetTransactionDetails(Guid id)
    {
        return await _transactionRepository.GetTransactionDetalized(id);
    }

    public async Task<Transaction> CreateDeposit(Transaction transaction)
    {
        var newTran = await _transactionRepository.Create(transaction);

        var account = await _accountRepository.GetById(transaction.SourceAccountId);

        account.CheckAccountStatus();

        account.ChangeBalance(account.Balance + transaction.Amount);

        if (account.Status != AccountStatus.Active) await _accountRepository.ActivateAccount(account);

        await _accountRepository.Update(account);
        await _transactionRepository.Save();

        return newTran;
    }

    public async Task<Transaction> CreateWithdrawal(Transaction transaction)
    {
        Transaction newTran = null; 
        var account = await _accountRepository.GetById(transaction.SourceAccountId);

        account.CheckAccountStatus();

        account.CheckEligibleForWithdrawal(transaction.Amount);

        newTran = await _transactionRepository.Create(transaction);

        account.ChangeBalance(account.Balance - transaction.Amount);

        await _accountRepository.Update(account);
        await _transactionRepository.Save();

        return newTran;
    }

    public async Task<Transaction> CreateTransfer(Transaction transaction)
    {
        Transaction newTran = null;

        var sAccount = await _accountRepository.GetById(transaction.SourceAccountId);
        var dAccount = await _accountRepository.GetById(transaction.DestinationAccountId);

        sAccount.CheckAccountStatus();
        dAccount.CheckAccountStatus();
        
        sAccount.CheckMoneyBalance(transaction.Amount);

        newTran = await _transactionRepository.Create(transaction);

        sAccount.DoTransaction(newTran, "+");
        dAccount.DoTransaction(newTran, "-");

        //sAccount.ChangeBalance(sAccount.Balance - transaction.Amount);
        //dAccount.ChangeBalance(dAccount.Balance + transaction.Amount);

        await _accountRepository.Update(sAccount);
        await _accountRepository.Update(dAccount);
        await _transactionRepository.Save();

        return newTran;
    }

    public async Task<Transaction> UpdateTransaction(Guid accountId, Transaction transaction)
    {
        var transactionToUpdate = await _transactionRepository.GetById(transaction.Id);

        transactionToUpdate.IsWaitingTransfer();

        if(transaction.CheckClientTransferStatus(accountId)) 
            transactionToUpdate.ChangeState(transaction.StateOfTransaction);
        else
        {
            await TransactionHelper.RejectedTransactionBalanceUpdate(transactionToUpdate, _accountRepository);
            transactionToUpdate.ChangeState(StateOfTransaction.Rejected);
        }

        await _transactionRepository.Update(transactionToUpdate);
        await _transactionRepository.Save();
        
        return transactionToUpdate;
    }

    public async Task ExecuteUpdateTransaction(Guid accountId, Transaction transaction)
    {
        var transactionToUpdate = await _transactionRepository.GetById(transaction.Id);

        transactionToUpdate.IsWaitingTransfer();

        if (transaction.CheckClientTransferStatus(accountId))
            await _transactionRepository.ExecuteUpdate(
                accountId, 
                transactionToUpdate,
                t => t.StateOfTransaction == StateOfTransaction.Waiting && t.Id == transaction.Id,
                p => p.SetProperty(t => t.StateOfTransaction, StateOfTransaction.Done));
        else
        {
            await _transactionRepository.ExecuteUpdate(
                accountId, 
                transactionToUpdate,
                t => t.StateOfTransaction == StateOfTransaction.Waiting && t.Id == transaction.Id,
                p => p.SetProperty(t => t.StateOfTransaction, StateOfTransaction.Rejected));

            await TransactionHelper.ExecuteUpdateAccountsBalance(transactionToUpdate, _accountRepository);
        }
    }


    public async Task DeleteTransaction(Guid accountId, Guid transactionId)
    {
        var transaction = await _transactionRepository.GetById(transactionId);

        transaction.IsWaitingTransfer();

        await _transactionRepository.SoftDelete(accountId, transaction);
        await _transactionRepository.Save();
    }
}

