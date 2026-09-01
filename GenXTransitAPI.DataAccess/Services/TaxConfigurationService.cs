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
    public class TaxConfigurationService : ITaxConfigurationService
    {
        private readonly ITaxConfigurationRepository _repo;

        public TaxConfigurationService(ITaxConfigurationRepository repo)
        {
            _repo = repo;
        }

        public async Task<ApiResponse<IEnumerable<TaxConfigurationDTO>>> GetAllAsync(
            string? searchText,
            string? taxType,
            decimal? rateFrom,
            decimal? rateTo,
            bool? isActive,
            int? scopeToUser,
            int pageNumber = 1,
            int pageSize = 10)
        {
            try
            {
                var items = await _repo.GetAllAsync(searchText, taxType, rateFrom, rateTo, isActive, scopeToUser, pageNumber, pageSize);
                var totalCount = items.FirstOrDefault()?.totalCount ?? 0;
                return ApiResponse<IEnumerable<TaxConfigurationDTO>>.Ok(items, null, totalCount);
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<TaxConfigurationDTO>>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<TaxConfigurationDTO>> GetByIdAsync(int taxId)
        {
            try
            {
                var item = await _repo.GetByIdAsync(taxId);
                if (item == null)
                    return ApiResponse<TaxConfigurationDTO>.Fail($"Tax configuration with ID {taxId} not found.");

                return ApiResponse<TaxConfigurationDTO>.Ok(item);
            }
            catch (Exception ex)
            {
                return ApiResponse<TaxConfigurationDTO>.Fail(ex.Message);
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

        public async Task<ApiResponse<int>> InsertAsync(TaxConfigurationDTO entity, int userId)
        {
            try
            {
                // Validations
                if (string.IsNullOrWhiteSpace(entity.taxType))
                    return ApiResponse<int>.Fail("Tax Type is required.");

                if (entity.rate < 0)
                    return ApiResponse<int>.Fail("Tax rate cannot be negative.");

                // Validate Tax Type
                var validTaxTypes = new[] { "GST", "Service Tax", "VAT", "Cess", "Others" };
                if (!validTaxTypes.Contains(entity.taxType))
                    return ApiResponse<int>.Fail("Invalid Tax Type. Valid types are: GST, Service Tax, VAT, Cess, Others.");

                var id = await _repo.InsertAsync(entity, userId);
                return ApiResponse<int>.Ok(id, "Tax configuration created successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<int>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> UpdateAsync(TaxConfigurationDTO entity, int userId)
        {
            try
            {
                // Validations
                if (string.IsNullOrEmpty(entity.taxId))
                    return ApiResponse<bool>.Fail("Tax ID is required.");

                if (!int.TryParse(entity.taxId, out int taxId))
                    return ApiResponse<bool>.Fail("Invalid Tax ID format.");

                if (string.IsNullOrWhiteSpace(entity.taxType))
                    return ApiResponse<bool>.Fail("Tax Type is required.");

                if (entity.rate < 0)
                    return ApiResponse<bool>.Fail("Tax rate cannot be negative.");

                // Validate Tax Type
                var validTaxTypes = new[] { "GST", "Service Tax", "VAT", "Cess", "Others" };
                if (!validTaxTypes.Contains(entity.taxType))
                    return ApiResponse<bool>.Fail("Invalid Tax Type. Valid types are: GST, Service Tax, VAT, Cess, Others.");

                var success = await _repo.UpdateAsync(entity, userId);
                if (!success)
                    return ApiResponse<bool>.Fail($"Tax configuration with ID {entity.taxId} not found.");

                return ApiResponse<bool>.Ok(true, "Tax configuration updated successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> DeleteAsync(int taxId, int deletedBy)
        {
            try
            {
                var success = await _repo.DeleteAsync(taxId, deletedBy);
                if (!success)
                    return ApiResponse<bool>.Fail($"Tax configuration with ID {taxId} not found or already inactive.");

                return ApiResponse<bool>.Ok(true, "Tax configuration deleted successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.Fail(ex.Message);
            }
        }
    }
}