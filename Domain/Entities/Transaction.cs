using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enums;
using Domain.Models;
using System.Linq.Expressions;
using Domain.Exceptions;

namespace Domain.Entities;

public class Transaction : FullAuditableEntity
{
    public Guid SourceAccountId { get; private set; }

    public Account SourceAccount { get; private set; }

    public OperationType OperationType { get; private set; }

    public decimal Amount { get; private set; }

    public StateOfTransaction StateOfTransaction { get; private set; }

    public string? Description { get; private set; }

    public Guid? DestinationAccountId { get; private set; }

    public Account? DestinationAccount { get; private set; }

    public Transaction()
    { }

    public Transaction(Guid sourceAccountId, decimal amount,
        StateOfTransaction state, OperationType type, 
        Guid? destinationAccountId, string? description)
    {
        Id = Guid.NewGuid();
        SourceAccountId = sourceAccountId;
        Amount = amount;
        StateOfTransaction = state;
        OperationType = type;
        DestinationAccountId = destinationAccountId;
        Description = description;
    }

    public Transaction(Guid transactionId,
        Guid? sourceAccountId, decimal amount,
        StateOfTransaction? state, OperationType type,
        Guid? destinationAccountId, string? description)
    {
        Id = transactionId;
        SourceAccountId = (Guid)sourceAccountId;
        Amount = amount;
        StateOfTransaction = (StateOfTransaction)state;
        OperationType = type;
        DestinationAccountId = destinationAccountId;
        Description = description;
    }

    public Expression<Func<Transaction, bool>> GetEligibleForDeleteExpression(Guid id)
    {
        return x => (x.OperationType != OperationType.Transfer && x.SourceAccountId == id) ||
                    (x.DestinationAccount.IsDeleted == true &&
                    x.SourceAccount.IsDeleted == true) && (x.SourceAccountId == id || x.DestinationAccountId == id);
    }

    public void IsWaitingTransfer()
    {
        if (!(OperationType == OperationType.Transfer && StateOfTransaction == StateOfTransaction.Waiting))
            throw new WaitingTransferException();
    }

    public bool CheckClientTransferStatus(Guid accountId)
    {
        if (SourceAccountId == accountId)
            throw new Exception("You didn't have permission to change this transaction.");
        return StateOfTransaction == StateOfTransaction.Done && SourceAccountId != accountId;
    }

    public void DoneTransaction()
    {
        StateOfTransaction = StateOfTransaction.Done;
    }

    public void RejectTransaction()
    {
        StateOfTransaction = StateOfTransaction.Rejected;
    }
}

