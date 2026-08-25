using GenXTransitAPI.DataAccess.Interface.IRepositories;
using GenXTransitAPI.DataAccess.Interface.IServices;
using GenXTransitAPI.DataAccess.Repositories;
using GenXTransitAPI.DataAccess.Security;
using GenXTransitAPI.Models;
using GenXTransitAPI.Models.DTO_s;
using GenXTransitAPI.Models.DTOs;
using GenXTransitAPI.Models.Entities;
//using Microsoft.AspNetCore.Identity.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenXTransitAPI.DataAccess.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepo;
        private readonly IPasswordService _passwordService;
        private readonly IEmailService _emailService;
        private readonly IJwtService _jwtService;

        public AuthService(
       IAuthRepository authRepo,
       IPasswordService passwordService,
       IEmailService emailService,
       IJwtService jwtService)
        {
            _authRepo = authRepo;
            _passwordService = passwordService;
            _emailService = emailService;
            _jwtService = jwtService;
        }

        public async Task<RegisterUserResponse> RegisterAsync(
       RegisterUserRequest request)
        {
            if (await _authRepo.EmailExistsAsync(request.Email))
            {
                throw new Exception("Email already exists.");
            }

            if (await _authRepo.UserNameExistsAsync(request.UserName))
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
                CreatedBy = 1
            };

            var userId =
                await _authRepo.RegisterUserAsync(user);

            await _emailService.SendUserCreatedEmail(
                request.Email,
                request.UserName,
                temporaryPassword);

            return new RegisterUserResponse
            {
                UserId = userId,
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
                await _authRepo.GetUserByIdAsync(userId);

            if (existingUser == null)
            {
                return ApiResponse<UpdateUserResponse>.Fail(
                    "User not found.");
            }

            if (!existingUser.IsActive)
            {
                return ApiResponse<UpdateUserResponse>.Fail(
                    "User account is inactive.");
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
                await _authRepo.UpdateUserAsync(
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

        public async Task<ApiResponse<LoginResponse>> LoginAsync(LoginRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.LoginId))
            {
                return ApiResponse<LoginResponse>.Fail(
                    "Login ID is required.");
            }

            if (string.IsNullOrWhiteSpace(request.Password))
            {
                return ApiResponse<LoginResponse>.Fail(
                    "Password is required.");
            }

            var user =
                await _authRepo.GetUserForLoginAsync(
                    request.LoginId.Trim());

            if (user == null)
            {
                return ApiResponse<LoginResponse>.Fail(
                    "Invalid login ID or password.");
            }

            if (!user.IsActive)
            {
                return ApiResponse<LoginResponse>.Fail(
                    "Your account is inactive. Please contact administrator.");
            }

            var passwordValid =
                _passwordService.VerifyPassword(
                    request.Password,
                    user.PasswordHash);

            if (!passwordValid)
            {
                return ApiResponse<LoginResponse>.Fail(
                    "Invalid login ID or password.");
            }

            // Generate Access Token
            var accessToken =
                _jwtService.GenerateAccessToken(user);

            // Generate Refresh Token
            var refreshToken =
                _jwtService.GenerateRefreshToken(user);

            var response = new LoginResponse
            {
                UserId = user.UserId,

                UserName = user.UserName,

                Email = user.Email,

                //RoleId = user.RoleId,

                AccessToken = accessToken,

                RefreshToken = refreshToken,

                //IsFirstLogin = user.IsFirstLogin,

                //Message = user.IsFirstLogin
                //    ? "Login successful. Please change your password."
                //    : "Login successful."
            };

            return ApiResponse<LoginResponse>.Ok(
                response,
                "Login successful.");
        }
    }
}
