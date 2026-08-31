using GenXTransitAPI.Models.DTO_s;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GenXTransitAPI.DataAccess.Interface.IRepositories
{
    public interface IComplaintCategoryRepository
    {
        Task<IEnumerable<ComplaintCategoryDTO>> GetAllAsync(
            string? searchText,
            string? complaintCategory,
            string? sla,
            bool? isActive,
            int? scopeToUser,
            int pageNumber = 1,
            int pageSize = 10);

        Task<ComplaintCategoryDTO> GetByIdAsync(int complaintId);
        Task<string> GetNextCodeAsync();
        Task<int> InsertAsync(ComplaintCategoryDTO entity, int userId);
        Task<bool> UpdateAsync(ComplaintCategoryDTO entity, int userId);
        Task<bool> DeleteAsync(int complaintId, int deletedBy);
    }
}