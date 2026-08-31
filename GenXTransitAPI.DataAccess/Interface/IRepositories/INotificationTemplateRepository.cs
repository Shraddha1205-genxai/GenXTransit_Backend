using GenXTransitAPI.Models.DTO_s;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GenXTransitAPI.DataAccess.Interface.IRepositories
{
    public interface INotificationTemplateRepository
    {
        Task<IEnumerable<NotificationTemplateDTO>> GetAllAsync(
            string? searchText,
            string? channel,
            bool? isActive,
            int? scopeToUser,
            int pageNumber = 1,
            int pageSize = 10);

        Task<NotificationTemplateDTO> GetByIdAsync(int notificationId);
        Task<string> GetNextCodeAsync();
        Task<int> InsertAsync(NotificationTemplateDTO entity, int userId);
        Task<bool> UpdateAsync(NotificationTemplateDTO entity, int userId);
        Task<bool> DeleteAsync(int notificationId, int deletedBy);
    }
}