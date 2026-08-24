using GenXTransitAPI.DataAccess.Interface.IRepositories;
using GenXTransitAPI.Models.Entities;
using GenXTransittAPI.DataAccess.Data;
using System;
using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenXTransitAPI.DataAccess.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DBHelper _db;
        public AuthRepository(DBHelper db) => _db = db;


        public async Task<bool> EmailExistsAsync(string email)
        {
            using var conn = _db.CreateConnection();

            var result = await conn.ExecuteScalarAsync<int>(
                "usp_User_EmailExists",
                new
                {
                    Email = email
                },
                commandType: CommandType.StoredProcedure);

            return result > 0;
        }

        public async Task<bool> UserNameExistsAsync(string userName)
        {
            using var conn = _db.CreateConnection();

            var result = await conn.ExecuteScalarAsync<int>(
                "usp_User_UserNameExists",
                new
                {
                    UserName = userName
                },
                commandType: CommandType.StoredProcedure);

            return result > 0;
        }

        public async Task<int> RegisterUserAsync(User user)
        {
            using var conn = _db.CreateConnection();
            var p = new DynamicParameters();

            p.Add("@UserName", user.UserName);
            p.Add("@Email", user.Email);
            p.Add("@MobileNo", user.MobileNo);
            p.Add("@PasswordHash", user.PasswordHash);
            p.Add("@FirstName", user.FirstName);
            p.Add("@LastName", user.LastName);
            p.Add("@RoleId", user.RoleId);
            p.Add("@IsActive", user.IsActive);
            p.Add("@IsEmailVerified", user.IsEmailVerified);
            p.Add("@IsMobileVerified", user.IsMobileVerified);
            p.Add("@IsFirstLogin", user.IsFirstLogin);
            p.Add("@PasswordChangedDate", user.PasswordChangedDate);
            p.Add("@CreatedBy", user.CreatedBy);

            return await conn.ExecuteScalarAsync<int>(
         "User_Register",
         p,
         commandType: CommandType.StoredProcedure);
        }
    }
}
    