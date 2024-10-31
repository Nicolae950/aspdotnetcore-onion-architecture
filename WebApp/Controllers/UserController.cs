using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using WebApp.DTOs;
using WebApp.Models;
using WebApp.Models.User;

namespace WebApp.Controllers
{
    public class UserController : Controller
    {
        private readonly HttpClient _client;
        public UserController()
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri("https://localhost:44316/api/User");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoginAsync(UserDTO user)
        {
            var loggedResult = new ApiResponseViewModel<UserViewModel>();
            HttpResponseMessage response = await _client.PostAsJsonAsync(_client.BaseAddress + "/Login", user);

            if(response.IsSuccessStatusCode)
            {
                loggedResult = await response.Content.ReadAsAsync<ApiResponseViewModel<UserViewModel>>();
                
                HttpContext.Response.Cookies.Append("token", loggedResult.Data.Token);

                return RedirectToAction("Overview", "Account", new { accId = loggedResult.Data.Id });
            }
            else
            {
                return RedirectToAction("Login");
            }

            //Index -> GetAllAccounts / Account -> controller / object = Data.Token
        }
    }
}
