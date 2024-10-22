using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories;

public class UserRepository : BaseRepository<User>, IUserRepository
{ 
    public UserRepository(ApplicationDbContext applicationDbContext) 
        : base(applicationDbContext)
    { }

    public async Task<User> GetByEmailAsync(string email)
    {
        return await _dbSet
            .AsNoTracking()
            .Where(x => x.Email.Equals(email))
            .SingleAsync();
    }
}
