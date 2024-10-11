using Application.DTOs;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces;

public interface ITransactionService
{
    Task<IEnumerable<Transaction>> GetAllTransactionsForAccount(Guid accountId, FilterDTO filter);
    Task<Transaction> GetTransactionDetails(Guid id);
    Task<Transaction> CreateDeposit(Transaction transaction);
    Task<Transaction> CreateWithdrawal(Transaction transaction);
    Task<Transaction> CreateTransfer(Transaction transaction);
    Task<Transaction> UpdateTransaction(Guid accountId, Transaction transaction);
    Task ExecuteUpdateTransaction(Guid accountId, Transaction transaction);
    //Task DeleteTransaction(Guid accountId, Guid transactionId);
}
