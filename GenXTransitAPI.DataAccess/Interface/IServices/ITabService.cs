using GenXTransitAPI.Models;
using GenXTransitAPI.Models.DTOs;
using GenXTransitAPI.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenXTransitAPI.DataAccess.Interface.IServices
{
    public interface ITabService
    {
        Task<ApiResponse<List<Tab>>> GetAllTabsAsync(int? menuId,
    int? sectionId,
    bool? isActive,
    string? searchText,
    int pageNumber,
    int pageSize);

        Task<ApiResponse<Tab>> GetTabByIdAsync(int tabId);

        Task<ApiResponse<int>> CreateTabAsync(TabCreateDto request, int createdBy);

        Task<ApiResponse<int>> UpdateTabAsync(TabUpdateDto request, int modifiedBy);

        Task<ApiResponse<int>> DeleteTabAsync(int tabId, int modifiedBy);
    }
}
