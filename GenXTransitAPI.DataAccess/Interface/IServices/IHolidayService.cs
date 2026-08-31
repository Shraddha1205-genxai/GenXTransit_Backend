using GenXTransitAPI.Models;
using GenXTransitAPI.Models.DTO_s;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GenXTransitAPI.DataAccess.Interface.IServices
{
    public interface IHolidayService
    {
        Task<ApiResponse<IEnumerable<HolidayDTO>>> GetAllAsync(
            string? searchText,
            string? type,
            DateTime? startDate,
            DateTime? endDate,
            bool? isActive,
            int? scopeToUser,
            int pageNumber = 1,
            int pageSize = 10);

        Task<ApiResponse<HolidayDTO>> GetByIdAsync(int holidayId);
        Task<ApiResponse<string>> GetNextCodeAsync();
        Task<ApiResponse<int>> InsertAsync(HolidayDTO entity, int userId);
        Task<ApiResponse<bool>> UpdateAsync(HolidayDTO entity, int userId);
        Task<ApiResponse<bool>> DeleteAsync(int holidayId, int deletedBy);
    }
}