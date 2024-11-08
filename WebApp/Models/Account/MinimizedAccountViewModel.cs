using Domain.Enums;

namespace WebApp.Models.Account;

public class MinimizedAccountViewModel
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public decimal Balance { get; set; }
    public AccountStatus Status { get; set; }
}
