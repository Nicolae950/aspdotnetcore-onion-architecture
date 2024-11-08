using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using NuGet.Common;
using System.Net.Http.Headers;
using System.Text;
using WebApp.DTOs;
using WebApp.Helper;
using WebApp.Models;
using WebApp.Models.Account;
using WebApp.Models.User;

namespace WebApp.Controllers;

public class AccountController : Controller
{
    private readonly HttpClient _client;

    public AccountController()
    {
        _client = new HttpClient();
        _client.BaseAddress = new Uri("https://localhost:44316/api/Account");
    }

    [HttpGet]
    public async Task<IActionResult> OverviewAsync(Guid accId)
    {
        ViewBag.userId = accId;
        var accounts = new ApiResponseViewModel<DetalizedUserViewModel>();
        _client.DefaultRequestHeaders.Authorization = LoginExtension.ReturnBearerToken(this);

        string request = _client.BaseAddress + $"/GetAllAccounts/{accId}";

        HttpResponseMessage response = await _client.GetAsync(request);

        if (response.IsSuccessStatusCode)
            accounts = await response.Content.ReadAsAsync<ApiResponseViewModel<DetalizedUserViewModel>>();

        return View(accounts.Data);
    }

    [HttpGet]
    public async Task<IActionResult> IndexAsync(Guid accId)
    {
        var account = new ApiResponseViewModel<AccountViewModel>();

        _client.DefaultRequestHeaders.Authorization = LoginExtension.ReturnBearerToken(this);

        string request = _client.BaseAddress + $"/GetAccountById/{accId}";

        HttpResponseMessage response = await _client.GetAsync(request);
        
        if (response.IsSuccessStatusCode)
            account = await response.Content.ReadAsAsync<ApiResponseViewModel<AccountViewModel>>();

        return View(account.Data);
    }

    [HttpGet]
    public async Task<IActionResult> DetailsAsync(Guid accId)
    {
        var account = new ApiResponseViewModel<DetalizedAccountViewModel>();

        _client.DefaultRequestHeaders.Authorization = LoginExtension.ReturnBearerToken(this);
        
        string request = _client.BaseAddress + $"/Details/{accId}";

        HttpResponseMessage response = await _client.GetAsync(request);
        
        if (response.IsSuccessStatusCode)
            account = await response.Content.ReadAsAsync<ApiResponseViewModel<DetalizedAccountViewModel>>();

        return View(account.Data);
    }

    [HttpGet]
    public IActionResult Create(Guid accId)
    {
        TempData["userId"] = accId;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync(Guid accId, AccountDTO accountDTO)
    {
        var account = new ApiResponseViewModel<AccountViewModel>();

        _client.DefaultRequestHeaders.Authorization = LoginExtension.ReturnBearerToken(this);

        string request = _client.BaseAddress + $"/CreateAccount/{accId}";

        HttpResponseMessage response = await _client.PostAsJsonAsync(request, accountDTO);

        if (response.IsSuccessStatusCode)
            account = await response.Content.ReadAsAsync<ApiResponseViewModel<AccountViewModel>>();

        return RedirectToAction("Index", new { accId = account.Data.Id });
    }

    [HttpGet]
    public async Task<IActionResult> UpdateAsync(Guid accId)
    {
        var account = new ApiResponseViewModel<DetalizedAccountViewModel>();

        _client.DefaultRequestHeaders.Authorization = LoginExtension.ReturnBearerToken(this);

        string request = _client.BaseAddress + $"/Details/{accId}";

        HttpResponseMessage response = await _client.GetAsync(request);
        
        if (response.IsSuccessStatusCode)
            account = await response.Content.ReadAsAsync<ApiResponseViewModel<DetalizedAccountViewModel>>();

        return View(account.Data);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateAsync(DetalizedAccountViewModel detalizedAccount)
    {
        var accountDTO = new AccountDTO();
        accountDTO.FirstName = detalizedAccount.FirstName;
        accountDTO.LastName = detalizedAccount.LastName;

        _client.DefaultRequestHeaders.Authorization = LoginExtension.ReturnBearerToken(this);

        string request = _client.BaseAddress + $"/UpdateAccount/{detalizedAccount.Id}";

        HttpResponseMessage response = await _client.PutAsJsonAsync(request, accountDTO);

        response.EnsureSuccessStatusCode();

        return RedirectToAction("Details", new { accId = detalizedAccount.Id });
    }
}
