using GenXTransitAPI.Models;
using GenXTransitAPI.Models.DTO_s;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GenXTransitAPI.DataAccess.Interface.IServices
{
    public interface IOrgZoneService
    {
        Task<ApiResponse<IEnumerable<OrgZoneDTO>>> GetAllAsync(
            string? searchText,
            int? regionId,
            bool? isActive,
            int? scopeToUser,
            int pageNumber = 1,
            int pageSize = 10);

        Task<ApiResponse<OrgZoneDTO>> GetByIdAsync(int zoneId);
        Task<ApiResponse<string>> GetNextCodeAsync();
        Task<ApiResponse<int>> InsertAsync(OrgZoneDTO entity, int userId);
        Task<ApiResponse<bool>> UpdateAsync(OrgZoneDTO entity, int userId);
        Task<ApiResponse<bool>> DeleteAsync(int zoneId, int deletedBy);
    }
}