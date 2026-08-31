using GenXTransitAPI.Models;
using GenXTransitAPI.Models.DTOs;
using GenXTransitAPI.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenXTransitAPI.DataAccess.Interface.IServices
{
    public interface IUserService
    {
        Task<AddUserResponse> AddUserAsync(
            AddUserRequest request, int userId);
        Task<ApiResponse<UpdateUserResponse>> UpdateUserAsync(UpdateUserRequest request, int userId);

        //    Task<ApiResponse<List<User>>> GetAllUsersAsync(string? searchText,
        //bool? isActive,
        //int currentUserId,
        //int pageNumber,
        //int pageSize);
        Task<ApiResponse<PagedResponse<User>>> GetAllUsersAsync(
        string? searchText,
        bool? isActive,
        int currentUserId,
        int pageNumber,
        int pageSize);

        Task<ApiResponse<User>> GetUserByIdAsync(int userId);

        Task<ApiResponse<string>> DeleteUserAsync(int userId);
    }
}
