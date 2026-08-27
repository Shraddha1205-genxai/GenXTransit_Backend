using GenXTransitAPI.Models.DTO_s;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GenXTransitAPI.DataAccess.Interface.IRepositories
{
    public interface IFarePolicyRepository
    {
        Task<IEnumerable<FarePolicyDTO>> GetAllAsync(
            string? searchText,
            string? model,
            string? policyStatus,
            int? categoryId,
            int? routeId,
            bool? isActive,
            int? scopeToUser,
            int pageNumber = 1,
            int pageSize = 10);

        Task<FarePolicyDTO> GetByIdAsync(int policyId);
        Task<string> GetNextCodeAsync();
        Task<int> InsertAsync(FarePolicyDTO entity, int userId);
        Task<bool> UpdateAsync(FarePolicyDTO entity, int userId);
        Task<bool> DeleteAsync(int policyId, int deletedBy);
    }
}