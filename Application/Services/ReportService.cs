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
        var accounts = await _accountRepository.GetAllAvailableAsync();
        foreach(var account in accounts)
        {
            int daysInMonth = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
            var fromDate = DateTime.Now.Subtract(TimeSpan.FromDays(daysInMonth));
            var toDate = DateTime.Now;
            await _reportRepository.CreateAsync(new Report(account.Id, fromDate, toDate));
            var count = await _reportRepository.GetReportsCountAsync(account.Id);
            if(count > 2)
                await _reportRepository.HardDeleteAsync(account.Id);
            await _reportRepository.SaveAsync();
        }
    }
}
