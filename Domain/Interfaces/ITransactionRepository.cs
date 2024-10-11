using Domain.Entities;
using System.Linq.Expressions;

namespace Domain.Interfaces;

public interface ITransactionRepository : IFullAuditableRepository<Transaction>
{
    Task<IQueryable<Transaction>> GetAllTransactionsForAccount(Guid id);
    Task<Transaction> GetTransactionDetalized(Guid id);
}
