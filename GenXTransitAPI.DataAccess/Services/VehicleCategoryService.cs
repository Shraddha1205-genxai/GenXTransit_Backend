using GenXTransitAPI.DataAccess.Interface.IRepositories;
using GenXTransitAPI.DataAccess.Interface.IServices;
using GenXTransitAPI.Models;
using GenXTransitAPI.Models.DTO_s;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GenXTransitAPI.DataAccess.Services
{
    public class VehicleCategoryService : IVehicleCategoryService
    {
        private readonly IVehicleCategoryRepository _repo;

        public VehicleCategoryService(IVehicleCategoryRepository repo)
        {
            _repo = repo;
        }

        public async Task<ApiResponse<IEnumerable<VehicleCategoryDTO>>> GetAllAsync(
            string? searchText,
            string? type,
            string? vehicleClass,
            bool? isActive,
            int? scopeToUser,
            int pageNumber = 1,
            int pageSize = 10)
        {
            try
            {
                var items = await _repo.GetAllAsync(searchText, type, vehicleClass, isActive, scopeToUser, pageNumber, pageSize);
                var totalCount = items.FirstOrDefault()?.totalCount ?? 0;
                return ApiResponse<IEnumerable<VehicleCategoryDTO>>.Ok(items, null, totalCount);
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<VehicleCategoryDTO>>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<VehicleCategoryDTO>> GetByIdAsync(int categoryId)
        {
            try
            {
                var item = await _repo.GetByIdAsync(categoryId);
                if (item == null)
                    return ApiResponse<VehicleCategoryDTO>.Fail($"Vehicle category with ID {categoryId} not found.");

                return ApiResponse<VehicleCategoryDTO>.Ok(item);
            }
            catch (Exception ex)
            {
                return ApiResponse<VehicleCategoryDTO>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<string>> GetNextCodeAsync()
        {
            try
            {
                var nextCode = await _repo.GetNextCodeAsync();
                return ApiResponse<string>.Ok(nextCode);
            }
            catch (Exception ex)
            {
                return ApiResponse<string>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<int>> InsertAsync(VehicleCategoryDTO entity, int userId)
        {
            try
            {
                // Validations
                if (string.IsNullOrWhiteSpace(entity.categoryName))
                    return ApiResponse<int>.Fail("Category Name is required.");

                if (entity.capacity < 0)
                    return ApiResponse<int>.Fail("Capacity cannot be negative.");

                if (string.IsNullOrWhiteSpace(entity.type))
                    return ApiResponse<int>.Fail("Type is required.");

                if (string.IsNullOrWhiteSpace(entity.@class))
                    return ApiResponse<int>.Fail("Vehicle Class is required.");

                var id = await _repo.InsertAsync(entity, userId);
                return ApiResponse<int>.Ok(id, "Vehicle category created successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<int>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> UpdateAsync(VehicleCategoryDTO entity, int userId)
        {
            try
            {
                // Validations
                if (string.IsNullOrEmpty(entity.categoryId))
                    return ApiResponse<bool>.Fail("Category ID is required.");

                if (!int.TryParse(entity.categoryId, out int categoryId))
                    return ApiResponse<bool>.Fail("Invalid Category ID format.");

                if (string.IsNullOrWhiteSpace(entity.categoryName))
                    return ApiResponse<bool>.Fail("Category Name is required.");

                if (entity.capacity < 0)
                    return ApiResponse<bool>.Fail("Capacity cannot be negative.");

                if (string.IsNullOrWhiteSpace(entity.type))
                    return ApiResponse<bool>.Fail("Type is required.");

                if (string.IsNullOrWhiteSpace(entity.@class))
                    return ApiResponse<bool>.Fail("Vehicle Class is required.");

                var success = await _repo.UpdateAsync(entity, userId);
                if (!success)
                    return ApiResponse<bool>.Fail($"Vehicle category with ID {entity.categoryId} not found.");

                return ApiResponse<bool>.Ok(true, "Vehicle category updated successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> DeleteAsync(int categoryId, int deletedBy)
        {
            try
            {
                var success = await _repo.DeleteAsync(categoryId, deletedBy);
                if (!success)
                    return ApiResponse<bool>.Fail($"Vehicle category with ID {categoryId} not found or already inactive.");

                return ApiResponse<bool>.Ok(true, "Vehicle category deleted successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.Fail(ex.Message);
            }
        }
    }
}