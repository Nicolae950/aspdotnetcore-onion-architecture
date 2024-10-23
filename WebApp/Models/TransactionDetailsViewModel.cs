using Domain.Enums;

namespace WebApp.Models
{
    public class TransactionDetailsViewModel
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
    }
}
