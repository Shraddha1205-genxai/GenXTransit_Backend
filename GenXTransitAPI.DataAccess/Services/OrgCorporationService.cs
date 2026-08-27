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
    public class OrgCorporationService : IOrgCorporationService
    {
        private readonly IOrgCorporationRepository _repo;

        public OrgCorporationService(IOrgCorporationRepository repo)
        {
            _repo = repo;
        }

        public async Task<ApiResponse<IEnumerable<OrgCorporationDTO>>> GetAllAsync(
            string? searchText,
            string? stateName,
            bool? isActive,
            int? scopeToUser,
            int pageNumber = 1,
            int pageSize = 10)
        {
            try
            {
                var items = await _repo.GetAllAsync(searchText, stateName, isActive, scopeToUser, pageNumber, pageSize);
                var totalCount = items.FirstOrDefault()?.totalCount ?? 0;
                return ApiResponse<IEnumerable<OrgCorporationDTO>>.Ok(items, null, totalCount);
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<OrgCorporationDTO>>.Fail($"Error retrieving corporations: {ex.Message}");
            }
        }

        public async Task<ApiResponse<OrgCorporationDTO>> GetByIdAsync(int corporationId)
        {
            try
            {
                var item = await _repo.GetByIdAsync(corporationId);
                if (item == null)
                    return ApiResponse<OrgCorporationDTO>.Fail($"Corporation with ID {corporationId} not found.");

                return ApiResponse<OrgCorporationDTO>.Ok(item);
            }
            catch (Exception ex)
            {
                return ApiResponse<OrgCorporationDTO>.Fail($"Error retrieving corporation: {ex.Message}");
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

        public async Task<ApiResponse<int>> InsertAsync(OrgCorporationDTO entity, int userId)
        {
            try
            {
                // Validations
                if (string.IsNullOrWhiteSpace(entity.corporationName))
                    return ApiResponse<int>.Fail("Corporation Name is required.");

                if (string.IsNullOrWhiteSpace(entity.stateName))
                    return ApiResponse<int>.Fail("State Name is required.");

                if (string.IsNullOrWhiteSpace(entity.districtName))
                    return ApiResponse<int>.Fail("District Name is required.");

                if (string.IsNullOrWhiteSpace(entity.cityName))
                    return ApiResponse<int>.Fail("City Name is required.");

                var id = await _repo.InsertAsync(entity, userId);
                return ApiResponse<int>.Ok(id, "Corporation created successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<int>.Fail($"An error occurred while creating corporation. {ex.Message}");
            }
        }

        public async Task<ApiResponse<bool>> UpdateAsync(OrgCorporationDTO entity, int userId)
        {
            try
            {
                // Validations
                if (string.IsNullOrEmpty(entity.corpId))
                    return ApiResponse<bool>.Fail("Corporation ID is required.");

                if (!int.TryParse(entity.corpId, out int id))
                    return ApiResponse<bool>.Fail("Invalid Corporation ID format.");

                if (string.IsNullOrWhiteSpace(entity.corporationName))
                    return ApiResponse<bool>.Fail("Corporation Name is required.");

                if (string.IsNullOrWhiteSpace(entity.stateName))
                    return ApiResponse<bool>.Fail("State Name is required.");

                if (string.IsNullOrWhiteSpace(entity.districtName))
                    return ApiResponse<bool>.Fail("District Name is required.");

                if (string.IsNullOrWhiteSpace(entity.cityName))
                    return ApiResponse<bool>.Fail("City Name is required.");

                var success = await _repo.UpdateAsync(entity, userId);
                if (!success)
                    return ApiResponse<bool>.Fail($"Corporation with ID {entity.corpId} not found.");

                return ApiResponse<bool>.Ok(true, "Corporation updated successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.Fail($"An error occurred while updating corporation. {ex.Message}");
            }
        }

        public async Task<ApiResponse<bool>> DeleteAsync(int corporationId, int deletedBy)
        {
            try
            {
                var success = await _repo.DeleteAsync(corporationId, deletedBy);
                if (!success)
                    return ApiResponse<bool>.Fail($"Corporation with ID {corporationId} not found or already inactive.");

                return ApiResponse<bool>.Ok(true, "Corporation and all associated records deleted successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.Fail($"An error occurred while deleting corporation. {ex.Message}");
            }
        }
    }
}