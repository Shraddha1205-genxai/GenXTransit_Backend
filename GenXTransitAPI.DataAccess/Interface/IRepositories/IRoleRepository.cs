using GenXTransitAPI.Models;
using System;
using System.Collections.Generic;
using System.Text;
using GenXTransitAPI.Models.DTOs;
using GenXTransitAPI.Models.Entities;

namespace GenXTransitAPI.DataAccess.Interface.IRepositories
{
    public interface IRoleRepository
    {
        Task<ApiResponse<int>> CreateRoleAsync(CreateRoleRequest request,
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
