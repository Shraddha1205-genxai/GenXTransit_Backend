using System;
using System.Collections.Generic;

namespace GenXTransitAPI.Models.DTO_s
{
    // ✅ Main Trip DTO matching UI (camelCase)
    public class TripDTO
    {
        // UI Properties (camelCase)
        public string? tripId { get; set; }
        public string? tripCode { get; set; }
        public string? routeId { get; set; }
        public string? routeCode { get; set; }
        public string? routeName { get; set; }
        public string? fleetId { get; set; }
        public string? vehicleNumber { get; set; }
        public string? fleetStatus { get; set; }
        public string? driverId { get; set; }
        public string? driverName { get; set; }
        public string? conductorId { get; set; }
        public string? conductorName { get; set; }
        public string? scheduleTime { get; set; }
        public string? actualTime { get; set; }
        public string? tripStatus { get; set; } // Scheduled, OnTime, Delayed, Completed, Cancelled
        public bool isActive { get; set; } = true;

        // Audit fields
        public int? createdBy { get; set; }
        public DateTime? createdDate { get; set; }
        public int? modifiedBy { get; set; }
        public DateTime? modifiedDate { get; set; }
        public int totalCount { get; set; }
    }

    // ✅ Request model for Insert
    public class InsertTripRequest
    {
        public string? routeId { get; set; }
        public string? fleetId { get; set; }
        public string? driverId { get; set; }
        public string? conductorId { get; set; }
        public string? scheduleTime { get; set; }
        public string? actualTime { get; set; }
        public string? tripStatus { get; set; } = "Scheduled";
        public bool isActive { get; set; } = true;
    }

    // ✅ Request model for Update
    public class UpdateTripRequest
    {
        public string? tripId { get; set; }
        public string? routeId { get; set; }
        public string? fleetId { get; set; }
        public string? driverId { get; set; }
        public string? conductorId { get; set; }
        public string? scheduleTime { get; set; }
        public string? actualTime { get; set; }
        public string? tripStatus { get; set; }
        public bool isActive { get; set; } = true;
    }

    // ✅ Request model for Delete
    public class DeleteTripRequest
    {
        public string? tripId { get; set; }
    }

    // ✅ DB DTO for mapping from stored procedure
    public class TripDbDTO
    {
        public int? Trip_Id { get; set; }
        public string? Trip_Code { get; set; }
        public DateTime? Schedule_Time { get; set; }
        public DateTime? Actual_Time { get; set; }
        public string? Trip_Status { get; set; }
        public bool IsActive { get; set; } = true;
        public int? Created_By { get; set; }
        public DateTime? Created_Date { get; set; }
        public int? Modified_By { get; set; }
        public DateTime? Modified_Date { get; set; }

        // Route details
        public int? Route_Id { get; set; }
        public string? Route_Code { get; set; }
        public string? Route_Name { get; set; }

        // Fleet details
        public int? Fleet_Id { get; set; }
        public string? Vehicle_Number { get; set; }
        public string? Fleet_Status { get; set; }

        // Driver details (from Users table)
        public int? UserId_Driver { get; set; }
        public string? DriverName { get; set; }

        // Conductor details (from Users table)
        public int? UserId_Conductor { get; set; }
        public string? ConductorName { get; set; }

        public int TotalCount { get; set; }
    }
}