using System;
using System.Collections.Generic;

namespace GenXTransitAPI.Models.DTO_s
{
    // ✅ Main Depot DTO matching UI (camelCase)
    public class OrgDepotDTO
    {
        // UI Properties (camelCase)
        public string? depotId { get; set; }
        public string? depotCode { get; set; }
        public string? depotName { get; set; }
        public string? regionId { get; set; }
        public string? regionCode { get; set; }
        public string? regionName { get; set; }
        public string? divisionId { get; set; }
        public string? divisionCode { get; set; }
        public string? divisionName { get; set; }
        public string? zoneId { get; set; }
        public string? zoneCode { get; set; }
        public string? zoneName { get; set; }
        public string? corpId { get; set; }
        public string? corpCode { get; set; }
        public string? corporationName { get; set; }
        public string? service { get; set; }
        public bool isActive { get; set; } = true;

        // UI-specific properties (counts)
        public int stationCount { get; set; }
        public int workshopCount { get; set; }
        public int parkingYardCount { get; set; }

        // Audit fields
        public int? createdBy { get; set; }
        public DateTime? createdDate { get; set; }
        public int? modifiedBy { get; set; }
        public DateTime? modifiedDate { get; set; }
        public int totalCount { get; set; }

        // ✅ JSON data for child records (GetById)
        public object? stations { get; set; }
        public object? workshops { get; set; }
        public object? parkingYards { get; set; }
    }

    // ✅ Request model for Insert
    public class InsertDepotRequest
    {
        public string? depotName { get; set; }
        public string? service { get; set; }
        public string? regionId { get; set; }
        public string? divisionId { get; set; }
        public string? zoneId { get; set; }
        public string? corpId { get; set; }
        public bool isActive { get; set; } = true;
    }

    // ✅ Request model for Update
    public class UpdateDepotRequest
    {
        public string? depotId { get; set; }
        public string? depotName { get; set; }
        public string? service { get; set; }
        public string? regionId { get; set; }
        public string? divisionId { get; set; }
        public string? zoneId { get; set; }
        public string? corpId { get; set; }
        public bool isActive { get; set; } = true;
    }

    // ✅ Request model for Delete
    public class DeleteDepotRequest
    {
        public string? depotId { get; set; }
    }

    // ✅ DB DTO for mapping from stored procedure
    public class OrgDepotDbDTO
    {
        public int? Depot_Id { get; set; }
        public string? Depot_Code { get; set; }
        public string? Depot_Name { get; set; }
        public string? Service { get; set; }
        public bool IsActive { get; set; } = true;
        public int? Created_By { get; set; }
        public DateTime? Created_Date { get; set; }
        public int? Modified_By { get; set; }
        public DateTime? Modified_Date { get; set; }

        // Corporation details
        public int? Corporation_Id { get; set; }
        public string? Corp_Code { get; set; }
        public string? Corporation_Name { get; set; }
        public bool? Corporation_IsActive { get; set; }

        // Region details
        public int? Region_Id { get; set; }
        public string? Region_Code { get; set; }
        public string? Region_Name { get; set; }
        public bool? Region_IsActive { get; set; }

        // Division details
        public int? Division_Id { get; set; }
        public string? Division_Code { get; set; }
        public string? Division_Name { get; set; }
        public bool? Division_IsActive { get; set; }

        // Zone details
        public int? Zone_Id { get; set; }
        public string? Zone_Code { get; set; }
        public string? Zone_Name { get; set; }
        public bool? Zone_IsActive { get; set; }

        // Counts
        public int StationCount { get; set; }
        public int WorkshopCount { get; set; }
        public int ParkingYardCount { get; set; }
        public int TotalCount { get; set; }

        // JSON data for GetById
        public string? Stations { get; set; }
        public string? Workshops { get; set; }
        public string? ParkingYards { get; set; }
    }
}