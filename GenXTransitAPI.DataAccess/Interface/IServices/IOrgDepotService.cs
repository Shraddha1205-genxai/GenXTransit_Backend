using GenXTransitAPI.Models;
using GenXTransitAPI.Models.DTO_s;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GenXTransitAPI.DataAccess.Interface.IServices
{
    public interface IOrgDepotService
    {
        Task<ApiResponse<IEnumerable<OrgDepotDTO>>> GetAllAsync(
            string? searchText,
            int? corporationId,
            int? regionId,
            int? divisionId,
            int? zoneId,
            bool? isActive,
            int? scopeToUser,
            int pageNumber = 1,
            int pageSize = 10);

        Task<ApiResponse<OrgDepotDTO>> GetByIdAsync(int depotId);
        Task<ApiResponse<string>> GetNextCodeAsync();
        Task<ApiResponse<int>> InsertAsync(OrgDepotDTO entity, int userId);
        Task<ApiResponse<bool>> UpdateAsync(OrgDepotDTO entity, int userId);
        Task<ApiResponse<bool>> DeleteAsync(int depotId, int deletedBy);
    }
}