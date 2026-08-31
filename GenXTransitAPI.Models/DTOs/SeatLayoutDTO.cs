using System;

namespace GenXTransitAPI.Models.DTO_s
{
    // ✅ Main Seat Layout DTO matching UI (camelCase)
    public class SeatLayoutDTO
    {
        // UI Properties (camelCase)
        public string? layoutId { get; set; }
        public string? layoutCode { get; set; }
        public string? description { get; set; }
        public string? categoryId { get; set; }
        public string? categoryCode { get; set; }
        public string? categoryName { get; set; }
        public bool isActive { get; set; } = true;

        // Audit fields
        public int? createdBy { get; set; }
        public DateTime? createdDate { get; set; }
        public int? modifiedBy { get; set; }
        public DateTime? modifiedDate { get; set; }
        public int totalCount { get; set; }
    }

    // ✅ Request model for Insert
    public class InsertSeatLayoutRequest
    {
        public string? description { get; set; }
        public string? categoryId { get; set; }
        public bool isActive { get; set; } = true;
    }

    // ✅ Request model for Update
    public class UpdateSeatLayoutRequest
    {
        public string? layoutId { get; set; }
        public string? description { get; set; }
        public string? categoryId { get; set; }
        public bool isActive { get; set; } = true;
    }

    // ✅ Request model for Delete
    public class DeleteSeatLayoutRequest
    {
        public string? layoutId { get; set; }
    }

    // ✅ DB DTO for mapping from stored procedure
    public class SeatLayoutDbDTO
    {
        public int? Layout_Id { get; set; }
        public string? Layout_Code { get; set; }
        public string? Description { get; set; }
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

        public int TotalCount { get; set; }
    }
}