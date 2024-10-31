using Application.Interfaces;
using Application.Services;
using Domain.Entities;
using Domain.Interfaces;
using Hangfire;
using Infrastructure.Database;
using Infrastructure.Repositories;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace WebAPI;

public class Program
{
    public static void Main(string[] args)
    {
        BuildWebHost(args).Run();
    }

    public static IWebHost BuildWebHost(string[] args) => WebHost.CreateDefaultBuilder(args)
            .UseStartup<Startup>()
            .Build();

    //{
    //var builder = WebApplication.CreateBuilder(args);
    //builder.Services.AddMvc(mvcOpt =>
    //{
    //    mvcOpt.EnableEndpointRouting = false;
    //});

    //builder.Services.AddControllers();

    //builder.Services.AddEndpointsApiExplorer();
    //builder.Services.AddSwaggerGen();

    //builder.Services.AddHttpContextAccessor();

    //builder.Services.AddScoped<ITokenService, TokenService>();

    //builder.Services.AddScoped<IUserRepository, UserRepository>();
    //builder.Services.AddScoped<IUserService, UserService>();

    //builder.Services.AddScoped<IAccountRepository, AccountRepository>();
    //builder.Services.AddScoped<IAccountService, AccountService>();

    //builder.Services.AddScoped<ITransactionService, TransactionService>();
    //builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();

    //builder.Services.AddScoped<IReportRepository, ReportRepository>();
    //builder.Services.AddScoped<IReportService, ReportService>();

    //builder.Services.AddDbContext<ApplicationDbContext>(opt =>
    //{

    //    opt.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=BankWebAPI;Trusted_Connection=True;"
    //                      , x => x.MigrationsAssembly("Infrastructure"));
    //});

    //builder.Services.AddAuthorization();
    //builder.Services.AddAuthentication(options =>
    //{
    //    options.DefaultAuthenticateScheme =
    //    options.DefaultChallengeScheme =
    //    options.DefaultForbidScheme = 
    //    options.DefaultScheme = 
    //    options.DefaultSignInScheme = 
    //    options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
    //}).AddJwtBearer(options =>
    //{
    //    options.TokenValidationParameters = new TokenValidationParameters
    //    {
    //        ValidateIssuer = true,
    //        ValidIssuer = builder.Configuration["JWT:Issuer"],

    //        ValidateAudience = true,
    //        ValidAudience = builder.Configuration["JWT:Audience"],

    //        ValidateIssuerSigningKey = true,
    //        IssuerSigningKey = new SymmetricSecurityKey(
    //            Encoding.UTF8.GetBytes(builder.Configuration["JWT:SigningKey"]))
    //    };
    //});

    //builder.Services.AddHangfire(configuration => configuration
    //    .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
    //    .UseSimpleAssemblyNameTypeSerializer()
    //    .UseRecommendedSerializerSettings()
    //    .UseSqlServerStorage(@"Server=(localdb)\mssqllocaldb;Database=BankWebAPI;Trusted_Connection=True;Integrated Security=SSPI;")
    //    );

    //builder.Services.AddHangfireServer();

    //var app = builder.Build();


    //if (app.Environment.IsDevelopment())
    //{
    //    app.UseSwagger();
    //    app.UseSwaggerUI();
    //}

    //app.UseAuthentication();
    //app.UseAuthorization();

    //app.UseMvc();

    //app.Run();
    //}
}
