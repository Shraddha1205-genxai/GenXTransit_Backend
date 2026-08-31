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
    public class ComplaintCategoryService : IComplaintCategoryService
    {
        private readonly IComplaintCategoryRepository _repo;

        public ComplaintCategoryService(IComplaintCategoryRepository repo)
        {
            _repo = repo;
        }

        public async Task<ApiResponse<IEnumerable<ComplaintCategoryDTO>>> GetAllAsync(
            string? searchText,
            string? complaintCategory,
            string? sla,
            bool? isActive,
            int? scopeToUser,
            int pageNumber = 1,
            int pageSize = 10)
        {
            try
            {
                var items = await _repo.GetAllAsync(searchText, complaintCategory, sla, isActive, scopeToUser, pageNumber, pageSize);
                var totalCount = items.FirstOrDefault()?.totalCount ?? 0;
                return ApiResponse<IEnumerable<ComplaintCategoryDTO>>.Ok(items, null, totalCount);
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<ComplaintCategoryDTO>>.Fail($"Error retrieving complaint categories: {ex.Message}");
            }
        }

        public async Task<ApiResponse<ComplaintCategoryDTO>> GetByIdAsync(int complaintId)
        {
            try
            {
                var item = await _repo.GetByIdAsync(complaintId);
                if (item == null)
                    return ApiResponse<ComplaintCategoryDTO>.Fail($"Complaint category with ID {complaintId} not found.");

                return ApiResponse<ComplaintCategoryDTO>.Ok(item);
            }
            catch (Exception ex)
            {
                return ApiResponse<ComplaintCategoryDTO>.Fail($"Error retrieving complaint category: {ex.Message}");
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

        public async Task<ApiResponse<int>> InsertAsync(ComplaintCategoryDTO entity, int userId)
        {
            try
            {
                // Validations
                if (string.IsNullOrWhiteSpace(entity.complaintTitle))
                    return ApiResponse<int>.Fail("Complaint Title is required.");

                if (string.IsNullOrWhiteSpace(entity.complaintCategory))
                    return ApiResponse<int>.Fail("Complaint Category is required.");

                if (string.IsNullOrWhiteSpace(entity.sla))
                    return ApiResponse<int>.Fail("SLA is required.");

                // Validate Complaint Category
                var validCategories = new[] { "General", "Technical", "Billing", "Service", "Other" };
                if (!validCategories.Contains(entity.complaintCategory))
                    return ApiResponse<int>.Fail("Invalid Complaint Category. Valid categories are: General, Technical, Billing, Service, Other.");

                // Validate SLA
                var validSLAs = new[] { "24 Hours", "48 Hours", "72 Hours", "1 Week", "2 Weeks" };
                if (!validSLAs.Contains(entity.sla))
                    return ApiResponse<int>.Fail("Invalid SLA. Valid SLAs are: 24 Hours, 48 Hours, 72 Hours, 1 Week, 2 Weeks.");

                var id = await _repo.InsertAsync(entity, userId);
                return ApiResponse<int>.Ok(id, "Complaint category created successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<int>.Fail($"An error occurred while creating complaint category. {ex.Message}");
            }
        }

        public async Task<ApiResponse<bool>> UpdateAsync(ComplaintCategoryDTO entity, int userId)
        {
            try
            {
                // Validations
                if (string.IsNullOrEmpty(entity.complaintId))
                    return ApiResponse<bool>.Fail("Complaint ID is required.");

                if (!int.TryParse(entity.complaintId, out int complaintId))
                    return ApiResponse<bool>.Fail("Invalid Complaint ID format.");

                if (string.IsNullOrWhiteSpace(entity.complaintTitle))
                    return ApiResponse<bool>.Fail("Complaint Title is required.");

                if (string.IsNullOrWhiteSpace(entity.complaintCategory))
                    return ApiResponse<bool>.Fail("Complaint Category is required.");

                if (string.IsNullOrWhiteSpace(entity.sla))
                    return ApiResponse<bool>.Fail("SLA is required.");

                // Validate Complaint Category
                var validCategories = new[] { "General", "Technical", "Billing", "Service", "Other" };
                if (!validCategories.Contains(entity.complaintCategory))
                    return ApiResponse<bool>.Fail("Invalid Complaint Category. Valid categories are: General, Technical, Billing, Service, Other.");

                // Validate SLA
                var validSLAs = new[] { "24 Hours", "48 Hours", "72 Hours", "1 Week", "2 Weeks" };
                if (!validSLAs.Contains(entity.sla))
                    return ApiResponse<bool>.Fail("Invalid SLA. Valid SLAs are: 24 Hours, 48 Hours, 72 Hours, 1 Week, 2 Weeks.");

                var success = await _repo.UpdateAsync(entity, userId);
                if (!success)
                    return ApiResponse<bool>.Fail($"Complaint category with ID {entity.complaintId} not found.");

                return ApiResponse<bool>.Ok(true, "Complaint category updated successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.Fail($"An error occurred while updating complaint category. {ex.Message}");
            }
        }

        public async Task<ApiResponse<bool>> DeleteAsync(int complaintId, int deletedBy)
        {
            try
            {
                var success = await _repo.DeleteAsync(complaintId, deletedBy);
                if (!success)
                    return ApiResponse<bool>.Fail($"Complaint category with ID {complaintId} not found or already inactive.");

                return ApiResponse<bool>.Ok(true, "Complaint category deleted successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.Fail($"An error occurred while deleting complaint category. {ex.Message}");
            }
        }
    }
}