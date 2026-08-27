using GenXTransitAPI.Models;
using GenXTransitAPI.Models.DTO_s;
using GenXTransitAPI.Models.DTOs;
using GenXTransitAPI.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenXTransitAPI.DataAccess.Interface.IServices
{
    public interface IAuthService
    {
        Task<RegisterUserResponse> RegisterAsync(
            RegisterUserRequest request);
        Task<ApiResponse<UpdateUserResponse>> UpdateUserAsync( UpdateUserRequest request, int userId);


        Task<ApiResponse<LoginResponse>> LoginAsync(LoginRequest request);
        Task<ApiResponse<string>> ChangePasswordAsync( ChangePasswordRequest request,int userId);
        Task<ApiResponse<string>> ForgotPasswordAsync( ForgotPasswordRequest request);

        Task<ApiResponse<string>> ResetPasswordAsync( ResetPasswordRequest request);

        Task<ApiResponse<RefreshTokenResponse>> RefreshTokenAsync(RefreshTokenRequest request);
    }
}
