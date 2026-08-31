using Dapper;
using GenXTransitAPI.DataAccess.Data;
using GenXTransitAPI.DataAccess.Interface.IRepositories;
using GenXTransitAPI.Models;
using GenXTransitAPI.Models.DTOs;
using GenXTransitAPI.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenXTransitAPI.DataAccess.Repositories
{
    public class UserMasterRepository : IUserRepository
    {
        private readonly DBHelper _db;

        public UserMasterRepository(DBHelper dbHelper)
        {
            _db = dbHelper;
        }
        public async Task<bool> EmailExistsAsync(string email)
        {
            using var conn = _db.CreateConnection();

            var result = await conn.ExecuteScalarAsync<int>(
                "usp_User_EmailExists",
                new
                {
                    Email = email
                },
                commandType: CommandType.StoredProcedure);

            return result > 0;
        }

        public async Task<bool> UserNameExistsAsync(string userName)
        {
            using var conn = _db.CreateConnection();

            var result = await conn.ExecuteScalarAsync<int>(
                "usp_User_UserNameExists",
                new
                {
                    UserName = userName
                },
                commandType: CommandType.StoredProcedure);

            return result > 0;
        }

        public async Task<int> AddUserAsync(User user, int userId)
        {
            using var conn = _db.CreateConnection();
            var p = new DynamicParameters();

            p.Add("@UserName", user.UserName);
            p.Add("@Email", user.Email);
            p.Add("@MobileNo", user.MobileNo);
            p.Add("@PasswordHash", user.PasswordHash);
            p.Add("@FirstName", user.FirstName);
            p.Add("@LastName", user.LastName);
            p.Add("@RoleId", user.RoleId);
            p.Add("@IsActive", user.IsActive);
            p.Add("@IsEmailVerified", user.IsEmailVerified);
            p.Add("@IsMobileVerified", user.IsMobileVerified);
            p.Add("@IsFirstLogin", user.IsFirstLogin);
            //p.Add("@PasswordChangedDate", user.PasswordChangedDate);
            p.Add("@CreatedBy", userId);

            return await conn.ExecuteScalarAsync<int>(
         "Add_User",
         p,
         commandType: CommandType.StoredProcedure);
        }

       // public async Task<ApiResponse<PagedResponse<User>>> GetAllUsersAsync(
       //string? searchText,
       //bool? isActive,
       //int currentUserId,
       //int pageNumber,
       //int pageSize)
       // {
       //     try
       //     {
       //         using var conn = _db.CreateConnection();

       //         var parameters = new DynamicParameters();

       //         parameters.Add("@SearchText", searchText);
       //         parameters.Add("@IsActive", isActive);
       //         parameters.Add("@CurrentUserId", currentUserId);
       //         parameters.Add("@PageNumber", pageNumber);
       //         parameters.Add("@PageSize", pageSize);

       //         using var multi = await conn.QueryMultipleAsync(
       //             "usp_User_GetAll",
       //             parameters,
       //             commandType: CommandType.StoredProcedure);

       //         // First result set = Total Records
       //         var totalRecords = await multi.ReadFirstOrDefaultAsync<int>();

       //         // Second result set = Users
       //         var users = (await multi.ReadAsync<User>()).ToList();

       //         var response = new PagedResponse<User>
       //         {
       //             Items = users,
       //             TotalRecords = totalRecords,
       //             PageNumber = pageNumber,
       //             PageSize = pageSize,
       //             TotalPages = (int)Math.Ceiling(
       //                 totalRecords / (double)pageSize)
       //         };

       //         return ApiResponse<PagedResponse<User>>.Ok(
       //             response,
       //             "Users fetched successfully.");
       //     }
       //     catch (Exception ex)
       //     {
       //         return ApiResponse<PagedResponse<User>>.Fail(
       //             $"Error while fetching users: {ex.Message}");
       //     }
       // }

        public async Task<ApiResponse<User>> GetAllUsersAsync(
    string? searchText,
    bool? isActive,
    int currentUserId,
    int pageNumber,
    int pageSize)
        {
            try
            {
                using var conn = _db.CreateConnection();

                var parameters = new DynamicParameters();

                parameters.Add("@SearchText", searchText);
                parameters.Add("@IsActive", isActive);
                parameters.Add("@CurrentUserId", currentUserId);
                parameters.Add("@PageNumber", pageNumber);
                parameters.Add("@PageSize", pageSize);

                using var multi = await conn.QueryMultipleAsync(
                    "usp_User_GetAll",
                    parameters,
                    commandType: CommandType.StoredProcedure);

                // Read total records
                await multi.ReadFirstOrDefaultAsync<int>();

                // Read users
                var users = (await multi.ReadAsync<User>()).ToList();

                if (users == null || users.Count == 0)
                {
                    return ApiResponse<User>.Fail(
                        "No users found.");
                }

                // Return first user
                return ApiResponse<User>.Ok(
                    users.First(),
                    "Users fetched successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<User>.Fail(
                    $"Error while fetching users: {ex.Message}");
            }
        }
        public async Task<ApiResponse<User>> GetUserByIdAsync(
     int userId)
        {
            try
            {
                var parameters = new DynamicParameters();
                using var conn = _db.CreateConnection();
                parameters.Add("@UserId", userId);

                var result = await conn.QueryFirstOrDefaultAsync<User>(
                    "usp_User_GetById",
                    parameters,
                    commandType: CommandType.StoredProcedure);

                if (result == null)
                {
                    return ApiResponse<User>.Fail(
                        "User not found.");
                }

                return ApiResponse<User>.Ok(
                    result,
                    "User fetched successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<User>.Fail(
                    $"Error while fetching user: {ex.Message}");
            }
        }

        public async Task<bool> UpdateUserAsync(int userId, UpdateUserRequest request)
        {
            using var conn = _db.CreateConnection();

            var result = await conn.ExecuteScalarAsync<bool>(
                "usp_User_Update",
                new
                {
                    UserId = userId,
                    UserName = request.UserName,
                    Email = request.Email,
                    MobileNo = request.MobileNo,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    RoleId= request.RoleId
                },
                commandType: CommandType.StoredProcedure);

            return result;
        }

        public async Task<ApiResponse<string>> DeleteUserAsync(
    int userId)
        {
            try
            {
                var parameters = new DynamicParameters();
                using var conn = _db.CreateConnection();

                parameters.Add("@UserId", userId);

                var result = await conn.QueryFirstOrDefaultAsync<dynamic>(
                    "usp_User_Delete",
                    parameters,
                    commandType: CommandType.StoredProcedure);

                if (result == null)
                {
                    return ApiResponse<string>.Fail(
                        "Unable to delete user.");
                }

                if (result.Status == 0)
                {
                    return ApiResponse<string>.Fail(
                        result.Message);
                }

                return ApiResponse<string>.Ok(
                    result.Message);
            }
            catch (Exception ex)
            {
                return ApiResponse<string>.Fail(
                    $"Error while deleting user: {ex.Message}");
            }
        }
    }
}


   