using Microsoft.AspNetCore.Mvc;
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
        //_client.DefaultRequestHeaders.Authorization
    }

    [HttpGet]
    public async Task<IActionResult> Index(Guid accId)
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> DetailsAsync(Guid accId)
    {
        var account = new ApiResponseViewModel<DetalizedAccountViewModel>();
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
        HttpResponseMessage response = await _client.PostAsJsonAsync(_client.BaseAddress
            + $"/CreateAccount", accountDTO);
        response.EnsureSuccessStatusCode();
        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> Update(Guid accId)
    {
        var account = new ApiResponseViewModel<DetalizedAccountViewModel>();
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
        HttpResponseMessage response = await _client.PutAsJsonAsync(_client.BaseAddress
            + $"/UpdateAccount/{detalizedAccount.Id}", accountDTO);

        response.EnsureSuccessStatusCode();
        return RedirectToAction("Details", new { accId = detalizedAccount.Id });
    }
}
