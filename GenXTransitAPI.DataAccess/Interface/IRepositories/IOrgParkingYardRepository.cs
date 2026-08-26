using GenXTransitAPI.Models.DTO_s;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GenXTransitAPI.DataAccess.Interface.IRepositories
{
    public interface IOrgParkingYardRepository
    {
        Task<IEnumerable<OrgParkingYardDTO>> GetAllAsync(
            string? searchText,
            int? regionId,
            int? divisionId,
            int? depotId,
            bool? isActive,
            int? scopeToUser,
            int pageNumber = 1,
            int pageSize = 10);

        Task<OrgParkingYardDTO> GetByIdAsync(int yardId);
        Task<string> GetNextCodeAsync();
        Task<int> InsertAsync(OrgParkingYardDTO entity, int userId);
        Task<bool> UpdateAsync(OrgParkingYardDTO entity, int userId);
        Task<bool> DeleteAsync(int yardId, int deletedBy);
    }
}