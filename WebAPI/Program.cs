using Application.Interfaces;
using Application.Services;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Database;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace WebAPI;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddMvc(mvcOpt =>
        {
            mvcOpt.EnableEndpointRouting = false;
        });
        
        builder.Services.AddControllers();

        builder.Services.AddHttpContextAccessor();

        builder.Services.AddScoped<IAccountRepository, AccountRepository>();
        builder.Services.AddScoped<IAccountService, AccountService>();

        builder.Services.AddScoped<ITransactionService, TransactionService>();
        builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();

        builder.Services.AddDbContext<ApplicationDbContext>(opt =>
        {
            opt.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=BankWebAPI;Trusted_Connection=True;"
                              , x => x.MigrationsAssembly("Infrastructure"));
        });

        var app = builder.Build();

        app.UseMvc();

        app.Run();
    }
}
