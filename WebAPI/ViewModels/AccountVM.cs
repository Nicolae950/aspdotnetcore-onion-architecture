using Domain.Entities;

namespace WebAPI.ViewModels;

public class AccountVM
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public decimal Balance { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public IEnumerable<TransactionVM>? TransactionsAsSource { get; set; }
    public IEnumerable<TransactionVM>? TransactionsAsDestination { get; set; }

    public AccountVM(Account account)
    {
        FirstName = account.FirstName;
        LastName = account.LastName;
        Balance = account.Balance;
        CreatedAt = account.CreatedAt;
        TransactionsAsSource = account.TransactionsAsSource != null ? 
            account
            .TransactionsAsSource
            .Select(tran => new TransactionVM(tran)) : null;
        TransactionsAsDestination = account.TransactionsAsDestination != null ?
            account
            .TransactionsAsDestination
            .Select(tran => new TransactionVM(tran)) : null;
    }
}
