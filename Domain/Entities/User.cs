using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class User : BaseEntity
    {
        public string Email { get; private set; }
        public string Password { get; private set; }

        public User() { }

        public User(string email, string password)
        {
            Email = email;
            byte[] bytes = Encoding.UTF8.GetBytes(password);
            byte[] hashValue = SHA256.HashData(bytes);
            Password = Convert.ToHexString(hashValue);
        }

        public User(Guid id, string email, string password)
        {
            Id = id;
            Email = email;
            Password = password;
        }
    }
}
