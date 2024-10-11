using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using WebAPI.DTOs;
using WebAPI.ViewModels;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class AccountController : Controller
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAccountById(Guid id)
    {
        try
        {
            var account = new AccountVM(await _accountService.GetAccountDetails(id));
            return Ok(account);
        }catch(Exception ex)
        {
            return Content(ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateAccount([FromBody] AccountDTO accountDTO)
    {
        try
        {
            var account = accountDTO.MapDTOToAccount();
            var createdAccount = await _accountService.AddAccount(account);
            return Ok(createdAccount);
        }catch(Exception ex)
        {
            return Content(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAccount(Guid id,[FromBody] AccountDTO accountDTO)
    {
        try
        {
            var account = accountDTO.MapDTOToAccountWithId(id);
            var updatedAccount = await _accountService.UpdateAccount(account);
            var accountVM = new AccountVM(updatedAccount);
            return Ok(accountVM);
        }
        catch (Exception ex)
        {
            return Content(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> InactivateAccount(Guid id)
    {
        try
        {
            await _accountService.InactivateAccount(id);
            return Content($"Account with ID {id} is Inactive, for any reasons of information please contact us!");
        }
        catch (Exception ex)
        {
            return Content(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAccount(Guid id)
    {
        try
        {
            await _accountService.DeleteAccount(id);
            return Content($"Account with ID {id} was Deleted, for any reasons of information please contact us!");
        }catch(Exception ex)
        {
            return Content(ex.Message);
        }
    }
}
