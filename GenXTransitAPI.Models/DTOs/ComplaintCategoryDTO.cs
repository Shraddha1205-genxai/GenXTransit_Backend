using System;

namespace GenXTransitAPI.Models.DTO_s
{
    // ✅ Main Complaint Category DTO matching UI (camelCase)
    public class ComplaintCategoryDTO
    {
        // UI Properties (camelCase)
        public string? complaintId { get; set; }
        public string? complaintCode { get; set; }
        public string? complaintTitle { get; set; }
        public string? description { get; set; }
        public string? complaintCategory { get; set; }
        public string? sla { get; set; }
        public bool isActive { get; set; } = true;

        // Audit fields
        public int? createdBy { get; set; }
        public DateTime? createdDate { get; set; }
        public int? modifiedBy { get; set; }
        public DateTime? modifiedDate { get; set; }
        public int totalCount { get; set; }
    }

    // ✅ Request model for Insert
    public class InsertComplaintCategoryRequest
    {
        public string? complaintTitle { get; set; }
        public string? description { get; set; }
        public string? complaintCategory { get; set; }
        public string? sla { get; set; }
        public bool isActive { get; set; } = true;
    }

    // ✅ Request model for Update
    public class UpdateComplaintCategoryRequest
    {
        public string? complaintId { get; set; }
        public string? complaintTitle { get; set; }
        public string? description { get; set; }
        public string? complaintCategory { get; set; }
        public string? sla { get; set; }
        public bool isActive { get; set; } = true;
    }

    // ✅ Request model for Delete
    public class DeleteComplaintCategoryRequest
    {
        public string? complaintId { get; set; }
    }

    // ✅ DB DTO for mapping from stored procedure
    public class ComplaintCategoryDbDTO
    {
        public int? Complaint_Id { get; set; }
        public string? Complaint_Code { get; set; }
        public string? Complaint_Title { get; set; }
        public string? Description { get; set; }
        public string? Complaint_Category { get; set; }
        public string? SLA { get; set; }
        public bool IsActive { get; set; } = true;
        public int? Created_By { get; set; }
        public DateTime? Created_Date { get; set; }
        public int? Modified_By { get; set; }
        public DateTime? Modified_Date { get; set; }
        public int TotalCount { get; set; }
    }
}