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
    public class FarePolicyService : IFarePolicyService
    {
        private readonly IFarePolicyRepository _repo;

        public FarePolicyService(IFarePolicyRepository repo)
        {
            _repo = repo;
        }

        public async Task<ApiResponse<IEnumerable<FarePolicyDTO>>> GetAllAsync(
            string? searchText,
            string? model,
            string? policyStatus,
            int? categoryId,
            int? routeId,
            bool? isActive,
            int? scopeToUser,
            int pageNumber = 1,
            int pageSize = 10)
        {
            try
            {
                var items = await _repo.GetAllAsync(searchText, model, policyStatus, categoryId, routeId, isActive, scopeToUser, pageNumber, pageSize);
                var totalCount = items.FirstOrDefault()?.totalCount ?? 0;
                return ApiResponse<IEnumerable<FarePolicyDTO>>.Ok(items, null, totalCount);
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<FarePolicyDTO>>.Fail($"Error retrieving fare policies: {ex.Message}");
            }
        }

        public async Task<ApiResponse<FarePolicyDTO>> GetByIdAsync(int policyId)
        {
            try
            {
                var item = await _repo.GetByIdAsync(policyId);
                if (item == null)
                    return ApiResponse<FarePolicyDTO>.Fail($"Fare policy with ID {policyId} not found.");

                return ApiResponse<FarePolicyDTO>.Ok(item);
            }
            catch (Exception ex)
            {
                return ApiResponse<FarePolicyDTO>.Fail($"Error retrieving fare policy: {ex.Message}");
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

        public async Task<ApiResponse<int>> InsertAsync(FarePolicyDTO entity, int userId)
        {
            try
            {
                // Validations
                if (string.IsNullOrWhiteSpace(entity.model))
                    return ApiResponse<int>.Fail("Model is required.");

                if (string.IsNullOrWhiteSpace(entity.policyStatus))
                    return ApiResponse<int>.Fail("Policy Status is required.");

                if (string.IsNullOrWhiteSpace(entity.categoryId))
                    return ApiResponse<int>.Fail("Category is required.");

                if (string.IsNullOrWhiteSpace(entity.routeId))
                    return ApiResponse<int>.Fail("Route is required.");

                if (entity.baseFare <= 0)
                    return ApiResponse<int>.Fail("Base Fare must be greater than 0.");

                // Parse IDs
                if (!int.TryParse(entity.categoryId, out int categoryId))
                    return ApiResponse<int>.Fail("Invalid Category ID format.");

                if (!int.TryParse(entity.routeId, out int routeId))
                    return ApiResponse<int>.Fail("Invalid Route ID format.");

                var id = await _repo.InsertAsync(entity, userId);
                return ApiResponse<int>.Ok(id, "Fare policy created successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<int>.Fail($"An error occurred while creating fare policy. {ex.Message}");
            }
        }

        public async Task<ApiResponse<bool>> UpdateAsync(FarePolicyDTO entity, int userId)
        {
            try
            {
                // Validations
                if (string.IsNullOrEmpty(entity.policyId))
                    return ApiResponse<bool>.Fail("Policy ID is required.");

                if (!int.TryParse(entity.policyId, out int policyId))
                    return ApiResponse<bool>.Fail("Invalid Policy ID format.");

                if (string.IsNullOrWhiteSpace(entity.model))
                    return ApiResponse<bool>.Fail("Model is required.");

                if (string.IsNullOrWhiteSpace(entity.policyStatus))
                    return ApiResponse<bool>.Fail("Policy Status is required.");

                if (string.IsNullOrWhiteSpace(entity.categoryId))
                    return ApiResponse<bool>.Fail("Category is required.");

                if (string.IsNullOrWhiteSpace(entity.routeId))
                    return ApiResponse<bool>.Fail("Route is required.");

                if (entity.baseFare <= 0)
                    return ApiResponse<bool>.Fail("Base Fare must be greater than 0.");

                // Parse IDs
                if (!int.TryParse(entity.categoryId, out int categoryId))
                    return ApiResponse<bool>.Fail("Invalid Category ID format.");

                if (!int.TryParse(entity.routeId, out int routeId))
                    return ApiResponse<bool>.Fail("Invalid Route ID format.");

                var success = await _repo.UpdateAsync(entity, userId);
                if (!success)
                    return ApiResponse<bool>.Fail($"Fare policy with ID {entity.policyId} not found.");

                return ApiResponse<bool>.Ok(true, "Fare policy updated successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.Fail($"An error occurred while updating fare policy. {ex.Message}");
            }
        }

        public async Task<ApiResponse<bool>> DeleteAsync(int policyId, int deletedBy)
        {
            try
            {
                var success = await _repo.DeleteAsync(policyId, deletedBy);
                if (!success)
                    return ApiResponse<bool>.Fail($"Fare policy with ID {policyId} not found or already inactive.");

                return ApiResponse<bool>.Ok(true, "Fare policy deleted successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.Fail($"An error occurred while deleting fare policy. {ex.Message}");
            }
        }
    }
}