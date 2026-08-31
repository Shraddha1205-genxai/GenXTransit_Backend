using GenXTransitAPI.DataAccess.Interface.IServices;
using GenXTransitAPI.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GenXTransitAPI.Controllers
{
    [Route("api/role")]
    [ApiController]
    public class RoleController : BaseController
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        // POST: api/roles
        [AllowAnonymous]
        [HttpPost("create")]
        public async Task<IActionResult> CreateRole(
            [FromBody] CreateRoleRequest request)
        {
            //var userId = GetUserId();

            var response = await _roleService.CreateRoleAsync(
                request,
                CurrentUserId);

            return Ok(response);
        }

        // PUT: api/roles/1
        [HttpPost("update")]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateRole([FromBody] UpdateRoleRequest request)
        {
            //var userId = GetUserId();

            var response = await _roleService.UpdateRoleAsync(
                request.RoleId,
                request,
                CurrentUserId);

            return Ok(response);
        }

        // GET: api/roles
        [AllowAnonymous]
        [HttpGet("getall")]
        public async Task<IActionResult> GetAllRoles()
        {
            var response =
                await _roleService.GetAllRolesAsync();

            return Ok(response);
        }

        // GET: api/roles/1
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoleById(
            int id)
        {
            var response =
                await _roleService.GetRoleByIdAsync(id);

            return Ok(response);
        }

        // DELETE: api/roles/1
        [AllowAnonymous]
        [HttpPost("delete")]
        public async Task<IActionResult> DeleteRole([FromBody] DeleteRoleRequest request)
        {
            //var userId = GetUserId();

            var response =
                await _roleService.DeleteRoleAsync(
                    request.RoleId,
                    CurrentUserId);

            return Ok(response);
        }
    }
}
    