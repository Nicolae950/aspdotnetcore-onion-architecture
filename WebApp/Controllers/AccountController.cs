using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using NuGet.Common;
using System.Net.Http.Headers;
using System.Text;
using WebApp.DTOs;
using WebApp.Helper;
using WebApp.Models;
using WebApp.Models.Account;

namespace WebApp.Controllers;

public class AccountController : Controller
{
    private readonly HttpClient _client;
    private readonly IWebHostEnvironment _environment;

    public AccountController(IWebHostEnvironment environment)
    {
        _client = new HttpClient();
        _client.BaseAddress = new Uri("https://localhost:44316/api/Account");
        _environment = environment;
    }

    [HttpGet]
    public async Task<IActionResult> OverviewAsync(Guid accId)
    {
        ViewBag.userId = accId;
        var accounts = new ApiResponseViewModel<IEnumerable<MinimizedAccountViewModel>>();
        _client.DefaultRequestHeaders.Authorization = LoginExtension.ReturnBearerToken(this);

        HttpResponseMessage response = await _client.GetAsync(_client.BaseAddress
            + $"/GetAllAccounts/{accId}");

        if (response.IsSuccessStatusCode)
        {
            accounts = await response.Content.ReadAsAsync<ApiResponseViewModel<IEnumerable<MinimizedAccountViewModel>>>();
        }

        return View(accounts.Data);
    }

    [HttpGet]
    public async Task<IActionResult> IndexAsync(Guid accId)
    {

        var account = new ApiResponseViewModel<AccountViewModel>();

        _client.DefaultRequestHeaders.Authorization = LoginExtension.ReturnBearerToken(this);

        HttpResponseMessage response = await _client.GetAsync(_client.BaseAddress
            + $"/GetAccountById/{accId}");
        
        if (response.IsSuccessStatusCode)
        {
            account = await response.Content.ReadAsAsync<ApiResponseViewModel<AccountViewModel>>();
        }
        return View(account.Data);
    }

    [HttpGet]
    public async Task<IActionResult> DetailsAsync(Guid accId)
    {
        var account = new ApiResponseViewModel<DetalizedAccountViewModel>();

        _client.DefaultRequestHeaders.Authorization = LoginExtension.ReturnBearerToken(this);

        HttpResponseMessage response = await _client.GetAsync(_client.BaseAddress
            + $"/Details/{accId}");
        if (response.IsSuccessStatusCode)
        {
            account = await response.Content.ReadAsAsync<ApiResponseViewModel<DetalizedAccountViewModel>>();
        }

        return View(account.Data);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync(AccountDTO accountDTO)
    {
        var account = new ApiResponseViewModel<AccountViewModel>();

        _client.DefaultRequestHeaders.Authorization = LoginExtension.ReturnBearerToken(this);

        HttpResponseMessage response = await _client.PostAsJsonAsync(_client.BaseAddress
            + $"/CreateAccount", accountDTO);

        if (response.IsSuccessStatusCode)
        {
            account = await response.Content.ReadAsAsync<ApiResponseViewModel<AccountViewModel>>();
        }

        return RedirectToAction("Index", new { accId = account.Data.Id });
    }

    [HttpGet]
    public async Task<IActionResult> UpdateAsync(Guid accId)
    {
        var account = new ApiResponseViewModel<DetalizedAccountViewModel>();

        _client.DefaultRequestHeaders.Authorization = LoginExtension.ReturnBearerToken(this);

        HttpResponseMessage response = await _client.GetAsync(_client.BaseAddress
            + $"/Details/{accId}");
        if (response.IsSuccessStatusCode)
        {
            account = await response.Content.ReadAsAsync<ApiResponseViewModel<DetalizedAccountViewModel>>();
        }

        return View(account.Data);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateAsync(DetalizedAccountViewModel detalizedAccount)
    {
        var accountDTO = new AccountDTO();
        accountDTO.FirstName = detalizedAccount.FirstName;
        accountDTO.LastName = detalizedAccount.LastName;

        _client.DefaultRequestHeaders.Authorization = LoginExtension.ReturnBearerToken(this);

        HttpResponseMessage response = await _client.PutAsJsonAsync(_client.BaseAddress
            + $"/UpdateAccount/{detalizedAccount.Id}", accountDTO);

        response.EnsureSuccessStatusCode();
        return RedirectToAction("Details", new { accId = detalizedAccount.Id });
    }

    [HttpGet]
    public async Task<FileResult> ReportAsync(Guid accId, [FromQuery]DateTime From, [FromQuery]DateTime To)
    {
        var report = new ApiResponseViewModel<AccountViewModel>();
        _client.DefaultRequestHeaders.Authorization = LoginExtension.ReturnBearerToken(this);

        HttpResponseMessage response = await _client.GetAsync(_client.BaseAddress
            + $"/GetReport/{accId}?From={From}&To={To}");

        if (response.IsSuccessStatusCode)
        {
            report = await response.Content.ReadAsAsync<ApiResponseViewModel<AccountViewModel>>();
        }
        var createdDate = DateTime.Now;
        string fileName = $"Report_{accId}_{createdDate.Year}{createdDate.Month}{createdDate.Day}{createdDate.Hour}{createdDate.Minute}{createdDate.Second}.pdf";
        string path = Path.Combine(_environment.ContentRootPath, $"Files\\{fileName}");
            
        FileStream document = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite);
        var pdfdoc = FileExtension.GenerateReportDocument(report.Data, document);

        return File(document, "application/pdf", path);
    }
}
