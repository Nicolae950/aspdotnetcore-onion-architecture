using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces;

public interface IAccountService
{
    Task<Account> AddAccount(Account account);
    Task<Account> UpdateAccount(Account account);
    Task InactivateAccount(Guid id);
    Task DeleteAccount(Guid id);
    Task<Account> GetAccountDetails(Guid? id);
}