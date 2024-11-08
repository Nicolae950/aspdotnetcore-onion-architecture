using WebApp.Models.Transaction;

namespace WebApp.Models.Report
{
    public class ReportedAccountViewModel
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public decimal Balance { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public Guid UserId { get; set; }
        public IEnumerable<TransactionViewModel>? Transactions { get; set; }
    }
}
