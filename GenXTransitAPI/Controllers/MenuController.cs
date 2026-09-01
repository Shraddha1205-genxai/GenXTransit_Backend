using GenXTransitAPI.DataAccess.Interface.IServices;
using GenXTransitAPI.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GenXTransitAPI.Controllers
{
    [Route("api/menu")]
    [ApiController]
    [AllowAnonymous]
    public class MenuController : BaseController
    {
        private readonly IMenuService _menuService;

        public MenuController(IMenuService menuService)
        {
            _menuService = menuService;
        }

        [HttpPost("insert")]
        public async Task<IActionResult> InsertMenu(
        [FromBody] MenuInsertDto request)
        {
            var response = await _menuService.InsertMenuAsync(request,CurrentUserId);

            return Ok(response);
        }

        [HttpPost("update")]
        public async Task<IActionResult> UpdateMenu( [FromBody] MenuUpdateDto request)
        {
            var response = await _menuService.UpdateMenuAsync(request,CurrentUserId);

            return Ok(response);
        }

        // GET ALL
        [HttpGet]
        public async Task<IActionResult> GetAllMenus(
            [FromQuery] string? searchText = null,
            [FromQuery] bool? isActive = null)
        {
            var response = await _menuService.GetAllMenusAsync(
                searchText,
                isActive);

            return Ok(response);
        }


        // GET BY ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMenuById(
            int id)
        {
            var response = await _menuService.GetMenuByIdAsync(id);

            return Ok(response);
        }


        // DELETE
        [HttpPost("delete")]
       
         public async Task<IActionResult> DeleteMenu(DeleteMenuRequest request)
        {
            var response = await _menuService.DeleteMenuAsync(request.Id,CurrentUserId);

            return Ok(response);
        }


    }
}

