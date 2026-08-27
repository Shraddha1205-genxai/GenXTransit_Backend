using GenXTransitAPI.Models.DTO_s;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GenXTransitAPI.DataAccess.Interface.IRepositories
{
    public interface IStopRepository
    {
        Task<IEnumerable<StopDTO>> GetAllAsync(
            string? searchText,
            int? routeId,
            bool? isActive,
            int? scopeToUser,
            int pageNumber = 1,
            int pageSize = 10);

        Task<StopDTO> GetByIdAsync(int stopId);
        Task<string> GetNextCodeAsync();
        Task<int> InsertAsync(StopDTO entity, int userId);
        Task<bool> UpdateAsync(StopDTO entity, int userId);
        Task<bool> DeleteAsync(int stopId, int deletedBy);
    }
}