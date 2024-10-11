using Domain.Entities;
using Domain.Enums;

namespace WebAPI.DTOs;

public class TransactionDTO
{
    public Guid? Id { get; set; }
    public Guid? SourceAccountId { get; set; }
    public StateOfTransaction StateOfTransaction { get; set; }
    public OperationType OperationType { get; set; }
    public decimal Amount { get; set; }
    public Guid? DestinationAccountId { get; set; }
    public string? Description { get; set; }

    public Transaction MapDTOToTransaction(Guid sId, StateOfTransaction state, OperationType type)
    {
        return new Transaction(sId, Amount, state, type, DestinationAccountId, Description);
    }

    public Transaction MapDTOToTransactionWithId(Guid id)
    {
        return new Transaction(id, SourceAccountId, Amount, StateOfTransaction, OperationType.Transfer, DestinationAccountId, Description);
    }
}
