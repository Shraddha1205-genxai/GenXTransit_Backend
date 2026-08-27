using GenXTransitAPI.Models;
using GenXTransitAPI.Models.DTO_s;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GenXTransitAPI.DataAccess.Interface.IServices
{
    public interface IStopService
    {
        Task<ApiResponse<IEnumerable<StopDTO>>> GetAllAsync(
            string? searchText,
            int? routeId,
            bool? isActive,
            int? scopeToUser,
            int pageNumber = 1,
            int pageSize = 10);

        Task<ApiResponse<StopDTO>> GetByIdAsync(int stopId);
        Task<ApiResponse<string>> GetNextCodeAsync();
        Task<ApiResponse<int>> InsertAsync(StopDTO entity, int userId);
        Task<ApiResponse<bool>> UpdateAsync(StopDTO entity, int userId);
        Task<ApiResponse<bool>> DeleteAsync(int stopId, int deletedBy);
    }
}