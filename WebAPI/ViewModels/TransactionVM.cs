using Domain.Entities;
using Domain.Enums;

namespace WebAPI.ViewModels
{
    public class TransactionVM
    {
        public Guid Id { get; set; }
        public decimal Amount { get; set; }
        public OperationType OperationType { get; set; }
        public Guid? DestinationAccountId { get; set; }
        public string? DestinationFirstName { get; set; }
        public string? DestinationLastName { get; set; }

        public TransactionVM(Transaction transaction)
        {
            Id = transaction.Id;
            Amount = transaction.Amount;
            OperationType = transaction.OperationType;
            if (transaction.DestinationAccount != null)
            {
                DestinationAccountId = transaction.DestinationAccountId;
                DestinationFirstName = transaction.DestinationAccount.FirstName;
                DestinationLastName = transaction.DestinationAccount.LastName;
            }
        }
    }
}
