using WebApp.Models.Transaction;

namespace WebApp.Models;

public class PaginatedViewModel
{
    public IEnumerable<TransactionViewModel> Transactions { get; set; }
    public int TotalTransactions { get; set; }
}
