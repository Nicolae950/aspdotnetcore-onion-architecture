﻿using Domain.Entities;
using WebAPI.ViewModels.TransactionModels;

namespace WebAPI.ViewModels.AccountModels
{
    public class AccountTransactionsVM
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public decimal Balance { get; set; }
        public Guid UserId { get; set; }
        public IEnumerable<TransactionVM>? Transactions { get; set; }

        public AccountTransactionsVM(Account account, IEnumerable<Transaction>? transactions)
        {
            Id = account.Id;
            FirstName = account.FirstName;
            LastName = account.LastName;
            Balance = account.Balance;
            UserId = account.UserId;
            Transactions = transactions?.Select(x => new TransactionVM(x));
        }
    }
}
