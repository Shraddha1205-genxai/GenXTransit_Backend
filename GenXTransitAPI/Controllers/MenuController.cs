using GenXTransitAPI.DataAccess.Interface.IServices;
using GenXTransitAPI.Models.DTOs;
using GenXTransitAPI.Models.Entities;
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
            [FromQuery] int? sectionId = null,
            [FromQuery] bool? isActive = null)
        {
            var response = await _menuService.GetAllMenusAsync(
                searchText,
                sectionId,
                isActive);

            return Ok(response);
        }


        // GET BY ID
        [HttpGet("{menuId}")]
        public async Task<IActionResult> GetMenuById(
            int menuId)
        {
            var response = await _menuService.GetMenuByIdAsync(menuId);

            return Ok(response);
        }


        // DELETE
        [HttpPost("delete")]
       
         public async Task<IActionResult> DeleteMenu(DeleteMenuRequest request)
        {
            var response = await _menuService.DeleteMenuAsync(request.MenuId,CurrentUserId);

            return Ok(response);
        }


    }
}

