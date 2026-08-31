using GenXTransitAPI.Models.DTO_s;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GenXTransitAPI.DataAccess.Interface.IRepositories
{
    public interface ISeatLayoutRepository
    {
        Task<IEnumerable<SeatLayoutDTO>> GetAllAsync(
            string? searchText,
            int? categoryId,
            bool? isActive,
            int? scopeToUser,
            int pageNumber = 1,
            int pageSize = 10);

        Task<SeatLayoutDTO> GetByIdAsync(int layoutId);
        Task<string> GetNextCodeAsync();
        Task<int> InsertAsync(SeatLayoutDTO entity, int userId);
        Task<bool> UpdateAsync(SeatLayoutDTO entity, int userId);
        Task<bool> DeleteAsync(int layoutId, int deletedBy);
    }
}