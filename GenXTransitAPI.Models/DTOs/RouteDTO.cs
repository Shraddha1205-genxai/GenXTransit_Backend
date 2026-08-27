using System;

namespace GenXTransitAPI.Models.DTO_s
{
    // ✅ Main Route DTO matching UI (camelCase)
    public class RouteDTO
    {
        // UI Properties (camelCase)
        public string? routeId { get; set; }
        public string? routeCode { get; set; }
        public string? routeName { get; set; }
        public string? service { get; set; }
        public string? regionId { get; set; }
        public string? regionCode { get; set; }
        public string? regionName { get; set; }
        public string? fromStationId { get; set; }
        public string? fromStationCode { get; set; }
        public string? fromStationName { get; set; }
        public string? toStationId { get; set; }
        public string? toStationCode { get; set; }
        public string? toStationName { get; set; }
        public string? type { get; set; }
        public decimal distance { get; set; }
        public string? fareModel { get; set; }
        public TimeSpan? duration { get; set; }
        public bool isActive { get; set; } = true;

        // Audit fields
        public int? createdBy { get; set; }
        public DateTime? createdDate { get; set; }
        public int? modifiedBy { get; set; }
        public DateTime? modifiedDate { get; set; }
        public int totalCount { get; set; }
    }

    // ✅ Request model for Insert
    public class InsertRouteRequest
    {
        public string? routeName { get; set; }
        public string? service { get; set; }
        public string? fromStationId { get; set; }
        public string? toStationId { get; set; }
        public string? type { get; set; }
        public decimal distance { get; set; }
        public string? fareModel { get; set; }
        public TimeSpan? duration { get; set; }
        public bool isActive { get; set; } = true;
    }

    // ✅ Request model for Update
    public class UpdateRouteRequest
    {
        public string? routeId { get; set; }
        public string? routeName { get; set; }
        public string? service { get; set; }
        public string? fromStationId { get; set; }
        public string? toStationId { get; set; }
        public string? type { get; set; }
        public decimal distance { get; set; }
        public string? fareModel { get; set; }
        public TimeSpan? duration { get; set; }
        public bool isActive { get; set; } = true;
    }

    // ✅ Request model for Delete
    public class DeleteRouteRequest
    {
        public string? routeId { get; set; }
    }

    // ✅ DB DTO for mapping from stored procedure
    public class RouteDbDTO
    {
        public int? Route_Id { get; set; }
        public string? Route_Code { get; set; }
        public string? Route_Name { get; set; }
        public string? Service { get; set; }
        public string? Type { get; set; }
        public decimal Distance { get; set; }
        public string? Fare_Model { get; set; }
        public TimeSpan? Duration { get; set; }
        public bool IsActive { get; set; } = true;
        public int? Created_By { get; set; }
        public DateTime? Created_Date { get; set; }
        public int? Modified_By { get; set; }
        public DateTime? Modified_Date { get; set; }

        // From Station details
        public int? FromStationId { get; set; }
        public string? FromStationCode { get; set; }
        public string? FromStationName { get; set; }

        // To Station details
        public int? ToStationId { get; set; }
        public string? ToStationCode { get; set; }
        public string? ToStationName { get; set; }

        // Region details
        public int? RegionId { get; set; }
        public string? RegionCode { get; set; }
        public string? RegionName { get; set; }

        public int TotalCount { get; set; }
    }
}