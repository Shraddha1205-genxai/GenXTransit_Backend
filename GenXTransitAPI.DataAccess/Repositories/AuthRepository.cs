using GenXTransitAPI.DataAccess.Interface.IRepositories;
using GenXTransitAPI.Models.Entities;
using GenXTransitAPI.DataAccess.Data;
using System;
using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenXTransitAPI.Models.DTOs;

namespace GenXTransitAPI.DataAccess.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DBHelper _db;
        public AuthRepository(DBHelper db) => _db = db;


        //public async Task<bool> EmailExistsAsync(string email)
        //{
        //    using var conn = _db.CreateConnection();

        //    var result = await conn.ExecuteScalarAsync<int>(
        //        "usp_User_EmailExists",
        //        new
        //        {
        //            Email = email
        //        },
        //        commandType: CommandType.StoredProcedure);

        //    return result > 0;
        //}

        //public async Task<bool> UserNameExistsAsync(string userName)
        //{
        //    using var conn = _db.CreateConnection();

        //    var result = await conn.ExecuteScalarAsync<int>(
        //        "usp_User_UserNameExists",
        //        new
        //        {
        //            UserName = userName
        //        },
        //        commandType: CommandType.StoredProcedure);

        //    return result > 0;
        //}

        //public async Task<int> RegisterUserAsync(User user)
        //{
        //    using var conn = _db.CreateConnection();
        //    var p = new DynamicParameters();

        //    p.Add("@UserName", user.UserName);
        //    p.Add("@Email", user.Email);
        //    p.Add("@MobileNo", user.MobileNo);
        //    p.Add("@PasswordHash", user.PasswordHash);
        //    p.Add("@FirstName", user.FirstName);
        //    p.Add("@LastName", user.LastName);
        //   // p.Add("@RoleId", user.RoleId);
        //    p.Add("@IsActive", user.IsActive);
        //    p.Add("@IsEmailVerified", user.IsEmailVerified);
        //    p.Add("@IsMobileVerified", user.IsMobileVerified);
        //    p.Add("@IsFirstLogin", user.IsFirstLogin);
        //    p.Add("@PasswordChangedDate", user.PasswordChangedDate);
        //    p.Add("@CreatedBy", user.CreatedBy);

        //    return await conn.ExecuteScalarAsync<int>(
        // "User_Register",
        // p,
        // commandType: CommandType.StoredProcedure);
        //}

        //public async Task<User?> GetUserByIdAsync(int userId)
        //{
        //    using var conn = _db.CreateConnection();

        //    return await conn.QueryFirstOrDefaultAsync<User>(
        //        "usp_User_GetById",
        //        new
        //        {
        //            UserId = userId
        //        },
        //        commandType: CommandType.StoredProcedure);
        //}

        //public async Task<bool> UpdateUserAsync( int userId, UpdateUserRequest request)
        //{
        //    using var conn = _db.CreateConnection();

        //    var result = await conn.ExecuteScalarAsync<bool>(
        //        "usp_User_Update",
        //        new
        //        {
        //            UserId = userId,
        //            UserName = request.UserName,
        //            Email = request.Email,
        //            MobileNo = request.MobileNo,
        //            FirstName = request.FirstName,
        //            LastName = request.LastName
        //        },
        //        commandType: CommandType.StoredProcedure);

        //    return result;
        //}

        public async Task<User?> GetUserForLoginAsync(
            string loginId)
        {
            using var conn = _db.CreateConnection();

            return await conn.QueryFirstOrDefaultAsync<User>(
                "usp_User_Login",
                new
                {
                    LoginId = loginId
                },
                commandType: CommandType.StoredProcedure);
        }
        public async Task<List<LoginPermissionResponse>> GetUserPermissionsAsync(
    int userId)
        {
            using var conn = _db.CreateConnection();

            var result = await conn.QueryAsync<LoginPermissionResponse>(
                "usp_User_GetPermissions",
                new
                {
                    UserId = userId
                },
                commandType: CommandType.StoredProcedure);

            return result.ToList();
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            using var conn = _db.CreateConnection();

            return await conn.QueryFirstOrDefaultAsync<User>(
                "usp_User_GetByEmail",
                new
                {
                    Email = email
                },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<bool> ChangePasswordAsync( int userId, string newPassword)
        {
            using var conn = _db.CreateConnection();

            var parameters = new DynamicParameters();

            parameters.Add("@UserId", userId);
            parameters.Add("@NewPassword", newPassword);

            var result = await conn.QueryFirstOrDefaultAsync<int>(
                "usp_User_ChangePassword",
                parameters,
                commandType: CommandType.StoredProcedure);

            return result == 1;
        }


        public async Task<bool> UpdateUserPasswordAsync(
    int userId,
    string newPasswordHash,
    int modifiedBy)
        {
            using var conn = _db.CreateConnection();

            var result = await conn.QueryFirstOrDefaultAsync<PasswordUpdateResult>(
                "USP_UpdateUserPassword",
                new
                {
                    UserId = userId,
                    NewPasswordHash = newPasswordHash,
                    ModifiedBy = modifiedBy
                },
                commandType: CommandType.StoredProcedure);

            return result?.Status == 1;
        }

        public async Task<bool> RevokeAllUserRefreshTokensAsync( int userId,  int modifiedBy)
        {
            using var conn = _db.CreateConnection();

            var result = await conn.ExecuteAsync(
                "USP_RevokeAllUserRefreshTokens",
                new
                {
                    UserId = userId,
                    ModifiedBy = modifiedBy
                },
                commandType: CommandType.StoredProcedure);

            return result > 0;
        }
        public async Task<PasswordResetTokenResult> CreatePasswordResetTokenAsync(
    int userId,
    string tokenHash,
    DateTime tokenExpiry)
        {
            using var conn = _db.CreateConnection();

            var result =
                await conn.QueryFirstOrDefaultAsync<PasswordResetTokenResult>(
                    "USP_CreatePasswordResetToken",
                    new
                    {
                        UserId = userId,
                        TokenHash = tokenHash,
                        TokenExpiry = tokenExpiry
                    },
                    commandType: CommandType.StoredProcedure);

            return result ?? new PasswordResetTokenResult
            {
                RowsAffected = 0,
                Message = "Unable to create password reset token."
            };
        }

        public async Task<ResetPasswordResult> ResetUserPasswordAsync( string tokenHash, string newPasswordHash)
        {
            using var conn = _db.CreateConnection();

            var result =
                await conn.QueryFirstOrDefaultAsync<ResetPasswordResult>(
                    "USP_ResetUserPassword",
                    new
                    {
                        TokenHash = tokenHash,
                        NewPasswordHash = newPasswordHash
                      
                    },
                    commandType: CommandType.StoredProcedure);

            return result;
        }
    }
}

    