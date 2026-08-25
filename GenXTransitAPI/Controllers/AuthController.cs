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
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
       // [Authorize(Policy = "USER_CREATE")]
        public async Task<IActionResult> Register(
            [FromBody] RegisterUserRequest request)
        {
            var result =
                await _authService.RegisterAsync(request);

            return Ok(new ApiResponse<RegisterUserResponse>
            {
                Success = true,
                Message = "User registered successfully.",
                Data = result
            });
        }

       // [Authorize]
        [HttpPost("update-profile")]
        public async Task<IActionResult> UpdateProfile( [FromBody] UpdateUserRequest request)
        {
            var userIdClaim = User.FindFirst(
                ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
            {
                return Unauthorized(
                    ApiResponse<UpdateUserResponse>.Fail(
                        "User is not authenticated."));
            }

            if (!int.TryParse(
                userIdClaim.Value,
                out int userId))
            {
                return Unauthorized(
                    ApiResponse<UpdateUserResponse>.Fail(
                        "Invalid user identity."));
            }

            var result =
                await _authService.UpdateUserAsync(
                    request,
                    userId);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPost("login")]
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
    }
}

