using System;

namespace GenXTransitAPI.Models.DTO_s
{
    // ✅ Main Payment Mode DTO matching UI (camelCase)
    public class PaymentModeDTO
    {
        // UI Properties (camelCase)
        public string? modeId { get; set; }
        public string? modeCode { get; set; }
        public string? modeName { get; set; }
        public string? modeStatus { get; set; }
        public string? description { get; set; }
        public bool isActive { get; set; } = true;

        // Audit fields
        public int? createdBy { get; set; }
        public DateTime? createdDate { get; set; }
        public int? modifiedBy { get; set; }
        public DateTime? modifiedDate { get; set; }
        public int totalCount { get; set; }
    }

    // ✅ Request model for Insert
    public class InsertPaymentModeRequest
    {
        public string? modeName { get; set; }
        public string? modeStatus { get; set; }
        public string? description { get; set; }
        public bool isActive { get; set; } = true;
    }

    // ✅ Request model for Update
    public class UpdatePaymentModeRequest
    {
        public string? modeId { get; set; }
        public string? modeName { get; set; }
        public string? modeStatus { get; set; }
        public string? description { get; set; }
        public bool isActive { get; set; } = true;
    }

    // ✅ Request model for Delete
    public class DeletePaymentModeRequest
    {
        public string? modeId { get; set; }
    }

    // ✅ DB DTO for mapping from stored procedure
    public class PaymentModeDbDTO
    {
        public int? Mode_Id { get; set; }
        public string? Mode_Code { get; set; }
        public string? Mode_Name { get; set; }
        public string? Mode_Status { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
        public int? Created_By { get; set; }
        public DateTime? Created_Date { get; set; }
        public int? Modified_By { get; set; }
        public DateTime? Modified_Date { get; set; }
        public int TotalCount { get; set; }
    }
}