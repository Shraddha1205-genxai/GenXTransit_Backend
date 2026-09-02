using Dapper;
using GenXTransitAPI.DataAccess.Data;
using GenXTransitAPI.DataAccess.Interface.IRepositories;
using GenXTransitAPI.Models;
using GenXTransitAPI.Models.DTOs;
using GenXTransitAPI.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenXTransitAPI.DataAccess.Repositories
{
    public class TabRepository : ITabRepository
    {
        private readonly DBHelper _db;

        public TabRepository(DBHelper db)
        {
            _db = db;
        }

        public async Task<ApiResponse<List<Tab>>> GetAllTabsAsync(
    int? menuId,
    int? sectionId,
    bool? isActive,
    string? searchText,
    int pageNumber,
    int pageSize)
        {
            try
            {
                using var conn = _db.CreateConnection();

                var parameters = new DynamicParameters();

                parameters.Add("@MenuId", menuId);
                parameters.Add("@SectionId", sectionId);
                parameters.Add("@IsActive", isActive);
                parameters.Add("@SearchText", searchText);
                parameters.Add("@PageNumber", pageNumber);
                parameters.Add("@PageSize", pageSize);

                using var multi = await conn.QueryMultipleAsync(
                    "usp_Tab_GetAll",
                    parameters,
                    commandType: System.Data.CommandType.StoredProcedure);

                // First result set - Total Records
                await multi.ReadFirstAsync<int>();

                // Second result set - Tab data
                var result = (await multi.ReadAsync<Tab>()).ToList();

                return new ApiResponse<List<Tab>>
                {
                    Success = true,
                    Message = "Tabs retrieved successfully.",
                    Data = result
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<Tab>>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<ApiResponse<Tab>> GetTabByIdAsync(int tabId)
        {
            try
            {
                using var conn = _db.CreateConnection();

                var result = await conn.QueryFirstOrDefaultAsync<Tab>(
                    "usp_Tab_GetById",
                    new
                    {
                        TabId = tabId
                    },
                    commandType: System.Data.CommandType.StoredProcedure);

                if (result == null)
                {
                    return new ApiResponse<Tab>
                    {
                        Success = false,
                        Message = "Tab not found.",
                        Data = null
                    };
                }

                return new ApiResponse<Tab>
                {
                    Success = true,
                    Message = "Tab retrieved successfully.",
                    Data = result
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<Tab>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<ApiResponse<int>> CreateTabAsync(
            TabCreateDto request, int createdBy)
        {
            try
            {
                using var conn = _db.CreateConnection();

                var result = await conn.QueryFirstOrDefaultAsync<dynamic>(
                    "usp_Tab_Insert",
                    new
                    {
                        request.SectionId,
                        request.MenuId,
                        request.TabName,
                        request.SortOrder,
                        request.URL,
                        request.IsActive,
                        createdBy
                    },
                    commandType: System.Data.CommandType.StoredProcedure);

                return new ApiResponse<int>
                {
                    Success = true,
                    Message = result.Message,
                    Data = result.TabId ?? 0
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<int>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = 0
                };
            }
        }

        public async Task<ApiResponse<int>> UpdateTabAsync(
            TabUpdateDto request, int modifiedBy)
        {
            try
            {
                using var conn = _db.CreateConnection();

                var result = await conn.QueryFirstOrDefaultAsync<dynamic>(
                    "usp_Tab_Update",
                    new
                    {
                        request.TabId,
                        request.SectionId,
                        request.MenuId,
                        request.TabName,
                        request.SortOrder,
                        request.URL,
                        request.IsActive,
                        modifiedBy
                    },
                    commandType: System.Data.CommandType.StoredProcedure);

                return new ApiResponse<int>
                {
                    Success = true,
                    Message = result.Message,
                    Data = result.TabId ?? 0
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<int>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = 0
                };
            }
        }

        public async Task<ApiResponse<int>> DeleteTabAsync(
            int tabId,
            int modifiedBy)
        {
            try
            {
                using var conn = _db.CreateConnection();

                var result = await conn.QueryFirstOrDefaultAsync<dynamic>(
                    "usp_Tab_Delete",
                    new
                    {
                        TabId = tabId,
                        ModifiedBy = modifiedBy
                    },
                    commandType: System.Data.CommandType.StoredProcedure);

                return new ApiResponse<int>
                {
                    Success = true,
                    Message = result.Message,
                    Data = result.TabId ?? 0
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<int>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = 0
                };
            }
        }
    }
}
