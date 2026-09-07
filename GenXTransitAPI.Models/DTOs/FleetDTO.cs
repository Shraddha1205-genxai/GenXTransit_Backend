using System;
using System.Collections.Generic;

namespace GenXTransitAPI.Models.DTO_s
{
    // ✅ Main Fleet DTO matching UI (camelCase)
    public class FleetDTO
    {
        // UI Properties (camelCase)
        public string? fleetId { get; set; }
        public string? vehicleNumber { get; set; }
        public string? categoryId { get; set; }
        public string? categoryCode { get; set; }
        public string? categoryName { get; set; }
        public string? depotId { get; set; }
        public string? depotCode { get; set; }
        public string? depotName { get; set; }
        public string? seriesType { get; set; }
        public string? fleetStatus { get; set; }
        public List<FleetDocumentDTO>? docExpiry { get; set; }
        public bool isActive { get; set; } = true;

        // Audit fields
        public int? createdBy { get; set; }
        public DateTime? createdDate { get; set; }
        public int? modifiedBy { get; set; }
        public DateTime? modifiedDate { get; set; }
        public int totalCount { get; set; }
        public string? serviceId { get; set; }
        public string? serviceStatus { get; set; } // Pending, Inprogress, Completed, Cancelled
        public int? serviceCount { get; set; } // Total services for this fleet
    }

    // ✅ Fleet Document DTO
    public class FleetDocumentDTO
    {
        public string? docType { get; set; }
        public string? docExpiryDate { get; set; }
    }

    // ✅ Request model for Insert
    public class InsertFleetRequest
    {
        public string? vehicleNumber { get; set; }
        public string? categoryId { get; set; }
        public string? seriesType { get; set; }
        public string? depotId { get; set; }
        public string? fleetStatus { get; set; } = "Available";
        public List<FleetDocumentDTO>? docExpiry { get; set; }
        public bool isActive { get; set; } = true;
    }

    // ✅ Request model for Update
    public class UpdateFleetRequest
    {
        public string? fleetId { get; set; }
        public string? vehicleNumber { get; set; }
        public string? categoryId { get; set; }
        public string? seriesType { get; set; }
        public string? depotId { get; set; }
        public string? fleetStatus { get; set; }
        public List<FleetDocumentDTO>? docExpiry { get; set; }
        public bool isActive { get; set; } = true;
    }

    // ✅ Request model for Delete
    public class DeleteFleetRequest
    {
        public string? fleetId { get; set; }
    }

    // ✅ DB DTO for mapping from stored procedure
    public class FleetDbDTO
    {
        public int? Fleet_Id { get; set; }
        public string? Vehicle_Number { get; set; }
        public string? Series_Type { get; set; }
        public string? Fleet_Status { get; set; }
        public string? Doc_Expiry { get; set; }
        public bool IsActive { get; set; } = true;
        public int? Created_By { get; set; }
        public DateTime? Created_Date { get; set; }
        public int? Modified_By { get; set; }
        public DateTime? Modified_Date { get; set; }

        // Category details
        public int? Category_Id { get; set; }
        public string? Category_Code { get; set; }
        public string? Category_Name { get; set; }
        public string? Category_Type { get; set; }

        // Depot details
        public int? Depot_Id { get; set; }
        public string? Depot_Code { get; set; }
        public string? Depot_Name { get; set; }

        public int TotalCount { get; set; }
    }
}