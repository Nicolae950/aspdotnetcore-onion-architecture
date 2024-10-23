using Microsoft.AspNetCore.Mvc;
using WebApp.DTOs;
using WebApp.Models;

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
        public async Task<IActionResult> Login(UserDTO user)
        {
            var loggedResult = new ApiResponseViewModel<UserViewModel>();
            HttpResponseMessage response = await _client.GetAsync(_client.BaseAddress + "/Login");

            if(response.IsSuccessStatusCode)
                loggedResult = await response.Content.ReadAsAsync<ApiResponseViewModel<UserViewModel>>();

            return RedirectToAction(); //Index -> GetAllAccounts / Account -> controller / object = Data.Token
        }
    }
}
