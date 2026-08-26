using GenXTransitAPI.Models.DTO_s;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GenXTransitAPI.DataAccess.Interface.IRepositories
{
    public interface IOrgDepotRepository
    {
        Task<IEnumerable<OrgDepotDTO>> GetAllAsync(
            string? searchText,
            int? corporationId,
            int? regionId,
            int? divisionId,
            int? zoneId,
            bool? isActive,
            int? scopeToUser,
            int pageNumber = 1,
            int pageSize = 10);

        Task<OrgDepotDTO> GetByIdAsync(int depotId);
        Task<string> GetNextCodeAsync();
        Task<int> InsertAsync(OrgDepotDTO entity, int userId);
        Task<bool> UpdateAsync(OrgDepotDTO entity, int userId);
        Task<bool> DeleteAsync(int depotId, int deletedBy);
    }
}