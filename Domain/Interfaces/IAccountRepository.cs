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
    Task<Account> GetAccountWithTransactions(Guid? id);
    Task InactivateEntity(Account account);
    Task ActivateAccount(Account account);
}
