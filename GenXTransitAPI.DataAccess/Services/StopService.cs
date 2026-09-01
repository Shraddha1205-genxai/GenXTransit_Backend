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
    public class StopService : IStopService
    {
        private readonly IStopRepository _repo;

        public StopService(IStopRepository repo)
        {
            _repo = repo;
        }

        public async Task<ApiResponse<IEnumerable<StopDTO>>> GetAllAsync(
            string? searchText,
            int? routeId,
            bool? isActive,
            int? scopeToUser,
            int pageNumber = 1,
            int pageSize = 10)
        {
            try
            {
                var items = await _repo.GetAllAsync(searchText, routeId, isActive, scopeToUser, pageNumber, pageSize);
                var totalCount = items.FirstOrDefault()?.totalCount ?? 0;
                return ApiResponse<IEnumerable<StopDTO>>.Ok(items, null, totalCount);
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<StopDTO>>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<StopDTO>> GetByIdAsync(int stopId)
        {
            try
            {
                var item = await _repo.GetByIdAsync(stopId);
                if (item == null)
                    return ApiResponse<StopDTO>.Fail($"Stop with ID {stopId} not found.");

                return ApiResponse<StopDTO>.Ok(item);
            }
            catch (Exception ex)
            {
                return ApiResponse<StopDTO>.Fail(ex.Message);
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

        public async Task<ApiResponse<int>> InsertAsync(StopDTO entity, int userId)
        {
            try
            {
                // Validations
                if (string.IsNullOrWhiteSpace(entity.stopName))
                    return ApiResponse<int>.Fail("Stop Name is required.");

                if (string.IsNullOrWhiteSpace(entity.routeId))
                    return ApiResponse<int>.Fail("Route is required.");

                if (entity.stopOrder < 1)
                    return ApiResponse<int>.Fail("Stop Order must be greater than 0.");

                // Parse IDs
                if (!int.TryParse(entity.routeId, out int routeId))
                    return ApiResponse<int>.Fail("Invalid Route ID format.");

                var id = await _repo.InsertAsync(entity, userId);
                return ApiResponse<int>.Ok(id, "Stop created successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<int>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> UpdateAsync(StopDTO entity, int userId)
        {
            try
            {
                // Validations
                if (string.IsNullOrEmpty(entity.stopId))
                    return ApiResponse<bool>.Fail("Stop ID is required.");

                if (!int.TryParse(entity.stopId, out int stopId))
                    return ApiResponse<bool>.Fail("Invalid Stop ID format.");

                if (string.IsNullOrWhiteSpace(entity.stopName))
                    return ApiResponse<bool>.Fail("Stop Name is required.");

                if (string.IsNullOrWhiteSpace(entity.routeId))
                    return ApiResponse<bool>.Fail("Route is required.");

                if (entity.stopOrder < 1)
                    return ApiResponse<bool>.Fail("Stop Order must be greater than 0.");

                // Parse IDs
                if (!int.TryParse(entity.routeId, out int routeId))
                    return ApiResponse<bool>.Fail("Invalid Route ID format.");

                var success = await _repo.UpdateAsync(entity, userId);
                if (!success)
                    return ApiResponse<bool>.Fail($"Stop with ID {entity.stopId} not found.");

                return ApiResponse<bool>.Ok(true, "Stop updated successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> DeleteAsync(int stopId, int deletedBy)
        {
            try
            {
                var success = await _repo.DeleteAsync(stopId, deletedBy);
                if (!success)
                    return ApiResponse<bool>.Fail($"Stop with ID {stopId} not found or already inactive.");

                return ApiResponse<bool>.Ok(true, "Stop deleted successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.Fail(ex.Message);
            }
        }
    }
}