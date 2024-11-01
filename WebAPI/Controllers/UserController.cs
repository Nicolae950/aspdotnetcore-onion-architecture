using Application.Interfaces;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using WebAPI.DTOs;
using WebAPI.ViewModels;

namespace WebAPI.Controllers;

[ApiController]
[Route("/api/[controller]/[action]")]
public class UserController : Controller
{
    private readonly IUserService _userService;
    private readonly ITokenService _tokenService;
    public UserController(IUserService userService, ITokenService tokenService, IRecurringJobManager recurringJobManager)
    {
        _userService = userService;
        _tokenService = tokenService;
    }

    [HttpPost]
    public async Task<IActionResult> RegistrationAsync([FromBody] UserDTO userDTO)
    {
        try
        {
            var newUser = userDTO.MapDTOToUser();
            await _userService.RegisterAsync(newUser);
            return Ok(new StatusVM<UserVM>());
        }catch(Exception ex)
        {
            return BadRequest(new StatusVM<UserVM>(ex.Message));
        }
    }

    [HttpPost]
    public async Task<IActionResult> LoginAsync([FromBody] UserDTO userDTO)
    {
        try
        {
            var tryingToLogUser = userDTO.MapDTOToUser();
            var loginResult = await _userService.LoginAsync(tryingToLogUser);
            if (loginResult != null)
            {
                var encodedResult = new UserVM(loginResult.Id, loginResult.Email, _tokenService.GetToken(loginResult));
                return Ok(new StatusVM<UserVM>(encodedResult));
            }
            else
                return BadRequest(new StatusVM<UserVM>("Username or Password isn't corect!"));
        }
        catch(Exception ex)
        {
            return BadRequest(new StatusVM<UserVM>(ex.Message));
        }
    }
}
