using System;

namespace GenXTransitAPI.Models.DTO_s
{
    // ✅ UI DTO - Matching UI interface with camelCase
    public class OrgCorporationDTO
    {
        public string? corpId { get; set; }
        public string? corpCode { get; set; }
        public string? corporationName { get; set; }
        public string? stateName { get; set; }
        public string? districtName { get; set; }
        public string? cityName { get; set; }
        public bool isActive { get; set; } = true;
        public int? createdBy { get; set; }
        public DateTime? createdDate { get; set; }
        public int? modifiedBy { get; set; }
        public DateTime? modifiedDate { get; set; }
        public bool isDeleted { get; set; } = false;
        public int? deletedBy { get; set; }
        public DateTime? deletedDate { get; set; }
        public int totalCount { get; set; }
    }

    // ✅ Request model for Insert
    public class InsertCorporationRequest
    {
        public string? corporationName { get; set; }
        public string? stateName { get; set; }
        public string? districtName { get; set; }
        public string? cityName { get; set; }
        public bool isActive { get; set; } = true;
    }

    // ✅ Request model for Update
    public class UpdateCorporationRequest
    {
        public string? corporationId { get; set; }
        public string? corporationName { get; set; }
        public string? stateName { get; set; }
        public string? districtName { get; set; }
        public string? cityName { get; set; }
        public bool isActive { get; set; } = true;
    }

    // ✅ Request model for Delete
    public class DeleteCorporationRequest
    {
        public string? corporationId { get; set; }
    }

    // ✅ DB DTO for mapping from stored procedure
    public class OrgCorporationDbDTO
    {
        public int? Corporation_Id { get; set; }
        public string? Corp_Code { get; set; }
        public string? Corporation_Name { get; set; }
        public string? State_Name { get; set; }
        public string? District_Name { get; set; }
        public string? City_Name { get; set; }
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