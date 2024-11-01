using Application.Interfaces;
using Application.Services;
using Domain.Entities;
using Domain.Interfaces;
using Hangfire;
using Infrastructure.Database;
using Infrastructure.Repositories;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace WebAPI;

public class Startup
{
    public IConfiguration Configuration { get; set; }

    public Startup(IWebHostEnvironment env)
    {
        var builder = new ConfigurationBuilder();
        builder.AddEnvironmentVariables();
        builder.SetBasePath(env.ContentRootPath);
        builder.AddJsonFile("appsettings.json");
        Configuration = builder.Build();
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        services.AddHttpContextAccessor();

        services.AddScoped<ITokenService, TokenService>();

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserService, UserService>();

        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<IAccountService, AccountService>();

        services.AddScoped<ITransactionService, TransactionService>();
        services.AddScoped<ITransactionRepository, TransactionRepository>();

        services.AddScoped<IReportRepository, ReportRepository>();
        services.AddScoped<IReportService, ReportService>();

        services.AddDbContext<ApplicationDbContext>(opt =>
        {

            opt.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=BankWebAPI;Trusted_Connection=True;"
                              , x => x.MigrationsAssembly("Infrastructure"));
        });

        services.AddAuthorization();
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme =
            options.DefaultChallengeScheme =
            options.DefaultForbidScheme =
            options.DefaultScheme =
            options.DefaultSignInScheme =
            options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = Configuration["JWT:Issuer"],

                ValidateAudience = true,
                ValidAudience = Configuration["JWT:Audience"],

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(Configuration["JWT:SigningKey"]))
            };
        });

        services.AddHangfire(configuration => configuration
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseSqlServerStorage(@"Server=(localdb)\mssqllocaldb;Database=BankWebAPI;Trusted_Connection=True;Integrated Security=SSPI;")
            );

        services.AddHangfireServer();
        
        services.AddMvc(mvcOpt =>
        {
            mvcOpt.EnableEndpointRouting = false;
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IRecurringJobManager recurringJobManager)
    {
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseHangfireDashboard();

        //app.UseEndpoints(endpoints =>
        //{
        //    endpoints.MapHangfireDashboard();
        //});

        recurringJobManager.AddOrUpdate<IReportService>("program-Job", report => report.CreateReportsForAllAsync(), Cron.Hourly);

        app.UseMvc();
    }
}
