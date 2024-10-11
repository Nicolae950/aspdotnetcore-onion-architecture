using Domain.Enums;
using Domain.Exceptions;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Domain.Entities;

public class Account : FullAuditableEntity
{
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public decimal Balance { get; private set; }
    public ICollection<Transaction> TransactionsAsSource { get; private set; }
    public ICollection<Transaction> TransactionsAsDestination { get; private set; }
    public AccountStatus Status { get; private set; }

    public Account()
    { }
    public Account(Guid id, string firstName, string lastName)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
    }
    public Account(string firstName, string lastName)
    {
        Id = Guid.NewGuid();
        FirstName = firstName;
        LastName = lastName;
        Balance = 0;
        Status = AccountStatus.None;
    }

    public void DoTransaction(Transaction transaction, string operation)
    {

    }

    public void ChangeAccountName(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }

    public void GhangeAccountStatus(AccountStatus status)
    {
        Status = status;
    }

    public void CheckEligibleForWithdrawal(decimal amount)
    {
        if (Status == AccountStatus.Active)
            CheckMoneyBalance(amount);
        else 
            throw new ApplicationException("Account is not active! Please do a Deposit operation to activate account!");
    }

    public void CheckMoneyBalance(decimal amount)
    {
        if (amount > Balance)
            throw new NotMoneyException();
    }

    public void CheckAccountStatus()
    {
        if (this == null)
            throw new NotFoundException<Account>();

        if (Status == AccountStatus.Inactive)
            throw new ApplicationException("This account is inactive, please contact us for more information!");

        if (IsDeleted == true)
            throw new NotFoundException<Account>();
    }
}

