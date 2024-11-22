using Microsoft.AspNetCore.Mvc;

using IronPdf;
using IronPdf.Extensions.Mvc.Core;

using System.Text;
using WebApp.Helper;
using WebApp.Models;
using WebApp.Models.Account;
using WebApp.Models.Report;
using Razor.Templating.Core;
using Rotativa.AspNetCore;

//using Rotativa.AspNetCore;

namespace WebApp.Controllers;

public class ReportController:Controller
{
    private readonly HttpClient _client;
    private readonly IWebHostEnvironment _environment;

    public ReportController(IWebHostEnvironment environment)
    {
        _client = new HttpClient();
        _client.BaseAddress = new Uri("https://localhost:44316/api/Report");
        _environment = environment;
    }

    [HttpGet]
    public async Task<IActionResult> IndexAsync(Guid accId)
    {
        var reports = new ApiResponseViewModel<IEnumerable<ReportViewModel>>();

        TempData["accId"] = accId;

        string request = _client.BaseAddress + $"/GetAllReportsForAccount/{accId}";

        HttpResponseMessage response = await _client.GetAsync(request);

        if (response.IsSuccessStatusCode)
            reports = await response.Content.ReadAsAsync<ApiResponseViewModel<IEnumerable<ReportViewModel>>>();

        return View(reports.Data);
    }

    [HttpGet]
    public async Task<IActionResult> ReportAsync(Guid accId, [FromQuery] DateTime From, [FromQuery] DateTime To)
    {
        var report = new ApiResponseViewModel<ReportedAccountViewModel>();
        
        _client.DefaultRequestHeaders.Authorization = LoginExtension.ReturnBearerToken(this);

        string request = _client.BaseAddress + $"/GetReport/{accId}?From={From}&To={To}";

        HttpResponseMessage response = await _client.GetAsync(request);

        if (response.IsSuccessStatusCode)
            report = await response.Content.ReadAsAsync<ApiResponseViewModel<ReportedAccountViewModel>>();
        
        var createdDate = DateTime.Now;
        string fileName = $"Report_{accId}_{createdDate.Year}{createdDate.Month}{createdDate.Day}{createdDate.Hour}{createdDate.Minute}{createdDate.Second}.pdf";

        // string html = await RazorTemplateEngine.RenderAsync("Views/Report/Report.cshtml", report.Data);
        // ChromePdfRenderer renderer = new ChromePdfRenderer();
        // var pdf = renderer.RenderHtmlAsPdf(html);
        //Response.Headers.Add("Content-Disposition", "inline");

        return new ViewAsPdf("Report", report.Data)
        {
            FileName = fileName
        };

        //return View(report.Data);
    }
}
