using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces;

public interface IAccountService
{
    Task<Account> AddAccountAsync(Account account);
    Task<Account> UpdateAccountAsync(Account account);
    Task InactivateAccountAsync(Guid id);
    Task DeleteAccountAsync(Guid id);
    Task<Account> GetAccountDetailsAsync(Guid? id);
}