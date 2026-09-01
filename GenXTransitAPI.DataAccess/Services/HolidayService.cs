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
    public class HolidayService : IHolidayService
    {
        private readonly IHolidayRepository _repo;

        public HolidayService(IHolidayRepository repo)
        {
            _repo = repo;
        }

        public async Task<ApiResponse<IEnumerable<HolidayDTO>>> GetAllAsync(
            string? searchText,
            string? type,
            DateTime? startDate,
            DateTime? endDate,
            bool? isActive,
            int? scopeToUser,
            int pageNumber = 1,
            int pageSize = 10)
        {
            try
            {
                var items = await _repo.GetAllAsync(searchText, type, startDate, endDate, isActive, scopeToUser, pageNumber, pageSize);
                var totalCount = items.FirstOrDefault()?.totalCount ?? 0;
                return ApiResponse<IEnumerable<HolidayDTO>>.Ok(items, null, totalCount);
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<HolidayDTO>>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<HolidayDTO>> GetByIdAsync(int holidayId)
        {
            try
            {
                var item = await _repo.GetByIdAsync(holidayId);
                if (item == null)
                    return ApiResponse<HolidayDTO>.Fail($"Holiday with ID {holidayId} not found.");

                return ApiResponse<HolidayDTO>.Ok(item);
            }
            catch (Exception ex)
            {
                return ApiResponse<HolidayDTO>.Fail(ex.Message);
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

        public async Task<ApiResponse<int>> InsertAsync(HolidayDTO entity, int userId)
        {
            try
            {
                // Validations
                if (string.IsNullOrWhiteSpace(entity.holidayName))
                    return ApiResponse<int>.Fail("Holiday Name is required.");

                if (string.IsNullOrWhiteSpace(entity.occasion))
                    return ApiResponse<int>.Fail("Occasion is required.");

                if (string.IsNullOrWhiteSpace(entity.date))
                    return ApiResponse<int>.Fail("Date is required.");

                if (string.IsNullOrWhiteSpace(entity.type))
                    return ApiResponse<int>.Fail("Type is required.");

                // Validate date format
                if (!DateTime.TryParse(entity.date, out DateTime date))
                    return ApiResponse<int>.Fail("Invalid date format. Please use yyyy-MM-dd.");

                var id = await _repo.InsertAsync(entity, userId);
                return ApiResponse<int>.Ok(id, "Holiday created successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<int>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> UpdateAsync(HolidayDTO entity, int userId)
        {
            try
            {
                // Validations
                if (string.IsNullOrEmpty(entity.holidayId))
                    return ApiResponse<bool>.Fail("Holiday ID is required.");

                if (!int.TryParse(entity.holidayId, out int holidayId))
                    return ApiResponse<bool>.Fail("Invalid Holiday ID format.");

                if (string.IsNullOrWhiteSpace(entity.holidayName))
                    return ApiResponse<bool>.Fail("Holiday Name is required.");

                if (string.IsNullOrWhiteSpace(entity.occasion))
                    return ApiResponse<bool>.Fail("Occasion is required.");

                if (string.IsNullOrWhiteSpace(entity.date))
                    return ApiResponse<bool>.Fail("Date is required.");

                if (string.IsNullOrWhiteSpace(entity.type))
                    return ApiResponse<bool>.Fail("Type is required.");

                // Validate date format
                if (!DateTime.TryParse(entity.date, out DateTime date))
                    return ApiResponse<bool>.Fail("Invalid date format. Please use yyyy-MM-dd.");

                var success = await _repo.UpdateAsync(entity, userId);
                if (!success)
                    return ApiResponse<bool>.Fail($"Holiday with ID {entity.holidayId} not found.");

                return ApiResponse<bool>.Ok(true, "Holiday updated successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> DeleteAsync(int holidayId, int deletedBy)
        {
            try
            {
                var success = await _repo.DeleteAsync(holidayId, deletedBy);
                if (!success)
                    return ApiResponse<bool>.Fail($"Holiday with ID {holidayId} not found or already inactive.");

                return ApiResponse<bool>.Ok(true, "Holiday deleted successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.Fail(ex.Message);
            }
        }
    }
}