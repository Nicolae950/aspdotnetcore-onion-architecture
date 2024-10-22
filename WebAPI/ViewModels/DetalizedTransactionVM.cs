using Domain.Entities;
using Domain.Enums;

namespace WebAPI.ViewModels;

public class DetalizedTransactionVM
{
    public Guid Id { get; set; }
    public Guid? SourceAccountId { get; set; }
    public string? SourceFirstName { get; set; }
    public string? SourceLastName { get; set; }
    public decimal Amount { get; set; }
    public StateOfTransaction StateOfTransaction { get; set; }
    public OperationType OperationType { get; set; }
    public string? Description { get; set; }
    public Guid? DestinationAccountId { get; set; }
    public string? DestinationFirstName { get; set; }
    public string? DestinationLastName { get; set; }
    public DateTimeOffset TransactionTime { get; set; }

    public DetalizedTransactionVM(Transaction transaction)
    {
        Id = transaction.Id;
        if(transaction.SourceAccount != null)
        {
            SourceAccountId = transaction.SourceAccountId;
            SourceFirstName = transaction.SourceAccount.FirstName;
            SourceLastName = transaction.SourceAccount.LastName;
        }
        Amount = transaction.Amount;
        StateOfTransaction = transaction.StateOfTransaction;
        OperationType = transaction.OperationType;
        Description = transaction.Description;
        if(transaction.DestinationAccount != null)
        {
            DestinationAccountId = transaction.DestinationAccountId;
            DestinationFirstName = transaction.DestinationAccount.FirstName;
            DestinationLastName = transaction.DestinationAccount.LastName;
        }
        TransactionTime = transaction.CreatedAt;
    }
}
