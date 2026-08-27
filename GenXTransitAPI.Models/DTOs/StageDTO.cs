using System;

namespace GenXTransitAPI.Models.DTO_s
{
    // ✅ Main Stage DTO matching UI (camelCase)
    public class StageDTO
    {
        // UI Properties (camelCase)
        public string? stageId { get; set; }
        public string? stageCode { get; set; }
        public string? stageName { get; set; }
        public string? routeId { get; set; }
        public string? routeCode { get; set; }
        public string? routeName { get; set; }
        public string? sectionFromId { get; set; }
        public string? sectionFromCode { get; set; }
        public string? sectionFromName { get; set; }
        public string? sectionToId { get; set; }
        public string? sectionToCode { get; set; }
        public string? sectionToName { get; set; }
        public decimal distance { get; set; }
        public bool isActive { get; set; } = true;

        // Audit fields
        public int? createdBy { get; set; }
        public DateTime? createdDate { get; set; }
        public int? modifiedBy { get; set; }
        public DateTime? modifiedDate { get; set; }
        public int totalCount { get; set; }
    }

    // ✅ Request model for Insert
    public class InsertStageRequest
    {
        public string? stageName { get; set; }
        public string? routeId { get; set; }
        public string? sectionFromId { get; set; }
        public string? sectionToId { get; set; }
        public decimal distance { get; set; }
        public bool isActive { get; set; } = true;
    }

    // ✅ Request model for Update
    public class UpdateStageRequest
    {
        public string? stageId { get; set; }
        public string? stageName { get; set; }
        public string? routeId { get; set; }
        public string? sectionFromId { get; set; }
        public string? sectionToId { get; set; }
        public decimal distance { get; set; }
        public bool isActive { get; set; } = true;
    }

    // ✅ Request model for Delete
    public class DeleteStageRequest
    {
        public string? stageId { get; set; }
    }

    // ✅ DB DTO for mapping from stored procedure
    public class StageDbDTO
    {
        public int? Stage_Id { get; set; }
        public string? Stage_Code { get; set; }
        public string? Stage_Name { get; set; }
        public decimal Distance { get; set; }
        public bool IsActive { get; set; } = true;
        public int? Created_By { get; set; }
        public DateTime? Created_Date { get; set; }
        public int? Modified_By { get; set; }
        public DateTime? Modified_Date { get; set; }

        // Route details
        public int? Route_Id { get; set; }
        public string? Route_Code { get; set; }
        public string? Route_Name { get; set; }
        public bool? Route_IsActive { get; set; }

        // Section From Stop details
        public int? Section_From_Id { get; set; }
        public string? SectionFromCode { get; set; }
        public string? SectionFromName { get; set; }
        public int? SectionFromSequence { get; set; }

        // Section To Stop details
        public int? Section_To_Id { get; set; }
        public string? SectionToCode { get; set; }
        public string? SectionToName { get; set; }
        public int? SectionToSequence { get; set; }

        public int TotalCount { get; set; }
    }
}