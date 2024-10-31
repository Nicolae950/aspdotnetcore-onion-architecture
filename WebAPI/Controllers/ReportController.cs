using Application.Interfaces;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using WebAPI.ViewModels;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class ReportController : Controller
{
    private readonly IReportService _reportService;
    public ReportController(IReportService reportService, IRecurringJobManager recurringJob)
    {
        _reportService = reportService;
        recurringJob.AddOrUpdate("reportsJob", () => Console.WriteLine("report time"), Cron.Minutely);
        //RecurringJob.AddOrUpdate("reportsJob", () => _reportService.CreateReportsForAllAsync(), Cron.Minutely);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAllReportForAccountAsync(Guid id)
    {
        try
        {
            var reports = await _reportService.GetAllReportsForAccountAsync(id);
            var reportsVM = reports
                .Select(r => new ReportVM(r));
            return Ok(new StatusVM<IEnumerable<ReportVM>>(reportsVM));
        }catch(Exception ex)
        {
            return BadRequest(new StatusVM<IEnumerable<ReportVM>>(ex.Message));
        }
    }

    public void StartingReportJob(IRecurringJobManager recurringJob)
    {
        recurringJob.AddOrUpdate("report-job", () => Console.WriteLine("Report Time"), Cron.Minutely);
    }
}
