using Domain.Entities;
using System.Linq.Expressions;

namespace Domain.Interfaces;

public interface ITransactionRepository : IFullAuditableRepository<Transaction>
{
    Task<IQueryable<Transaction>> GetAllTransactionsForAccountAsync(Guid id);
    Task<Transaction> GetTransactionDetalizedAsync(Guid id);
}
