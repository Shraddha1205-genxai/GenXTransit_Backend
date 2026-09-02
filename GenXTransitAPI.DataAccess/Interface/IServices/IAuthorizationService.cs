using GenXTransitAPI.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenXTransitAPI.DataAccess.Interface.IServices
{
    public interface IAuthorizationService
    {
        Task<IEnumerable<AuthorizationRowDto>> GetByRoleAsync(
       int roleId,
       string? searchText);

        Task<bool> SaveAllAsync(
            List<AuthorizationItem> items,
            int userId);

        //Task<bool> SaveAsync(
        //AuthorizationSaveDto request,
        //int userId);

        //Task<bool> UpdateAsync(
        //    AuthorizationUpdateDto request,
        //    int userId);
    }
}
