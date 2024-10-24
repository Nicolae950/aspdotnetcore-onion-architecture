using Microsoft.AspNetCore.Mvc;
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
        var path = $"/GetAllTransaction/{accId}?PageNumber={paginated.PageNumber}&PageSize={paginated.PageSize}";
        HttpResponseMessage response = await _client.GetAsync(_client.BaseAddress + path);

        if (response.IsSuccessStatusCode)
        {
            transactions = await response.Content.ReadAsAsync<ApiResponseViewModel<List<TransactionViewModel>>>();
            //var data = await response.Content.ReadAsAsync<ApiResponseViewModel<List<TransactionViewModel>>>();
            //transactions = JsonConvert.DeserializeObject<>(data);
        }
        return View(transactions);
    }

    [HttpGet]
    public IActionResult Deposit(Guid accId)
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> DepositAsync(Guid accId, TransactionDTO transactionDTO)
    {
        HttpResponseMessage response = await _client.PostAsJsonAsync(_client.BaseAddress //serializarea
            + $"/CreateDeposit/{accId}", transactionDTO);
        response.EnsureSuccessStatusCode();

        return RedirectToAction();
    }

    [HttpGet]
    public IActionResult Withdraw(Guid accId)
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> WithdrawAsync(Guid accId, TransactionDTO transactionDTO)
    {
        HttpResponseMessage response = await _client.PostAsJsonAsync(_client.BaseAddress //serializarea
            + $"/CreateWithdrawal/{accId}", transactionDTO);
        response.EnsureSuccessStatusCode();

        return RedirectToAction();
    }

    [HttpGet]
    public IActionResult Transfer(Guid accId)
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> TransferAsync(Guid accId, TransactionDTO transactionDTO)
    {
        HttpResponseMessage response = await _client.PostAsJsonAsync(_client.BaseAddress //serializarea
            + $"/CreateTransfer/{accId}", transactionDTO);
        response.EnsureSuccessStatusCode();

        return RedirectToAction();
    }

    [HttpGet]
    public async Task<IActionResult> DetailsAsync(Guid accId, Guid tranId)
    {
        var transaction = new ApiResponseViewModel<TransactionDetailsViewModel>();
        var path = $"/GetTransactionDetails/{accId}/{tranId}";
        HttpResponseMessage response = await _client.GetAsync(_client.BaseAddress + path);
        
        if (response.IsSuccessStatusCode)
            transaction = await response.Content.ReadAsAsync<ApiResponseViewModel<TransactionDetailsViewModel>>(); // deserializare
        ViewData["Id"] = accId;
        return View(transaction.Data);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateAsync(Guid accId, TransactionDetailsViewModel transaction)
    {
        HttpResponseMessage response = await _client.PutAsJsonAsync(_client.BaseAddress +
            $"/UpdateTransfer/{accId}/{transaction.Id}", transaction);
        response.EnsureSuccessStatusCode();

        return RedirectToAction("Details", new { id = transaction.DestinationAccountId, tranId = transaction.Id});
    }
}
