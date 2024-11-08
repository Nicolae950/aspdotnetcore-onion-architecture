namespace WebAPI.ViewModels.UserModels
{
    public class UserVM
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }

        public UserVM(Guid id, string email, string token)
        {
            Id = id;
            Email = email;
            Token = token;
        }
    }
}
