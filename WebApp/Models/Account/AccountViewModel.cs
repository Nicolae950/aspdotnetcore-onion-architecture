using WebApp.Models.Transaction;

namespace WebApp.Models.Account;

public class AccountViewModel
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public decimal Balance { get; set; }
    public Guid UserId { get; set; }
    public IEnumerable<TransactionViewModel>? Transactions { get; set; }
}
