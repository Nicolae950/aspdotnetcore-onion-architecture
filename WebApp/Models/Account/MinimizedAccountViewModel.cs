using Domain.Enums;

namespace WebApp.Models.Account;

public class MinimizedAccountViewModel
{
    public decimal Balance { get; set; }
    public AccountStatus Status { get; set; }
    public Guid Id { get; set; }
}
