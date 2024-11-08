using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> LoginAsync(User user)
        {
            var findedUser = await _userRepository.GetByEmailAsync(user.Email);
            if (findedUser.Password == user.Password)
                return findedUser;
            else
                return null;
        }

        public async Task RegisterAsync(User user)
        {
            await _userRepository.CreateAsync(user);
            await _userRepository.SaveAsync();
        }

        public async Task<User> GetUserAsync(Guid id)
        {
            return await _userRepository.GetUserAsync(id);
        }
    }
}
