using GenXTransitAPI.DataAccess.Interface.IRepositories;
using GenXTransitAPI.DataAccess.Interface.IServices;
using GenXTransitAPI.DataAccess.Repositories;
using GenXTransitAPI.Models;
using GenXTransitAPI.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenXTransitAPI.DataAccess.Services
{
    public class MenuService : IMenuService
    {
        private readonly IMenuRepository _menuRepo;

        public MenuService(IMenuRepository menuRepository)
        {
            _menuRepo = menuRepository;
        }

        public async Task<ApiResponse<int>> InsertMenuAsync(
      MenuInsertDto request, int createdBy)
        {
            return await _menuRepo.InsertMenuAsync(request,createdBy);
        }

        public async Task<ApiResponse<int>> UpdateMenuAsync(MenuUpdateDto request, int modifiedBy)
        {
            return await _menuRepo.UpdateMenuAsync(request,modifiedBy);
        }
        public async Task<ApiResponse<List<MenuResponseDto>>> GetAllMenusAsync(
           string? searchText,
           int? sectionId,
           bool? isActive)
        {
            return await _menuRepo.GetAllMenusAsync(
                searchText,
                sectionId,
                isActive);
        }


        public async Task<ApiResponse<MenuResponseDto>> GetMenuByIdAsync(
            int menuId)
        {
            return await _menuRepo.GetMenuByIdAsync(menuId);
        }


        public async Task<ApiResponse<bool>> DeleteMenuAsync(
            int id, int modifiedBy)

        {
            return await _menuRepo.DeleteMenuAsync(
                id,
                modifiedBy);
        }
    }
}
