using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using WebApp.DTOs;
using WebApp.Helper;
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
    public async Task<IActionResult> IndexAsync(Guid accId, [FromQuery]PaginatedDTO paginated)
    {
        var transactions = new ApiResponseViewModel<PaginatedViewModel>();

        _client.DefaultRequestHeaders.Authorization = LoginExtension.ReturnBearerToken(this);

        var path = $"/GetAllTransactions/{accId}?PageNumber={paginated.PageNumber}&PageSize={paginated.PageSize}";
        HttpResponseMessage response = await _client.GetAsync(_client.BaseAddress + path);

        if (response.IsSuccessStatusCode)
        {
            transactions = await response.Content.ReadAsAsync<ApiResponseViewModel<PaginatedViewModel>>();
        }

        TempData["PageNumber"] = paginated.PageNumber;
        TempData["PageSize"] = paginated.PageSize;

        ViewBag.accId = accId;
        ViewBag.CurrentPage = paginated.PageNumber;
        ViewBag.Previous = paginated.PageNumber > 1 ? paginated.PageNumber - 1 : paginated.PageNumber;
        ViewBag.Next = paginated.PageNumber <= transactions.Data.TotalTransactions/paginated.PageSize ? paginated.PageNumber + 1 : paginated.PageNumber;
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
        _client.DefaultRequestHeaders.Authorization = LoginExtension.ReturnBearerToken(this);

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
        _client.DefaultRequestHeaders.Authorization = LoginExtension.ReturnBearerToken(this);

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
        _client.DefaultRequestHeaders.Authorization = LoginExtension.ReturnBearerToken(this);

        HttpResponseMessage response = await _client.PostAsJsonAsync(_client.BaseAddress //serializarea
            + $"/CreateTransfer/{accId}", transactionDTO);
        response.EnsureSuccessStatusCode();

        return RedirectToAction("Index", "Account", new { accId = accId });
    }

    [HttpGet]
    public async Task<IActionResult> DetailsAsync(Guid accId, Guid tranId)
    {
        _client.DefaultRequestHeaders.Authorization = LoginExtension.ReturnBearerToken(this);

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
        _client.DefaultRequestHeaders.Authorization = LoginExtension.ReturnBearerToken(this);

        HttpResponseMessage response = await _client.PutAsJsonAsync(_client.BaseAddress +
            $"/UpdateTransfer/{accId}/{transaction.Id}", transaction);

        var api_response = new ApiResponseViewModel<TransactionDetailsViewModel>();

        api_response = await response.Content.ReadAsAsync<ApiResponseViewModel<TransactionDetailsViewModel>>();

        //if (response.IsSuccessStatusCode)
        //{
        //    api_response = await response.Content.ReadAsAsync<ApiResponseViewModel<TransactionDetailsViewModel>>();
        //}
        TempData["message"] = api_response.Message;
        return RedirectToAction("Details", "Transaction", new { accId = transaction.DestinationAccountId, tranId = transaction.Id });
    }
}
