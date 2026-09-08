using GenXTransitAPI.Models.DTO_s;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GenXTransitAPI.DataAccess.Interface.IRepositories
{
    public interface IServiceRepository
    {
        Task<IEnumerable<ServiceDTO>> GetAllAsync(
            string? searchText,
            int? fleetId,
            string? status,
            bool? isActive,
            int pageNumber = 1,
            int pageSize = 10);

        Task<ServiceDTO> GetByIdAsync(int serviceId);
        Task<int> InsertAsync(ServiceDTO entity, int userId);
        Task<bool> UpdateAsync(ServiceDTO entity, int userId);
        Task<bool> DeleteAsync(int serviceId, int deletedBy);
    }
}