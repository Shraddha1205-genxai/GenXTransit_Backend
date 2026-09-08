using GenXTransitAPI.DataAccess.Interface.IRepositories;
using GenXTransitAPI.DataAccess.Interface.IServices;
using GenXTransitAPI.Models;
using GenXTransitAPI.Models.DTO_s;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GenXTransitAPI.DataAccess.Services
{
    public class FleetService : IFleetService
    {
        private readonly IFleetRepository _repo;

        // Vehicle Number Regex Pattern
        private const string VehicleNumberPattern = @"^[A-Z]{2}-[0-9]{2}-[A-Z]{2}-[0-9]{4}$";

        public FleetService(IFleetRepository repo)
        {
            _repo = repo;
        }

        // Helper method to validate vehicle number
        private bool IsValidVehicleNumber(string vehicleNumber)
        {
            if (string.IsNullOrWhiteSpace(vehicleNumber))
                return false;

            return Regex.IsMatch(vehicleNumber, VehicleNumberPattern);
        }

        public async Task<ApiResponse<IEnumerable<FleetDTO>>> GetAllAsync(
            string? searchText,
            int? categoryId,
            int? depotId,
            string? fleetStatus,
            bool? isActive,
            int pageNumber = 1,
            int pageSize = 10)
        {
            try
            {
                var items = await _repo.GetAllAsync(searchText, categoryId, depotId, fleetStatus, isActive, pageNumber, pageSize);
                var totalCount = items.FirstOrDefault()?.totalCount ?? 0;
                return ApiResponse<IEnumerable<FleetDTO>>.Ok(items, null, totalCount);
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<FleetDTO>>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<FleetDTO>> GetByIdAsync(int fleetId)
        {
            try
            {
                var item = await _repo.GetByIdAsync(fleetId);
                if (item == null)
                    return ApiResponse<FleetDTO>.Fail($"Fleet with ID {fleetId} not found.");

                return ApiResponse<FleetDTO>.Ok(item);
            }
            catch (Exception ex)
            {
                return ApiResponse<FleetDTO>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<int>> InsertAsync(FleetDTO entity, int userId)
        {
            try
            {
                // Validate Vehicle Number format
                if (string.IsNullOrWhiteSpace(entity.vehicleNumber))
                    return ApiResponse<int>.Fail("Vehicle Number is required.");

                if (!IsValidVehicleNumber(entity.vehicleNumber))
                    return ApiResponse<int>.Fail("Invalid vehicle number format. Expected format: XX-99-XX-9999 (e.g., MH-12-AB-1234)");

                if (string.IsNullOrWhiteSpace(entity.categoryId))
                    return ApiResponse<int>.Fail("Vehicle Category is required.");

                if (string.IsNullOrWhiteSpace(entity.seriesType))
                    return ApiResponse<int>.Fail("Series Type is required.");

                if (string.IsNullOrWhiteSpace(entity.depotId))
                    return ApiResponse<int>.Fail("Depot is required.");

                // Validate Series Type
                var validSeriesTypes = new[] { "BH", "State" };
                if (!validSeriesTypes.Contains(entity.seriesType))
                    return ApiResponse<int>.Fail("Invalid Series Type. Valid types are: BH, State.");

                // Validate IDs
                if (!int.TryParse(entity.categoryId, out int categoryId))
                    return ApiResponse<int>.Fail("Invalid Category ID format.");

                if (!int.TryParse(entity.depotId, out int depotId))
                    return ApiResponse<int>.Fail("Invalid Depot ID format.");

                var id = await _repo.InsertAsync(entity, userId);
                return ApiResponse<int>.Ok(id, "Fleet vehicle created successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<int>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> UpdateAsync(FleetDTO entity, int userId)
        {
            try
            {
                if (string.IsNullOrEmpty(entity.fleetId))
                    return ApiResponse<bool>.Fail("Fleet ID is required.");

                if (!int.TryParse(entity.fleetId, out int fleetId))
                    return ApiResponse<bool>.Fail("Invalid Fleet ID format.");

                if (string.IsNullOrWhiteSpace(entity.vehicleNumber))
                    return ApiResponse<bool>.Fail("Vehicle Number is required.");

                if (!IsValidVehicleNumber(entity.vehicleNumber))
                    return ApiResponse<bool>.Fail("Invalid vehicle number format. Expected format: XX-99-XX-9999 (e.g., MH-12-AB-1234)");

                if (string.IsNullOrWhiteSpace(entity.categoryId))
                    return ApiResponse<bool>.Fail("Vehicle Category is required.");

                if (string.IsNullOrWhiteSpace(entity.seriesType))
                    return ApiResponse<bool>.Fail("Series Type is required.");

                if (string.IsNullOrWhiteSpace(entity.depotId))
                    return ApiResponse<bool>.Fail("Depot is required.");

                // Validate Series Type
                var validSeriesTypes = new[] { "BH", "State" };
                if (!validSeriesTypes.Contains(entity.seriesType))
                    return ApiResponse<bool>.Fail("Invalid Series Type. Valid types are: BH, State.");

                // Validate IDs
                if (!int.TryParse(entity.categoryId, out int categoryId))
                    return ApiResponse<bool>.Fail("Invalid Category ID format.");

                if (!int.TryParse(entity.depotId, out int depotId))
                    return ApiResponse<bool>.Fail("Invalid Depot ID format.");

                var success = await _repo.UpdateAsync(entity, userId);
                if (!success)
                    return ApiResponse<bool>.Fail($"Fleet with ID {entity.fleetId} not found.");

                return ApiResponse<bool>.Ok(true, "Fleet vehicle updated successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> DeleteAsync(int fleetId, int deletedBy)
        {
            try
            {
                var success = await _repo.DeleteAsync(fleetId, deletedBy);
                if (!success)
                    return ApiResponse<bool>.Fail($"Fleet with ID {fleetId} not found or already inactive.");

                return ApiResponse<bool>.Ok(true, "Fleet vehicle deleted successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.Fail(ex.Message);
            }
        }
    }
}