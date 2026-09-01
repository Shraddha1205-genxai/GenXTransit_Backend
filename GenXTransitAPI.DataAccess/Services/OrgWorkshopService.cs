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
    public class OrgWorkshopService : IOrgWorkshopService
    {
        private readonly IOrgWorkshopRepository _repo;

        public OrgWorkshopService(IOrgWorkshopRepository repo)
        {
            _repo = repo;
        }

        public async Task<ApiResponse<IEnumerable<OrgWorkshopDTO>>> GetAllAsync(
            string? searchText,
            int? regionId,
            int? divisionId,
            int? depotId,
            bool? isActive,
            int? scopeToUser,
            int pageNumber = 1,
            int pageSize = 10)
        {
            try
            {
                var items = await _repo.GetAllAsync(searchText, regionId, divisionId, depotId, isActive, scopeToUser, pageNumber, pageSize);
                var totalCount = items.FirstOrDefault()?.totalCount ?? 0;
                return ApiResponse<IEnumerable<OrgWorkshopDTO>>.Ok(items, null, totalCount);
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<OrgWorkshopDTO>>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<OrgWorkshopDTO>> GetByIdAsync(int workShopId)
        {
            try
            {
                var item = await _repo.GetByIdAsync(workShopId);
                if (item == null)
                    return ApiResponse<OrgWorkshopDTO>.Fail($"Workshop with ID {workShopId} not found.");

                return ApiResponse<OrgWorkshopDTO>.Ok(item);
            }
            catch (Exception ex)
            {
                return ApiResponse<OrgWorkshopDTO>.Fail(ex.Message);
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

        public async Task<ApiResponse<int>> InsertAsync(OrgWorkshopDTO entity, int userId)
        {
            try
            {
                // Validations
                if (string.IsNullOrWhiteSpace(entity.workShopName))
                    return ApiResponse<int>.Fail("Workshop Name is required.");

                if (string.IsNullOrWhiteSpace(entity.regionId))
                    return ApiResponse<int>.Fail("Region is required.");

                if (string.IsNullOrWhiteSpace(entity.divisionId))
                    return ApiResponse<int>.Fail("Division is required.");

                if (string.IsNullOrWhiteSpace(entity.depotId))
                    return ApiResponse<int>.Fail("Depot is required.");

                if (entity.workBays < 0)
                    return ApiResponse<int>.Fail("Work Bays cannot be negative.");

                if (entity.activeRepairJobs < 0)
                    return ApiResponse<int>.Fail("Active Repair Jobs cannot be negative.");

                // Parse IDs
                if (!int.TryParse(entity.regionId, out int regionId))
                    return ApiResponse<int>.Fail("Invalid Region ID format.");

                if (!int.TryParse(entity.divisionId, out int divisionId))
                    return ApiResponse<int>.Fail("Invalid Division ID format.");

                if (!int.TryParse(entity.depotId, out int depotId))
                    return ApiResponse<int>.Fail("Invalid Depot ID format.");

                var id = await _repo.InsertAsync(entity, userId);
                return ApiResponse<int>.Ok(id, "Workshop created successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<int>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> UpdateAsync(OrgWorkshopDTO entity, int userId)
        {
            try
            {
                // Validations
                if (string.IsNullOrEmpty(entity.workShopId))
                    return ApiResponse<bool>.Fail("Workshop ID is required.");

                if (!int.TryParse(entity.workShopId, out int workShopId))
                    return ApiResponse<bool>.Fail("Invalid Workshop ID format.");

                if (string.IsNullOrWhiteSpace(entity.workShopName))
                    return ApiResponse<bool>.Fail("Workshop Name is required.");

                if (string.IsNullOrWhiteSpace(entity.regionId))
                    return ApiResponse<bool>.Fail("Region is required.");

                if (string.IsNullOrWhiteSpace(entity.divisionId))
                    return ApiResponse<bool>.Fail("Division is required.");

                if (string.IsNullOrWhiteSpace(entity.depotId))
                    return ApiResponse<bool>.Fail("Depot is required.");

                if (entity.workBays < 0)
                    return ApiResponse<bool>.Fail("Work Bays cannot be negative.");

                if (entity.activeRepairJobs < 0)
                    return ApiResponse<bool>.Fail("Active Repair Jobs cannot be negative.");

                // Parse IDs
                if (!int.TryParse(entity.regionId, out int regionId))
                    return ApiResponse<bool>.Fail("Invalid Region ID format.");

                if (!int.TryParse(entity.divisionId, out int divisionId))
                    return ApiResponse<bool>.Fail("Invalid Division ID format.");

                if (!int.TryParse(entity.depotId, out int depotId))
                    return ApiResponse<bool>.Fail("Invalid Depot ID format.");

                var success = await _repo.UpdateAsync(entity, userId);
                if (!success)
                    return ApiResponse<bool>.Fail($"Workshop with ID {entity.workShopId} not found.");

                return ApiResponse<bool>.Ok(true, "Workshop updated successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> DeleteAsync(int workShopId, int deletedBy)
        {
            try
            {
                var success = await _repo.DeleteAsync(workShopId, deletedBy);
                if (!success)
                    return ApiResponse<bool>.Fail($"Workshop with ID {workShopId} not found or already inactive.");

                return ApiResponse<bool>.Ok(true, "Workshop deleted successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.Fail(ex.Message);
            }
        }
    }
}