using Dapper;
using GenXTransitAPI.DataAccess.Data;
using GenXTransitAPI.DataAccess.Interface.IRepositories;
using GenXTransitAPI.Models;
using GenXTransitAPI.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace GenXTransitAPI.DataAccess.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly DBHelper _db;
        public RoleRepository(DBHelper db) => _db = db;

        public async Task<ApiResponse<int>> CreateRoleAsync(
          CreateRoleRequest request,
          int? userId)
        {
            try
            {
                using var conn = _db.CreateConnection();
                var parameters = new DynamicParameters();

                parameters.Add("@RoleName",
                    request.RoleName?.Trim());

                parameters.Add("@Description",
                    request.Description?.Trim());

                //parameters.Add("@IsActive",
                //    request.IsActive);

                parameters.Add("@CreatedBy",
                    userId);

                
                var result = await conn.QueryFirstOrDefaultAsync<dynamic>(
                    "usp_Role_Save",
                    parameters,
                    commandType: CommandType.StoredProcedure);

                if (result == null)
                {
                    return ApiResponse<int>.Fail(
                        "Unable to create role.");
                }

                if (result.Status == 0)
                {
                    return ApiResponse<int>.Fail(
                        result.Message);
                }

                return ApiResponse<int>.Ok(
                    result.RoleId,
                    result.Message);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ApiResponse<string>> UpdateRoleAsync(
    int roleId,
    UpdateRoleRequest request,
    int? userId)
        {
            try
            {
                using var conn = _db.CreateConnection();
                var parameters = new DynamicParameters();

                parameters.Add("@RoleId", roleId);

                parameters.Add("@RoleName",
                    request.RoleName?.Trim());

                parameters.Add("@Description",
                    request.Description?.Trim());

                parameters.Add("@IsActive",
                    request.IsActive);

                parameters.Add("@ModifiedBy",
                    userId);
                
                var result = await conn.QueryFirstOrDefaultAsync<dynamic>(
                    "usp_Role_Update",
                    parameters,
                    commandType: CommandType.StoredProcedure);

                if (result == null)
                {
                    return ApiResponse<string>.Fail(
                        "Unable to update role.");
                }

                if (result.Status == 0)
                {
                    return ApiResponse<string>.Fail(
                        result.Message);
                }

                return ApiResponse<string>.Ok(
                    result.Message);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ApiResponse<List<RoleResponse>>> GetAllRolesAsync(string? searchText, string? isActive)
        {
            try
            {
                var parameters = new DynamicParameters();
                using var conn = _db.CreateConnection();
                parameters.Add("@SearchText", searchText);
                parameters.Add("@IsActive", isActive);
                var result = await conn.QueryAsync<RoleResponse>(
                    "usp_Role_GetAll",
                    parameters,
                    commandType: CommandType.StoredProcedure);

                return ApiResponse<List<RoleResponse>>.Ok(
                    result.ToList());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ApiResponse<RoleResponse>> GetRoleByIdAsync(
    int roleId)
        {
            try
            {
                using var conn = _db.CreateConnection();
                var parameters = new DynamicParameters();

                parameters.Add("@RoleId", roleId);

                var result = await conn.QueryFirstOrDefaultAsync<RoleResponse>(
                    "usp_Role_GetById",
                    parameters,
                     commandType: CommandType.StoredProcedure);

                if (result == null)
                {
                    return ApiResponse<RoleResponse>.Fail(
                        "Role not found.");
                }

                return ApiResponse<RoleResponse>.Ok(result);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ApiResponse<string>> DeleteRoleAsync(int roleId, int? userId)
        {
            try
            {
                var parameters = new DynamicParameters();
                using var conn = _db.CreateConnection();

                parameters.Add("@RoleId", roleId);
                parameters.Add("@ModifiedBy", userId);

                var result = await conn.QueryFirstOrDefaultAsync<dynamic>(
                    "usp_Role_Delete",
                    parameters,
                    commandType: CommandType.StoredProcedure);

                if (result == null)
                {
                    return ApiResponse<string>.Fail(
                        "Unable to delete role.");
                }

                if (result.Status == 0)
                {
                    return ApiResponse<string>.Fail(
                        result.Message);
                }

                return ApiResponse<string>.Ok(
                    result.Message);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}


   