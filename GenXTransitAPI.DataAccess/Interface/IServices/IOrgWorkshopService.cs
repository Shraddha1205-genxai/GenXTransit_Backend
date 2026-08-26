using GenXTransitAPI.Models;
using GenXTransitAPI.Models.DTO_s;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GenXTransitAPI.DataAccess.Interface.IServices
{
    public interface IOrgWorkshopService
    {
        Task<ApiResponse<IEnumerable<OrgWorkshopDTO>>> GetAllAsync(
            string? searchText,
            int? regionId,
            int? divisionId,
            int? depotId,
            bool? isActive,
            int? scopeToUser,
            int pageNumber = 1,
            int pageSize = 10);

        Task<ApiResponse<OrgWorkshopDTO>> GetByIdAsync(int workShopId);
        Task<ApiResponse<string>> GetNextCodeAsync();
        Task<ApiResponse<int>> InsertAsync(OrgWorkshopDTO entity, int userId);
        Task<ApiResponse<bool>> UpdateAsync(OrgWorkshopDTO entity, int userId);
        Task<ApiResponse<bool>> DeleteAsync(int workShopId, int deletedBy);
    }
}