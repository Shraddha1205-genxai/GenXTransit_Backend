using GenXTransitAPI.Models;
using GenXTransitAPI.Models.DTO_s;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GenXTransitAPI.DataAccess.Interface.IServices
{
    public interface IOrgParkingYardService
    {
        Task<ApiResponse<IEnumerable<OrgParkingYardDTO>>> GetAllAsync(
            string? searchText,
            int? regionId,
            int? divisionId,
            int? depotId,
            bool? isActive,
            int? scopeToUser,
            int pageNumber = 1,
            int pageSize = 10);

        Task<ApiResponse<OrgParkingYardDTO>> GetByIdAsync(int yardId);
        Task<ApiResponse<string>> GetNextCodeAsync();
        Task<ApiResponse<int>> InsertAsync(OrgParkingYardDTO entity, int userId);
        Task<ApiResponse<bool>> UpdateAsync(OrgParkingYardDTO entity, int userId);
        Task<ApiResponse<bool>> DeleteAsync(int yardId, int deletedBy);
    }
}