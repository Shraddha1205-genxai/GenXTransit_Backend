using System;

namespace GenXTransitAPI.Models.DTO_s
{
    // ✅ Main Parking Yard DTO matching UI (camelCase)
    public class OrgParkingYardDTO
    {
        // UI Properties (camelCase)
        public string? yardId { get; set; }
        public string? yardCode { get; set; }
        public string? yardName { get; set; }
        public string? regionId { get; set; }
        public string? regionCode { get; set; }
        public string? regionName { get; set; }
        public string? divisionId { get; set; }
        public string? divisionCode { get; set; }
        public string? divisionName { get; set; }
        public string? depotId { get; set; }
        public string? depotCode { get; set; }
        public string? depotName { get; set; }
        public int capacity { get; set; }
        public int occupied { get; set; }
        public bool isActive { get; set; } = true;

        // UI-specific properties
        public int availableSpots { get; set; } // Capacity - Occupied

        // Audit fields
        public int? createdBy { get; set; }
        public DateTime? createdDate { get; set; }
        public int? modifiedBy { get; set; }
        public DateTime? modifiedDate { get; set; }
        public int totalCount { get; set; }
    }

    // ✅ Request model for Insert
    public class InsertParkingYardRequest
    {
        public string? yardName { get; set; }
        public string? regionId { get; set; }
        public string? divisionId { get; set; }
        public string? depotId { get; set; }
        public int capacity { get; set; }
        public int occupied { get; set; } = 0;
        public bool isActive { get; set; } = true;
    }

    // ✅ Request model for Update
    public class UpdateParkingYardRequest
    {
        public string? yardId { get; set; }
        public string? yardName { get; set; }
        public string? regionId { get; set; }
        public string? divisionId { get; set; }
        public string? depotId { get; set; }
        public int capacity { get; set; }
        public int occupied { get; set; } = 0;
        public bool isActive { get; set; } = true;
    }

    // ✅ Request model for Delete
    public class DeleteParkingYardRequest
    {
        public string? yardId { get; set; }
    }

    // ✅ DB DTO for mapping from stored procedure
    public class OrgParkingYardDbDTO
    {
        public int? Yard_ID { get; set; }
        public string? Yard_Code { get; set; }
        public string? Yard_Name { get; set; }
        public int Capacity { get; set; }
        public int Occupied { get; set; }
        public bool IsActive { get; set; } = true;
        public int? Created_By { get; set; }
        public DateTime? Created_Date { get; set; }
        public int? Modified_By { get; set; }
        public DateTime? Modified_Date { get; set; }

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

        // Depot details
        public int? Depot_Id { get; set; }
        public string? Depot_Code { get; set; }
        public string? Depot_Name { get; set; }
        public bool? Depot_IsActive { get; set; }

        public int TotalCount { get; set; }
    }
}