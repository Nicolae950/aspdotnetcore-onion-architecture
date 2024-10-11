using Application.DTOs;
using Application.Interfaces;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using WebAPI.DTOs;
using WebAPI.ViewModels;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class TransactionController : Controller
{
    private readonly ITransactionService _transactionService;
    public TransactionController(ITransactionService transactionService)
    {
        _transactionService = transactionService;
    }

    [HttpGet("{accountId}")]
    public async Task<IActionResult> GetAllTransactions(Guid accountId, [FromBody] FilterDTO filter)
    {
        try
        {
            var transactions = await _transactionService.GetAllTransactionsForAccount(accountId, filter);
            var transactionsVM = transactions
                .Select(x => new TransactionVM(x));
            return Ok(transactionsVM);
        }catch(Exception ex)
        {
            return Content(ex.Message);
        }
    }

    [HttpGet("{accountId}/{tranId}")]
    public async Task<IActionResult> GetTransactionDetails(Guid tranId)
    {
        try
        {
            var transactionVM = new TransactionVM(await _transactionService.GetTransactionDetails(tranId));
            return Ok(transactionVM);
        }catch(Exception ex)
        {
            return Content(ex.Message);
        }
    }

    [HttpPost("{accountId}")]
    public async Task<IActionResult> CreateDeposit(Guid accountId, [FromBody] TransactionDTO transactionDTO)
    {
        try
        {
            var transaction = transactionDTO.MapDTOToTransaction(accountId, StateOfTransaction.Done, OperationType.Deposit);
            var transactionVM = new TransactionVM(await _transactionService.CreateDeposit(transaction));
            return Ok(transactionVM);
        }catch(Exception ex)
        {
            return Content(ex.Message);
        }
    }

    [HttpPost("{accountId}")]
    public async Task<IActionResult> CreateWithdrawal(Guid accountId, [FromBody] TransactionDTO transactionDTO)
    {
        try
        {
            var transaction = transactionDTO.MapDTOToTransaction(accountId, StateOfTransaction.Done, OperationType.Withdrawal);
            var createdTransaction = await _transactionService.CreateWithdrawal(transaction);
            var transactionVM = new TransactionVM(transaction);
            return Ok(transactionVM);
        }
        catch (Exception ex)
        {
            return Content(ex.Message);
        }
    }

    [HttpPost("{accountId}")]
    public async Task<IActionResult> CreateTransfer(Guid accountId, [FromBody] TransactionDTO transactionDTO)
    {
        try
        {
            var transaction = transactionDTO.MapDTOToTransaction(accountId, StateOfTransaction.Waiting, OperationType.Transfer);
            var createdTransaction = await _transactionService.CreateTransfer(transaction);
            var transactionVM = new TransactionVM(transaction);
            return Ok(transactionVM);
        }catch(Exception ex)
        {
            return Content(ex.Message);
        }
    }

    [HttpPut("{accountId}/{tranId}")]
    public async Task<IActionResult> UpdateTransfer(Guid accountId, Guid tranId, [FromBody] TransactionDTO transactionDTO)
    {
        try
        {
            var transaction = transactionDTO.MapDTOToTransactionWithId(tranId);
            var updatedTransaction = await _transactionService.UpdateTransaction(accountId, transaction);
            var transactionVM = new TransactionVM(updatedTransaction);
            return Ok(transactionVM);
        }catch(Exception ex)
        {
            return Content(ex.Message);
        }
    }

    [HttpPut("{accountId}/{tranId}")]
    public async Task<IActionResult> ExecuteUpdateTransfer(Guid accountId, Guid tranId, [FromBody] TransactionDTO transactionDTO)
    {
        try
        {
            var transaction = transactionDTO.MapDTOToTransactionWithId(tranId);
            await _transactionService.ExecuteUpdateTransaction(accountId, transaction);
            return Ok();
        }catch(Exception ex)
        {
            return Content(ex.Message);
        }
    }
}
