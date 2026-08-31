using GenXTransitAPI.Models.DTO_s;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GenXTransitAPI.DataAccess.Interface.IRepositories
{
    public interface IPaymentModeRepository
    {
        Task<IEnumerable<PaymentModeDTO>> GetAllAsync(
            string? searchText,
            string? modeStatus,
            bool? isActive,
            int? scopeToUser,
            int pageNumber = 1,
            int pageSize = 10);

        Task<PaymentModeDTO> GetByIdAsync(int modeId);
        Task<string> GetNextCodeAsync();
        Task<int> InsertAsync(PaymentModeDTO entity, int userId);
        Task<bool> UpdateAsync(PaymentModeDTO entity, int userId);
        Task<bool> DeleteAsync(int modeId, int deletedBy);
    }
}