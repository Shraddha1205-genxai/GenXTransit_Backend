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
    public class SeatLayoutService : ISeatLayoutService
    {
        private readonly ISeatLayoutRepository _repo;

        public SeatLayoutService(ISeatLayoutRepository repo)
        {
            _repo = repo;
        }

        public async Task<ApiResponse<IEnumerable<SeatLayoutDTO>>> GetAllAsync(
            string? searchText,
            int? categoryId,
            bool? isActive,
            int? scopeToUser,
            int pageNumber = 1,
            int pageSize = 10)
        {
            try
            {
                var items = await _repo.GetAllAsync(searchText, categoryId, isActive, scopeToUser, pageNumber, pageSize);
                var totalCount = items.FirstOrDefault()?.totalCount ?? 0;
                return ApiResponse<IEnumerable<SeatLayoutDTO>>.Ok(items, null, totalCount);
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<SeatLayoutDTO>>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<SeatLayoutDTO>> GetByIdAsync(int layoutId)
        {
            try
            {
                var item = await _repo.GetByIdAsync(layoutId);
                if (item == null)
                    return ApiResponse<SeatLayoutDTO>.Fail($"Seat layout with ID {layoutId} not found.");

                return ApiResponse<SeatLayoutDTO>.Ok(item);
            }
            catch (Exception ex)
            {
                return ApiResponse<SeatLayoutDTO>.Fail(ex.Message);
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

        public async Task<ApiResponse<int>> InsertAsync(SeatLayoutDTO entity, int userId)
        {
            try
            {
                // Validations
                if (string.IsNullOrWhiteSpace(entity.categoryId))
                    return ApiResponse<int>.Fail("Category is required.");

                // Parse IDs
                if (!int.TryParse(entity.categoryId, out int categoryId))
                    return ApiResponse<int>.Fail("Invalid Category ID format.");

                var id = await _repo.InsertAsync(entity, userId);
                return ApiResponse<int>.Ok(id, "Seat layout created successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<int>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> UpdateAsync(SeatLayoutDTO entity, int userId)
        {
            try
            {
                // Validations
                if (string.IsNullOrEmpty(entity.layoutId))
                    return ApiResponse<bool>.Fail("Layout ID is required.");

                if (!int.TryParse(entity.layoutId, out int layoutId))
                    return ApiResponse<bool>.Fail("Invalid Layout ID format.");

                if (string.IsNullOrWhiteSpace(entity.categoryId))
                    return ApiResponse<bool>.Fail("Category is required.");

                // Parse IDs
                if (!int.TryParse(entity.categoryId, out int categoryId))
                    return ApiResponse<bool>.Fail("Invalid Category ID format.");

                var success = await _repo.UpdateAsync(entity, userId);
                if (!success)
                    return ApiResponse<bool>.Fail($"Seat layout with ID {entity.layoutId} not found.");

                return ApiResponse<bool>.Ok(true, "Seat layout updated successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> DeleteAsync(int layoutId, int deletedBy)
        {
            try
            {
                var success = await _repo.DeleteAsync(layoutId, deletedBy);
                if (!success)
                    return ApiResponse<bool>.Fail($"Seat layout with ID {layoutId} not found or already inactive.");

                return ApiResponse<bool>.Ok(true, "Seat layout deleted successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.Fail(ex.Message);
            }
        }
    }
}