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
    public class TripService : ITripService
    {
        private readonly ITripRepository _repo;

        public TripService(ITripRepository repo)
        {
            _repo = repo;
        }

        public async Task<ApiResponse<IEnumerable<TripDTO>>> GetAllAsync(
            string? searchText,
            int? depotId,
            int? routeId,
            int? fleetId,
            string? tripStatus,
            DateTime? startDate,
            DateTime? endDate,
            bool? isActive,
            int? scopeToUser,
            int pageNumber = 1,
            int pageSize = 10)
        {
            try
            {
                var items = await _repo.GetAllAsync(searchText, depotId, routeId, fleetId, tripStatus, startDate, endDate, isActive, scopeToUser, pageNumber, pageSize);
                var totalCount = items.FirstOrDefault()?.totalCount ?? 0;
                return ApiResponse<IEnumerable<TripDTO>>.Ok(items, null, totalCount);
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<TripDTO>>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<TripDTO>> GetByIdAsync(int tripId)
        {
            try
            {
                var item = await _repo.GetByIdAsync(tripId);
                if (item == null)
                    return ApiResponse<TripDTO>.Fail($"Trip with ID {tripId} not found.");

                return ApiResponse<TripDTO>.Ok(item);
            }
            catch (Exception ex)
            {
                return ApiResponse<TripDTO>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<int>> InsertAsync(TripDTO entity, int userId)
        {
            try
            {
                // Validations
                if (string.IsNullOrWhiteSpace(entity.depotId))
                    return ApiResponse<int>.Fail("Depot is required.");

                if (string.IsNullOrWhiteSpace(entity.routeId))
                    return ApiResponse<int>.Fail("Route is required.");

                if (string.IsNullOrWhiteSpace(entity.fleetId))
                    return ApiResponse<int>.Fail("Fleet is required.");

                if (string.IsNullOrWhiteSpace(entity.driverId))
                    return ApiResponse<int>.Fail("Driver is required.");

                if (string.IsNullOrWhiteSpace(entity.conductorId))
                    return ApiResponse<int>.Fail("Conductor is required.");

                if (string.IsNullOrWhiteSpace(entity.scheduleTime))
                    return ApiResponse<int>.Fail("Schedule Time is required.");

                // Parse IDs
                if (!int.TryParse(entity.depotId, out int depotId))
                    return ApiResponse<int>.Fail("Invalid Depot ID format.");

                if (!int.TryParse(entity.routeId, out int routeId))
                    return ApiResponse<int>.Fail("Invalid Route ID format.");

                if (!int.TryParse(entity.fleetId, out int fleetId))
                    return ApiResponse<int>.Fail("Invalid Fleet ID format.");

                if (!int.TryParse(entity.driverId, out int driverId))
                    return ApiResponse<int>.Fail("Invalid Driver ID format.");

                if (!int.TryParse(entity.conductorId, out int conductorId))
                    return ApiResponse<int>.Fail("Invalid Conductor ID format.");

                // Validate Schedule Time
                if (!DateTime.TryParse(entity.scheduleTime, out DateTime scheduleTime))
                    return ApiResponse<int>.Fail("Invalid Schedule Time format.");

                if (scheduleTime < DateTime.Now)
                    return ApiResponse<int>.Fail("Schedule Time cannot be in the past.");

                // Validate Trip Status
                var validStatuses = new[] { "Scheduled", "OnTime", "Delayed", "Completed", "Cancelled" };
                if (!string.IsNullOrWhiteSpace(entity.tripStatus) && !validStatuses.Contains(entity.tripStatus))
                    return ApiResponse<int>.Fail("Invalid Trip Status. Valid statuses are: Scheduled, OnTime, Delayed, Completed, Cancelled.");

                var id = await _repo.InsertAsync(entity, userId);
                return ApiResponse<int>.Ok(id, "Trip created successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<int>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> UpdateAsync(TripDTO entity, int userId)
        {
            try
            {
                if (string.IsNullOrEmpty(entity.tripId))
                    return ApiResponse<bool>.Fail("Trip ID is required.");

                if (!int.TryParse(entity.tripId, out int tripId))
                    return ApiResponse<bool>.Fail("Invalid Trip ID format.");

                if (string.IsNullOrWhiteSpace(entity.depotId))
                    return ApiResponse<bool>.Fail("Depot is required.");

                if (string.IsNullOrWhiteSpace(entity.routeId))
                    return ApiResponse<bool>.Fail("Route is required.");

                if (string.IsNullOrWhiteSpace(entity.fleetId))
                    return ApiResponse<bool>.Fail("Fleet is required.");

                if (string.IsNullOrWhiteSpace(entity.driverId))
                    return ApiResponse<bool>.Fail("Driver is required.");

                if (string.IsNullOrWhiteSpace(entity.conductorId))
                    return ApiResponse<bool>.Fail("Conductor is required.");

                if (string.IsNullOrWhiteSpace(entity.scheduleTime))
                    return ApiResponse<bool>.Fail("Schedule Time is required.");

                // Parse IDs
                if (!int.TryParse(entity.depotId, out int depotId))
                    return ApiResponse<bool>.Fail("Invalid Depot ID format.");

                if (!int.TryParse(entity.routeId, out int routeId))
                    return ApiResponse<bool>.Fail("Invalid Route ID format.");

                if (!int.TryParse(entity.fleetId, out int fleetId))
                    return ApiResponse<bool>.Fail("Invalid Fleet ID format.");

                if (!int.TryParse(entity.driverId, out int driverId))
                    return ApiResponse<bool>.Fail("Invalid Driver ID format.");

                if (!int.TryParse(entity.conductorId, out int conductorId))
                    return ApiResponse<bool>.Fail("Invalid Conductor ID format.");

                // Validate Schedule Time
                if (!DateTime.TryParse(entity.scheduleTime, out DateTime scheduleTime))
                    return ApiResponse<bool>.Fail("Invalid Schedule Time format.");

                // Validate Trip Status
                var validStatuses = new[] { "Scheduled", "OnTime", "Delayed", "Completed", "Cancelled" };
                if (!string.IsNullOrWhiteSpace(entity.tripStatus) && !validStatuses.Contains(entity.tripStatus))
                    return ApiResponse<bool>.Fail("Invalid Trip Status. Valid statuses are: Scheduled, OnTime, Delayed, Completed, Cancelled.");

                var success = await _repo.UpdateAsync(entity, userId);
                if (!success)
                    return ApiResponse<bool>.Fail($"Trip with ID {entity.tripId} not found.");

                return ApiResponse<bool>.Ok(true, "Trip updated successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> DeleteAsync(int tripId, int deletedBy)
        {
            try
            {
                var success = await _repo.DeleteAsync(tripId, deletedBy);
                if (!success)
                    return ApiResponse<bool>.Fail($"Trip with ID {tripId} not found or already inactive.");

                return ApiResponse<bool>.Ok(true, "Trip deleted successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.Fail(ex.Message);
            }
        }
    }
}