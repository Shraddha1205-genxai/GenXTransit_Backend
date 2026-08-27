using GenXTransitAPI.Models;
using GenXTransitAPI.Models.DTO_s;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GenXTransitAPI.DataAccess.Interface.IServices
{
    public interface IRouteService
    {
        Task<ApiResponse<IEnumerable<RouteDTO>>> GetAllAsync(
            string? searchText,
            string? service,
            string? type,
            bool? isActive,
            int? scopeToUser,
            int pageNumber = 1,
            int pageSize = 10);

        Task<ApiResponse<RouteDTO>> GetByIdAsync(int routeId);
        Task<ApiResponse<string>> GetNextCodeAsync();
        Task<ApiResponse<int>> InsertAsync(RouteDTO entity, int userId);
        Task<ApiResponse<bool>> UpdateAsync(RouteDTO entity, int userId);
        Task<ApiResponse<bool>> DeleteAsync(int routeId, int deletedBy);
    }
}