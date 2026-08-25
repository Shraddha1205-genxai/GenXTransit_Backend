using GenXTransitAPI.DataAccess.Data;
using GenXTransitAPI.DataAccess.Interfaces.Repositories;
using GenXTransitAPI.DataAccess.Interfaces.Services;
using GenXTransitAPI.DataAccess.Repositories;
using GenXTransitAPI.DataAccess.Services;

namespace GenXTransitAPI.Middleware
{
    public static class repositoryServiceExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            // ─── DI registrations ─────────────────────────────────────────────────────────
            services.AddSingleton<DBHelper>();

            //  -- CORPORATION
            services.AddScoped<IOrgCorporationRepository, OrgCorporationRepository>();
            services.AddScoped<IOrgCorporationService, OrgCorporationService>();

            //  -- REGION
            services.AddScoped<IOrgRegionRepository, OrgRegionRepository>();
            services.AddScoped<IOrgRegionService, OrgRegionService>();

            //  -- DIVISION
            services.AddScoped<IOrgDivisionRepository, OrgDivisionRepository>();
            services.AddScoped<IOrgDivisionService, OrgDivisionService>();

            return services;
        }
    }
}