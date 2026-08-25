using GenXTransitAPI.Models.DTO_s;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GenXTransitAPI.DataAccess.Interfaces.Repositories
{
    public interface IOrgRegionRepository
    {
        Task<IEnumerable<OrgRegionDTO>> GetAllAsync(
            string? searchText,
            bool? isActive,
            int? scopeToUser,
            int pageNumber = 1,    
            int pageSize = 10);    

        Task<OrgRegionDTO> GetByIdAsync(int regionId);
        Task<string> GetNextCodeAsync();
        Task<int> InsertAsync(OrgRegionDTO entity, int userId);
        Task<bool> UpdateAsync(OrgRegionDTO entity, int userId);
        Task<bool> DeleteAsync(int regionId, int deletedBy);
    }
}