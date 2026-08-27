using System;

namespace GenXTransitAPI.Models.DTO_s
{
    // ✅ Main Ticket Type DTO matching UI (camelCase)
    public class TicketTypeDTO
    {
        // UI Properties (camelCase)
        public string? ticketId { get; set; }
        public string? ticketCode { get; set; }
        public string? ticketName { get; set; }
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
    public class InsertTicketTypeRequest
    {
        public string? ticketName { get; set; }
        public string? description { get; set; }
        public bool isActive { get; set; } = true;
    }

    // ✅ Request model for Update
    public class UpdateTicketTypeRequest
    {
        public string? ticketId { get; set; }
        public string? ticketName { get; set; }
        public string? description { get; set; }
        public bool isActive { get; set; } = true;
    }

    // ✅ Request model for Delete
    public class DeleteTicketTypeRequest
    {
        public string? ticketId { get; set; }
    }

    // ✅ DB DTO for mapping from stored procedure
    public class TicketTypeDbDTO
    {
        public int? Ticket_ID { get; set; }
        public string? Ticket_Code { get; set; }
        public string? Ticket_Name { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
        public int? Created_By { get; set; }
        public DateTime? Created_Date { get; set; }
        public int? Modified_By { get; set; }
        public DateTime? Modified_Date { get; set; }
        public int TotalCount { get; set; }
    }
}