using Domain.Entities;
using WebAPI.ViewModels.AccountModels;

namespace WebAPI.ViewModels.UserModels;

public class DetalizedUserVM
{
    public Guid Id { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public string Email { get; set; }
    public IEnumerable<MinimizedAccountVM>? Accounts { get; set; }

    public DetalizedUserVM(User user, IEnumerable<Account>? accounts)
    {
        Id = user.Id;
        CreatedAt = user.CreatedAt;
        Email = user.Email;
        Accounts = accounts?.Select(x => new MinimizedAccountVM(x));
    }
}
