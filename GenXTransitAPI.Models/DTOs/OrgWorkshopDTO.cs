using System;

namespace GenXTransitAPI.Models.DTO_s
{
    // ✅ Main Workshop DTO matching UI (camelCase)
    public class OrgWorkshopDTO
    {
        // UI Properties (camelCase)
        public string? workShopId { get; set; }
        public string? workShopCode { get; set; }
        public string? workShopName { get; set; }
        public string? regionId { get; set; }
        public string? regionCode { get; set; }
        public string? regionName { get; set; }
        public string? divisionId { get; set; }
        public string? divisionCode { get; set; }
        public string? divisionName { get; set; }
        public string? depotId { get; set; }
        public string? depotCode { get; set; }
        public string? depotName { get; set; }
        public int workBays { get; set; }
        public int activeRepairJobs { get; set; }
        public bool isActive { get; set; } = true;

        // Audit fields
        public int? createdBy { get; set; }
        public DateTime? createdDate { get; set; }
        public int? modifiedBy { get; set; }
        public DateTime? modifiedDate { get; set; }
        public int totalCount { get; set; }
    }

    // ✅ Request model for Insert
    public class InsertWorkshopRequest
    {
        public string? workShopName { get; set; }
        public string? regionId { get; set; }
        public string? divisionId { get; set; }
        public string? depotId { get; set; }
        public int workBays { get; set; }
        public int activeRepairJobs { get; set; } = 0;
        public bool isActive { get; set; } = true;
    }

    // ✅ Request model for Update
    public class UpdateWorkshopRequest
    {
        public string? workShopId { get; set; }
        public string? workShopName { get; set; }
        public string? regionId { get; set; }
        public string? divisionId { get; set; }
        public string? depotId { get; set; }
        public int workBays { get; set; }
        public int activeRepairJobs { get; set; } = 0;
        public bool isActive { get; set; } = true;
    }

    // ✅ Request model for Delete
    public class DeleteWorkshopRequest
    {
        public string? workShopId { get; set; }
    }

    // ✅ DB DTO for mapping from stored procedure
    public class OrgWorkshopDbDTO
    {
        public int? WorkShop_ID { get; set; }
        public string? WorkShop_Code { get; set; }
        public string? WorkShop_Name { get; set; }
        public int Work_Bays { get; set; }
        public int Active_Repair_Jobs { get; set; }
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