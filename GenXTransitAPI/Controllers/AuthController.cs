using GenXTransitAPI.DataAccess.Interface.IServices;
using GenXTransitAPI.Models;
using GenXTransitAPI.Models.DTO_s;
using GenXTransitAPI.Models.DTOs;
using GenXTransitAPI.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GenXTransitAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : BaseController
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }


        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(
            [FromBody] LoginRequest request)
        {
            var result =
                await _authService.LoginAsync(request);

            if (!result.Success)
            {
                return Unauthorized(result);
            }

            return Ok(result);
        }


         [Authorize]
        //[AllowAnonymous]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            var userIdClaim =
                User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim))
            {
                return Unauthorized(
                    ApiResponse<string>.Fail("Invalid token."));
            }

            if (!int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized(
                    ApiResponse<string>.Fail("Invalid user identity."));
            }

            var result = await _authService.ChangePasswordAsync(
                request,
                userId);

            return result.Success
                ? Ok(result)
                : BadRequest(result);
        }


        [HttpPost("forgot-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            var response =
                await _authService.ForgotPasswordAsync(request);

            return Ok(response);
        }

        [HttpPost("reset-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            var response =
                await _authService.ResetPasswordAsync(request);

            return Ok(response);
        }

       // [AllowAnonymous]
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            var response =
                await _authService.RefreshTokenAsync(request);

            if (!response.Success)
            {
                return Unauthorized(response);
            }

            return Ok(response);
        }
    }
}

