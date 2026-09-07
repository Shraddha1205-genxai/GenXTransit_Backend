using GenXTransitAPI.Models.DTO_s;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GenXTransitAPI.DataAccess.Interface.IRepositories
{
    public interface ITripRepository
    {
        Task<IEnumerable<TripDTO>> GetAllAsync(
            string? searchText,
            int? routeId,
            int? fleetId,
            string? tripStatus,
            DateTime? startDate,
            DateTime? endDate,
            bool? isActive,
            int? scopeToUser,
            int pageNumber = 1,
            int pageSize = 10);

        Task<TripDTO> GetByIdAsync(int tripId);
        Task<int> InsertAsync(TripDTO entity, int userId);
        Task<bool> UpdateAsync(TripDTO entity, int userId);
        Task<bool> DeleteAsync(int tripId, int deletedBy);
    }
}