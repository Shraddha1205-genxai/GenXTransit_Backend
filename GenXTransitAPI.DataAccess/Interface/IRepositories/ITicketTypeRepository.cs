using GenXTransitAPI.Models.DTO_s;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GenXTransitAPI.DataAccess.Interface.IRepositories
{
    public interface ITicketTypeRepository
    {
        Task<IEnumerable<TicketTypeDTO>> GetAllAsync(
            string? searchText,
            bool? isActive,
            int? scopeToUser,
            int pageNumber = 1,
            int pageSize = 10);

        Task<TicketTypeDTO> GetByIdAsync(int ticketId);
        Task<string> GetNextCodeAsync();
        Task<int> InsertAsync(TicketTypeDTO entity, int userId);
        Task<bool> UpdateAsync(TicketTypeDTO entity, int userId);
        Task<bool> DeleteAsync(int ticketId, int deletedBy);
    }
}