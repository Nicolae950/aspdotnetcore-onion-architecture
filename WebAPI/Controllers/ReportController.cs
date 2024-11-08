using Application.Interfaces;
using Application.Services;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using WebAPI.ViewModels;
using WebAPI.ViewModels.AccountModels;
using WebAPI.ViewModels.ReportModels;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class ReportController : Controller
{
    private readonly IReportService _reportService;
    private readonly IAccountService _accountService;
    private readonly ITransactionService _transactionService;
    public ReportController(IReportService reportService, IAccountService accountService, ITransactionService transactionService)
    {
        _reportService = reportService;
        _accountService = accountService;
        _transactionService = transactionService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAllReportsForAccountAsync(Guid id)
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

    [HttpGet("{id}")]
    public async Task<IActionResult> GetReportAsync(Guid id, [FromQuery] DateTime From, [FromQuery] DateTime To)
    {
        try
        {
            var account = new ReportedAccountVM(
                await _accountService.GetAccountDetailsAsync(id),
                await _transactionService.GetTransactionsByTimeAsync(id, From, To));

            return Ok(new StatusVM<ReportedAccountVM>(account));
        }
        catch (Exception ex)
        {
            return BadRequest(new StatusVM<ReportedAccountVM>(ex.Message));
        }
    }
}
