using GenXTransitAPI.Models;
using GenXTransitAPI.Models.DTO_s;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GenXTransitAPI.DataAccess.Interface.IServices
{
    public interface IFarePolicyService
    {
        Task<ApiResponse<IEnumerable<FarePolicyDTO>>> GetAllAsync(
            string? searchText,
            string? model,
            string? policyStatus,
            int? categoryId,
            int? routeId,
            bool? isActive,
            int? scopeToUser,
            int pageNumber = 1,
            int pageSize = 10);

        Task<ApiResponse<FarePolicyDTO>> GetByIdAsync(int policyId);
        Task<ApiResponse<string>> GetNextCodeAsync();
        Task<ApiResponse<int>> InsertAsync(FarePolicyDTO entity, int userId);
        Task<ApiResponse<bool>> UpdateAsync(FarePolicyDTO entity, int userId);
        Task<ApiResponse<bool>> DeleteAsync(int policyId, int deletedBy);
    }
}