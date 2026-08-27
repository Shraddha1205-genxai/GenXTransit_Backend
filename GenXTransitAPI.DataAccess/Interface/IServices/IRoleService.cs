using GenXTransitAPI.Models;
using GenXTransitAPI.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace GenXTransitAPI.DataAccess.Interface.IServices
{
    public interface IRoleService
    {
        Task<ApiResponse<int>> CreateRoleAsync(
          CreateRoleRequest request,
          int? userId);

        Task<ApiResponse<string>> UpdateRoleAsync(
            int roleId,
            UpdateRoleRequest request,
            int? userId);

        Task<ApiResponse<List<RoleResponse>>> GetAllRolesAsync();

        Task<ApiResponse<RoleResponse>> GetRoleByIdAsync(
            int roleId);

        Task<ApiResponse<string>> DeleteRoleAsync(
            int roleId,
            int? userId);
    }
}
