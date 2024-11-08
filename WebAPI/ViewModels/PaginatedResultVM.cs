using WebAPI.ViewModels.TransactionModels;

namespace WebAPI.ViewModels;

public class PaginatedResultVM
{
    public IEnumerable<TransactionVM> Transactions { get; set; }
    public int TotalTransactions { get; set; }

    public PaginatedResultVM(IEnumerable<TransactionVM> transactions, int totalTransactions)
    {
        Transactions = transactions;
        TotalTransactions = totalTransactions;
    }
}
