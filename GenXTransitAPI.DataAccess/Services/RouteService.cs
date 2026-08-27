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
    public class RouteService : IRouteService
    {
        private readonly IRouteRepository _repo;

        public RouteService(IRouteRepository repo)
        {
            _repo = repo;
        }

        public async Task<ApiResponse<IEnumerable<RouteDTO>>> GetAllAsync(
            string? searchText,
            string? service,
            string? type,
            bool? isActive,
            int? scopeToUser,
            int pageNumber = 1,
            int pageSize = 10)
        {
            try
            {
                var items = await _repo.GetAllAsync(searchText, service, type, isActive, scopeToUser, pageNumber, pageSize);
                var totalCount = items.FirstOrDefault()?.totalCount ?? 0;
                return ApiResponse<IEnumerable<RouteDTO>>.Ok(items, null, totalCount);
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<RouteDTO>>.Fail($"Error retrieving routes: {ex.Message}");
            }
        }

        public async Task<ApiResponse<RouteDTO>> GetByIdAsync(int routeId)
        {
            try
            {
                var item = await _repo.GetByIdAsync(routeId);
                if (item == null)
                    return ApiResponse<RouteDTO>.Fail($"Route with ID {routeId} not found.");

                return ApiResponse<RouteDTO>.Ok(item);
            }
            catch (Exception ex)
            {
                return ApiResponse<RouteDTO>.Fail($"Error retrieving route: {ex.Message}");
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

        public async Task<ApiResponse<int>> InsertAsync(RouteDTO entity, int userId)
        {
            try
            {
                // Validations
                if (string.IsNullOrWhiteSpace(entity.routeName))
                    return ApiResponse<int>.Fail("Route Name is required.");

                if (string.IsNullOrWhiteSpace(entity.service))
                    return ApiResponse<int>.Fail("Service is required.");

                if (string.IsNullOrWhiteSpace(entity.fromStationId))
                    return ApiResponse<int>.Fail("From Station is required.");

                if (string.IsNullOrWhiteSpace(entity.toStationId))
                    return ApiResponse<int>.Fail("To Station is required.");

                if (string.IsNullOrWhiteSpace(entity.type))
                    return ApiResponse<int>.Fail("Type is required.");

                if (entity.distance <= 0)
                    return ApiResponse<int>.Fail("Distance must be greater than 0.");

                if (string.IsNullOrWhiteSpace(entity.fareModel))
                    return ApiResponse<int>.Fail("Fare Model is required.");

                if (entity.duration == null || entity.duration == TimeSpan.Zero)
                    return ApiResponse<int>.Fail("Duration is required.");

                // Parse IDs
                if (!int.TryParse(entity.fromStationId, out int fromStationId))
                    return ApiResponse<int>.Fail("Invalid From Station ID format.");

                if (!int.TryParse(entity.toStationId, out int toStationId))
                    return ApiResponse<int>.Fail("Invalid To Station ID format.");

                // Check if From and To stations are different
                if (fromStationId == toStationId)
                    return ApiResponse<int>.Fail("From and To stations cannot be the same.");

                var id = await _repo.InsertAsync(entity, userId);
                return ApiResponse<int>.Ok(id, "Route created successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<int>.Fail($"An error occurred while creating route. {ex.Message}");
            }
        }

        public async Task<ApiResponse<bool>> UpdateAsync(RouteDTO entity, int userId)
        {
            try
            {
                // Validations
                if (string.IsNullOrEmpty(entity.routeId))
                    return ApiResponse<bool>.Fail("Route ID is required.");

                if (!int.TryParse(entity.routeId, out int routeId))
                    return ApiResponse<bool>.Fail("Invalid Route ID format.");

                if (string.IsNullOrWhiteSpace(entity.routeName))
                    return ApiResponse<bool>.Fail("Route Name is required.");

                if (string.IsNullOrWhiteSpace(entity.service))
                    return ApiResponse<bool>.Fail("Service is required.");

                if (string.IsNullOrWhiteSpace(entity.fromStationId))
                    return ApiResponse<bool>.Fail("From Station is required.");

                if (string.IsNullOrWhiteSpace(entity.toStationId))
                    return ApiResponse<bool>.Fail("To Station is required.");

                if (string.IsNullOrWhiteSpace(entity.type))
                    return ApiResponse<bool>.Fail("Type is required.");

                if (entity.distance <= 0)
                    return ApiResponse<bool>.Fail("Distance must be greater than 0.");

                if (string.IsNullOrWhiteSpace(entity.fareModel))
                    return ApiResponse<bool>.Fail("Fare Model is required.");

                if (entity.duration == null || entity.duration == TimeSpan.Zero)
                    return ApiResponse<bool>.Fail("Duration is required.");

                // Parse IDs
                if (!int.TryParse(entity.fromStationId, out int fromStationId))
                    return ApiResponse<bool>.Fail("Invalid From Station ID format.");

                if (!int.TryParse(entity.toStationId, out int toStationId))
                    return ApiResponse<bool>.Fail("Invalid To Station ID format.");

                // Check if From and To stations are different
                if (fromStationId == toStationId)
                    return ApiResponse<bool>.Fail("From and To stations cannot be the same.");

                var success = await _repo.UpdateAsync(entity, userId);
                if (!success)
                    return ApiResponse<bool>.Fail($"Route with ID {entity.routeId} not found.");

                return ApiResponse<bool>.Ok(true, "Route updated successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.Fail($"An error occurred while updating route. {ex.Message}");
            }
        }

        public async Task<ApiResponse<bool>> DeleteAsync(int routeId, int deletedBy)
        {
            try
            {
                var success = await _repo.DeleteAsync(routeId, deletedBy);
                if (!success)
                    return ApiResponse<bool>.Fail($"Route with ID {routeId} not found or already inactive.");

                return ApiResponse<bool>.Ok(true, "Route deleted successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.Fail($"An error occurred while deleting route. {ex.Message}");
            }
        }
    }
}