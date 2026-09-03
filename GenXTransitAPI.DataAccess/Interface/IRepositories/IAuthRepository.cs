using GenXTransitAPI.Models.DTOs;
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
      
        Task<User?> GetUserForLoginAsync(string loginId);
        Task<List<LoginPermissionResponse>> GetUserPermissionsAsync(int userId);
        Task<User?> GetUserByEmailAsync(string email);
        Task<bool> ChangePasswordAsync( int userId, string newPassword);

        Task<bool> UpdateUserPasswordAsync( int userId,string newPasswordHash,  int modifiedBy);

        Task<bool> RevokeAllUserRefreshTokensAsync(int userId, int modifiedBy);
        Task<PasswordResetTokenResult> CreatePasswordResetTokenAsync(int userId, string tokenHash, DateTime tokenExpiry);

        Task<ResetPasswordResult> ResetUserPasswordAsync( string tokenHash,  string newPasswordHash);

    }
}
