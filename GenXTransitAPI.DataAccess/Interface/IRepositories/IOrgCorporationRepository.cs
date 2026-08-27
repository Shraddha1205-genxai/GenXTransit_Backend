using GenXTransitAPI.Models.DTO_s;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GenXTransitAPI.DataAccess.Interface.IRepositories
{
    public interface IOrgCorporationRepository
    {
        Task<IEnumerable<OrgCorporationDTO>> GetAllAsync(
            string? searchText,
            string? stateName,
            string? districtName,
            string? cityName,
            bool? isActive,
            int? scopeToUser,
            int pageNumber = 1,
            int pageSize = 10);

        Task<OrgCorporationDTO> GetByIdAsync(int corporationId);
        Task<string> GetNextCodeAsync();
        Task<int> InsertAsync(OrgCorporationDTO entity, int userId);
        Task<bool> UpdateAsync(OrgCorporationDTO entity, int userId);
        Task<bool> DeleteAsync(int corporationId, int deletedBy);
    }
}