using GenXTransitAPI.Models;
using GenXTransitAPI.Models.DTO_s;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GenXTransitAPI.DataAccess.Interface.IServices
{
    public interface ITaxConfigurationService
    {
        Task<ApiResponse<IEnumerable<TaxConfigurationDTO>>> GetAllAsync(
            string? searchText,
            string? taxType,
            decimal? rateFrom,
            decimal? rateTo,
            bool? isActive,
            int? scopeToUser,
            int pageNumber = 1,
            int pageSize = 10);

        Task<ApiResponse<TaxConfigurationDTO>> GetByIdAsync(int taxId);
        Task<ApiResponse<string>> GetNextCodeAsync();
        Task<ApiResponse<int>> InsertAsync(TaxConfigurationDTO entity, int userId);
        Task<ApiResponse<bool>> UpdateAsync(TaxConfigurationDTO entity, int userId);
        Task<ApiResponse<bool>> DeleteAsync(int taxId, int deletedBy);
    }
}