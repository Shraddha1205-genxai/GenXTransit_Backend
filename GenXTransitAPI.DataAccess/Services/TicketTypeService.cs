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
    public class TicketTypeService : ITicketTypeService
    {
        private readonly ITicketTypeRepository _repo;

        public TicketTypeService(ITicketTypeRepository repo)
        {
            _repo = repo;
        }

        public async Task<ApiResponse<IEnumerable<TicketTypeDTO>>> GetAllAsync(
            string? searchText,
            bool? isActive,
            int? scopeToUser,
            int pageNumber = 1,
            int pageSize = 10)
        {
            try
            {
                var items = await _repo.GetAllAsync(searchText, isActive, scopeToUser, pageNumber, pageSize);
                var totalCount = items.FirstOrDefault()?.totalCount ?? 0;
                return ApiResponse<IEnumerable<TicketTypeDTO>>.Ok(items, null, totalCount);
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<TicketTypeDTO>>.Fail($"Error retrieving ticket types: {ex.Message}");
            }
        }

        public async Task<ApiResponse<TicketTypeDTO>> GetByIdAsync(int ticketId)
        {
            try
            {
                var item = await _repo.GetByIdAsync(ticketId);
                if (item == null)
                    return ApiResponse<TicketTypeDTO>.Fail($"Ticket type with ID {ticketId} not found.");

                return ApiResponse<TicketTypeDTO>.Ok(item);
            }
            catch (Exception ex)
            {
                return ApiResponse<TicketTypeDTO>.Fail($"Error retrieving ticket type: {ex.Message}");
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

        public async Task<ApiResponse<int>> InsertAsync(TicketTypeDTO entity, int userId)
        {
            try
            {
                // Validations
                if (string.IsNullOrWhiteSpace(entity.ticketName))
                    return ApiResponse<int>.Fail("Ticket Name is required.");

                var id = await _repo.InsertAsync(entity, userId);
                return ApiResponse<int>.Ok(id, "Ticket type created successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<int>.Fail($"An error occurred while creating ticket type. {ex.Message}");
            }
        }

        public async Task<ApiResponse<bool>> UpdateAsync(TicketTypeDTO entity, int userId)
        {
            try
            {
                // Validations
                if (string.IsNullOrEmpty(entity.ticketId))
                    return ApiResponse<bool>.Fail("Ticket ID is required.");

                if (!int.TryParse(entity.ticketId, out int ticketId))
                    return ApiResponse<bool>.Fail("Invalid Ticket ID format.");

                if (string.IsNullOrWhiteSpace(entity.ticketName))
                    return ApiResponse<bool>.Fail("Ticket Name is required.");

                var success = await _repo.UpdateAsync(entity, userId);
                if (!success)
                    return ApiResponse<bool>.Fail($"Ticket type with ID {entity.ticketId} not found.");

                return ApiResponse<bool>.Ok(true, "Ticket type updated successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.Fail($"An error occurred while updating ticket type. {ex.Message}");
            }
        }

        public async Task<ApiResponse<bool>> DeleteAsync(int ticketId, int deletedBy)
        {
            try
            {
                var success = await _repo.DeleteAsync(ticketId, deletedBy);
                if (!success)
                    return ApiResponse<bool>.Fail($"Ticket type with ID {ticketId} not found or already inactive.");

                return ApiResponse<bool>.Ok(true, "Ticket type deleted successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.Fail($"An error occurred while deleting ticket type. {ex.Message}");
            }
        }
    }
}