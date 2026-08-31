using GenXTransitAPI.Models;
using GenXTransitAPI.Models.DTOs;
using GenXTransitAPI.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenXTransitAPI.DataAccess.Interface.IRepositories
{
    public interface IUserRepository
    {
        Task<bool> EmailExistsAsync(string email);

        Task<bool> UserNameExistsAsync(string userName);

        Task<int> AddUserAsync(User user, int userId);

        Task<bool> UpdateUserAsync(int userId, UpdateUserRequest request);

        //    Task<ApiResponse<List<User>>> GetAllUsersAsync(string? searchText,
        //bool? isActive,
        //int currentUserId,
        //int pageNumber,
        //int pageSize);

        //    Task<ApiResponse<PagedResponse<User>>> GetAllUsersAsync(
        //string? searchText,
        //bool? isActive,
        //int currentUserId,
        //int pageNumber,
        //int pageSize);

        Task<ApiResponse<User>> GetAllUsersAsync(
    string? searchText,
    bool? isActive,
    int currentUserId,
    int pageNumber,
    int pageSize);

        Task<ApiResponse<User>> GetUserByIdAsync(int userId);

        Task<ApiResponse<string>> DeleteUserAsync(int userId);
    }
}
