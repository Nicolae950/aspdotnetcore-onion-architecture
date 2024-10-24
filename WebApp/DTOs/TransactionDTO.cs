using Domain.Enums;

namespace WebApp.DTOs
{
    public class TransactionDTO
    {
        public OperationType OperationType { get; set; } // drop down -> daca e transfer va permite si adaugarea celui 1 camp de id
        public decimal Amount { get; set; }
        public string? Description { get; set; }
        public Guid? DestinationAccountId { get; set; }
    }
}
