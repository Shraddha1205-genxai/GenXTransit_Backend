using System;

namespace GenXTransitAPI.Models.DTO_s
{
    // ✅ Main Tax Configuration DTO matching UI (camelCase)
    public class TaxConfigurationDTO
    {
        // UI Properties (camelCase)
        public string? taxId { get; set; }
        public string? taxCode { get; set; }
        public string? taxType { get; set; }
        public string? description { get; set; }
        public decimal rate { get; set; }
        public bool isActive { get; set; } = true;

        // Audit fields
        public int? createdBy { get; set; }
        public DateTime? createdDate { get; set; }
        public int? modifiedBy { get; set; }
        public DateTime? modifiedDate { get; set; }
        public int totalCount { get; set; }
    }

    // ✅ Request model for Insert
    public class InsertTaxConfigurationRequest
    {
        public string? taxType { get; set; }
        public string? description { get; set; }
        public decimal rate { get; set; }
        public bool isActive { get; set; } = true;
    }

    // ✅ Request model for Update
    public class UpdateTaxConfigurationRequest
    {
        public string? taxId { get; set; }
        public string? taxType { get; set; }
        public string? description { get; set; }
        public decimal rate { get; set; }
        public bool isActive { get; set; } = true;
    }

    // ✅ Request model for Delete
    public class DeleteTaxConfigurationRequest
    {
        public string? taxId { get; set; }
    }

    // ✅ DB DTO for mapping from stored procedure
    public class TaxConfigurationDbDTO
    {
        public int? Tax_Id { get; set; }
        public string? Tax_Code { get; set; }
        public string? Tax_Type { get; set; }
        public string? Description { get; set; }
        public decimal Rate { get; set; }
        public bool IsActive { get; set; } = true;
        public int? Created_By { get; set; }
        public DateTime? Created_Date { get; set; }
        public int? Modified_By { get; set; }
        public DateTime? Modified_Date { get; set; }
        public int TotalCount { get; set; }
    }
}