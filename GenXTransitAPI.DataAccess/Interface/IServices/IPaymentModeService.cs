using GenXTransitAPI.Models;
using GenXTransitAPI.Models.DTO_s;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GenXTransitAPI.DataAccess.Interface.IServices
{
    public interface IPaymentModeService
    {
        Task<ApiResponse<IEnumerable<PaymentModeDTO>>> GetAllAsync(
            string? searchText,
            string? modeStatus,
            bool? isActive,
            int? scopeToUser,
            int pageNumber = 1,
            int pageSize = 10);

        Task<ApiResponse<PaymentModeDTO>> GetByIdAsync(int modeId);
        Task<ApiResponse<string>> GetNextCodeAsync();
        Task<ApiResponse<int>> InsertAsync(PaymentModeDTO entity, int userId);
        Task<ApiResponse<bool>> UpdateAsync(PaymentModeDTO entity, int userId);
        Task<ApiResponse<bool>> DeleteAsync(int modeId, int deletedBy);
    }
}