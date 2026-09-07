using GenXTransitAPI.Models;
using GenXTransitAPI.Models.DTO_s;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GenXTransitAPI.DataAccess.Interface.IServices
{
    public interface ITripService
    {
        Task<ApiResponse<IEnumerable<TripDTO>>> GetAllAsync(
            string? searchText,
            int? depotId,
            int? routeId,
            int? fleetId,
            string? tripStatus,
            DateTime? startDate,
            DateTime? endDate,
            bool? isActive,
            int? scopeToUser,
            int pageNumber = 1,
            int pageSize = 10);

        Task<ApiResponse<TripDTO>> GetByIdAsync(int tripId);
        Task<ApiResponse<int>> InsertAsync(TripDTO entity, int userId);
        Task<ApiResponse<bool>> UpdateAsync(TripDTO entity, int userId);
        Task<ApiResponse<bool>> DeleteAsync(int tripId, int deletedBy);
    }
}