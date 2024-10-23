using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebApp.DTOs;
using WebApp.Models;

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
    public async Task<IActionResult> Index(Guid id, [FromQuery]PaginatedViewModel paginated)
    {
        var transactions = new ApiResponseViewModel<List<TransactionViewModel>>();
        var path = $"/GetAllTransaction/{id}?PageNumber={paginated.PageNumber}&PageSize={paginated.PageSize}";
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
    public IActionResult Deposit(Guid id)
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Deposit(Guid id, TransactionDTO transactionDTO)
    {
        HttpResponseMessage response = await _client.PostAsJsonAsync(_client.BaseAddress //serializarea
            + $"/CreateDeposit/{id}", transactionDTO);
        response.EnsureSuccessStatusCode();

        return RedirectToAction();
    }

    [HttpGet]
    public async Task<IActionResult> Details(Guid id, Guid tranId)
    {
        var transaction = new ApiResponseViewModel<TransactionDetailsViewModel>();
        var path = $"/GetTransactionDetails/{id}/{tranId}";
        HttpResponseMessage response = await _client.GetAsync(_client.BaseAddress + path);
        
        if (response.IsSuccessStatusCode)
            transaction = await response.Content.ReadAsAsync<ApiResponseViewModel<TransactionDetailsViewModel>>(); // deserializare
        
        return View(transaction);
    }

    [HttpPut]
    public async Task<IActionResult> Update(TransactionDetailsViewModel transaction)
    {
        HttpResponseMessage response = await _client.PutAsJsonAsync(_client.BaseAddress + 
            $"/UpdateTransfer/{transaction.DestinationAccountId}/{transaction.Id}", transaction);
        response.EnsureSuccessStatusCode();

        return RedirectToAction();
    }
}
