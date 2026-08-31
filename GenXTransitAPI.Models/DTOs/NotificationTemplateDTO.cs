using System;

namespace GenXTransitAPI.Models.DTO_s
{
    // ✅ Main Notification Template DTO matching UI (camelCase)
    public class NotificationTemplateDTO
    {
        // UI Properties (camelCase)
        public string? notificationId { get; set; }
        public string? notificationCode { get; set; }
        public string? notificationTitle { get; set; }
        public string? channel { get; set; }
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
    public class InsertNotificationTemplateRequest
    {
        public string? notificationTitle { get; set; }
        public string? channel { get; set; }
        public string? description { get; set; }
        public bool isActive { get; set; } = true;
    }

    // ✅ Request model for Update
    public class UpdateNotificationTemplateRequest
    {
        public string? notificationId { get; set; }
        public string? notificationTitle { get; set; }
        public string? channel { get; set; }
        public string? description { get; set; }
        public bool isActive { get; set; } = true;
    }

    // ✅ Request model for Delete
    public class DeleteNotificationTemplateRequest
    {
        public string? notificationId { get; set; }
    }

    // ✅ DB DTO for mapping from stored procedure
    public class NotificationTemplateDbDTO
    {
        public int? Notification_Id { get; set; }
        public string? Notification_Code { get; set; }
        public string? Notification_Title { get; set; }
        public string? Channel { get; set; }
        public string? Notification_Description { get; set; }
        public bool IsActive { get; set; } = true;
        public int? Created_By { get; set; }
        public DateTime? Created_Date { get; set; }
        public int? Modified_By { get; set; }
        public DateTime? Modified_Date { get; set; }
        public int TotalCount { get; set; }
    }
}