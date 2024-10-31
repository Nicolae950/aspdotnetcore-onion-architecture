using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IReportRepository : IBaseRepository<Report>
    {
        Task<IEnumerable<Report>> GetAllForAccountAsync(Guid accountId);
        Task<int> GetReportsCountAsync(Guid accountId);
        Task HardDeleteAsync(Guid accountId);
    }
}
