using GenXTransitAPI.Models;
using GenXTransitAPI.Models.DTO_s;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GenXTransitAPI.DataAccess.Interface.IServices
{
    public interface ITicketTypeService
    {
        Task<ApiResponse<IEnumerable<TicketTypeDTO>>> GetAllAsync(
            string? searchText,
            bool? isActive,
            int? scopeToUser,
            int pageNumber = 1,
            int pageSize = 10);

        Task<ApiResponse<TicketTypeDTO>> GetByIdAsync(int ticketId);
        Task<ApiResponse<string>> GetNextCodeAsync();
        Task<ApiResponse<int>> InsertAsync(TicketTypeDTO entity, int userId);
        Task<ApiResponse<bool>> UpdateAsync(TicketTypeDTO entity, int userId);
        Task<ApiResponse<bool>> DeleteAsync(int ticketId, int deletedBy);
    }
}