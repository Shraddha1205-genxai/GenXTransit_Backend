using GenXTransitAPI.DataAccess.Interface.IRepositories;
using GenXTransitAPI.DataAccess.Interface.IServices;
using GenXTransitAPI.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenXTransitAPI.DataAccess.Services
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly IAuthorizationRepository _repo;

        public AuthorizationService(
            IAuthorizationRepository repo)
        {
            _repo = repo;
        }
        public async Task<IEnumerable<AuthorizationRowDto>>
        GetByRoleAsync(
            int roleId,
            string? searchText)
        {
            return await _repo.GetByRoleAsync(
                roleId,
                searchText);
        }

        public async Task<bool> SaveAllAsync(
        List<AuthorizationItem> items,
        int userId)
        {
            return await _repo.SaveAllAsync(
                items,
                userId);
        }


        //public async Task<bool> SaveAsync(
        //AuthorizationSaveDto request,
        //int userId)
        //{
        //    return await _repo.SaveAsync(
        //        request,
        //        userId);
        //}


        //public async Task<bool> UpdateAsync(
        //    AuthorizationUpdateDto request,
        //    int userId)
        //{
        //    return await _repo.UpdateAsync(
        //        request,
        //        userId);
        //}
    }
}
