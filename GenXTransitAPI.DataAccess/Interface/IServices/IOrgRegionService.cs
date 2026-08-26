using GenXTransitAPI.Models;
using GenXTransitAPI.Models.DTO_s;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GenXTransitAPI.DataAccess.Interface.IServices
{
    public interface IOrgRegionService
    {
        Task<ApiResponse<IEnumerable<OrgRegionDTO>>> GetAllAsync(
            string? searchText,
            bool? isActive,
            int? scopeToUser,
            int pageNumber = 1,
            int pageSize = 10);

        Task<ApiResponse<OrgRegionDTO>> GetByIdAsync(int regionId);
        Task<ApiResponse<string>> GetNextCodeAsync();
        Task<ApiResponse<int>> InsertAsync(OrgRegionDTO entity, int userId);
        Task<ApiResponse<bool>> UpdateAsync(OrgRegionDTO entity, int userId);
        Task<ApiResponse<bool>> DeleteAsync(int regionId, int deletedBy);
    }
}