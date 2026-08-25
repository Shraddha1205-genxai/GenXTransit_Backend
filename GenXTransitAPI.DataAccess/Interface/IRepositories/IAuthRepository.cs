using GenXTransitAPI.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenXTransitAPI.DataAccess.Interface.IRepositories
{
    public interface IAuthRepository
    {
        Task<bool> EmailExistsAsync(string email);

        Task<bool> UserNameExistsAsync(string userName);

        Task<int> RegisterUserAsync(User user);
        Task<User?> GetUserByIdAsync(int userId);

        Task<bool> UpdateUserAsync( int userId,UpdateUserRequest request);
        Task<User?> GetUserForLoginAsync(string loginId);

        Task<bool> ChangePasswordAsync( int userId, string newPassword);

        Task<bool> UpdateUserPasswordAsync( int userId,string newPasswordHash,  int modifiedBy);

        Task<bool> RevokeAllUserRefreshTokensAsync(int userId, int modifiedBy);

    }
}
