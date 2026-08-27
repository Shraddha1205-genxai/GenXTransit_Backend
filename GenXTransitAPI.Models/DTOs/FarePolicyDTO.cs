using System;

namespace GenXTransitAPI.Models.DTO_s
{
    // ✅ Main Fare Policy DTO matching UI (camelCase)
    public class FarePolicyDTO
    {
        // UI Properties (camelCase)
        public string? policyId { get; set; }
        public string? policyCode { get; set; }
        public string? model { get; set; }
        public string? policyStatus { get; set; }
        public string? categoryId { get; set; }
        public string? categoryCode { get; set; }
        public string? categoryName { get; set; }
        public string? routeId { get; set; }
        public string? routeCode { get; set; }
        public string? routeName { get; set; }
        public decimal baseFare { get; set; }
        public string? rateDescription { get; set; }
        public bool isActive { get; set; } = true;

        // Audit fields
        public int? createdBy { get; set; }
        public DateTime? createdDate { get; set; }
        public int? modifiedBy { get; set; }
        public DateTime? modifiedDate { get; set; }
        public int totalCount { get; set; }
    }

    // ✅ Request model for Insert
    public class InsertFarePolicyRequest
    {
        public string? model { get; set; }
        public string? policyStatus { get; set; }
        public string? categoryId { get; set; }
        public string? routeId { get; set; }
        public decimal baseFare { get; set; }
        public string? rateDescription { get; set; }
        public bool isActive { get; set; } = true;
    }

    // ✅ Request model for Update
    public class UpdateFarePolicyRequest
    {
        public string? policyId { get; set; }
        public string? model { get; set; }
        public string? policyStatus { get; set; }
        public string? categoryId { get; set; }
        public string? routeId { get; set; }
        public decimal baseFare { get; set; }
        public string? rateDescription { get; set; }
        public bool isActive { get; set; } = true;
    }

    // ✅ Request model for Delete
    public class DeleteFarePolicyRequest
    {
        public string? policyId { get; set; }
    }

    // ✅ DB DTO for mapping from stored procedure
    public class FarePolicyDbDTO
    {
        public int? Policy_Id { get; set; }
        public string? Policy_Code { get; set; }
        public string? Model { get; set; }
        public string? Policy_Status { get; set; }
        public decimal Base_Fare { get; set; }
        public string? Rate_Description { get; set; }
        public bool IsActive { get; set; } = true;
        public int? Created_By { get; set; }
        public DateTime? Created_Date { get; set; }
        public int? Modified_By { get; set; }
        public DateTime? Modified_Date { get; set; }

        // Category details
        public int? Category_Id { get; set; }
        public string? Category_Code { get; set; }
        public string? Category_Name { get; set; }
        public bool? Category_IsActive { get; set; }

        // Route details
        public int? Route_Id { get; set; }
        public string? Route_Code { get; set; }
        public string? Route_Name { get; set; }
        public bool? Route_IsActive { get; set; }

        public int TotalCount { get; set; }
    }
}