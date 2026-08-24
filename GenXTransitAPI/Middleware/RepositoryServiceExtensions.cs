using GenXTransitAPI.DataAccess.Interface.IRepositories;
using GenXTransitAPI.DataAccess.Interface.IServices;
using GenXTransitAPI.DataAccess.Repositories;
using GenXTransitAPI.DataAccess.Security;
using GenXTransitAPI.DataAccess.Services;
using GenXTransitAPI.Models.DTO_s;
using GenXTransittAPI.DataAccess.Data;
using Microsoft.AspNetCore.Authorization;

namespace GenXTransitAPI.Middleware
{
    public static class RepositoryServiceExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services, IConfiguration configuration)
        {

            //    // ─── DI registrations ─────────────────────────────────────────────────────────
                services.AddSingleton<DBHelper>();


            // DATA ACCESS
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IAuthRepository, AuthRepository>();

            // SERVICES
            services.AddScoped<IPasswordService, PasswordService>();
            services.AddScoped<IEmailService, EmailService>();

            // EMAIL SETTINGS
            services.Configure<SendEmailDto>(
                configuration.GetSection("EmailSettings"));

            return services;

        }
    }
}
