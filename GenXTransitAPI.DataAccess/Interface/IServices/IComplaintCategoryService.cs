using GenXTransitAPI.Models;
using GenXTransitAPI.Models.DTO_s;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GenXTransitAPI.DataAccess.Interface.IServices
{
    public interface IComplaintCategoryService
    {
        Task<ApiResponse<IEnumerable<ComplaintCategoryDTO>>> GetAllAsync(
            string? searchText,
            string? complaintCategory,
            string? sla,
            bool? isActive,
            int? scopeToUser,
            int pageNumber = 1,
            int pageSize = 10);

        Task<ApiResponse<ComplaintCategoryDTO>> GetByIdAsync(int complaintId);
        Task<ApiResponse<string>> GetNextCodeAsync();
        Task<ApiResponse<int>> InsertAsync(ComplaintCategoryDTO entity, int userId);
        Task<ApiResponse<bool>> UpdateAsync(ComplaintCategoryDTO entity, int userId);
        Task<ApiResponse<bool>> DeleteAsync(int complaintId, int deletedBy);
    }
}