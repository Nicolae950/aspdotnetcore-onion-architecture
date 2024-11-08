
using Rotativa.AspNetCore;

namespace WebApp;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllersWithViews();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=User}/{action=Login}/{accId?}/{tranId?}");
            //pattern: "{controller=Account}/{action=Overview}/{accId = 01A11E2C-7671-4B62-8B8E-08DCEF6616A4}/{tranId?}");

        RotativaConfiguration.Setup(app.Environment.WebRootPath, @"C:\\Program Files\\wkhtmltopdf\\bin");

        app.Run();
    }
}
