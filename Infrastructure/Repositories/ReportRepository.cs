using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories;

public class ReportRepository : BaseRepository<Report>, IReportRepository
{
    public ReportRepository(ApplicationDbContext context) 
        : base(context)
    { }

    public async Task<IEnumerable<Report>> GetAllForAccountAsync(Guid accountId)
    {
        return await _dbSet
            .Where(r => r.AccountId == accountId)
            .ToListAsync();
    }

    public async Task<int> GetReportsCountAsync(Guid accountId)
    {
        return await _dbSet
            .Where(r => r.AccountId == accountId)
            .CountAsync();
    }

    public async Task HardDeleteAsync(Guid accountId)
    {
        var report = _dbSet
            .Where(r => r.AccountId == accountId)
            .OrderBy(r => r.CreatedAt)
            .First();

        await Task.Factory.StartNew(() =>
        {
            _dbSet.Remove(report);
        });
    }
}
