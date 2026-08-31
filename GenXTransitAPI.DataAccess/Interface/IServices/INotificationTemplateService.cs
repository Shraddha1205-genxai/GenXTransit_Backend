using GenXTransitAPI.Models;
using GenXTransitAPI.Models.DTO_s;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GenXTransitAPI.DataAccess.Interface.IServices
{
    public interface INotificationTemplateService
    {
        Task<ApiResponse<IEnumerable<NotificationTemplateDTO>>> GetAllAsync(
            string? searchText,
            string? channel,
            bool? isActive,
            int? scopeToUser,
            int pageNumber = 1,
            int pageSize = 10);

        Task<ApiResponse<NotificationTemplateDTO>> GetByIdAsync(int notificationId);
        Task<ApiResponse<string>> GetNextCodeAsync();
        Task<ApiResponse<int>> InsertAsync(NotificationTemplateDTO entity, int userId);
        Task<ApiResponse<bool>> UpdateAsync(NotificationTemplateDTO entity, int userId);
        Task<ApiResponse<bool>> DeleteAsync(int notificationId, int deletedBy);
    }
}