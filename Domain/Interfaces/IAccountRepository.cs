using Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces;

public interface IAccountRepository : IFullAuditableRepository<Account>
{
    Task<IEnumerable<Account>> GetAllAccountsAsync(Guid id);
    Task<Account> GetAccountAsync(Guid? id);
    Task InactivateEntityAsync(Account account);
}
