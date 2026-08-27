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
    public class StageService : IStageService
    {
        private readonly IStageRepository _repo;

        public StageService(IStageRepository repo)
        {
            _repo = repo;
        }

        public async Task<ApiResponse<IEnumerable<StageDTO>>> GetAllAsync(
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
                return ApiResponse<IEnumerable<StageDTO>>.Ok(items, null, totalCount);
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<StageDTO>>.Fail($"Error retrieving stages: {ex.Message}");
            }
        }

        public async Task<ApiResponse<StageDTO>> GetByIdAsync(int stageId)
        {
            try
            {
                var item = await _repo.GetByIdAsync(stageId);
                if (item == null)
                    return ApiResponse<StageDTO>.Fail($"Stage with ID {stageId} not found.");

                return ApiResponse<StageDTO>.Ok(item);
            }
            catch (Exception ex)
            {
                return ApiResponse<StageDTO>.Fail($"Error retrieving stage: {ex.Message}");
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

        public async Task<ApiResponse<int>> InsertAsync(StageDTO entity, int userId)
        {
            try
            {
                // Validations
                if (string.IsNullOrWhiteSpace(entity.stageName))
                    return ApiResponse<int>.Fail("Stage Name is required.");

                if (string.IsNullOrWhiteSpace(entity.routeId))
                    return ApiResponse<int>.Fail("Route is required.");

                if (string.IsNullOrWhiteSpace(entity.sectionFromId))
                    return ApiResponse<int>.Fail("Section From is required.");

                if (string.IsNullOrWhiteSpace(entity.sectionToId))
                    return ApiResponse<int>.Fail("Section To is required.");

                if (entity.distance <= 0)
                    return ApiResponse<int>.Fail("Distance must be greater than 0.");

                // Parse IDs
                if (!int.TryParse(entity.routeId, out int routeId))
                    return ApiResponse<int>.Fail("Invalid Route ID format.");

                if (!int.TryParse(entity.sectionFromId, out int sectionFromId))
                    return ApiResponse<int>.Fail("Invalid Section From ID format.");

                if (!int.TryParse(entity.sectionToId, out int sectionToId))
                    return ApiResponse<int>.Fail("Invalid Section To ID format.");

                // Check if From and To are different
                if (sectionFromId == sectionToId)
                    return ApiResponse<int>.Fail("Section From and Section To cannot be the same.");

                var id = await _repo.InsertAsync(entity, userId);
                return ApiResponse<int>.Ok(id, "Stage created successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<int>.Fail($"An error occurred while creating stage. {ex.Message}");
            }
        }

        public async Task<ApiResponse<bool>> UpdateAsync(StageDTO entity, int userId)
        {
            try
            {
                // Validations
                if (string.IsNullOrEmpty(entity.stageId))
                    return ApiResponse<bool>.Fail("Stage ID is required.");

                if (!int.TryParse(entity.stageId, out int stageId))
                    return ApiResponse<bool>.Fail("Invalid Stage ID format.");

                if (string.IsNullOrWhiteSpace(entity.stageName))
                    return ApiResponse<bool>.Fail("Stage Name is required.");

                if (string.IsNullOrWhiteSpace(entity.routeId))
                    return ApiResponse<bool>.Fail("Route is required.");

                if (string.IsNullOrWhiteSpace(entity.sectionFromId))
                    return ApiResponse<bool>.Fail("Section From is required.");

                if (string.IsNullOrWhiteSpace(entity.sectionToId))
                    return ApiResponse<bool>.Fail("Section To is required.");

                if (entity.distance <= 0)
                    return ApiResponse<bool>.Fail("Distance must be greater than 0.");

                // Parse IDs
                if (!int.TryParse(entity.routeId, out int routeId))
                    return ApiResponse<bool>.Fail("Invalid Route ID format.");

                if (!int.TryParse(entity.sectionFromId, out int sectionFromId))
                    return ApiResponse<bool>.Fail("Invalid Section From ID format.");

                if (!int.TryParse(entity.sectionToId, out int sectionToId))
                    return ApiResponse<bool>.Fail("Invalid Section To ID format.");

                // Check if From and To are different
                if (sectionFromId == sectionToId)
                    return ApiResponse<bool>.Fail("Section From and Section To cannot be the same.");

                var success = await _repo.UpdateAsync(entity, userId);
                if (!success)
                    return ApiResponse<bool>.Fail($"Stage with ID {entity.stageId} not found.");

                return ApiResponse<bool>.Ok(true, "Stage updated successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.Fail($"An error occurred while updating stage. {ex.Message}");
            }
        }

        public async Task<ApiResponse<bool>> DeleteAsync(int stageId, int deletedBy)
        {
            try
            {
                var success = await _repo.DeleteAsync(stageId, deletedBy);
                if (!success)
                    return ApiResponse<bool>.Fail($"Stage with ID {stageId} not found or already inactive.");

                return ApiResponse<bool>.Ok(true, "Stage deleted successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.Fail($"An error occurred while deleting stage. {ex.Message}");
            }
        }
    }
}