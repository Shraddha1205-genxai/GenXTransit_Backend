using GenXTransitAPI.Models;
using GenXTransitAPI.Models.DTO_s;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GenXTransitAPI.DataAccess.Interface.IServices
{
    public interface IStageService
    {
        Task<ApiResponse<IEnumerable<StageDTO>>> GetAllAsync(
            string? searchText,
            int? routeId,
            bool? isActive,
            int? scopeToUser,
            int pageNumber = 1,
            int pageSize = 10);

        Task<ApiResponse<StageDTO>> GetByIdAsync(int stageId);
        Task<ApiResponse<string>> GetNextCodeAsync();
        Task<ApiResponse<int>> InsertAsync(StageDTO entity, int userId);
        Task<ApiResponse<bool>> UpdateAsync(StageDTO entity, int userId);
        Task<ApiResponse<bool>> DeleteAsync(int stageId, int deletedBy);
    }
}