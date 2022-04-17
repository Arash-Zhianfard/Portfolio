using Abstraction.Interfaces.Repositories;
using Abstraction.Interfaces.Services;
using Abstraction.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Repository;
using Repository.Repositories;
using Service.Implementation;
using Service.Model;
using System.Text;
using WebApp.Filter;

namespace WebApp
{
    public static class ServicesExtensions
    {
        public static void AddCustomServices(this IServiceCollection services, IConfiguration configurations)
        {
            var jwtSection = configurations.GetSection("JwtSetting");
            services.Configure<JwtSetting>(configurations.GetSection("jwtSetting"));
            services.Configure<VwdServicesApiSetting>(configurations.GetSection("VwdservicesApiSetting"));
            services.Configure<ConvertCurrencyApiSetting>(configurations.GetSection("ConvertCurrencyApiSetting"));
            services.AddScoped<AuthorizeFilterAttribute>();
            services.AddScoped<ICurrencyConvertor, CurrencyConvertor>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IPortfolioService, PortfolioService>();
            services.AddScoped<IStockService, StockService>();
            services.AddScoped<IStockRepository, StockRepository>();
            services.AddScoped<IPortfolioRepository, PortfolioRepository>();
            services.AddScoped<IExchangeService, ExchangeService>();
            services.AddScoped<IPositionRepository, PositionRepository>();
            services.AddScoped<IPositionService, PositionService>();
            services.AddScoped<IVwdService, VwdService>();
            services.AddScoped<IApiCaller, ApiCaller>();
            services.AddScoped<IProfitCalculator, ProfitCalculator>();
            services.AddScoped<IEncryptService, EncryptService>();
            services.Configure<FormOptions>(options => options.BufferBody = true);
            JwtSetting _jwtSetting = new JwtSetting();
            jwtSection.Bind(_jwtSetting);
            var key = Encoding.ASCII.GetBytes(_jwtSetting.SecretKey);

            services.AddMvc()
        .AddSessionStateTempDataProvider();
            services.AddSession();
            // If using IIS:
            services.Configure<IISServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
           .AddJwtBearer(x =>
           {
               x.RequireHttpsMetadata = false;
               x.SaveToken = true;
               x.TokenValidationParameters = new TokenValidationParameters
               {
                   ValidateIssuerSigningKey = true,
                   IssuerSigningKey = new SymmetricSecurityKey(key),
                   ValidateIssuer = false,
                   ValidateAudience = false,
                   RequireExpirationTime = true,
                   ValidateLifetime = true,
                   ClockSkew = TimeSpan.Zero,
               };
           });

            services.AddDbContextPool<AppDbContext>(options =>
            {
                options.UseInMemoryDatabase(databaseName: "ApplicantDB");
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

            }, 1024);

        }
    }
}