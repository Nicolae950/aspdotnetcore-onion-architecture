using Domain.Entities;
using Domain.Enums;

namespace WebAPI.ViewModels.AccountModels;

public class MinimizedAccountVM
{
    public decimal Balance { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public AccountStatus Status { get; set; }
    public Guid Id { get; set; }

    public MinimizedAccountVM(Account account)
    {
        FirstName = account.FirstName;
        LastName = account.LastName;
        Balance = account.Balance;
        Status = account.Status;
        Id = account.Id;
    }
}
