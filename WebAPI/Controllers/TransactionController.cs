﻿using Application.DTOs;
using Application.Interfaces;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.DTOs;
using WebAPI.ViewModels;
using WebAPI.ViewModels.TransactionModels;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[Authorize]
public class TransactionController : Controller
{
    private readonly ITransactionService _transactionService;
    public TransactionController(ITransactionService transactionService)
    {
        _transactionService = transactionService;
    }

    [HttpGet("{accountId}")]
    public async Task<IActionResult> GetAllTransactionsAsync(Guid accountId, [FromQuery] FilterDTO filter)
    {
        try
        {
            var transactions = await _transactionService.GetAllTransactionsForAccountAsync(accountId, filter);
            
            var transactionsVM = transactions.Transactions
                .Select(x => new TransactionVM(x));

            var paginatedResultVM = new PaginatedResultVM(transactionsVM, transactions.TotalTransactions);
            var result = new StatusVM<PaginatedResultVM>(paginatedResultVM);
            return Ok(result);
        
        }catch(Exception ex)
        {
            return BadRequest(new StatusVM<PaginatedResultVM>(ex.Message));
        }
    }

    [HttpGet("{accountId}/{tranId}")]
    public async Task<IActionResult> GetTransactionDetailsAsync(Guid tranId)
    {
        try
        {
            var transactionVM = new DetalizedTransactionVM(await _transactionService.GetTransactionDetailsAsync(tranId));
            
            return Ok(new StatusVM<DetalizedTransactionVM>(transactionVM));

        }catch(Exception ex)
        {
            return BadRequest(new StatusVM<DetalizedTransactionVM>(ex.Message));
        }
    }

    [HttpPost("{accountId}")]
    public async Task<IActionResult> CreateTransactionAsync(Guid accountId, [FromBody] TransactionDTO transactionDTO)
    {
        try
        {
            DetalizedTransactionVM transactionVM = null;
            switch (transactionDTO.OperationType)
            {
                case OperationType.Deposit:
                    var transactionD = transactionDTO.MapDTOToTransaction(accountId, StateOfTransaction.Done, OperationType.Deposit);
                    //var transactionDVM = new DetalizedTransactionVM(await _transactionService.CreateDepositAsync(transactionD));
                    transactionVM = new DetalizedTransactionVM(await _transactionService.CreateDepositAsync(transactionD));
                    break;

                case OperationType.Withdrawal:
                    var transactionW = transactionDTO.MapDTOToTransaction(accountId, StateOfTransaction.Done, OperationType.Withdrawal);
                    var createdTransactionW = await _transactionService.CreateWithdrawalAsync(transactionW);
                    //var transactionWVM = new DetalizedTransactionVM(transactionW);
                    transactionVM = new DetalizedTransactionVM(transactionW);
                    break;

                case OperationType.Transfer:
                    var transactionT = transactionDTO.MapDTOToTransaction(accountId, StateOfTransaction.Waiting, OperationType.Transfer);
                    var createdTransactionT = await _transactionService.CreateTransferAsync(transactionT);
                    //var transactionTVM = new DetalizedTransactionVM(transactionT);
                    transactionVM = new DetalizedTransactionVM(transactionT);
                    break;
            }
            return Ok(new StatusVM<DetalizedTransactionVM>(transactionVM));
        }
        catch (Exception ex)
        {
            return BadRequest(new StatusVM<DetalizedTransactionVM>(ex.Message));
        }
    }

    [HttpPost("{accountId}")]
    public async Task<IActionResult> CreateDepositAsync(Guid accountId, [FromBody] TransactionDTO transactionDTO)
    {
        try
        {
            var transaction = transactionDTO.MapDTOToTransaction(accountId, StateOfTransaction.Done, OperationType.Deposit);
            var transactionVM = new DetalizedTransactionVM(await _transactionService.CreateDepositAsync(transaction));
            return Ok(new StatusVM<DetalizedTransactionVM>(transactionVM));
        }catch(Exception ex)
        {
            return BadRequest(new StatusVM<DetalizedTransactionVM>(ex.Message));
        }
    }

    [HttpPost("{accountId}")]
    public async Task<IActionResult> CreateWithdrawalAsync(Guid accountId, [FromBody] TransactionDTO transactionDTO)
    {
        try
        {
            var transaction = transactionDTO.MapDTOToTransaction(accountId, StateOfTransaction.Done, OperationType.Withdrawal);
            var createdTransaction = await _transactionService.CreateWithdrawalAsync(transaction);
            var transactionVM = new DetalizedTransactionVM(transaction);

            return Ok(new StatusVM<DetalizedTransactionVM>(transactionVM));
        }
        catch (Exception ex)
        {
            return BadRequest(new StatusVM<DetalizedTransactionVM>(ex.Message));
        }
    }

    [HttpPost("{accountId}")]
    public async Task<IActionResult> CreateTransferAsync(Guid accountId, [FromBody] TransactionDTO transactionDTO)
    {
        try
        {
            var transaction = transactionDTO.MapDTOToTransaction(accountId, StateOfTransaction.Waiting, OperationType.Transfer);
            var createdTransaction = await _transactionService.CreateTransferAsync(transaction);
            var transactionVM = new DetalizedTransactionVM(transaction);
            return Ok(new StatusVM<DetalizedTransactionVM>(transactionVM));
        }catch(Exception ex)
        {
            return BadRequest(new StatusVM<DetalizedTransactionVM>(ex.Message));
        }
    }

    [HttpPut("{accountId}/{tranId}")]
    public async Task<IActionResult> UpdateTransferAsync(Guid accountId, Guid tranId, [FromBody] TransactionDTO transactionDTO)
    {
        try
        {
            var transaction = transactionDTO.MapDTOToTransactionWithId(tranId);
            var updatedTransaction = await _transactionService.UpdateTransactionAsync(accountId, transaction);
            var transactionVM = new DetalizedTransactionVM(updatedTransaction);
            return Ok( new StatusVM<DetalizedTransactionVM>(transactionVM));
        }catch(Exception ex)
        {
            return BadRequest(new StatusVM<DetalizedTransactionVM>(ex.Message));
        }
    }

    [HttpPut("{accountId}/{tranId}")]
    public async Task<IActionResult> ExecuteUpdateTransferAsync(Guid accountId, Guid tranId, [FromBody] TransactionDTO transactionDTO)
    {
        try
        {
            var transaction = transactionDTO.MapDTOToTransactionWithId(tranId);
            await _transactionService.ExecuteUpdateTransactionAsync(accountId, transaction);
            var updatedTransaction = new DetalizedTransactionVM(await _transactionService.GetTransactionDetailsAsync(tranId));

            return Ok(new StatusVM<DetalizedTransactionVM>(updatedTransaction));
        }catch(Exception ex)
        {
            return BadRequest(new StatusVM<DetalizedTransactionVM>(ex.Message));
        }
    }
}
