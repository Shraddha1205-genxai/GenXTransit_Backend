using GenXTransitAPI.DataAccess.Interface.IRepositories;
using GenXTransitAPI.DataAccess.Interface.IServices;
using GenXTransitAPI.DataAccess.Repositories;
using GenXTransitAPI.DataAccess.Security;
using GenXTransitAPI.Models;
using GenXTransitAPI.Models.DTO_s;
using GenXTransitAPI.Models.DTOs;
using GenXTransitAPI.Models.Entities;
using Org.BouncyCastle.Crypto.Generators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GenXTransitAPI.DataAccess.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepo;
        private readonly IUserRepository _userRepo;
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


        public async Task<ApiResponse<string>> ChangePasswordAsync(
    ChangePasswordRequest request,
    int userId)
        {
            if (request == null)
            {
                return ApiResponse<string>.Fail(
                    "Invalid request.");
            }

            if (string.IsNullOrWhiteSpace(request.OldPassword))
            {
                return ApiResponse<string>.Fail(
                    "Old password is required.");
            }

            if (string.IsNullOrWhiteSpace(request.NewPassword))
            {
                return ApiResponse<string>.Fail(
                    "New password is required.");
            }

            if (string.IsNullOrWhiteSpace(request.ConfirmPassword))
            {
                return ApiResponse<string>.Fail(
                    "Confirm password is required.");
            }

            if (request.NewPassword != request.ConfirmPassword)
            {
                return ApiResponse<string>.Fail(
                    "New password and confirm password do not match.");
            }

            if (request.NewPassword.Length < 8)
            {
                return ApiResponse<string>.Fail(
                    "New password must be at least 8 characters long.");
            }

            // Get user
            var user =
                await _userRepo.GetUserByIdAsync(userId);

            if (user == null)
            {
                return ApiResponse<string>.Fail(
                    "User not found.");
            }

            // Verify old password using PBKDF2 PasswordService
            var oldPasswordValid = _passwordService.VerifyPassword(
                request.OldPassword,
                user.Data.PasswordHash);

            if (!oldPasswordValid)
            {
                return ApiResponse<string>.Fail(
                    "Old password is incorrect.");
            }

            // Prevent same password
            var samePassword = _passwordService.VerifyPassword(
                request.NewPassword,
                user.Data.PasswordHash);

            if (samePassword)
            {
                return ApiResponse<string>.Fail(
                    "New password cannot be same as old password.");
            }

            // Generate new PBKDF2 password hash
            var newHash = _passwordService.HashPassword(
                request.NewPassword);

            // Update password
            var passwordUpdated =
                await _authRepo.UpdateUserPasswordAsync(
                    userId,
                    newHash,
                    userId);

            if (!passwordUpdated)
            {
                return ApiResponse<string>.Fail(
                    "Password could not be changed.");
            }

            // Revoke all refresh tokens
            await _authRepo.RevokeAllUserRefreshTokensAsync(
                userId,
                userId);

            return ApiResponse<string>.Ok(
                "Password changed successfully.");
        }
        private string GenerateResetToken()
        {
            // Use a GUID + random bytes for even more uniqueness
            byte[] randomBytes = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }
            // Use Base64UrlEncoder or manual conversion
            return Base64UrlEncode(randomBytes);
        }

        private string Base64UrlEncode(byte[] input)
        {
            return Convert.ToBase64String(input)
                .TrimEnd('=')
                .Replace('+', '-')
                .Replace('/', '_');
        }

        private string HashResetToken(string token)
        {
            using var sha256 = SHA256.Create();
            // Use UTF8 encoding consistently
            byte[] bytes = Encoding.UTF8.GetBytes(token);
            byte[] hash = sha256.ComputeHash(bytes);
            // Use standard Base64 for storage
            return Convert.ToBase64String(hash);
        }

        //private string GenerateResetToken()
        //{
        //    byte[] tokenBytes = RandomNumberGenerator.GetBytes(32);

        //    return Convert.ToBase64String(tokenBytes)
        //        .Replace("+", "-")
        //        .Replace("/", "_")
        //        .Replace("=", "");
        //}

        //private string HashResetToken(string token)
        //{
        //    using var sha256 = SHA256.Create();

        //    byte[] bytes =
        //        Encoding.UTF8.GetBytes(token);

        //    byte[] hash =
        //        sha256.ComputeHash(bytes);

        //    return Convert.ToBase64String(hash);
        //}

        public async Task<ApiResponse<string>> ForgotPasswordAsync( ForgotPasswordRequest request)
        {
            try
            {
                if (request == null)
                {
                    return ApiResponse<string>.Fail(
                        "Invalid request.");
                }

                if (string.IsNullOrWhiteSpace(request.Email))
                {
                    return ApiResponse<string>.Fail(
                        "Email is required.");
                }

                string email = request.Email.Trim();

                // Get user
                var user =
                    await _authRepo.GetUserByEmailAsync(email);

                // Do not reveal whether email exists
                if (user == null)
                {
                    return ApiResponse<string>.Ok(
                        "No account found with the provided email address.");
                }

                // Generate secure random token
                string token =
                    GenerateResetToken();

                // Hash token before saving in DB
                string tokenHash =
                    HashResetToken(token);

              
                // Token valid for 30 minutes
                DateTime tokenExpiry =
                    DateTime.UtcNow.AddMinutes(30);

                // Save token hash
                var result =
                    await _authRepo.CreatePasswordResetTokenAsync(
                        user.UserId,
                        tokenHash,
                        tokenExpiry);

                if (result == null ||
                    result.RowsAffected <= 0)
                {
                  
                    return ApiResponse<string>.Fail(
                        "Unable to generate password reset link.");
                }

                // Create reset URL
                string resetUrl =
                    $"https://asset.genxai.com/reset-password?token={Uri.EscapeDataString(token)}";

                // Send email
                _emailService.SendEmailAsync(
                    user.Email,
                    "Reset Your Password",
                    $@"
            <html>
            <body>
                <p>Dear User,</p>

                <p>
                    We received a request to reset your password.
                </p>

                <p>
                    Click the button below to reset your password:
                </p>

                <p>
                    <a href='{resetUrl}'
                       style='
                       display:inline-block;
                       padding:10px 20px;
                       background-color:#007bff;
                       color:white;
                       text-decoration:none;
                       border-radius:5px;'>
                       Reset Password
                    </a>
                </p>

                <p>
                    This link will expire in 30 minutes.
                </p>

                <p>
                    If you did not request a password reset,
                    please ignore this email.
                </p>

                <p>
                    Regards,<br/>
                    GenXAI Platform
                </p>
            </body>
            </html>"
                );

                return ApiResponse<string>.Ok(
                    "Password reset link has been sent.");
            }
            catch (Exception ex)
            {
                return ApiResponse<string>.Fail(
                    ex.Message);
            }
        }


        public async Task<ApiResponse<string>> ResetPasswordAsync( ResetPasswordRequest request)
        {
            try
            {
                if (request == null)
                {
                    return ApiResponse<string>.Fail(
                        "Invalid request.");
                }

                if (string.IsNullOrWhiteSpace(request.Token))
                {
                    return ApiResponse<string>.Fail(
                        "Reset password token is required.");
                }

                //if (string.IsNullOrWhiteSpace(request.NewPassword))
                //{
                //    return ApiResponse<string>.Fail(
                //        "New password is required.");
                //}

                if (string.IsNullOrWhiteSpace(request.ConfirmPassword))
                {
                    return ApiResponse<string>.Fail(
                        "Confirm password is required.");
                }

                if (request.NewPassword != request.ConfirmPassword)
                {
                    return ApiResponse<string>.Fail(
                        "New password and confirm password do not match.");
                }

                if (request.NewPassword.Length < 8)
                {
                    return ApiResponse<string>.Fail(
                        "New password must be at least 8 characters long.");
                }
               

                //// Hash token received from frontend
                //string tokenHash =
                //    HashResetToken(request.Token);

            
                // Generate PBKDF2 password hash
                string newPasswordHash =
                    _passwordService.HashPassword(
                        request.NewPassword);

                // Reset password
                var result =
                    await _authRepo.ResetUserPasswordAsync(
                        request.Token,
                        newPasswordHash
                        );

                if (result == null ||
                    result.Status != 1)
                {
                    return ApiResponse<string>.Fail(
                        result?.Message ??
                        "Password reset failed.");
                }

                // Revoke all refresh tokens
                if (result.UserId.HasValue)
                {
                    await _authRepo.RevokeAllUserRefreshTokensAsync(
                        result.UserId.Value,
                        result.UserId.Value);
                }

                return ApiResponse<string>.Ok(
                    "Password reset successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<string>.Fail(
                    ex.Message);
            }
        }

        public async Task<ApiResponse<RefreshTokenResponse>> RefreshTokenAsync(
    RefreshTokenRequest request)
        {
            try
            {
                // 1. Validate request
                if (request == null)
                {
                    return ApiResponse<RefreshTokenResponse>.Fail(
                        "Invalid request.");
                }

                // 2. Validate refresh token
                if (string.IsNullOrWhiteSpace(request.RefreshToken))
                {
                    return ApiResponse<RefreshTokenResponse>.Fail(
                        "Refresh token is required.");
                }

                // 3. Validate JWT refresh token
                var principal =
                    _jwtService.ValidateRefreshToken(
                        request.RefreshToken);

                if (principal == null)
                {
                    return ApiResponse<RefreshTokenResponse>.Fail(
                        "Invalid or expired refresh token.");
                }

                // 4. Get UserId from refresh token
                var userIdClaim =
                    principal.FindFirst(
                        ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrWhiteSpace(userIdClaim))
                {
                    return ApiResponse<RefreshTokenResponse>.Fail(
                        "Invalid refresh token.");
                }

                if (!int.TryParse(
                    userIdClaim,
                    out int userId))
                {
                    return ApiResponse<RefreshTokenResponse>.Fail(
                        "Invalid user identity.");
                }

                // 5. Get user from database
                var user =
                    await _userRepo.GetUserByIdAsync(userId);

                if (user == null)
                {
                    return ApiResponse<RefreshTokenResponse>.Fail(
                        "User not found.");
                }

                // 6. Check whether user is active
                if (!user.Data.IsActive)
                {
                    return ApiResponse<RefreshTokenResponse>.Fail(
                        "Your account is inactive. Please contact administrator.");
                }

                // 7. Generate new Access Token
                var newAccessToken =
                    _jwtService.GenerateAccessToken(user.Data);

                // 8. Generate new Refresh Token
                var newRefreshToken =
                    _jwtService.GenerateRefreshToken(user.Data);

                // 9. Create response
                var response = new RefreshTokenResponse
                {
                    UserId = user.Data.UserId,

                    AccessToken = newAccessToken,

                    RefreshToken = newRefreshToken
                };

                return ApiResponse<RefreshTokenResponse>.Ok(
                    response,
                    "Token refreshed successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<RefreshTokenResponse>.Fail(
                    ex.Message);
            }
        }

    }
}
