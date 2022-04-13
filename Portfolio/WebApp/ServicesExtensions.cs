using Abstraction.Interfaces.Repositories;
using Abstraction.Interfaces.Services;
using Abstraction.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Repository;
using Repository.Repositories;
using Service.Implementation;
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
            services.AddScoped<AuthorizeFilterAttribute>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IPortfolioService, PortfolioService>();
            services.AddScoped<IStockService, StockService>();
            services.AddScoped<IStockRepository, StockRepository>();
            services.AddScoped<IPortfolioRepository, PortfolioRepository>();
            services.AddScoped<IExchangeService, ExchangeService>();
            services.AddScoped<IPositionRepository, PositionRepository>();
            services.AddScoped<IVwdService, VwdService>();
            services.AddScoped<IEncryptService, EncryptService>();
            services.Configure<FormOptions>(options => options.BufferBody = true);
            JwtSetting _jwtSetting = new JwtSetting();
            jwtSection.Bind(_jwtSetting);
            var key = Encoding.ASCII.GetBytes(_jwtSetting.SecretKey);



            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1.1", new OpenApiInfo { Title = "Values Api", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Authorization: Bearer ",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
{
    {
        new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
            },
            Scheme = "Authorization",
            Name = "Bearer",
            In = ParameterLocation.Header,

        },
        new List<string>()
    }
});
            });



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