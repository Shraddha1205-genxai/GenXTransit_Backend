using System;
using System.Collections.Generic;

namespace GenXTransitAPI.Models.DTO_s
{
    // ✅ Main Zone DTO matching UI
    public class OrgZoneDTO
    {
        // UI Properties (camelCase)
        public string? zoneId { get; set; }
        public string? zoneCode { get; set; }
        public string? zoneName { get; set; }
        public string? regionId { get; set; }
        public string? regionCode { get; set; }
        public string? regionName { get; set; }
        public List<string>? districts { get; set; }
        public bool isActive { get; set; } = true;

        // ✅ Added: Count of active depots in this zone
        public int depotCount { get; set; }

        // Audit fields (camelCase for UI)
        public int? createdBy { get; set; }
        public DateTime? createdDate { get; set; }
        public int? modifiedBy { get; set; }
        public DateTime? modifiedDate { get; set; }
        public int totalCount { get; set; }  // For pagination

        // ✅ Added: JSON data for child records (GetById)
        public object? depotsList { get; set; }
    }

    // ✅ Request model for Insert
    public class InsertZoneRequest
    {
        public string? zoneName { get; set; }
        public string? regionId { get; set; }
        public List<string>? districts { get; set; }
        public bool isActive { get; set; } = true;
    }

    // ✅ Request model for Update
    public class UpdateZoneRequest
    {
        public string? zoneId { get; set; }
        public string? zoneName { get; set; }
        public string? regionId { get; set; }
        public List<string>? districts { get; set; }
        public bool isActive { get; set; } = true;
    }

    // ✅ Request model for Delete
    public class DeleteZoneRequest
    {
        public string? zoneId { get; set; }
    }

    // ✅ DB DTO for mapping from stored procedure (PascalCase for DB)
    public class OrgZoneDbDTO
    {
        public int? Zone_ID { get; set; }
        public string? Zone_Code { get; set; }
        public string? Zone_Name { get; set; }
        public int? Region_ID { get; set; }
        public string? Region_Code { get; set; }
        public string? Region_Name { get; set; }
        public string? Districts { get; set; }  // Comma-separated from DB
        public bool IsActive { get; set; } = true;
        public int? Created_By { get; set; }
        public DateTime? Created_Date { get; set; }
        public int? Modified_By { get; set; }
        public DateTime? Modified_Date { get; set; }
        public int TotalCount { get; set; }
        public int DepotCount { get; set; }

        // ✅ Added: JSON data for GetById
        public string? Depots { get; set; }
    }
}