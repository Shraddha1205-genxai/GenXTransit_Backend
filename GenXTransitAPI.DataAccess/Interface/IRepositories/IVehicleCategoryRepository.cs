using GenXTransitAPI.Models.DTO_s;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GenXTransitAPI.DataAccess.Interface.IRepositories
{
    public interface IVehicleCategoryRepository
    {
        Task<IEnumerable<VehicleCategoryDTO>> GetAllAsync(
            string? searchText,
            string? type,
            string? vehicleClass,
            bool? isActive,
            int? scopeToUser,
            int pageNumber = 1,
            int pageSize = 10);

        Task<VehicleCategoryDTO> GetByIdAsync(int categoryId);
        Task<string> GetNextCodeAsync();
        Task<int> InsertAsync(VehicleCategoryDTO entity, int userId);
        Task<bool> UpdateAsync(VehicleCategoryDTO entity, int userId);
        Task<bool> DeleteAsync(int categoryId, int deletedBy);
    }
}