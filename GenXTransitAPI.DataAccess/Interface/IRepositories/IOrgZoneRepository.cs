using GenXTransitAPI.Models.DTO_s;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GenXTransitAPI.DataAccess.Interface.IRepositories
{
    public interface IOrgZoneRepository
    {
        Task<IEnumerable<OrgZoneDTO>> GetAllAsync(
            string? searchText,
            int? regionId,
            bool? isActive,
            int? scopeToUser,
            int pageNumber = 1,
            int pageSize = 10);

        Task<OrgZoneDTO> GetByIdAsync(int zoneId);
        Task<string> GetNextCodeAsync();
        Task<int> InsertAsync(OrgZoneDTO entity, int userId);
        Task<bool> UpdateAsync(OrgZoneDTO entity, int userId);
        Task<bool> DeleteAsync(int zoneId, int deletedBy);
    }
}