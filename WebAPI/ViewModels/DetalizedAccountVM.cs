using Domain.Entities;
using Domain.Enums;

namespace WebAPI.ViewModels;

public class DetalizedAccountVM
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public decimal Balance { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public AccountStatus Status { get; set; }
    public DetalizedAccountVM(Account account)
    {
        Id = account.Id;
        FirstName = account.FirstName;
        LastName = account.LastName;
        Balance = account.Balance;
        CreatedAt = account.CreatedAt;
        Status = account.Status;
    }
}
