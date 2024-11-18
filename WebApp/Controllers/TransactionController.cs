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
    public async Task<IActionResult> IndexAsync(Guid accId, [FromQuery] FilterDTO paginated)
    {
        var transactions = new ApiResponseViewModel<PaginatedViewModel>();

        _client.DefaultRequestHeaders.Authorization = LoginExtension.ReturnBearerToken(this);

        string request = _client.BaseAddress
            + $"/GetAllTransactions/{accId}";

        HttpResponseMessage response = await _client.GetAsync(request);

        if (response.IsSuccessStatusCode)
            transactions = await response.Content.ReadAsAsync<ApiResponseViewModel<PaginatedViewModel>>();

        ViewBag.accId = accId;

        return View(transactions.Data);
    }

    [HttpPost]
    public async Task<IActionResult> PaginatedIndexAsync(Guid accId)
    {
        var draw = Request.Form["draw"].FirstOrDefault();

        var start = Request.Form["start"].FirstOrDefault();
        int skip = start != null ? Convert.ToInt32(start) : 0;

        var length = Request.Form["length"].FirstOrDefault();
        int pageSize = length != null ? Convert.ToInt32(length) : 0;

        var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
        var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();

        var transactions = new ApiResponseViewModel<PaginatedViewModel>();

        _client.DefaultRequestHeaders.Authorization = LoginExtension.ReturnBearerToken(this);

        string filterRequest = $"&OrderCol={sortColumn}&OrderDir={sortColumnDirection}";
        string request = _client.BaseAddress
            + $"/GetAllTransactions/{accId}?PageNumber={skip}&PageSize={pageSize}" + filterRequest;

        HttpResponseMessage response = await _client.GetAsync(request);

        if (response.IsSuccessStatusCode)
            transactions = await response.Content.ReadAsAsync<ApiResponseViewModel<PaginatedViewModel>>();

        int recordTotals = transactions.Data.TotalTransactions;
        var jsonData = new { draw = draw, recordsFiltered = recordTotals, recordsTotal = recordTotals, data =  transactions.Data.Transactions };

        return Ok(jsonData);
    }

    [HttpGet]
    public IActionResult Create(Guid accId)
    {
        ViewData["accId"] = accId;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync(Guid accId, TransactionDTO transactionDTO)
    {
        _client.DefaultRequestHeaders.Authorization = LoginExtension.ReturnBearerToken(this);

        string request = _client.BaseAddress + $"/CreateTransaction/{accId}";

        HttpResponseMessage response = await _client.PostAsJsonAsync(request, transactionDTO);
        response.EnsureSuccessStatusCode();

        return RedirectToAction("Index", "Account", new { accId = accId });
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

        string request = _client.BaseAddress + $"/CreateDeposit/{accId}";

        HttpResponseMessage response = await _client.PostAsJsonAsync(request, transactionDTO); //serializarea
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

        string request = _client.BaseAddress + $"/CreateWithdrawal/{accId}";

        HttpResponseMessage response = await _client.PostAsJsonAsync(request, transactionDTO);
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

        string request = _client.BaseAddress + $"/CreateTransfer/{accId}";

        HttpResponseMessage response = await _client.PostAsJsonAsync(request, transactionDTO);
        response.EnsureSuccessStatusCode();

        return RedirectToAction("Index", "Account", new { accId = accId });
    }

    [HttpGet]
    public async Task<IActionResult> DetailsAsync(Guid accId, Guid tranId)
    {
        var transaction = new ApiResponseViewModel<TransactionDetailsViewModel>();
        
        _client.DefaultRequestHeaders.Authorization = LoginExtension.ReturnBearerToken(this);

        string request = _client.BaseAddress + $"/GetTransactionDetails/{accId}/{tranId}";
        HttpResponseMessage response = await _client.GetAsync(request);
        
        if (response.IsSuccessStatusCode)
            transaction = await response.Content.ReadAsAsync<ApiResponseViewModel<TransactionDetailsViewModel>>(); // deserializare
        
        ViewData["accId"] = accId;
        return View(transaction.Data);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateAsync(Guid accId, TransactionDetailsViewModel transaction)
    {
        var api_response = new ApiResponseViewModel<TransactionDetailsViewModel>();
        _client.DefaultRequestHeaders.Authorization = LoginExtension.ReturnBearerToken(this);

        string request = _client.BaseAddress + $"/UpdateTransfer/{accId}/{transaction.Id}";

        HttpResponseMessage response = await _client.PutAsJsonAsync(request, transaction);

        api_response = await response.Content.ReadAsAsync<ApiResponseViewModel<TransactionDetailsViewModel>>();

        TempData["status"] = api_response.Succes;
        TempData["message"] = api_response.Message;

        return RedirectToAction("Details", "Transaction", new { accId = accId, tranId = transaction.Id });
    }
}
