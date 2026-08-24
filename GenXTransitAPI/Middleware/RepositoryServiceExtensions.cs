using Microsoft.AspNetCore.Authorization;

namespace GenXTransitAPI.Middleware
{
    public class RepositoryServiceExtensions
    {
        //public static IServiceCollection AddRepositories(this IServiceCollection services)
        //{

        //    // ─── DI registrations ─────────────────────────────────────────────────────────
        //    services.AddSingleton<DBHelper>();

        //    //  --DATAACCESS
        //    services.AddScoped<IAuthService, AuthService>();
        //    services.AddScoped<IEmailSender, EmailSender>();
        //    services.AddScoped<IEmailService, EmailService>();
        //    services.AddScoped<IAllocationBatchService, AllocationBatchService>();
        //    services.AddScoped<IAllocationService, AllocationService>();
        //    services.AddScoped<IAllocationRepository, AllocationRepository>();
        //    services.AddScoped<IAllocationBatchRepository, AllocationBatchRepository>();
        //    services.AddScoped<IAssetService, AssetService>();
        //    services.AddScoped<IAssetRepository, AssetRepository>();
        //    services.AddScoped<IAuditRepository, AuditRepository>();
        //    services.AddScoped<IAuditService, AuditService>();
        //    services.AddScoped<IDashboardService, DashboardService>();
        //    services.AddScoped<IDashboardRepository, DashboardRepository>();
        //    services.AddScoped<IVendorRepository, VendorRepository>();
        //    services.AddScoped<IVendorService, VendorService>();
        //    services.AddScoped<IAssetTypeRepository, AssetTypeRepository>();
        //    services.AddScoped<IAssetTypeService, AssetTypeService>();
        //    services.AddScoped<IProjectService, ProjectService>();
        //    services.AddScoped<IProjectRepository, ProjectRepository>();
        //    services.AddScoped<IRoleRepository, RoleRepository>();
        //    services.AddScoped<IRoleService, RoleService>();
        //    services.AddScoped<IScreenRepository, ScreenRepository>();
        //    services.AddScoped<IScreenService, ScreenService>();
        //    services.AddScoped<IOEMRepository, OEMRepository>();
        //    services.AddScoped<IOEMService, OEMService>();
        //    services.AddScoped<IStockRepository, StockRepository>();
        //    services.AddScoped<IStockService, StockService>();
        //    services.AddScoped<IAuthorizationRepository, AuthorizationRepository>();
        //    services.AddScoped<IAuthorizationService, AuthorizationService>();
        //    services.AddScoped<IClientRepository, ClientRepository>();
        //    services.AddScoped<IClientService, ClientService>();
        //    services.AddScoped<IOutwardService, OutwardService>();
        //    services.AddScoped<IOutwardRepository, OutwardRepository>();
        //    services.AddScoped<IInwardService, InwardService>();
        //    services.AddScoped<IInwardRepository, InwardRepository>();
        //    services.AddScoped<IRepairService, RepairService>();
        //    services.AddScoped<IRepairRepository, RepairRepository>();
        //    services.AddScoped<IDisposalService, DisposalService>();
        //    services.AddScoped<IDisposalRepository, DisposalRepository>();
        //    services.AddScoped<IReportRepository, ReportRepository>();
        //    services.AddScoped<IReportService, ReportService>();
        //    services.AddScoped<IRequestService, RequestService>();
        //    services.AddScoped<IRequestRepository, RequestRepository>();
        //    services.AddScoped<IDepotRepository, DepotRepository>();
        //    services.AddScoped<IDepotService, DepotService>();
        //    services.AddScoped<IProjectManagerRepository, ProjectManagerRepository>();
        //    services.AddScoped<IProjectManagerService, ProjectManagerService>();
        //    services.AddScoped<ITransferRepository, TransferRepository>();
        //    services.AddScoped<ITransferService, TransferService>();
        //    services.AddScoped<IAssetReturnRepository, AssetReturnRepository>();
        //    services.AddScoped<IUserService, UserService>();
        //    services.AddScoped<IUserRepository, UserRepository>();
        //    services.AddScoped<IAssetReturnService, AssetReturnService>();
        //    services.AddScoped<IWarrantyRepository, WarrantyRepository>();
        //    services.AddScoped<IWarrantyService, WarrantyService>();
        //    services.AddScoped<INotificationRepository, NotificationRepository>();
        //    services.AddScoped<INotificationService, NotificationService>();
        //    services.AddScoped<ILocationManagerService, LocationManagerService>();
        //    services.AddScoped<ILocationManagerRepository, LocationManagerRepository>();
        //    services.AddScoped<ILocationService, LocationService>();
        //    services.AddScoped<ILocationRepository, LocationRepository>();
        //    services.AddScoped<IMaintenanceRepository, MaintenanceRepository>();
        //    services.AddScoped<IMaintenanceService, MaintenanceService>();
        //    services.AddScoped<IJwtService, JwtService>();

        //    services.AddHostedService<NotificationBackgroundService>();
        //    services.AddScoped<IFirebaseService, FirebaseService>();

        //    services.AddScoped<IUserDeviceRepository, UserDeviceRepository>();
        //    services.AddScoped<IUserDeviceService, UserDeviceService>();
        //    services.AddScoped<IUserDeviceService, UserDeviceService>();

        //    return services;
        //}
    }
}
