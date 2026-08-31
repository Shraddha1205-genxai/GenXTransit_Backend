using GenXTransitAPI.Models.DTO_s;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GenXTransitAPI.DataAccess.Interface.IRepositories
{
    public interface ITaxConfigurationRepository
    {
        Task<IEnumerable<TaxConfigurationDTO>> GetAllAsync(
            string? searchText,
            string? taxType,
            decimal? rateFrom,
            decimal? rateTo,
            bool? isActive,
            int? scopeToUser,
            int pageNumber = 1,
            int pageSize = 10);

        Task<TaxConfigurationDTO> GetByIdAsync(int taxId);
        Task<string> GetNextCodeAsync();
        Task<int> InsertAsync(TaxConfigurationDTO entity, int userId);
        Task<bool> UpdateAsync(TaxConfigurationDTO entity, int userId);
        Task<bool> DeleteAsync(int taxId, int deletedBy);
    }
}