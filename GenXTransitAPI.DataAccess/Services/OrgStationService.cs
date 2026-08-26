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
    public class OrgStationService : IOrgStationService
    {
        private readonly IOrgStationRepository _repo;

        public OrgStationService(IOrgStationRepository repo)
        {
            _repo = repo;
        }

        public async Task<ApiResponse<IEnumerable<OrgStationDTO>>> GetAllAsync(
            string? searchText,
            int? regionId,
            int? divisionId,
            int? depotId,
            bool? isActive,
            int? scopeToUser,
            int pageNumber = 1,
            int pageSize = 10)
        {
            try
            {
                var items = await _repo.GetAllAsync(searchText, regionId, divisionId, depotId, isActive, scopeToUser, pageNumber, pageSize);
                var totalCount = items.FirstOrDefault()?.totalCount ?? 0;
                return ApiResponse<IEnumerable<OrgStationDTO>>.Ok(items, null, totalCount);
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<OrgStationDTO>>.Fail($"Error retrieving stations: {ex.Message}");
            }
        }

        public async Task<ApiResponse<OrgStationDTO>> GetByIdAsync(int stationId)
        {
            try
            {
                var item = await _repo.GetByIdAsync(stationId);
                if (item == null)
                    return ApiResponse<OrgStationDTO>.Fail($"Station with ID {stationId} not found.");

                return ApiResponse<OrgStationDTO>.Ok(item);
            }
            catch (Exception ex)
            {
                return ApiResponse<OrgStationDTO>.Fail($"Error retrieving station: {ex.Message}");
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

        public async Task<ApiResponse<int>> InsertAsync(OrgStationDTO entity, int userId)
        {
            try
            {
                // Validations
                if (string.IsNullOrWhiteSpace(entity.stationName))
                    return ApiResponse<int>.Fail("Station Name is required.");

                if (string.IsNullOrWhiteSpace(entity.regionId))
                    return ApiResponse<int>.Fail("Region is required.");

                if (string.IsNullOrWhiteSpace(entity.divisionId))
                    return ApiResponse<int>.Fail("Division is required.");

                if (string.IsNullOrWhiteSpace(entity.depotId))
                    return ApiResponse<int>.Fail("Depot is required.");

                if (entity.platforms < 0)
                    return ApiResponse<int>.Fail("Platforms cannot be negative.");

                if (entity.dailyFootfall < 0)
                    return ApiResponse<int>.Fail("Daily Footfall cannot be negative.");

                // Parse IDs
                if (!int.TryParse(entity.regionId, out int regionId))
                    return ApiResponse<int>.Fail("Invalid Region ID format.");

                if (!int.TryParse(entity.divisionId, out int divisionId))
                    return ApiResponse<int>.Fail("Invalid Division ID format.");

                if (!int.TryParse(entity.depotId, out int depotId))
                    return ApiResponse<int>.Fail("Invalid Depot ID format.");

                var id = await _repo.InsertAsync(entity, userId);
                return ApiResponse<int>.Ok(id, "Station created successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<int>.Fail($"An error occurred while creating station. {ex.Message}");
            }
        }

        public async Task<ApiResponse<bool>> UpdateAsync(OrgStationDTO entity, int userId)
        {
            try
            {
                // Validations
                if (string.IsNullOrEmpty(entity.stationId))
                    return ApiResponse<bool>.Fail("Station ID is required.");

                if (!int.TryParse(entity.stationId, out int stationId))
                    return ApiResponse<bool>.Fail("Invalid Station ID format.");

                if (string.IsNullOrWhiteSpace(entity.stationName))
                    return ApiResponse<bool>.Fail("Station Name is required.");

                if (string.IsNullOrWhiteSpace(entity.regionId))
                    return ApiResponse<bool>.Fail("Region is required.");

                if (string.IsNullOrWhiteSpace(entity.divisionId))
                    return ApiResponse<bool>.Fail("Division is required.");

                if (string.IsNullOrWhiteSpace(entity.depotId))
                    return ApiResponse<bool>.Fail("Depot is required.");

                if (entity.platforms < 0)
                    return ApiResponse<bool>.Fail("Platforms cannot be negative.");

                if (entity.dailyFootfall < 0)
                    return ApiResponse<bool>.Fail("Daily Footfall cannot be negative.");

                // Parse IDs
                if (!int.TryParse(entity.regionId, out int regionId))
                    return ApiResponse<bool>.Fail("Invalid Region ID format.");

                if (!int.TryParse(entity.divisionId, out int divisionId))
                    return ApiResponse<bool>.Fail("Invalid Division ID format.");

                if (!int.TryParse(entity.depotId, out int depotId))
                    return ApiResponse<bool>.Fail("Invalid Depot ID format.");

                var success = await _repo.UpdateAsync(entity, userId);
                if (!success)
                    return ApiResponse<bool>.Fail($"Station with ID {entity.stationId} not found.");

                return ApiResponse<bool>.Ok(true, "Station updated successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.Fail($"An error occurred while updating station. {ex.Message}");
            }
        }

        public async Task<ApiResponse<bool>> DeleteAsync(int stationId, int deletedBy)
        {
            try
            {
                var success = await _repo.DeleteAsync(stationId, deletedBy);
                if (!success)
                    return ApiResponse<bool>.Fail($"Station with ID {stationId} not found or already inactive.");

                return ApiResponse<bool>.Ok(true, "Station deleted successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.Fail($"An error occurred while deleting station. {ex.Message}");
            }
        }
    }
}