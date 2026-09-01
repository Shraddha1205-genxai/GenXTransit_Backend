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
    public class OrgDivisionService : IOrgDivisionService
    {
        private readonly IOrgDivisionRepository _repo;

        public OrgDivisionService(IOrgDivisionRepository repo)
        {
            _repo = repo;
        }

        public async Task<ApiResponse<IEnumerable<OrgDivisionDTO>>> GetAllAsync(
            string? searchText,
            int? regionId,
            bool? isActive,
            int? scopeToUser,
            int pageNumber = 1,
            int pageSize = 10)
        {
            try
            {
                var items = await _repo.GetAllAsync(searchText, regionId, isActive, scopeToUser, pageNumber, pageSize);
                var totalCount = items.FirstOrDefault()?.totalCount ?? 0;
                return ApiResponse<IEnumerable<OrgDivisionDTO>>.Ok(items, null, totalCount);
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<OrgDivisionDTO>>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<OrgDivisionDTO>> GetByIdAsync(int divisionId)
        {
            try
            {
                var item = await _repo.GetByIdAsync(divisionId);
                if (item == null)
                    return ApiResponse<OrgDivisionDTO>.Fail($"Division with ID {divisionId} not found.");

                return ApiResponse<OrgDivisionDTO>.Ok(item);
            }
            catch (Exception ex)
            {
                return ApiResponse<OrgDivisionDTO>.Fail(ex.Message);
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

        public async Task<ApiResponse<int>> InsertAsync(OrgDivisionDTO entity, int userId)
        {
            try
            {
                // Validations
                if (string.IsNullOrWhiteSpace(entity.divisionName))
                    return ApiResponse<int>.Fail("Division Name is required.");

                if (string.IsNullOrWhiteSpace(entity.regionId))
                    return ApiResponse<int>.Fail("Region is required.");

                if (!int.TryParse(entity.regionId, out int regionId))
                    return ApiResponse<int>.Fail("Invalid Region ID format.");

                var id = await _repo.InsertAsync(entity, userId);
                return ApiResponse<int>.Ok(id, "Division created successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<int>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> UpdateAsync(OrgDivisionDTO entity, int userId)
        {
            try
            {
                // Validations
                if (string.IsNullOrEmpty(entity.divisionId))
                    return ApiResponse<bool>.Fail("Division ID is required.");

                if (!int.TryParse(entity.divisionId, out int divisionId))
                    return ApiResponse<bool>.Fail("Invalid Division ID format.");

                if (string.IsNullOrWhiteSpace(entity.divisionName))
                    return ApiResponse<bool>.Fail("Division Name is required.");

                if (string.IsNullOrWhiteSpace(entity.regionId))
                    return ApiResponse<bool>.Fail("Region is required.");

                if (!int.TryParse(entity.regionId, out int regionId))
                    return ApiResponse<bool>.Fail("Invalid Region ID format.");

                var success = await _repo.UpdateAsync(entity, userId);
                if (!success)
                    return ApiResponse<bool>.Fail($"Division with ID {entity.divisionId} not found.");

                return ApiResponse<bool>.Ok(true, "Division updated successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> DeleteAsync(int divisionId, int deletedBy)
        {
            try
            {
                var success = await _repo.DeleteAsync(divisionId, deletedBy);
                if (!success)
                    return ApiResponse<bool>.Fail($"Division with ID {divisionId} not found or already inactive.");

                return ApiResponse<bool>.Ok(true, "Division and all associated records deleted successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.Fail(ex.Message);
            }
        }
    }
}