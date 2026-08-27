using GenXTransitAPI.Models.DTO_s;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GenXTransitAPI.DataAccess.Interface.IRepositories
{
    public interface IRouteRepository
    {
        Task<IEnumerable<RouteDTO>> GetAllAsync(
            string? searchText,
            string? service,
            string? type,
            bool? isActive,
            int? scopeToUser,
            int pageNumber = 1,
            int pageSize = 10);

        Task<RouteDTO> GetByIdAsync(int routeId);
        Task<string> GetNextCodeAsync();
        Task<int> InsertAsync(RouteDTO entity, int userId);
        Task<bool> UpdateAsync(RouteDTO entity, int userId);
        Task<bool> DeleteAsync(int routeId, int deletedBy);
    }
}