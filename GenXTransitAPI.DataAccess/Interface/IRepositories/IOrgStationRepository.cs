using GenXTransitAPI.Models.DTO_s;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GenXTransitAPI.DataAccess.Interface.IRepositories
{
    public interface IOrgStationRepository
    {
        Task<IEnumerable<OrgStationDTO>> GetAllAsync(
            string? searchText,
            int? regionId,
            int? divisionId,
            int? depotId,
            bool? isActive,
            int? scopeToUser,
            int pageNumber = 1,
            int pageSize = 10);

        Task<OrgStationDTO> GetByIdAsync(int stationId);
        Task<string> GetNextCodeAsync();
        Task<int> InsertAsync(OrgStationDTO entity, int userId);
        Task<bool> UpdateAsync(OrgStationDTO entity, int userId);
        Task<bool> DeleteAsync(int stationId, int deletedBy);
    }
}