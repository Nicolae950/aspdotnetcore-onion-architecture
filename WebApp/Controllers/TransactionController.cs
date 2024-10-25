using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using WebApp.DTOs;
using WebApp.Models;
using WebApp.Models.Transaction;

namespace WebApp.Controllers;

public class TransactionController : Controller
{
    private readonly HttpClient _client;
    public TransactionController()
    {
        _client = new HttpClient();
        _client.BaseAddress = new Uri("https://localhost:44316/api/Transaction");
    }
    [HttpGet]
    public async Task<IActionResult> IndexAsync(Guid accId, [FromQuery]PaginatedViewModel paginated)
    {
        var transactions = new ApiResponseViewModel<List<TransactionViewModel>>();

        var token = "";
        HttpContext.Request.Cookies.TryGetValue("token", out token);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var path = $"/GetAllTransactions/{accId}?PageNumber={paginated.PageNumber}&PageSize={paginated.PageSize}";
        HttpResponseMessage response = await _client.GetAsync(_client.BaseAddress + path);

        if (response.IsSuccessStatusCode)
        {
            transactions = await response.Content.ReadAsAsync<ApiResponseViewModel<List<TransactionViewModel>>>();
        }
        ViewData["accId"] = accId;
        return View(transactions.Data);
    }

    [HttpGet]
    public IActionResult Deposit(Guid accId)
    {
        ViewData["accId"] = accId;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> DepositAsync(Guid accId, TransactionDTO transactionDTO)
    {
        var token = "";
        HttpContext.Request.Cookies.TryGetValue("token", out token);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        HttpResponseMessage response = await _client.PostAsJsonAsync(_client.BaseAddress //serializarea
            + $"/CreateDeposit/{accId}", transactionDTO);
        response.EnsureSuccessStatusCode();

        return RedirectToAction("Index", "Account", new { accId = accId});
    }

    [HttpGet]
    public IActionResult Withdraw(Guid accId)
    {
        ViewData["accId"] = accId;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> WithdrawAsync(Guid accId, TransactionDTO transactionDTO)
    {
        var token = "";
        HttpContext.Request.Cookies.TryGetValue("token", out token);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        HttpResponseMessage response = await _client.PostAsJsonAsync(_client.BaseAddress //serializarea
            + $"/CreateWithdrawal/{accId}", transactionDTO);
        response.EnsureSuccessStatusCode();

        return RedirectToAction("Index", "Account", new { accId = accId });
    }

    [HttpGet]
    public IActionResult Transfer(Guid accId)
    {
        ViewData["accId"] = accId;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> TransferAsync(Guid accId, TransactionDTO transactionDTO)
    {
        var token = "";
        HttpContext.Request.Cookies.TryGetValue("token", out token);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        HttpResponseMessage response = await _client.PostAsJsonAsync(_client.BaseAddress //serializarea
            + $"/CreateTransfer/{accId}", transactionDTO);
        response.EnsureSuccessStatusCode();

        return RedirectToAction("Index", "Account", new { accId = accId });
    }

    [HttpGet]
    public async Task<IActionResult> DetailsAsync(Guid accId, Guid tranId)
    {
        var token = "";
        HttpContext.Request.Cookies.TryGetValue("token", out token);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var transaction = new ApiResponseViewModel<TransactionDetailsViewModel>();
        var path = $"/GetTransactionDetails/{accId}/{tranId}";
        HttpResponseMessage response = await _client.GetAsync(_client.BaseAddress + path);
        
        if (response.IsSuccessStatusCode)
            transaction = await response.Content.ReadAsAsync<ApiResponseViewModel<TransactionDetailsViewModel>>(); // deserializare
        ViewData["accId"] = accId;
        return View(transaction.Data);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateAsync(Guid accId, TransactionDetailsViewModel transaction)
    {
        var token = "";
        HttpContext.Request.Cookies.TryGetValue("token", out token);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        HttpResponseMessage response = await _client.PutAsJsonAsync(_client.BaseAddress +
            $"/UpdateTransfer/{accId}/{transaction.Id}", transaction);
        response.EnsureSuccessStatusCode();

        return RedirectToAction("Details", new { id = transaction.DestinationAccountId, tranId = transaction.Id});
    }
}
