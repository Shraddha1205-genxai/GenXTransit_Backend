using GenXTransitAPI.DataAccess.Interface.IServices;
using GenXTransitAPI.Models;
using GenXTransitAPI.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GenXTransitAPI.Controllers
{
    [Route("api/authorization")]
    [ApiController]
    [AllowAnonymous]
    public class AuthorizationController : BaseController
    {
        private readonly DataAccess.Interface.IServices.IAuthorizationService _svc;

        public AuthorizationController(
            DataAccess.Interface.IServices.IAuthorizationService svc)
        {
            _svc = svc;
        }
        [HttpGet("{roleId}")]
        public async Task<IActionResult> GetByRole(
           int roleId,
           string? searchText)
        {
            var result = await _svc.GetByRoleAsync(
                roleId,
                searchText);

            return Ok(
                ApiResponse<IEnumerable<AuthorizationRowDto>>
                    .Ok(result));
        }


        // SAVE
        [HttpPost("save")]
        public async Task<IActionResult> Save(
            [FromBody] AuthorizationSaveDto request)
        {
            if (request == null)
            {
                return BadRequest(
                    ApiResponse<bool>.Fail(
                        "Authorization details are required."));
            }

            var result = await _svc.SaveAsync(
                request,
                CurrentUserId);

            if (!result)
            {
                return BadRequest(
                    ApiResponse<bool>.Fail(
                        "Authorization already exists."));
            }

            return Ok(
                ApiResponse<bool>.Ok(
                    true,
                    "Authorization saved successfully."));
        }


        // UPDATE
        [HttpPost("update")]
        public async Task<IActionResult> Update(
            [FromBody] AuthorizationUpdateDto request)
        {
            if (request == null || request.AuthId <= 0)
            {
                return BadRequest(
                    ApiResponse<bool>.Fail(
                        "Valid AuthId is required."));
            }

            var result = await _svc.UpdateAsync(
                request,
                CurrentUserId);

            if (!result)
            {
                return BadRequest(
                    ApiResponse<bool>.Fail(
                        "Authorization not found or already exists."));
            }

            return Ok(
                ApiResponse<bool>.Ok(
                    true,
                    "Authorization updated successfully."));
        }
    }
}
   