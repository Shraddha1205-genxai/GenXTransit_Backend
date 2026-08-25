using System;

namespace GenXTransitAPI.Models.DTO_s
{
    // ✅ Main Region DTO matching UI
    public class OrgRegionDTO
    {
        // UI Properties (camelCase)
        public string? regionId { get; set; }
        public string? regionCode { get; set; }
        public string? regionName { get; set; }
        public bool isActive { get; set; } = true;

        // UI-specific properties (not in database)
        public int divisions { get; set; }
        public int depots { get; set; }
        public int stations { get; set; }
        public int workshops { get; set; }

        // Audit fields (camelCase for UI)
        public int? createdBy { get; set; }
        public DateTime? createdDate { get; set; }
        public int? modifiedBy { get; set; }
        public DateTime? modifiedDate { get; set; }
        public bool isDeleted { get; set; } = false;
        public int? deletedBy { get; set; }
        public DateTime? deletedDate { get; set; }
        public int TotalCount { get; set; } 
    }

    // ✅ Request model for Insert
    public class InsertRegionRequest
    {
        public string? regionName { get; set; }
        public bool isActive { get; set; } = true;
    }

    // ✅ Request model for Update
    public class UpdateRegionRequest
    {
        public string? regionId { get; set; }
        public string? regionName { get; set; }
        public bool isActive { get; set; } = true;
    }

    // ✅ Request model for Delete
    public class DeleteRegionRequest
    {
        public string? regionId { get; set; }
    }

    // ✅ DTO for Database mapping (PascalCase)
    public class OrgRegionDbDTO
    {
        public int? Region_ID { get; set; }
        public string? Region_Code { get; set; }
        public string? Region_Name { get; set; }
        public bool IsActive { get; set; } = true;
        public int? Created_By { get; set; }
        public DateTime? Created_Date { get; set; }
        public int? Modified_By { get; set; }
        public DateTime? Modified_Date { get; set; }
        public bool IsDeleted { get; set; } = false;
        public int? Deleted_By { get; set; }
        public DateTime? Deleted_Date { get; set; }
        public int TotalCount { get; set; }

        public int DivisionCount { get; set; }
        public int DepotCount { get; set; }
        public int StationCount { get; set; }
        public int WorkshopCount { get; set; }
    }
}