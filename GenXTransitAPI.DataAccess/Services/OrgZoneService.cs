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
    public class OrgZoneService : IOrgZoneService
    {
        private readonly IOrgZoneRepository _repo;

        public OrgZoneService(IOrgZoneRepository repo)
        {
            _repo = repo;
        }

        public async Task<ApiResponse<IEnumerable<OrgZoneDTO>>> GetAllAsync(
            string? searchText,
            int? regionId,
            bool? isActive,
            int? scopeToUser,
            int pageNumber = 1,
            int pageSize = 10)
        {
            try
            {
                var items = await _repo.GetAllAsync(searchText, regionId, isActive, scopeToUser, pageNumber, pageSize);
                var totalCount = items.FirstOrDefault()?.TotalCount ?? 0;
                return ApiResponse<IEnumerable<OrgZoneDTO>>.Ok(items, null, totalCount);
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<OrgZoneDTO>>.Fail($"Error retrieving zones: {ex.Message}");
            }
        }

        public async Task<ApiResponse<OrgZoneDTO>> GetByIdAsync(int zoneId)
        {
            try
            {
                var item = await _repo.GetByIdAsync(zoneId);
                if (item == null)
                    return ApiResponse<OrgZoneDTO>.Fail($"Zone with ID {zoneId} not found.");

                return ApiResponse<OrgZoneDTO>.Ok(item);
            }
            catch (Exception ex)
            {
                return ApiResponse<OrgZoneDTO>.Fail($"Error retrieving zone: {ex.Message}");
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

        public async Task<ApiResponse<int>> InsertAsync(OrgZoneDTO entity, int userId)
        {
            try
            {
                // Validations
                if (string.IsNullOrWhiteSpace(entity.zoneName))
                    return ApiResponse<int>.Fail("Zone Name is required.");

                if (string.IsNullOrWhiteSpace(entity.regionId))
                    return ApiResponse<int>.Fail("Region is required.");

                var id = await _repo.InsertAsync(entity, userId);
                return ApiResponse<int>.Ok(id, "Zone created successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<int>.Fail($"An error occurred while creating zone. {ex.Message}");
            }
        }

        public async Task<ApiResponse<bool>> UpdateAsync(OrgZoneDTO entity, int userId)
        {
            try
            {
                // Validations
                if (string.IsNullOrEmpty(entity.zoneId))
                    return ApiResponse<bool>.Fail("Zone ID is required.");

                if (string.IsNullOrWhiteSpace(entity.zoneName))
                    return ApiResponse<bool>.Fail("Zone Name is required.");

                if (string.IsNullOrWhiteSpace(entity.regionId))
                    return ApiResponse<bool>.Fail("Region is required.");

                var success = await _repo.UpdateAsync(entity, userId);
                if (!success)
                    return ApiResponse<bool>.Fail($"Zone with ID {entity.zoneId} not found.");

                return ApiResponse<bool>.Ok(true, "Zone updated successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.Fail($"An error occurred while updating zone. {ex.Message}");
            }
        }

        public async Task<ApiResponse<bool>> DeleteAsync(int zoneId, int deletedBy)
        {
            try
            {
                var success = await _repo.DeleteAsync(zoneId, deletedBy);
                if (!success)
                    return ApiResponse<bool>.Fail($"Zone with ID {zoneId} not found.");

                return ApiResponse<bool>.Ok(true, "Zone deleted successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.Fail($"An error occurred while deleting zone. {ex.Message}");
            }
        }
    }
}