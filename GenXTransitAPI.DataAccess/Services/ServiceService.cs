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
    public class ServiceService : IServiceService
    {
        private readonly IServiceRepository _repo;

        public ServiceService(IServiceRepository repo)
        {
            _repo = repo;
        }

        public async Task<ApiResponse<IEnumerable<ServiceDTO>>> GetAllAsync(
            string? searchText,
            int? fleetId,
            string? status,
            bool? isActive,
            int? scopeToUser,
            int pageNumber = 1,
            int pageSize = 10)
        {
            try
            {
                var items = await _repo.GetAllAsync(searchText, fleetId, status, isActive, scopeToUser, pageNumber, pageSize);
                var totalCount = items.FirstOrDefault()?.totalCount ?? 0;
                return ApiResponse<IEnumerable<ServiceDTO>>.Ok(items, null, totalCount);
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<ServiceDTO>>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<ServiceDTO>> GetByIdAsync(int serviceId)
        {
            try
            {
                var item = await _repo.GetByIdAsync(serviceId);
                if (item == null)
                    return ApiResponse<ServiceDTO>.Fail($"Service with ID {serviceId} not found.");

                return ApiResponse<ServiceDTO>.Ok(item);
            }
            catch (Exception ex)
            {
                return ApiResponse<ServiceDTO>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<int>> InsertAsync(ServiceDTO entity, int userId)
        {
            try
            {
                // Validations
                if (string.IsNullOrWhiteSpace(entity.fleetId))
                    return ApiResponse<int>.Fail("Fleet is required.");

                if (!int.TryParse(entity.fleetId, out int fleetId))
                    return ApiResponse<int>.Fail("Invalid Fleet ID format.");

                // Validate Status
                var validStatuses = new[] { "Pending", "Inprogress", "Completed", "Cancelled" };
                if (!string.IsNullOrWhiteSpace(entity.status) && !validStatuses.Contains(entity.status))
                    return ApiResponse<int>.Fail("Invalid status. Valid statuses are: Pending, Inprogress, Completed, Cancelled.");

                var id = await _repo.InsertAsync(entity, userId);
                return ApiResponse<int>.Ok(id, "Service created successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<int>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> UpdateAsync(ServiceDTO entity, int userId)
        {
            try
            {
                if (string.IsNullOrEmpty(entity.serviceId))
                    return ApiResponse<bool>.Fail("Service ID is required.");

                if (!int.TryParse(entity.serviceId, out int serviceId))
                    return ApiResponse<bool>.Fail("Invalid Service ID format.");

                if (string.IsNullOrWhiteSpace(entity.fleetId))
                    return ApiResponse<bool>.Fail("Fleet is required.");

                if (!int.TryParse(entity.fleetId, out int fleetId))
                    return ApiResponse<bool>.Fail("Invalid Fleet ID format.");

                // Validate Status
                var validStatuses = new[] { "Pending", "Inprogress", "Completed", "Cancelled" };
                if (!string.IsNullOrWhiteSpace(entity.status) && !validStatuses.Contains(entity.status))
                    return ApiResponse<bool>.Fail("Invalid status. Valid statuses are: Pending, Inprogress, Completed, Cancelled.");

                var success = await _repo.UpdateAsync(entity, userId);
                if (!success)
                    return ApiResponse<bool>.Fail($"Service with ID {entity.serviceId} not found.");

                return ApiResponse<bool>.Ok(true, "Service updated successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> DeleteAsync(int serviceId, int deletedBy)
        {
            try
            {
                var success = await _repo.DeleteAsync(serviceId, deletedBy);
                if (!success)
                    return ApiResponse<bool>.Fail($"Service with ID {serviceId} not found or already inactive.");

                return ApiResponse<bool>.Ok(true, "Service deleted successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.Fail(ex.Message);
            }
        }
    }
}