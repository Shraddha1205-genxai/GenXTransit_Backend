using GenXTransitAPI.Models;
using GenXTransitAPI.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenXTransitAPI.DataAccess.Interface.IServices
{
    public interface IMenuService
    {
        Task<ApiResponse<int>> InsertMenuAsync(MenuInsertDto request, int createdBy);

        Task<ApiResponse<int>> UpdateMenuAsync(MenuUpdateDto request, int modifiedBy);
        Task<ApiResponse<List<MenuResponseDto>>> GetAllMenusAsync(
       string? searchText,
       int? sectionId,
       bool? isActive);

        Task<ApiResponse<MenuResponseDto>> GetMenuByIdAsync(
            int menuId);

        Task<ApiResponse<bool>> DeleteMenuAsync(
           int menuId, int modifiedBy);
    }
}
