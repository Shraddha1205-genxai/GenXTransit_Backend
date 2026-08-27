using GenXTransitAPI.Models.DTO_s;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GenXTransitAPI.DataAccess.Interface.IRepositories
{
    public interface IStageRepository
    {
        Task<IEnumerable<StageDTO>> GetAllAsync(
            string? searchText,
            int? routeId,
            bool? isActive,
            int? scopeToUser,
            int pageNumber = 1,
            int pageSize = 10);

        Task<StageDTO> GetByIdAsync(int stageId);
        Task<string> GetNextCodeAsync();
        Task<int> InsertAsync(StageDTO entity, int userId);
        Task<bool> UpdateAsync(StageDTO entity, int userId);
        Task<bool> DeleteAsync(int stageId, int deletedBy);
    }
}