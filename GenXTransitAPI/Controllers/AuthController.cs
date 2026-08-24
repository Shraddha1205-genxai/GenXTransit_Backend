using GenXTransitAPI.DataAccess.Interface.IServices;
using GenXTransitAPI.Models;
using GenXTransitAPI.Models.DTO_s;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GenXTransitAPI.Controllers
{
    [Route("api/[controller]")]
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
    }
}

