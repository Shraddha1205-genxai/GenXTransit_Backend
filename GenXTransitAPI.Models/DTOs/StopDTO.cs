using System;

namespace GenXTransitAPI.Models.DTO_s
{
    // ✅ Main Stop DTO matching UI (camelCase)
    public class StopDTO
    {
        // UI Properties (camelCase)
        public string? stopId { get; set; }
        public string? stopCode { get; set; }
        public string? stopName { get; set; }
        public string? routeId { get; set; }
        public string? routeCode { get; set; }
        public string? routeName { get; set; }
        public int stopOrder { get; set; }
        public bool isActive { get; set; } = true;

        // Audit fields
        public int? createdBy { get; set; }
        public DateTime? createdDate { get; set; }
        public int? modifiedBy { get; set; }
        public DateTime? modifiedDate { get; set; }
        public int totalCount { get; set; }
    }

    // ✅ Request model for Insert
    public class InsertStopRequest
    {
        public string? stopName { get; set; }
        public string? routeId { get; set; }
        public int stopOrder { get; set; }
        public bool isActive { get; set; } = true;
    }

    // ✅ Request model for Update
    public class UpdateStopRequest
    {
        public string? stopId { get; set; }
        public string? stopName { get; set; }
        public string? routeId { get; set; }
        public int stopOrder { get; set; }
        public bool isActive { get; set; } = true;
    }

    // ✅ Request model for Delete
    public class DeleteStopRequest
    {
        public string? stopId { get; set; }
    }

    // ✅ DB DTO for mapping from stored procedure
    public class StopDbDTO
    {
        public int? Stop_Id { get; set; }
        public string? Stop_Code { get; set; }
        public string? Stop_Name { get; set; }
        public int Sequence { get; set; }
        public bool IsActive { get; set; } = true;
        public int? Created_By { get; set; }
        public DateTime? Created_Date { get; set; }
        public int? Modified_By { get; set; }
        public DateTime? Modified_Date { get; set; }

        // Route details
        public int? Route_Id { get; set; }
        public string? Route_Code { get; set; }
        public string? Route_Name { get; set; }
        public bool? Route_IsActive { get; set; }

        public int TotalCount { get; set; }
    }
}