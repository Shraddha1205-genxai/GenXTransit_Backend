using GenXTransitAPI.Models;
using GenXTransitAPI.Models.DTO_s;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GenXTransitAPI.DataAccess.Interface.IServices
{
    public interface IOrgCorporationService
    {
        Task<ApiResponse<IEnumerable<OrgCorporationDTO>>> GetAllAsync(
            string? searchText,
            string? stateName,
            string? districtName,
            string? cityName,
            bool? isActive,
            int? scopeToUser,
            int pageNumber = 1,
            int pageSize = 10);

        Task<ApiResponse<OrgCorporationDTO>> GetByIdAsync(int corporationId);
        Task<ApiResponse<string>> GetNextCodeAsync();
        Task<ApiResponse<int>> InsertAsync(OrgCorporationDTO entity, int userId);
        Task<ApiResponse<bool>> UpdateAsync(OrgCorporationDTO entity, int userId);
        Task<ApiResponse<bool>> DeleteAsync(int corporationId, int deletedBy);
    }
}