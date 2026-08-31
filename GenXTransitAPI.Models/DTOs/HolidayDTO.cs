using System;

namespace GenXTransitAPI.Models.DTO_s
{
    // ✅ Main Holiday DTO matching UI (camelCase)
    public class HolidayDTO
    {
        // UI Properties (camelCase)
        public string? holidayId { get; set; }
        public string? holidayCode { get; set; }
        public string? holidayName { get; set; }
        public string? occasion { get; set; }
        public string? date { get; set; } // Date in string format for UI
        public string? description { get; set; }
        public string? type { get; set; } // National, Regional, Festival, Optional
        public bool isActive { get; set; } = true;

        // Audit fields
        public int? createdBy { get; set; }
        public DateTime? createdDate { get; set; }
        public int? modifiedBy { get; set; }
        public DateTime? modifiedDate { get; set; }
        public int totalCount { get; set; }
    }

    // ✅ Request model for Insert
    public class InsertHolidayRequest
    {
        public string? holidayName { get; set; }
        public string? occasion { get; set; }
        public string? date { get; set; } // Date in string format
        public string? description { get; set; }
        public string? type { get; set; }
        public bool isActive { get; set; } = true;
    }

    // ✅ Request model for Update
    public class UpdateHolidayRequest
    {
        public string? holidayId { get; set; }
        public string? holidayName { get; set; }
        public string? occasion { get; set; }
        public string? date { get; set; }
        public string? description { get; set; }
        public string? type { get; set; }
        public bool isActive { get; set; } = true;
    }

    // ✅ Request model for Delete
    public class DeleteHolidayRequest
    {
        public string? holidayId { get; set; }
    }

    // ✅ DB DTO for mapping from stored procedure
    public class HolidayDbDTO
    {
        public int? Holiday_Id { get; set; }
        public string? Holiday_Code { get; set; }
        public string? Holiday_Name { get; set; }
        public string? Occasion { get; set; }
        public DateTime? Date { get; set; }
        public string? Description { get; set; }
        public string? Type { get; set; }
        public bool IsActive { get; set; } = true;
        public int? Created_By { get; set; }
        public DateTime? Created_Date { get; set; }
        public int? Modified_By { get; set; }
        public DateTime? Modified_Date { get; set; }
        public int TotalCount { get; set; }
    }
}