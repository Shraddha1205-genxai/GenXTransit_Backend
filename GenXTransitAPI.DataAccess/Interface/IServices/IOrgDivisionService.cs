using GenXTransitAPI.Models;
using GenXTransitAPI.Models.DTO_s;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GenXTransitAPI.DataAccess.Interface.IServices
{
    public interface IOrgDivisionService
    {
        Task<ApiResponse<IEnumerable<OrgDivisionDTO>>> GetAllAsync(
            string? searchText,
            int? regionId,
            bool? isActive,
            int? scopeToUser,
            int pageNumber = 1,
            int pageSize = 10);

        Task<ApiResponse<OrgDivisionDTO>> GetByIdAsync(int divisionId);
        Task<ApiResponse<string>> GetNextCodeAsync();
        Task<ApiResponse<int>> InsertAsync(OrgDivisionDTO entity, int userId);
        Task<ApiResponse<bool>> UpdateAsync(OrgDivisionDTO entity, int userId);
        Task<ApiResponse<bool>> DeleteAsync(int divisionId, int deletedBy);
    }
}