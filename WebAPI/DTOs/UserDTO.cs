using Domain.Entities;

namespace WebAPI.DTOs
{
    public class UserDTO
    {
        public required string Email { get; set; }
        public required string Password { get; set; }

        public User MapDTOToUser()
        {
            return new User(Email, Password); 
        }
    }
}
