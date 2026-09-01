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
    public class OrgParkingYardService : IOrgParkingYardService
    {
        private readonly IOrgParkingYardRepository _repo;

        public OrgParkingYardService(IOrgParkingYardRepository repo)
        {
            _repo = repo;
        }

        public async Task<ApiResponse<IEnumerable<OrgParkingYardDTO>>> GetAllAsync(
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
                return ApiResponse<IEnumerable<OrgParkingYardDTO>>.Ok(items, null, totalCount);
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<OrgParkingYardDTO>>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<OrgParkingYardDTO>> GetByIdAsync(int yardId)
        {
            try
            {
                var item = await _repo.GetByIdAsync(yardId);
                if (item == null)
                    return ApiResponse<OrgParkingYardDTO>.Fail($"Parking Yard with ID {yardId} not found.");

                return ApiResponse<OrgParkingYardDTO>.Ok(item);
            }
            catch (Exception ex)
            {
                return ApiResponse<OrgParkingYardDTO>.Fail(ex.Message);
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

        public async Task<ApiResponse<int>> InsertAsync(OrgParkingYardDTO entity, int userId)
        {
            try
            {
                // Validations
                if (string.IsNullOrWhiteSpace(entity.yardName))
                    return ApiResponse<int>.Fail("Parking Yard Name is required.");

                if (string.IsNullOrWhiteSpace(entity.regionId))
                    return ApiResponse<int>.Fail("Region is required.");

                if (string.IsNullOrWhiteSpace(entity.divisionId))
                    return ApiResponse<int>.Fail("Division is required.");

                if (string.IsNullOrWhiteSpace(entity.depotId))
                    return ApiResponse<int>.Fail("Depot is required.");

                if (entity.capacity < 0)
                    return ApiResponse<int>.Fail("Capacity cannot be negative.");

                if (entity.occupied < 0)
                    return ApiResponse<int>.Fail("Occupied cannot be negative.");

                if (entity.occupied > entity.capacity)
                    return ApiResponse<int>.Fail("Occupied cannot exceed Capacity.");

                // Parse IDs
                if (!int.TryParse(entity.regionId, out int regionId))
                    return ApiResponse<int>.Fail("Invalid Region ID format.");

                if (!int.TryParse(entity.divisionId, out int divisionId))
                    return ApiResponse<int>.Fail("Invalid Division ID format.");

                if (!int.TryParse(entity.depotId, out int depotId))
                    return ApiResponse<int>.Fail("Invalid Depot ID format.");

                var id = await _repo.InsertAsync(entity, userId);
                return ApiResponse<int>.Ok(id, "Parking Yard created successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<int>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> UpdateAsync(OrgParkingYardDTO entity, int userId)
        {
            try
            {
                // Validations
                if (string.IsNullOrEmpty(entity.yardId))
                    return ApiResponse<bool>.Fail("Parking Yard ID is required.");

                if (!int.TryParse(entity.yardId, out int yardId))
                    return ApiResponse<bool>.Fail("Invalid Parking Yard ID format.");

                if (string.IsNullOrWhiteSpace(entity.yardName))
                    return ApiResponse<bool>.Fail("Parking Yard Name is required.");

                if (string.IsNullOrWhiteSpace(entity.regionId))
                    return ApiResponse<bool>.Fail("Region is required.");

                if (string.IsNullOrWhiteSpace(entity.divisionId))
                    return ApiResponse<bool>.Fail("Division is required.");

                if (string.IsNullOrWhiteSpace(entity.depotId))
                    return ApiResponse<bool>.Fail("Depot is required.");

                if (entity.capacity < 0)
                    return ApiResponse<bool>.Fail("Capacity cannot be negative.");

                if (entity.occupied < 0)
                    return ApiResponse<bool>.Fail("Occupied cannot be negative.");

                if (entity.occupied > entity.capacity)
                    return ApiResponse<bool>.Fail("Occupied cannot exceed Capacity.");

                // Parse IDs
                if (!int.TryParse(entity.regionId, out int regionId))
                    return ApiResponse<bool>.Fail("Invalid Region ID format.");

                if (!int.TryParse(entity.divisionId, out int divisionId))
                    return ApiResponse<bool>.Fail("Invalid Division ID format.");

                if (!int.TryParse(entity.depotId, out int depotId))
                    return ApiResponse<bool>.Fail("Invalid Depot ID format.");

                var success = await _repo.UpdateAsync(entity, userId);
                if (!success)
                    return ApiResponse<bool>.Fail($"Parking Yard with ID {entity.yardId} not found.");

                return ApiResponse<bool>.Ok(true, "Parking Yard updated successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> DeleteAsync(int yardId, int deletedBy)
        {
            try
            {
                var success = await _repo.DeleteAsync(yardId, deletedBy);
                if (!success)
                    return ApiResponse<bool>.Fail($"Parking Yard with ID {yardId} not found or already inactive.");

                return ApiResponse<bool>.Ok(true, "Parking Yard deleted successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.Fail(ex.Message);
            }
        }
    }
}