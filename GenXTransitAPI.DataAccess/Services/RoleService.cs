using GenXTransitAPI.DataAccess.Interface.IRepositories;
using GenXTransitAPI.DataAccess.Interface.IServices;
using GenXTransitAPI.Models;
using GenXTransitAPI.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace GenXTransitAPI.DataAccess.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;

        public RoleService(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<ApiResponse<int>> CreateRoleAsync(
            CreateRoleRequest request,
            int? userId)
        {
            if (request == null)
            {
                return ApiResponse<int>.Fail(
                    "Invalid request.");
            }

            if (string.IsNullOrWhiteSpace(request.RoleName))
            {
                return ApiResponse<int>.Fail(
                    "Role name is required.");
            }

            request.RoleName = request.RoleName.Trim();

            if (!string.IsNullOrWhiteSpace(request.Description))
            {
                request.Description =
                    request.Description.Trim();
            }

            return await _roleRepository.CreateRoleAsync(
                request,
                userId);
        }

        public async Task<ApiResponse<string>> UpdateRoleAsync(
            int roleId,
            UpdateRoleRequest request,
            int? userId)
        {
            if (roleId <= 0)
            {
                return ApiResponse<string>.Fail(
                    "Invalid role id.");
            }

            if (request == null)
            {
                return ApiResponse<string>.Fail(
                    "Invalid request.");
            }

            if (string.IsNullOrWhiteSpace(request.RoleName))
            {
                return ApiResponse<string>.Fail(
                    "Role name is required.");
            }

            request.RoleName = request.RoleName.Trim();

            if (!string.IsNullOrWhiteSpace(request.Description))
            {
                request.Description =
                    request.Description.Trim();
            }

            return await _roleRepository.UpdateRoleAsync(
                roleId,
                request,
                userId);
        }

        public async Task<ApiResponse<List<RoleResponse>>> GetAllRolesAsync()
        {
            return await _roleRepository.GetAllRolesAsync();
        }

        public async Task<ApiResponse<RoleResponse>> GetRoleByIdAsync(
            int roleId)
        {
            if (roleId <= 0)
            {
                return ApiResponse<RoleResponse>.Fail(
                    "Invalid role id.");
            }

            return await _roleRepository.GetRoleByIdAsync(
                roleId);
        }

        public async Task<ApiResponse<string>> DeleteRoleAsync(
            int roleId,
            int? userId)
        {
            if (roleId <= 0)
            {
                return ApiResponse<string>.Fail(
                    "Invalid role id.");
            }

            return await _roleRepository.DeleteRoleAsync(
                roleId,
                userId);
        }
    }
}
   