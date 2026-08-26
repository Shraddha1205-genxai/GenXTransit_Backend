using GenXTransitAPI.Models;
using GenXTransitAPI.Models.DTO_s;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GenXTransitAPI.DataAccess.Interface.IServices
{
    public interface IOrgStationService
    {
        Task<ApiResponse<IEnumerable<OrgStationDTO>>> GetAllAsync(
            string? searchText,
            int? regionId,
            int? divisionId,
            int? depotId,
            bool? isActive,
            int? scopeToUser,
            int pageNumber = 1,
            int pageSize = 10);

        Task<ApiResponse<OrgStationDTO>> GetByIdAsync(int stationId);
        Task<ApiResponse<string>> GetNextCodeAsync();
        Task<ApiResponse<int>> InsertAsync(OrgStationDTO entity, int userId);
        Task<ApiResponse<bool>> UpdateAsync(OrgStationDTO entity, int userId);
        Task<ApiResponse<bool>> DeleteAsync(int stationId, int deletedBy);
    }
}