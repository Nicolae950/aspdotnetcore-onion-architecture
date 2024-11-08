using WebApp.Models.Account;

namespace WebApp.Models.User;

public class DetalizedUserViewModel
{
    public Guid Id { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public string Email { get; set; }
    public IEnumerable<MinimizedAccountViewModel> Accounts { get; set; }
}
