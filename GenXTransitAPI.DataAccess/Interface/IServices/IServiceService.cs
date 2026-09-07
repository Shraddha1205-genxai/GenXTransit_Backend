using GenXTransitAPI.Models;
using GenXTransitAPI.Models.DTO_s;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GenXTransitAPI.DataAccess.Interface.IServices
{
    public interface IServiceService
    {
        Task<ApiResponse<IEnumerable<ServiceDTO>>> GetAllAsync(
            string? searchText,
            int? fleetId,
            string? status,
            bool? isActive,
            int? scopeToUser,
            int pageNumber = 1,
            int pageSize = 10);

        Task<ApiResponse<ServiceDTO>> GetByIdAsync(int serviceId);
        Task<ApiResponse<int>> InsertAsync(ServiceDTO entity, int userId);
        Task<ApiResponse<bool>> UpdateAsync(ServiceDTO entity, int userId);
        Task<ApiResponse<bool>> DeleteAsync(int serviceId, int deletedBy);
    }
}