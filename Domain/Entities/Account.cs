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

    public void DoTransaction(Transaction transaction)
    {
        switch (transaction.OperationType)
        {
            case OperationType.Deposit:
                DoDeposit(transaction);
                break;
            case OperationType.Withdrawal:
                DoWithdrawal(transaction);
                break;
            case OperationType.Transfer:
                DoTransfer(transaction); 
                break;
        }
    }

    protected void DoDeposit(Transaction transaction)
    {
        CheckAccountStatus();
        Balance += transaction.Amount;
        if(Status != AccountStatus.Active)
            Status = AccountStatus.Active;
    }

    protected void DoWithdrawal(Transaction transaction)
    {
        CheckEligibleForWithdrawal(transaction.Amount);
        Balance -= transaction.Amount;
    }

    protected void DoTransfer(Transaction transaction)
    {
        if (transaction.SourceAccountId == this.Id)
        {
            CheckAccountStatus();
            CheckEligibleForWithdrawal(transaction.Amount);
            Balance -= transaction.Amount;
        }
        if (transaction.DestinationAccountId == this.Id)
            Balance += transaction.Amount;
    }

    public void ReverseTransfer(Transaction transaction)
    {
        if (transaction.DestinationAccountId == this.Id)
            Balance -= transaction.Amount;
        if (transaction.SourceAccountId == this.Id)
            Balance += transaction.Amount;
    }

    public void ChangeAccountName(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }

    public void InactivateAccount()
    {
        Status = AccountStatus.Inactive;
    }

    public void CheckEligibleForWithdrawal(decimal amount)
    {
        if (Status == AccountStatus.Active)
            CheckEnoughBalance(amount);
        else 
            throw new ApplicationException("Account is not active! Please do a Deposit operation to activate account!");
    }

    public void CheckEnoughBalance(decimal amount)
    {
        if (amount > Balance)
            throw new NotMoneyException();
    }

    public bool CheckDeleteBalance()
    {
        if (Balance == 0)
            return true;
        else
            throw new Exception("Cannot delete an account with balance!");
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

