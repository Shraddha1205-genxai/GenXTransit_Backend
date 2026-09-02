using GenXTransitAPI.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenXTransitAPI.DataAccess.Interface.IRepositories
{
    public interface IAuthorizationRepository
    {
        Task<IEnumerable<AuthorizationRowDto>> GetByRoleAsync(
      int roleId,
      string? searchText);

        Task<bool> SaveAsync(
         AuthorizationSaveDto request,
         int userId);

        Task<bool> UpdateAsync(
            AuthorizationUpdateDto request,
            int userId);
    }
}
