using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces;

public interface IReportService
{
    Task<IEnumerable<Report>> GetAllReportsForAccountAsync(Guid accountId);
    Task CreateReportsForAllAsync();
    Task CreateReportForAccountAsync(Report report);
    Task DeleteLastReportForAccountAsync(Guid accountId);
}
