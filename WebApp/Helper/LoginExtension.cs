using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace WebApp.Helper;

public static class LoginExtension
{
    public static AuthenticationHeaderValue ReturnBearerToken(ControllerBase context)
    {
        var token = "";
        context.HttpContext.Request.Cookies.TryGetValue("token", out token);
        return new AuthenticationHeaderValue("Bearer", token);
    }
}
