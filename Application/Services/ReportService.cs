using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services;

public class ReportService : IReportService
{
    private readonly IReportRepository _reportRepository;
    private readonly IAccountRepository _accountRepository;
    public ReportService(IReportRepository reportRepository, IAccountRepository accountRepository)
    {
        _reportRepository = reportRepository;
        _accountRepository = accountRepository;
    }

    public async Task CreateReportForAccountAsync(Report report)
    {
        await _reportRepository.CreateAsync(report);
        await _reportRepository.SaveAsync();
    }

    public async Task DeleteLastReportForAccountAsync(Guid accountId)
    {
        if(await _reportRepository.GetReportsCountAsync(accountId) > 4)
            await _reportRepository.HardDeleteAsync(accountId);

        await _reportRepository.SaveAsync();
    }

    public async Task<IEnumerable<Report>> GetAllReportsForAccountAsync(Guid accountId)
    {
        var reports = await _reportRepository.GetAllForAccountAsync(accountId);
        return reports;
    }

    public async Task CreateReportsForAllAsync()
    {
        var accounts = await _accountRepository.GetAllAsync(Guid.Empty);
        foreach(var account in accounts)
        {
            var fromDate = DateTime.Now.Subtract(TimeSpan.FromHours(1D));
            var toDate = DateTime.Now;
            await _reportRepository.CreateAsync(new Report(account.Id, fromDate, toDate));
            await _reportRepository.SaveAsync();
        }
    }
}
