using GenXTransitAPI.Models.DTO_s;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GenXTransitAPI.DataAccess.Interface.IRepositories
{
    public interface IHolidayRepository
    {
        Task<IEnumerable<HolidayDTO>> GetAllAsync(
            string? searchText,
            string? type,
            DateTime? startDate,
            DateTime? endDate,
            bool? isActive,
            int? scopeToUser,
            int pageNumber = 1,
            int pageSize = 10);

        Task<HolidayDTO> GetByIdAsync(int holidayId);
        Task<string> GetNextCodeAsync();
        Task<int> InsertAsync(HolidayDTO entity, int userId);
        Task<bool> UpdateAsync(HolidayDTO entity, int userId);
        Task<bool> DeleteAsync(int holidayId, int deletedBy);
    }
}