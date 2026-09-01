using GenXTransitAPI.DataAccess.Interface.IRepositories;
using GenXTransitAPI.DataAccess.Interface.IServices;
using GenXTransitAPI.Models;
using GenXTransitAPI.Models.DTOs;
using GenXTransitAPI.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenXTransitAPI.DataAccess.Services
{
    public class TabService : ITabService
    {
        private readonly ITabRepository _tabRepository;

        public TabService(ITabRepository tabRepository)
        {
            _tabRepository = tabRepository;
        }

        public async Task<ApiResponse<List<Tab>>> GetAllTabsAsync()
        {
            return await _tabRepository.GetAllTabsAsync();
        }

        public async Task<ApiResponse<Tab>> GetTabByIdAsync(int tabId)
        {
            return await _tabRepository.GetTabByIdAsync(tabId);
        }

        public async Task<ApiResponse<int>> CreateTabAsync(
            TabCreateDto request, int createdBy)
        {
            return await _tabRepository.CreateTabAsync(request,createdBy);
        }

        public async Task<ApiResponse<int>> UpdateTabAsync(
            TabUpdateDto request, int modifiedBy)
        {
            return await _tabRepository.UpdateTabAsync(request,modifiedBy);
        }

        public async Task<ApiResponse<int>> DeleteTabAsync(
            int tabId,
            int modifiedBy)
        {
            return await _tabRepository.DeleteTabAsync(
                tabId,
                modifiedBy);
        }
    }
}
