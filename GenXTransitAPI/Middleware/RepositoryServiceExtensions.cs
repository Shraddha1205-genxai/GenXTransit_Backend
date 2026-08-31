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
            services.AddScoped<IRoleRepository, RoleRepository>();

            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IUserRepository, UserMasterRepository>();
            services.AddScoped<IUserService, UserMasterService>();
            services.AddScoped<IOrgCorporationRepository, OrgCorporationRepository>();
            services.AddScoped<IOrgDivisionRepository, OrgDivisionRepository>();
            services.AddScoped<IOrgRegionRepository, OrgRegionRepository>();
            services.AddScoped<IOrgZoneRepository, OrgZoneRepository>();
            services.AddScoped<IOrgDepotRepository, OrgDepotRepository>();
            services.AddScoped<IOrgStationRepository, OrgStationRepository>();
            services.AddScoped<IOrgWorkshopRepository, OrgWorkshopRepository>();
            services.AddScoped<IOrgParkingYardRepository, OrgParkingYardRepository>();
            services.AddScoped<IVehicleCategoryRepository, VehicleCategoryRepository>();
            services.AddScoped<IRouteRepository, RouteRepository>();
            services.AddScoped<IStopRepository, StopRepository>();
            services.AddScoped<IStageRepository, StageRepository>();
            services.AddScoped<IFarePolicyRepository, FarePolicyRepository>();
            services.AddScoped<ITicketTypeRepository, TicketTypeRepository>();








            services.AddScoped<IOrgCorporationService, OrgCorporationService>();
            services.AddScoped<IOrgDivisionService, OrgDivisionService>();
            services.AddScoped<IOrgRegionService, OrgRegionService>();
            services.AddScoped<IOrgZoneService, OrgZoneService>();
            services.AddScoped<IOrgDepotService, OrgDepotService>();
            services.AddScoped<IOrgStationService, OrgStationService>();
            services.AddScoped<IOrgWorkshopService, OrgWorkshopService>();
            services.AddScoped<IOrgParkingYardService, OrgParkingYardService>();
            services.AddScoped<IVehicleCategoryService, VehicleCategoryService>();
            services.AddScoped<IRouteService, RouteService>();
            services.AddScoped<IStopService, StopService>();
            services.AddScoped<IStageService, StageService>();
            services.AddScoped<IFarePolicyService, FarePolicyService>();
            services.AddScoped<ITicketTypeService, TicketTypeService>();







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
