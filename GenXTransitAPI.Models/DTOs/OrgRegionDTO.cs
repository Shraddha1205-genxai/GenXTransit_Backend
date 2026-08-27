using System;
using System.Collections.Generic;

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
        public int zoneCount { get; set; } // ✅ Added: Count of active zones

        // Audit fields (camelCase for UI)
        public int? createdBy { get; set; }
        public DateTime? createdDate { get; set; }
        public int? modifiedBy { get; set; }
        public DateTime? modifiedDate { get; set; }
        public int totalCount { get; set; }

        // ✅ Added: JSON data for child records (GetById)
        public object? divisionsList { get; set; }
        public object? zonesList { get; set; }
        public object? depotsList { get; set; }
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
        public int TotalCount { get; set; }

        // Count properties from GetAll
        public int DivisionCount { get; set; }
        public int ZoneCount { get; set; } // ✅ Added
        public int DepotCount { get; set; }
        public int StationCount { get; set; }
        public int WorkshopCount { get; set; }

        // ✅ Added: JSON data for GetById
        public string? Divisions { get; set; }
        public string? Zones { get; set; }
        public string? Depots { get; set; }
    }
}