using GenXTransitAPI.Models.DTO_s;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GenXTransitAPI.DataAccess.Interface.IRepositories
{
    public interface IFleetRepository
    {
        Task<IEnumerable<FleetDTO>> GetAllAsync(
            string? searchText,
            int? categoryId,
            int? depotId,
            string? fleetStatus,
            bool? isActive,
            int? scopeToUser,
            int pageNumber = 1,
            int pageSize = 10);

        Task<FleetDTO> GetByIdAsync(int fleetId);
        Task<int> InsertAsync(FleetDTO entity, int userId);
        Task<bool> UpdateAsync(FleetDTO entity, int userId);
        Task<bool> DeleteAsync(int fleetId, int deletedBy);
    }
}