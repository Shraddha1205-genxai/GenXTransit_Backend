using GenXTransitAPI.Models.DTO_s;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GenXTransitAPI.DataAccess.Interface.IRepositories
{
    public interface IOrgWorkshopRepository
    {
        Task<IEnumerable<OrgWorkshopDTO>> GetAllAsync(
            string? searchText,
            int? regionId,
            int? divisionId,
            int? depotId,
            bool? isActive,
            int? scopeToUser,
            int pageNumber = 1,
            int pageSize = 10);

        Task<OrgWorkshopDTO> GetByIdAsync(int workShopId);
        Task<string> GetNextCodeAsync();
        Task<int> InsertAsync(OrgWorkshopDTO entity, int userId);
        Task<bool> UpdateAsync(OrgWorkshopDTO entity, int userId);
        Task<bool> DeleteAsync(int workShopId, int deletedBy);
    }
}