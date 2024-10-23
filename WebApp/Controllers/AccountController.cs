using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

public class AccountController : Controller
{
    public async Task<IActionResult> Index()
    {
        HttpClient client = new HttpClient();
        return View();
    }
}
