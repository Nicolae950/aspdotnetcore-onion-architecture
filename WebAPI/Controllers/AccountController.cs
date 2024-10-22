using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.DTOs;
using WebAPI.ViewModels;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[Authorize]
public class AccountController : Controller
{
    private readonly IAccountService _accountService;
    private readonly ITransactionService _transactionService;

    public AccountController(IAccountService accountService, ITransactionService transactionService)
    {
        _accountService = accountService;
        _transactionService = transactionService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> DetailsAsync(Guid id)
    {
        try
        {
            var account = new DetalizedAccountVM(await _accountService.GetAccountDetailsAsync(id));
            return Ok(new StatusVM<DetalizedAccountVM>(account));
        }catch(Exception ex)
        {
            return BadRequest(new StatusVM<DetalizedAccountVM>(ex.Message));
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAccountByIdAsync(Guid id)
    {
        try
        {
            var account = new AccountTransactionsVM(
                await _accountService.GetAccountDetailsAsync(id),
                await _transactionService.GetLastTransactionsAsync(id));

            return Ok(new StatusVM<AccountTransactionsVM>(account));
        }catch(Exception ex)
        {
            return BadRequest(new StatusVM<AccountTransactionsVM>(ex.Message));
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateAccountAsync([FromBody] AccountDTO accountDTO)
    {
        try
        {
            var account = accountDTO.MapDTOToAccount();
            var createdAccount = new DetalizedAccountVM(await _accountService.AddAccountAsync(account));
            return Ok(new StatusVM<DetalizedAccountVM>(createdAccount));
        }catch(Exception ex)
        {
            return BadRequest(new StatusVM<DetalizedAccountVM>(ex.Message));
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAccountAsync(Guid id,[FromBody] AccountDTO accountDTO)
    {
        try
        {
            var account = accountDTO.MapDTOToAccountWithId(id);
            var updatedAccount = await _accountService.UpdateAccountAsync(account);
            var accountVM = new DetalizedAccountVM(updatedAccount);
            return Ok(new StatusVM<DetalizedAccountVM>(accountVM));
        }
        catch (Exception ex)
        {
            return BadRequest(new StatusVM<DetalizedAccountVM>(ex.Message));
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> InactivateAccountAsync(Guid id)
    {
        try
        {
            await _accountService.InactivateAccountAsync(id);
            var message = $"Account with ID {id} is Inactive, for any reasons of information please contact us!";
            return Ok(new StatusVM<DetalizedAccountVM>());
        }
        catch (Exception ex)
        {
            return BadRequest(new StatusVM<DetalizedAccountVM>(ex.Message));
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAccountAsync(Guid id)
    {
        try
        {
            await _accountService.DeleteAccountAsync(id);
            var message = $"Account with ID {id} was Deleted, for any reasons of information please contact us!";
            return Ok(new StatusVM<DetalizedAccountVM>());
        }catch(Exception ex)
        {
            return BadRequest(new StatusVM<DetalizedAccountVM>(ex.Message));
        }
    }
}
