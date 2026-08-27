using GenXTransitAPI.Models;
using GenXTransitAPI.Models.DTO_s;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GenXTransitAPI.DataAccess.Interface.IServices
{
    public interface IVehicleCategoryService
    {
        Task<ApiResponse<IEnumerable<VehicleCategoryDTO>>> GetAllAsync(
            string? searchText,
            string? type,
            string? vehicleClass,
            bool? isActive,
            int? scopeToUser,
            int pageNumber = 1,
            int pageSize = 10);

        Task<ApiResponse<VehicleCategoryDTO>> GetByIdAsync(int categoryId);
        Task<ApiResponse<string>> GetNextCodeAsync();
        Task<ApiResponse<int>> InsertAsync(VehicleCategoryDTO entity, int userId);
        Task<ApiResponse<bool>> UpdateAsync(VehicleCategoryDTO entity, int userId);
        Task<ApiResponse<bool>> DeleteAsync(int categoryId, int deletedBy);
    }
}