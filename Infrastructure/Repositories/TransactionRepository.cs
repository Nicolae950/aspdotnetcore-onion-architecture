using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories;

public class TransactionRepository : FullAuditableRepository<Transaction>, ITransactionRepository
{
    public TransactionRepository(ApplicationDbContext context) 
        : base(context)
    { }

    public async Task<IQueryable<Transaction>> GetAllTransactionsForAccountAsync(Guid id)
    {
        var transactions = await Task<IQueryable<Transaction>>.Factory.StartNew(t =>
        {
            return _dbSet
            .Include(t => t.SourceAccount)
            .Include(t => t.DestinationAccount)
            .Where(t => t.SourceAccountId == id || t.DestinationAccountId == id)
            .AsNoTracking()
            .AsQueryable();
        }, id);

        return transactions;
    }

    public async Task<Transaction> GetTransactionDetalizedAsync(Guid id)
    {
        var transaction = await _dbSet
            .Include(t => t.SourceAccount)
            .Include(t => t.DestinationAccount)
            .AsNoTracking()
            .FirstAsync(t => t.Id == id);
        return transaction;
    }
}
