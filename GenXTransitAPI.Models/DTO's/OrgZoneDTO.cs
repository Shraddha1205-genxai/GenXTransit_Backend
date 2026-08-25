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

        // Audit fields (camelCase for UI)
        public int? createdBy { get; set; }
        public DateTime? createdDate { get; set; }
        public int? modifiedBy { get; set; }
        public DateTime? modifiedDate { get; set; }
        public bool isDeleted { get; set; } = false;
        public int? deletedBy { get; set; }
        public DateTime? deletedDate { get; set; }
        public int TotalCount { get; set; }  // For pagination
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
        public bool IsDeleted { get; set; } = false;
        public int? Deleted_By { get; set; }
        public DateTime? Deleted_Date { get; set; }
        public int TotalCount { get; set; }
    }
}