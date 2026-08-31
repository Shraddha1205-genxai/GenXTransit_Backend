using GenXTransitAPI.DataAccess.Interface.IServices;
using GenXTransitAPI.Models;
using GenXTransitAPI.Models.DTO_s;
using GenXTransitAPI.Models.DTOs;
using GenXTransitAPI.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GenXTransitAPI.Controllers
{
    [Route("api/usermaster")]
    [ApiController]
   // [Authorize]
    public class UserMasterController : BaseController
    {
        private readonly IUserService _userService;

        public UserMasterController(IUserService userService)
        {
            _userService = userService;
        }
        [AllowAnonymous]
        [HttpPost("AddUser")]
        
        public async Task<IActionResult> AddUser(
           [FromBody] AddUserRequest request)
        {
            var result =
                await _userService.AddUserAsync(request,CurrentUserId);

            return Ok(new ApiResponse<AddUserResponse>
            {
                Success = true,
                Message = "User Added successfully.",
                Data = result
            });
        }

        // [Authorize]
        [AllowAnonymous]
        [HttpPost("UpdateUser")]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserRequest request)
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
                await _userService.UpdateUserAsync(
                    request,
                    userId);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers([FromQuery] string? searchText,
    [FromQuery] bool? isActive,
    [FromQuery] int pageNumber = 1,
    [FromQuery] int pageSize = 10)
        {
            try
            {
                var result = await _userService.GetAllUsersAsync(searchText,
            isActive,
            CurrentUserId,
            pageNumber,
            pageSize);

                if (!result.Success)
                {
                    return BadRequest(result);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    ApiResponse<string>.Fail(
                        $"An unexpected error occurred: {ex.Message}"));
            }
        }

        [HttpGet("GetUserById/{userId}")]
        public async Task<IActionResult> GetUserById(int userId)
        {
            try
            {
                var result = await _userService.GetUserByIdAsync(userId);

                if (!result.Success)
                {
                    return NotFound(result);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    ApiResponse<string>.Fail(
                        $"An unexpected error occurred: {ex.Message}"));
            }
        }

        [HttpPost("delete")]
        public async Task<IActionResult> DeleteUser(DeleteUserRequest request )
        {
            try
            {
                var result = await _userService.DeleteUserAsync(request.UserId);

                if (!result.Success)
                {
                    return BadRequest(result);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    ApiResponse<string>.Fail(
                        $"An unexpected error occurred: {ex.Message}"));
            }
        }
    }
}
   