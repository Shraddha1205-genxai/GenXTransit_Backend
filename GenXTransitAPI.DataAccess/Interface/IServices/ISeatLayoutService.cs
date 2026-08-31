using GenXTransitAPI.Models;
using GenXTransitAPI.Models.DTO_s;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GenXTransitAPI.DataAccess.Interface.IServices
{
    public interface ISeatLayoutService
    {
        Task<ApiResponse<IEnumerable<SeatLayoutDTO>>> GetAllAsync(
            string? searchText,
            int? categoryId,
            bool? isActive,
            int? scopeToUser,
            int pageNumber = 1,
            int pageSize = 10);

        Task<ApiResponse<SeatLayoutDTO>> GetByIdAsync(int layoutId);
        Task<ApiResponse<string>> GetNextCodeAsync();
        Task<ApiResponse<int>> InsertAsync(SeatLayoutDTO entity, int userId);
        Task<ApiResponse<bool>> UpdateAsync(SeatLayoutDTO entity, int userId);
        Task<ApiResponse<bool>> DeleteAsync(int layoutId, int deletedBy);
    }
}