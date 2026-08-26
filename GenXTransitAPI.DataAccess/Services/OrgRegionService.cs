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
    public class OrgRegionService : IOrgRegionService
    {
        private readonly IOrgRegionRepository _repo;

        public OrgRegionService(IOrgRegionRepository repo)
        {
            _repo = repo;
        }

        public async Task<ApiResponse<IEnumerable<OrgRegionDTO>>> GetAllAsync(
            string? searchText,
            bool? isActive,
            int? scopeToUser,
            int pageNumber = 1,
            int pageSize = 10)
        {
            try
            {
                var items = await _repo.GetAllAsync(searchText, isActive, scopeToUser, pageNumber, pageSize);
                var totalCount = items.FirstOrDefault()?.totalCount ?? 0;
                return ApiResponse<IEnumerable<OrgRegionDTO>>.Ok(items, null, totalCount);
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<OrgRegionDTO>>.Fail($"Error retrieving regions: {ex.Message}");
            }
        }

        public async Task<ApiResponse<OrgRegionDTO>> GetByIdAsync(int regionId)
        {
            try
            {
                var item = await _repo.GetByIdAsync(regionId);
                if (item == null)
                    return ApiResponse<OrgRegionDTO>.Fail($"Region with ID {regionId} not found.");

                return ApiResponse<OrgRegionDTO>.Ok(item);
            }
            catch (Exception ex)
            {
                return ApiResponse<OrgRegionDTO>.Fail($"Error retrieving region: {ex.Message}");
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
                return ApiResponse<string>.Fail($"Error generating next code: {ex.Message}");
            }
        }

        public async Task<ApiResponse<int>> InsertAsync(OrgRegionDTO entity, int userId)
        {
            try
            {
                // Validations
                if (string.IsNullOrWhiteSpace(entity.regionName))
                    return ApiResponse<int>.Fail("Region Name is required.");

                var id = await _repo.InsertAsync(entity, userId);
                return ApiResponse<int>.Ok(id, "Region created successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<int>.Fail($"An error occurred while creating region. {ex.Message}");
            }
        }

        public async Task<ApiResponse<bool>> UpdateAsync(OrgRegionDTO entity, int userId)
        {
            try
            {
                // Validations
                if (string.IsNullOrEmpty(entity.regionId))
                    return ApiResponse<bool>.Fail("Region ID is required.");

                if (!int.TryParse(entity.regionId, out int id))
                    return ApiResponse<bool>.Fail("Invalid Region ID format.");

                if (string.IsNullOrWhiteSpace(entity.regionName))
                    return ApiResponse<bool>.Fail("Region Name is required.");

                var success = await _repo.UpdateAsync(entity, userId);
                if (!success)
                    return ApiResponse<bool>.Fail($"Region with ID {entity.regionId} not found.");

                return ApiResponse<bool>.Ok(true, "Region updated successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.Fail($"An error occurred while updating region. {ex.Message}");
            }
        }

        public async Task<ApiResponse<bool>> DeleteAsync(int regionId, int deletedBy)
        {
            try
            {
                var success = await _repo.DeleteAsync(regionId, deletedBy);
                if (!success)
                    return ApiResponse<bool>.Fail($"Region with ID {regionId} not found or already inactive.");

                return ApiResponse<bool>.Ok(true, "Region and all associated records deleted successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.Fail($"An error occurred while deleting region. {ex.Message}");
            }
        }
    }
}