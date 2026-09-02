using Dapper;
using GenXTransitAPI.DataAccess.Data;
using GenXTransitAPI.DataAccess.Interface.IRepositories;
using GenXTransitAPI.Models;
using GenXTransitAPI.Models.DTOs;
using GenXTransitAPI.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenXTransitAPI.DataAccess.Repositories
{
    public class MenuRepository : IMenuRepository
    {
        private readonly DBHelper _db;

        public MenuRepository(DBHelper db)
        {
            _db = db;
        }

        public async Task<ApiResponse<int>> InsertMenuAsync(
    MenuInsertDto request, int createdBy)
        {
            try
            {
                using var conn = _db.CreateConnection();

                var parameters = new DynamicParameters();

                parameters.Add("@IconName", request.IconName);
                parameters.Add("@SectionId", request.SectionId);
                parameters.Add("@SortOrder", request.SortOrder);
                parameters.Add("@MenuName", request.MenuName);
                parameters.Add("@IsActive", request.IsActive);
                parameters.Add("@CreatedBy", createdBy);

                var result = await conn.QueryFirstOrDefaultAsync<dynamic>(
                    "usp_Menu_Save",
                    parameters,
                    commandType: CommandType.StoredProcedure);

                return new ApiResponse<int>
                {
                    Success = result?.Status == 1,
                    Message = result?.Message,
                    Data = result?.Id ?? 0
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

        public async Task<ApiResponse<int>> UpdateMenuAsync(MenuUpdateDto request, int modifiedBy)
        {
            try
            {
                using var conn = _db.CreateConnection();

                var parameters = new DynamicParameters();

                parameters.Add("@Id", request.MenuId);
                parameters.Add("@IconName", request.IconName);
                parameters.Add("@SectionId", request.SectionId);
                parameters.Add("@SortOrder", request.SortOrder);
                parameters.Add("@MenuName", request.MenuName);
                parameters.Add("@IsActive", request.IsActive);
                parameters.Add("@ModifiedBy", modifiedBy);

                var result = await conn.QueryFirstOrDefaultAsync<dynamic>(
                    "usp_Menu_Update",
                    parameters,
                    commandType: CommandType.StoredProcedure);

                return new ApiResponse<int>
                {
                    Success = result?.Status == 1,
                    Message = result?.Message,
                    Data = result?.Id ?? request.MenuId
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<int>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = request.MenuId
                };
            }
        }
        // GET ALL
        public async Task<ApiResponse<List<MenuResponseDto>>> GetAllMenusAsync(
            string? searchText,
            int? sectionId,
            bool? isActive)
        {
            try
            {
                using var conn = _db.CreateConnection();

                var parameters = new DynamicParameters();

                parameters.Add("@SearchText", searchText);
                parameters.Add("@SectionId", sectionId);
                parameters.Add("@IsActive", isActive);

                var result = await conn.QueryAsync<MenuResponseDto>(
                    "usp_Menu_GetAll",
                    parameters,
                    commandType: CommandType.StoredProcedure);

                return new ApiResponse<List<MenuResponseDto>>
                {
                    Success = true,
                    Message = "Menus retrieved successfully.",
                    Data = result.ToList()
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<MenuResponseDto>>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }


        // GET BY ID
        public async Task<ApiResponse<MenuResponseDto>> GetMenuByIdAsync(
            int menuId)
        {
            try
            {
                using var conn = _db.CreateConnection();

                var parameters = new DynamicParameters();

                parameters.Add("@MenuId", menuId);

                var result = await conn.QueryFirstOrDefaultAsync<MenuResponseDto>(
                    "usp_Menu_GetById",
                    parameters,
                    commandType: CommandType.StoredProcedure);

                if (result == null)
                {
                    return new ApiResponse<MenuResponseDto>
                    {
                        Success = false,
                        Message = "Menu not found."
                    };
                }

                return new ApiResponse<MenuResponseDto>
                {
                    Success = true,
                    Message = "Menu retrieved successfully.",
                    Data = result
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<MenuResponseDto>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }


        // DELETE

                public async Task<ApiResponse<bool>> DeleteMenuAsync(
            int menuId, int modifiedBy)
        {
            try
            {
                using var conn = _db.CreateConnection();
                var parameters = new DynamicParameters();

                parameters.Add(
                    "@MenuId",
                    menuId,
                    DbType.Int32);
                parameters.Add(
                   "@ModifiedBy",
                   modifiedBy,
                   DbType.Int32);

                var result = await conn.QueryFirstOrDefaultAsync<dynamic>(
                    "usp_Menu_Delete",
                    parameters,
                    commandType: CommandType.StoredProcedure);

                if (result == null)
                {
                    return new ApiResponse<bool>
                    {
                        Success = false,
                        Message = "Unable to delete section.",
                        Data = false
                    };
                }

                return new ApiResponse<bool>
                {
                    Success = true,
                    Message = result.Message,
                    Data = Convert.ToInt32(result.Status) == 1
                };
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
