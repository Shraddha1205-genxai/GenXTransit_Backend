using System;

namespace GenXTransitAPI.Models.DTO_s
{
    // ✅ Main Vehicle Category DTO matching UI (camelCase)
    public class VehicleCategoryDTO
    {
        // UI Properties (camelCase)
        public string? categoryId { get; set; }
        public string? categoryCode { get; set; }
        public string? categoryName { get; set; }
        public int capacity { get; set; }
        public string? type { get; set; }
        public string? @class { get; set; } // ✅ 'class' is a reserved keyword, use @class
        public bool isActive { get; set; } = true;

        // Audit fields
        public int? createdBy { get; set; }
        public DateTime? createdDate { get; set; }
        public int? modifiedBy { get; set; }
        public DateTime? modifiedDate { get; set; }

        // ✅ For pagination - only used in GetAll, default 0 for single record
        public int totalCount { get; set; } = 0;
    }

    // ✅ Request model for Insert
    public class InsertVehicleCategoryRequest
    {
        public string? categoryName { get; set; }
        public int capacity { get; set; }
        public string? type { get; set; }
        public string? @class { get; set; }
        public bool isActive { get; set; } = true;
    }

    // ✅ Request model for Update
    public class UpdateVehicleCategoryRequest
    {
        public string? categoryId { get; set; }
        public string? categoryName { get; set; }
        public int capacity { get; set; }
        public string? type { get; set; }
        public string? @class { get; set; }
        public bool isActive { get; set; } = true;
    }

    // ✅ Request model for Delete
    public class DeleteVehicleCategoryRequest
    {
        public string? categoryId { get; set; }
    }

    // ✅ DB DTO for mapping from stored procedure
    public class VehicleCategoryDbDTO
    {
        public int? Category_Id { get; set; }
        public string? Category_Code { get; set; }
        public string? Category_Name { get; set; }
        public int Capacity { get; set; }
        public string? Type { get; set; }
        public string? Vehicle_Class { get; set; }
        public bool IsActive { get; set; } = true;
        public int? Created_By { get; set; }
        public DateTime? Created_Date { get; set; }
        public int? Modified_By { get; set; }
        public DateTime? Modified_Date { get; set; }
        public int TotalCount { get; set; } // ✅ Only populated in GetAll
    }
}