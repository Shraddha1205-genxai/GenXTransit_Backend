using GenXTransitAPI.DataAccess.Interface.IRepositories;
using GenXTransitAPI.DataAccess.Interface.IServices;
using GenXTransitAPI.DataAccess.Repositories;
using GenXTransitAPI.DataAccess.Security;
using GenXTransitAPI.DataAccess.Services;
using GenXTransitAPI.Models.DTO_s;
using GenXTransitAPI.Models.DTOs;
using GenXTransitAPI.DataAccess.Data;
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

            services.AddScoped<IOrgCorporationRepository, OrgCorporationRepository>();
            services.AddScoped<IOrgDivisionRepository, OrgDivisionRepository>();
            services.AddScoped<IOrgRegionRepository, OrgRegionRepository>();
            services.AddScoped<IOrgZoneRepository, OrgZoneRepository>();

            services.AddScoped<IOrgCorporationService, OrgCorporationService>();
            services.AddScoped<IOrgDivisionService, OrgDivisionService>();
            services.AddScoped<IOrgRegionService, OrgRegionService>();
            services.AddScoped<IOrgZoneService, OrgZoneService>();

            // SERVICES
            services.AddScoped<IPasswordService, PasswordService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IJwtService, JwtService>();

            // EMAIL SETTINGS
            services.Configure<SendEmailDto>(
                configuration.GetSection("EmailSettings"));

            services.Configure<JwtSettings>(
        configuration.GetSection("Jwt"));

            return services;

        }
    }
}
