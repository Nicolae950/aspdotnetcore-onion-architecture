using Domain.Entities;
using Domain.Enums;

namespace WebAPI.ViewModels;

public class MinimizedAccountVM
{
    public decimal Balance { get; set; }
    public AccountStatus Status { get; set; }
    public Guid Id { get; set; }

    public MinimizedAccountVM(Account account)
    {
        Balance = account.Balance;
        Status = account.Status;
        Id = account.Id;
    }
}
