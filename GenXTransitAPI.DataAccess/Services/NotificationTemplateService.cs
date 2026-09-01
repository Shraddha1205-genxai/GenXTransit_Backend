using GenXTransitAPI.DataAccess.Interface.IRepositories;
using GenXTransitAPI.DataAccess.Interface.IServices;
using GenXTransitAPI.Models;
using GenXTransitAPI.Models.DTO_s;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GenXTransitAPI.DataAccess.Services
{
    public class NotificationTemplateService : INotificationTemplateService
    {
        private readonly INotificationTemplateRepository _repo;

        public NotificationTemplateService(INotificationTemplateRepository repo)
        {
            _repo = repo;
        }

        public async Task<ApiResponse<IEnumerable<NotificationTemplateDTO>>> GetAllAsync(
            string? searchText,
            string? channel,
            bool? isActive,
            int? scopeToUser,
            int pageNumber = 1,
            int pageSize = 10)
        {
            try
            {
                var items = await _repo.GetAllAsync(searchText, channel, isActive, scopeToUser, pageNumber, pageSize);
                var totalCount = items.FirstOrDefault()?.totalCount ?? 0;
                return ApiResponse<IEnumerable<NotificationTemplateDTO>>.Ok(items, null, totalCount);
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<NotificationTemplateDTO>>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<NotificationTemplateDTO>> GetByIdAsync(int notificationId)
        {
            try
            {
                var item = await _repo.GetByIdAsync(notificationId);
                if (item == null)
                    return ApiResponse<NotificationTemplateDTO>.Fail($"Notification template with ID {notificationId} not found.");

                return ApiResponse<NotificationTemplateDTO>.Ok(item);
            }
            catch (Exception ex)
            {
                return ApiResponse<NotificationTemplateDTO>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<string>> GetNextCodeAsync()
        {
            try
            {
                var nextCode = await _repo.GetNextCodeAsync();
                return ApiResponse<string>.Ok(nextCode);
            }
            catch (Exception ex)
            {
                return ApiResponse<string>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<int>> InsertAsync(NotificationTemplateDTO entity, int userId)
        {
            try
            {
                // Validations
                if (string.IsNullOrWhiteSpace(entity.notificationTitle))
                    return ApiResponse<int>.Fail("Notification Title is required.");

                if (string.IsNullOrWhiteSpace(entity.channel))
                    return ApiResponse<int>.Fail("Channel is required.");

                // Validate channel
                var validChannels = new[] { "Email", "SMS", "Push", "InApp" };
                if (!validChannels.Contains(entity.channel))
                    return ApiResponse<int>.Fail("Invalid Channel. Valid channels are: Email, SMS, Push, InApp.");

                var id = await _repo.InsertAsync(entity, userId);
                return ApiResponse<int>.Ok(id, "Notification template created successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<int>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> UpdateAsync(NotificationTemplateDTO entity, int userId)
        {
            try
            {
                // Validations
                if (string.IsNullOrEmpty(entity.notificationId))
                    return ApiResponse<bool>.Fail("Notification ID is required.");

                if (!int.TryParse(entity.notificationId, out int notificationId))
                    return ApiResponse<bool>.Fail("Invalid Notification ID format.");

                if (string.IsNullOrWhiteSpace(entity.notificationTitle))
                    return ApiResponse<bool>.Fail("Notification Title is required.");

                if (string.IsNullOrWhiteSpace(entity.channel))
                    return ApiResponse<bool>.Fail("Channel is required.");

                // Validate channel
                var validChannels = new[] { "Email", "SMS", "Push", "InApp" };
                if (!validChannels.Contains(entity.channel))
                    return ApiResponse<bool>.Fail("Invalid Channel. Valid channels are: Email, SMS, Push, InApp.");

                var success = await _repo.UpdateAsync(entity, userId);
                if (!success)
                    return ApiResponse<bool>.Fail($"Notification template with ID {entity.notificationId} not found.");

                return ApiResponse<bool>.Ok(true, "Notification template updated successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> DeleteAsync(int notificationId, int deletedBy)
        {
            try
            {
                var success = await _repo.DeleteAsync(notificationId, deletedBy);
                if (!success)
                    return ApiResponse<bool>.Fail($"Notification template with ID {notificationId} not found or already inactive.");

                return ApiResponse<bool>.Ok(true, "Notification template deleted successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.Fail(ex.Message);
            }
        }
    }
}