﻿using Application.DTOs;
using Application.ViewModels;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces;

public interface ITransactionService
{
    Task<PaginatedVM> GetAllTransactionsForAccountAsync(Guid accountId, FilterDTO filter);
    Task<IEnumerable<Transaction>> GetLastTransactionsAsync(Guid accountId);
    Task<IEnumerable<Transaction>> GetTransactionsByTimeAsync(Guid accountId, DateTime? From, DateTime? To);
    Task<Transaction> GetTransactionDetailsAsync(Guid id);
    Task<Transaction> CreateDepositAsync(Transaction transaction);
    Task<Transaction> CreateWithdrawalAsync(Transaction transaction);
    Task<Transaction> CreateTransferAsync(Transaction transaction);
    Task<Transaction> UpdateTransactionAsync(Guid accountId, Transaction transaction);
    Task ExecuteUpdateTransactionAsync(Guid accountId, Transaction transaction);
    //Task DeleteTransaction(Guid accountId, Guid transactionId);
}
