using Microsoft.AspNetCore.Mvc;
using WebApp.Models;
using WebApp.Models.Report;

namespace WebApp.Controllers;

public class ReportController:Controller
{
    private readonly HttpClient _client;

    public ReportController()
    {
        _client = new HttpClient();
        _client.BaseAddress = new Uri("https://localhost:44316/api/Report");
    }

    [HttpGet]
    public async Task<IActionResult> GetAllReportsForAccountAsync(Guid accId)
    {
        var reports = new ApiResponseViewModel<ReportViewModel>();

        HttpResponseMessage response = await _client.GetAsync(_client.BaseAddress
            + $"/GetAllReportsForAccount/{accId}");

        if (response.IsSuccessStatusCode)
        {
            reports = await response.Content.ReadAsAsync<ApiResponseViewModel<ReportViewModel>>();
        }

        return View(reports.Data);
    }
}
