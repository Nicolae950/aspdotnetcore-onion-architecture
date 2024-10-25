using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using NuGet.Common;
using System.Net.Http.Headers;
using System.Text;
using WebApp.DTOs;
using WebApp.Models;
using WebApp.Models.Account;

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
    public async Task<IActionResult> IndexAsync(Guid accId)
    {
        var account = new ApiResponseViewModel<AccountViewModel>();

        var token = "";
        HttpContext.Request.Cookies.TryGetValue("token", out token);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        
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

        var token = "";
        HttpContext.Request.Cookies.TryGetValue("token", out token);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

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

        var token = "";
        HttpContext.Request.Cookies.TryGetValue("token", out token);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        HttpResponseMessage response = await _client.PostAsJsonAsync(_client.BaseAddress
            + $"/CreateAccount", accountDTO);

        if (response.IsSuccessStatusCode)
        {
            account = await response.Content.ReadAsAsync<ApiResponseViewModel<AccountViewModel>>();
        }

        return RedirectToAction("Index", new { accId = account.Data.Id });
    }

    [HttpGet]
    public async Task<IActionResult> Update(Guid accId)
    {
        var account = new ApiResponseViewModel<DetalizedAccountViewModel>();

        var token = "";
        HttpContext.Request.Cookies.TryGetValue("token", out token);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

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

        var token = "";
        HttpContext.Request.Cookies.TryGetValue("token", out token);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        HttpResponseMessage response = await _client.PutAsJsonAsync(_client.BaseAddress
            + $"/UpdateAccount/{detalizedAccount.Id}", accountDTO);

        response.EnsureSuccessStatusCode();
        return RedirectToAction("Details", new { accId = detalizedAccount.Id });
    }
}
