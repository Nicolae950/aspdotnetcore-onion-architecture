using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.DTOs;
using WebAPI.ViewModels;
using WebAPI.ViewModels.AccountModels;
using WebAPI.ViewModels.UserModels;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[Authorize]
public class AccountController : Controller
{
    private readonly IAccountService _accountService;
    private readonly ITransactionService _transactionService;
    private readonly IUserService _userService;

    public AccountController(IAccountService accountService, ITransactionService transactionService, IUserService userService)
    {
        _accountService = accountService;
        _transactionService = transactionService;
        _userService = userService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAllAccountsAsync(Guid id)
    {
        try
        {
            var accounts = await _accountService.GetAllAccountsAsync(id);
            var user = await _userService.GetUserAsync(id);
            var detalizedUserVM = new DetalizedUserVM(user, accounts);

            return Ok(new StatusVM<DetalizedUserVM>(detalizedUserVM));
        }catch(Exception ex)
        {
            return BadRequest(new StatusVM<DetalizedUserVM>(ex.Message));
        }
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

    [HttpPost("{id}")]
    public async Task<IActionResult> CreateAccountAsync(Guid id, [FromBody] AccountDTO accountDTO)
    {
        try
        {
            var account = accountDTO.MapDTOToAccount(id);
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
