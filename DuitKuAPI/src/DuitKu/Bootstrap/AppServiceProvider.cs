using DuitKu.Persistance.Database;
using DuitKu.Persistance.Repository;
using DuitKu.Services;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace DuitKu.Bootstrap
{
    public static class AppServiceProvider
    {
        public static IServiceCollection Boot(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddControllers();
            services.AddDbContext<ApplicationDBContext>();

            services.AddScoped<AccountRepository>();
            services.AddScoped<AccountService>();

            services.AddScoped<CategoryRepository>();
            services.AddScoped<CategoryService>();

            services.AddScoped<SubCategoryRepository>();
            services.AddScoped<SubCategoryService>();

            services.AddScoped<TransactionRepository>();
            services.AddScoped<TransactionService>();

            services.AddScoped<JwtService>();
            services.AddScoped<AuthService>();

            services.AddLogging(config =>
            {
                config.AddConsole();
                config.SetMinimumLevel(LogLevel.Information);
            });

            return services;
        }

        public static IServiceCollection AddJWTAuthentication(this IServiceCollection services, byte[] SecretKey)
        {
            services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(option =>
            {
                option.RequireHttpsMetadata = false;
                option.SaveToken = false;
                option.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(SecretKey),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                };
            });

            return services;
        }
    }
}