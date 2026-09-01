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
       bool? isActive);

        Task<ApiResponse<MenuResponseDto>> GetMenuByIdAsync(
            int id);

        Task<ApiResponse<bool>> DeleteMenuAsync(
           int id, int modifiedBy);
    }
}
