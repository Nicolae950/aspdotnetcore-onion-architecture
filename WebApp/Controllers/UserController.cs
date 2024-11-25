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
            var nullResult = new ApiResponseViewModel<UserViewModel>();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoginAsync(UserDTO user)
        {
            if (!ModelState.IsValid)
            {
                return View("Login");
            }
            
            var loggedResult = new ApiResponseViewModel<UserViewModel>();
            string request = _client.BaseAddress + "/Login";

            HttpResponseMessage response = await _client.PostAsJsonAsync(request, user);

            loggedResult = await response.Content.ReadAsAsync<ApiResponseViewModel<UserViewModel>>();
            
            if(response.IsSuccessStatusCode)
            {
                HttpContext.Response.Cookies.Append("token", loggedResult.Data.Token);
                TempData["UserId"] = loggedResult.Data.Id;
                return RedirectToAction("Overview", "Account", new { accId = loggedResult.Data.Id });
            }
            else
            {
                TempData["Message"] = loggedResult.Message;
                return RedirectToAction("Login");
            }
            
        }

        [HttpGet]
        public async Task<IActionResult> LogoutAsync()
        {
            await Task.Factory.StartNew(() => {
                HttpContext.Response.Cookies.Delete("token");
            }); 
            return RedirectToAction("Login", "User");
        }
    }
}
