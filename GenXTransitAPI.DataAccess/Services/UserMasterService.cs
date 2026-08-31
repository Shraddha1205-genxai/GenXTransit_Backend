using GenXTransitAPI.DataAccess.Interface.IRepositories;
using GenXTransitAPI.DataAccess.Interface.IServices;
using GenXTransitAPI.DataAccess.Security;
using GenXTransitAPI.Models;
using GenXTransitAPI.Models.DTO_s;
using GenXTransitAPI.Models.DTOs;
using GenXTransitAPI.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenXTransitAPI.DataAccess.Services
{
    public class UserMasterService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordService _passwordService;
        private readonly IEmailService _emailService;
        private readonly IJwtService _jwtService;

        public UserMasterService(IUserRepository userRepository, IPasswordService passwordService,
       IEmailService emailService,
       IJwtService jwtService)
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
            _emailService = emailService;
            _jwtService = jwtService;
        }

        public async Task<AddUserResponse> AddUserAsync(
      AddUserRequest request, int userId)
        {
            if (await _userRepository.EmailExistsAsync(request.Email))
            {
                throw new Exception("Email already exists.");
            }

            if (await _userRepository.UserNameExistsAsync(request.UserName))
            {
                throw new Exception("Username already exists.");
            }

            var temporaryPassword =
                _passwordService.GenerateTemporaryPassword();

            var passwordHash =
                _passwordService.HashPassword(
                    temporaryPassword);

            var user = new User
            {
                UserName = request.UserName,
                Email = request.Email,
                MobileNo = request.MobileNo,

                FirstName = request.FirstName,
                LastName = request.LastName,

                //RoleId = request.RoleId,

                PasswordHash = passwordHash,

                IsActive = true,
                IsEmailVerified = false,
                IsMobileVerified = false,

                IsFirstLogin = true,

                PasswordChangedDate = null,

                CreatedDate = DateTime.UtcNow,

                // Replace with current logged-in admin ID
                CreatedBy = userId
            };

            var newUserId =
                await _userRepository.AddUserAsync(user,userId);

            await _emailService.SendUserCreatedEmail(
                request.Email,
                request.UserName,
                temporaryPassword);

            return new AddUserResponse
            {
                UserId = newUserId,
                UserName = request.UserName,
                Email = request.Email,
                Message =
                    "User registered successfully. " +
                    "Login credentials have been sent to the registered email."
            };
        }

        public async Task<ApiResponse<UpdateUserResponse>> UpdateUserAsync(
    UpdateUserRequest request,
    int userId)
        {
            if (userId <= 0)
            {
                return ApiResponse<UpdateUserResponse>.Fail(
                    "Invalid user.");
            }

            var existingUser =
                await _userRepository.GetUserByIdAsync(userId);

            if (existingUser == null)
            {
                return ApiResponse<UpdateUserResponse>.Fail(
                    "User not found.");
            }

            if (string.IsNullOrWhiteSpace(request.UserName))
            {
                return ApiResponse<UpdateUserResponse>.Fail(
                    "User name is required.");
            }

            if (string.IsNullOrWhiteSpace(request.Email))
            {
                return ApiResponse<UpdateUserResponse>.Fail(
                    "Email is required.");
            }

            var updated =
                await _userRepository.UpdateUserAsync(
                    userId,
                    request);

            if (!updated)
            {
                return ApiResponse<UpdateUserResponse>.Fail(
                    "Unable to update user details.");
            }

            var response = new UpdateUserResponse
            {
                UserId = userId,
                UserName = request.UserName,
                Email = request.Email,
                MobileNo = request.MobileNo,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Message = "User details updated successfully."
            };

            return ApiResponse<UpdateUserResponse>.Ok(
                response,
                "User details updated successfully.");
        }

        //    public async Task<ApiResponse<List<User>>> GetAllUsersAsync(string? searchText,
        //bool? isActive,
        //int currentUserId,
        //int pageNumber,
        //int pageSize)
        //    {
        //        try
        //        {
        //            return await _userRepository.GetAllUsersAsync();
        //        }
        //        catch (Exception ex)
        //        {
        //            return ApiResponse<List<User>>.Fail(
        //                $"Error while fetching users: {ex.Message}");
        //        }
        //    }

    //    public async Task<ApiResponse<PagedResponse<User>>> GetAllUsersAsync(
    //string? searchText,
    //bool? isActive,
    //int currentUserId,
    //int pageNumber,
    //int pageSize)
    //    {
    //        try
    //        {
                
    //            return await _userRepository.GetAllUsersAsync(
    //                searchText,
    //                isActive,
    //                currentUserId,
    //                pageNumber,
    //                pageSize);
    //        }
    //        catch (Exception ex)
    //        {
    //            return ApiResponse<PagedResponse<User>>.Fail(
    //                $"Error while fetching users: {ex.Message}");
    //        }
    //    }

        public async Task<ApiResponse<User>> GetAllUsersAsync(
    string? searchText,
    bool? isActive,
    int currentUserId,
    int pageNumber,
    int pageSize)
        {
            try
            {
                return await _userRepository.GetAllUsersAsync(
                    searchText,
                    isActive,
                    currentUserId,
                    pageNumber,
                    pageSize);
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
                if (userId <= 0)
                {
                    return ApiResponse<User>.Fail(
                        "Invalid UserId.");
                }

                return await _userRepository.GetUserByIdAsync(userId);
            }
            catch (Exception ex)
            {
                return ApiResponse<User>.Fail(
                    $"Error while fetching user: {ex.Message}");
            }
        }
        public async Task<ApiResponse<string>> DeleteUserAsync(int userId)
        {
            try
            {
                if (userId <= 0)
                {
                    return ApiResponse<string>.Fail(
                        "Invalid UserId.");
                }

                return await _userRepository.DeleteUserAsync(userId);
            }
            catch (Exception ex)
            {
                return ApiResponse<string>.Fail(
                    $"Error while deleting user: {ex.Message}");
            }
        }
    }
}
   