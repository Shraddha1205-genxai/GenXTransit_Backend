using GenXTransitAPI.Models.DTO_s;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GenXTransitAPI.DataAccess.Interface.IRepositories
{
    public interface IOrgDivisionRepository
    {
        Task<IEnumerable<OrgDivisionDTO>> GetAllAsync(
            string? searchText,
            int? regionId,
            bool? isActive,
            int? scopeToUser,
            int pageNumber = 1,    
            int pageSize = 10);    

        Task<OrgDivisionDTO> GetByIdAsync(int divisionId);
        Task<string> GetNextCodeAsync();
        Task<int> InsertAsync(OrgDivisionDTO entity, int userId);
        Task<bool> UpdateAsync(OrgDivisionDTO entity, int userId);
        Task<bool> DeleteAsync(int divisionId, int deletedBy);
    }
}