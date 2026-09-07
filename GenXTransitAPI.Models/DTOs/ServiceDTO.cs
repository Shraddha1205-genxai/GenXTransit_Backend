using System;

namespace GenXTransitAPI.Models.DTO_s
{
    // ✅ Main Service DTO matching UI (camelCase)
    public class ServiceDTO
    {
        // UI Properties (camelCase)
        public string? serviceId { get; set; }
        public string? serviceCode { get; set; }
        public string? fleetId { get; set; }
        public string? vehicleNumber { get; set; }
        public string? fleetStatus { get; set; }
        public string? status { get; set; } // Pending, Inprogress, Completed, Cancelled
        public bool isActive { get; set; } = true;

        // Audit fields
        public int? createdBy { get; set; }
        public DateTime? createdDate { get; set; }
        public int? modifiedBy { get; set; }
        public DateTime? modifiedDate { get; set; }
        public int totalCount { get; set; }
    }

    // ✅ Request model for Insert
    public class InsertServiceRequest
    {
        public string? fleetId { get; set; }
        public string? status { get; set; } = "Pending";
        public bool isActive { get; set; } = true;
    }

    // ✅ Request model for Update
    public class UpdateServiceRequest
    {
        public string? serviceId { get; set; }
        public string? fleetId { get; set; }
        public string? status { get; set; }
        public bool isActive { get; set; } = true;
    }

    // ✅ Request model for Delete
    public class DeleteServiceRequest
    {
        public string? serviceId { get; set; }
    }

    // ✅ DB DTO for mapping from stored procedure
    public class ServiceDbDTO
    {
        public int? Service_Id { get; set; }
        public string? Service_Code { get; set; }
        public string? Status { get; set; }
        public bool IsActive { get; set; } = true;
        public int? Created_By { get; set; }
        public DateTime? Created_Date { get; set; }
        public int? Modified_By { get; set; }
        public DateTime? Modified_Date { get; set; }

        // Fleet details
        public int? Fleet_Id { get; set; }
        public string? Vehicle_Number { get; set; }
        public string? Fleet_Status { get; set; }

        public int TotalCount { get; set; }
    }
}