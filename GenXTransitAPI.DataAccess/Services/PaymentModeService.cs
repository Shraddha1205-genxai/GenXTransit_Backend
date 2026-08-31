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
    public class PaymentModeService : IPaymentModeService
    {
        private readonly IPaymentModeRepository _repo;

        public PaymentModeService(IPaymentModeRepository repo)
        {
            _repo = repo;
        }

        public async Task<ApiResponse<IEnumerable<PaymentModeDTO>>> GetAllAsync(
            string? searchText,
            string? modeStatus,
            bool? isActive,
            int? scopeToUser,
            int pageNumber = 1,
            int pageSize = 10)
        {
            try
            {
                var items = await _repo.GetAllAsync(searchText, modeStatus, isActive, scopeToUser, pageNumber, pageSize);
                var totalCount = items.FirstOrDefault()?.totalCount ?? 0;
                return ApiResponse<IEnumerable<PaymentModeDTO>>.Ok(items, null, totalCount);
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<PaymentModeDTO>>.Fail($"Error retrieving payment modes: {ex.Message}");
            }
        }

        public async Task<ApiResponse<PaymentModeDTO>> GetByIdAsync(int modeId)
        {
            try
            {
                var item = await _repo.GetByIdAsync(modeId);
                if (item == null)
                    return ApiResponse<PaymentModeDTO>.Fail($"Payment mode with ID {modeId} not found.");

                return ApiResponse<PaymentModeDTO>.Ok(item);
            }
            catch (Exception ex)
            {
                return ApiResponse<PaymentModeDTO>.Fail($"Error retrieving payment mode: {ex.Message}");
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

        public async Task<ApiResponse<int>> InsertAsync(PaymentModeDTO entity, int userId)
        {
            try
            {
                // Validations
                if (string.IsNullOrWhiteSpace(entity.modeName))
                    return ApiResponse<int>.Fail("Mode Name is required.");

                if (string.IsNullOrWhiteSpace(entity.modeStatus))
                    return ApiResponse<int>.Fail("Mode Status is required.");

                var id = await _repo.InsertAsync(entity, userId);
                return ApiResponse<int>.Ok(id, "Payment mode created successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<int>.Fail($"An error occurred while creating payment mode. {ex.Message}");
            }
        }

        public async Task<ApiResponse<bool>> UpdateAsync(PaymentModeDTO entity, int userId)
        {
            try
            {
                // Validations
                if (string.IsNullOrEmpty(entity.modeId))
                    return ApiResponse<bool>.Fail("Mode ID is required.");

                if (!int.TryParse(entity.modeId, out int modeId))
                    return ApiResponse<bool>.Fail("Invalid Mode ID format.");

                if (string.IsNullOrWhiteSpace(entity.modeName))
                    return ApiResponse<bool>.Fail("Mode Name is required.");

                if (string.IsNullOrWhiteSpace(entity.modeStatus))
                    return ApiResponse<bool>.Fail("Mode Status is required.");

                var success = await _repo.UpdateAsync(entity, userId);
                if (!success)
                    return ApiResponse<bool>.Fail($"Payment mode with ID {entity.modeId} not found.");

                return ApiResponse<bool>.Ok(true, "Payment mode updated successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.Fail($"An error occurred while updating payment mode. {ex.Message}");
            }
        }

        public async Task<ApiResponse<bool>> DeleteAsync(int modeId, int deletedBy)
        {
            try
            {
                var success = await _repo.DeleteAsync(modeId, deletedBy);
                if (!success)
                    return ApiResponse<bool>.Fail($"Payment mode with ID {modeId} not found or already inactive.");

                return ApiResponse<bool>.Ok(true, "Payment mode deleted successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.Fail($"An error occurred while deleting payment mode. {ex.Message}");
            }
        }
    }
}