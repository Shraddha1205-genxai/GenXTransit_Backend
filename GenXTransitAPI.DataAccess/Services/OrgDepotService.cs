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
    public class OrgDepotService : IOrgDepotService
    {
        private readonly IOrgDepotRepository _repo;

        public OrgDepotService(IOrgDepotRepository repo)
        {
            _repo = repo;
        }

        public async Task<ApiResponse<IEnumerable<OrgDepotDTO>>> GetAllAsync(
            string? searchText,
            int? corporationId,
            int? regionId,
            int? divisionId,
            int? zoneId,
            bool? isActive,
            int? scopeToUser,
            int pageNumber = 1,
            int pageSize = 10)
        {
            try
            {
                var items = await _repo.GetAllAsync(searchText, corporationId, regionId, divisionId, zoneId, isActive, scopeToUser, pageNumber, pageSize);
                var totalCount = items.FirstOrDefault()?.totalCount ?? 0;
                return ApiResponse<IEnumerable<OrgDepotDTO>>.Ok(items, null, totalCount);
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<OrgDepotDTO>>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<OrgDepotDTO>> GetByIdAsync(int depotId)
        {
            try
            {
                var item = await _repo.GetByIdAsync(depotId);
                if (item == null)
                    return ApiResponse<OrgDepotDTO>.Fail($"Depot with ID {depotId} not found.");

                return ApiResponse<OrgDepotDTO>.Ok(item);
            }
            catch (Exception ex)
            {
                return ApiResponse<OrgDepotDTO>.Fail(ex.Message);
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

        public async Task<ApiResponse<int>> InsertAsync(OrgDepotDTO entity, int userId)
        {
            try
            {
                // Validations
                if (string.IsNullOrWhiteSpace(entity.depotName))
                    return ApiResponse<int>.Fail("Depot Name is required.");

                if (string.IsNullOrWhiteSpace(entity.corpId))
                    return ApiResponse<int>.Fail("Corporation is required.");

                if (string.IsNullOrWhiteSpace(entity.regionId))
                    return ApiResponse<int>.Fail("Region is required.");

                if (string.IsNullOrWhiteSpace(entity.divisionId))
                    return ApiResponse<int>.Fail("Division is required.");

                if (string.IsNullOrWhiteSpace(entity.zoneId))
                    return ApiResponse<int>.Fail("Zone is required.");

                if (string.IsNullOrWhiteSpace(entity.service))
                    return ApiResponse<int>.Fail("Service is required.");

                // Parse IDs
                if (!int.TryParse(entity.corpId, out int corpId))
                    return ApiResponse<int>.Fail("Invalid Corporation ID format.");

                if (!int.TryParse(entity.regionId, out int regionId))
                    return ApiResponse<int>.Fail("Invalid Region ID format.");

                if (!int.TryParse(entity.divisionId, out int divisionId))
                    return ApiResponse<int>.Fail("Invalid Division ID format.");

                if (!int.TryParse(entity.zoneId, out int zoneId))
                    return ApiResponse<int>.Fail("Invalid Zone ID format.");

                var id = await _repo.InsertAsync(entity, userId);
                return ApiResponse<int>.Ok(id, "Depot created successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<int>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> UpdateAsync(OrgDepotDTO entity, int userId)
        {
            try
            {
                // Validations
                if (string.IsNullOrEmpty(entity.depotId))
                    return ApiResponse<bool>.Fail("Depot ID is required.");

                if (!int.TryParse(entity.depotId, out int depotId))
                    return ApiResponse<bool>.Fail("Invalid Depot ID format.");

                if (string.IsNullOrWhiteSpace(entity.depotName))
                    return ApiResponse<bool>.Fail("Depot Name is required.");

                if (string.IsNullOrWhiteSpace(entity.corpId))
                    return ApiResponse<bool>.Fail("Corporation is required.");

                if (string.IsNullOrWhiteSpace(entity.regionId))
                    return ApiResponse<bool>.Fail("Region is required.");

                if (string.IsNullOrWhiteSpace(entity.divisionId))
                    return ApiResponse<bool>.Fail("Division is required.");

                if (string.IsNullOrWhiteSpace(entity.zoneId))
                    return ApiResponse<bool>.Fail("Zone is required.");

                if (string.IsNullOrWhiteSpace(entity.service))
                    return ApiResponse<bool>.Fail("Service is required.");

                // Parse IDs
                if (!int.TryParse(entity.corpId, out int corpId))
                    return ApiResponse<bool>.Fail("Invalid Corporation ID format.");

                if (!int.TryParse(entity.regionId, out int regionId))
                    return ApiResponse<bool>.Fail("Invalid Region ID format.");

                if (!int.TryParse(entity.divisionId, out int divisionId))
                    return ApiResponse<bool>.Fail("Invalid Division ID format.");

                if (!int.TryParse(entity.zoneId, out int zoneId))
                    return ApiResponse<bool>.Fail("Invalid Zone ID format.");

                var success = await _repo.UpdateAsync(entity, userId);
                if (!success)
                    return ApiResponse<bool>.Fail($"Depot with ID {entity.depotId} not found.");

                return ApiResponse<bool>.Ok(true, "Depot updated successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> DeleteAsync(int depotId, int deletedBy)
        {
            try
            {
                var success = await _repo.DeleteAsync(depotId, deletedBy);
                if (!success)
                    return ApiResponse<bool>.Fail($"Depot with ID {depotId} not found or already inactive.");

                return ApiResponse<bool>.Ok(true, "Depot and all associated records deleted successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.Fail(ex.Message);
            }
        }
    }
}