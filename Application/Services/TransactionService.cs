using Application.DTOs;
using Application.Helpers;
using Application.Interfaces;
using Application.ViewModels;
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

    public async Task<PaginatedVM> GetAllTransactionsForAccountAsync(Guid accountId, FilterDTO filter)
    {
        var transactions = await _transactionRepository.GetAllTransactionsForAccountAsync(accountId);

        var count = await filter.GetFilter(transactions).CountAsync();

        var filteredTransactions = await filter.GetFilter(transactions)
                .Skip(filter.PageSize * (filter.PageNumber - 1))
                .Take(filter.PageSize)
                .ToListAsync();

        return new PaginatedVM(filteredTransactions, count);
    }

    public async Task<IEnumerable<Transaction>> GetLastTransactionsAsync(Guid accountId)
    {
        var transactions = await _transactionRepository.GetAllTransactionsForAccountAsync(accountId);

        return await transactions
            .OrderByDescending(t => t.CreatedAt)
            .Take(4)
            .ToListAsync();
    }

    public async Task<IEnumerable<Transaction>> GetTransactionsByTimeAsync(Guid accountId, DateTime? From, DateTime? To)
    {
        if (From == null) From = DateTime.Now.Subtract(TimeSpan.FromHours(1D));

        if (To == null) To = DateTime.Now;

        var transactions = await _transactionRepository.GetAllTransactionsForAccountAsync(accountId);
        return await transactions
            .OrderByDescending(t => t.CreatedAt)
            .Where(t => t.CreatedAt > From && t.CreatedAt < To)
            .ToListAsync();
    }

    public async Task<Transaction> GetTransactionDetailsAsync(Guid id)
    {
        return await _transactionRepository.GetTransactionDetalizedAsync(id);
    }

    public async Task<Transaction> CreateDepositAsync(Transaction transaction)
    {
        var account = await _accountRepository.GetByIdAsync(transaction.SourceAccountId);

        account.DoTransaction(transaction);

        var newTran = await _transactionRepository.CreateAsync(transaction);

        await _accountRepository.UpdateAsync(account);
        await _transactionRepository.SaveAsync();

        return newTran;
    }

    public async Task<Transaction> CreateWithdrawalAsync(Transaction transaction)
    {
        Transaction newTran = null; 
        var account = await _accountRepository.GetByIdAsync(transaction.SourceAccountId);
        
        account.DoTransaction(transaction);
        
        newTran = await _transactionRepository.CreateAsync(transaction);

        await _accountRepository.UpdateAsync(account);
        await _transactionRepository.SaveAsync();

        return newTran;
    }

    public async Task<Transaction> CreateTransferAsync(Transaction transaction)
    {
        Transaction newTran = null;

        var sAccount = await _accountRepository.GetByIdAsync(transaction.SourceAccountId);
        var dAccount = await _accountRepository.GetByIdAsync(transaction.DestinationAccountId);

        sAccount.DoTransaction(transaction);
        dAccount.DoTransaction(transaction);

        newTran = await _transactionRepository.CreateAsync(transaction);
        
        await _accountRepository.UpdateAsync(sAccount);
        await _accountRepository.UpdateAsync(dAccount);
        await _transactionRepository.SaveAsync();

        return newTran;
    }

    public async Task<Transaction> UpdateTransactionAsync(Guid accountId, Transaction transaction)
    {
        var transactionToUpdate = await _transactionRepository.GetByIdAsync(transaction.Id);

        transactionToUpdate.IsWaitingTransfer();

        if(transaction.CheckClientTransferStatus(accountId)) 
            transactionToUpdate.DoneTransaction();
        else
        {
            await TransactionHelper.RejectedTransactionBalanceUpdateAsync(transactionToUpdate, _accountRepository);
            transactionToUpdate.RejectTransaction();
        }

        await _transactionRepository.UpdateAsync(transactionToUpdate);
        await _transactionRepository.SaveAsync();
        
        return transactionToUpdate;
    }

    public async Task ExecuteUpdateTransactionAsync(Guid accountId, Transaction transaction)
    {
        var transactionToUpdate = await _transactionRepository.GetByIdAsync(transaction.Id);

        transactionToUpdate.IsWaitingTransfer();

        if (transaction.CheckClientTransferStatus(accountId))
            await _transactionRepository.ExecuteUpdateAsync(
                accountId, 
                transactionToUpdate,
                t => t.StateOfTransaction == StateOfTransaction.Waiting && t.Id == transaction.Id,
                p => p.SetProperty(t => t.StateOfTransaction, StateOfTransaction.Done));
        else
        {
            await _transactionRepository.ExecuteUpdateAsync(
                accountId, 
                transactionToUpdate,
                t => t.StateOfTransaction == StateOfTransaction.Waiting && t.Id == transaction.Id,
                p => p.SetProperty(t => t.StateOfTransaction, StateOfTransaction.Rejected));

            await TransactionHelper.ExecuteUpdateAccountsBalanceAsync(transactionToUpdate, _accountRepository);
        }
    }


    public async Task DeleteTransactionAsync(Guid accountId, Guid transactionId)
    {
        var transaction = await _transactionRepository.GetByIdAsync(transactionId);

        transaction.IsWaitingTransfer();

        await _transactionRepository.SoftDeleteAsync(accountId, transaction);
        await _transactionRepository.SaveAsync();
    }
}

