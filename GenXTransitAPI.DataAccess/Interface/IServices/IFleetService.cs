using GenXTransitAPI.Models;
using GenXTransitAPI.Models.DTO_s;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GenXTransitAPI.DataAccess.Interface.IServices
{
    public interface IFleetService
    {
        Task<ApiResponse<IEnumerable<FleetDTO>>> GetAllAsync(
            string? searchText,
            int? categoryId,
            int? depotId,
            string? fleetStatus,
            bool? isActive,
            int pageNumber = 1,
            int pageSize = 10);

        Task<ApiResponse<FleetDTO>> GetByIdAsync(int fleetId);
        Task<ApiResponse<int>> InsertAsync(FleetDTO entity, int userId);
        Task<ApiResponse<bool>> UpdateAsync(FleetDTO entity, int userId);
        Task<ApiResponse<bool>> DeleteAsync(int fleetId, int deletedBy);
    }
}