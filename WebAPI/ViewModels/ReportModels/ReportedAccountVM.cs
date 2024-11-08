using Domain.Entities;
using WebAPI.ViewModels.TransactionModels;

namespace WebAPI.ViewModels.ReportModels
{
    public class ReportedAccountVM
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public decimal Balance { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public Guid UserId { get; set; }
        public IEnumerable<TransactionVM>? Transactions { get; set; }

        public ReportedAccountVM(Account account, IEnumerable<Transaction>? transactions)
        {
            Id = account.Id;
            FirstName = account.FirstName;
            LastName = account.LastName;
            Balance = account.Balance;
            UserId = account.UserId;
            CreatedAt = account.CreatedAt;
            Transactions = transactions?.Select(x => new TransactionVM(x));
        }
    }
}
