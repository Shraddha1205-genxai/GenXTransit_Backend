using Dapper;
using GenXTransitAPI.DataAccess.Data;
using GenXTransitAPI.DataAccess.Interface.IRepositories;
using GenXTransitAPI.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenXTransitAPI.DataAccess.Repositories
{
    public class AuthorizationRepository : IAuthorizationRepository
    {
        private readonly DBHelper _db;

        public AuthorizationRepository(DBHelper db)
        {
            _db = db;
        }

        public async Task<IEnumerable<AuthorizationRowDto>>
        GetByRoleAsync(
            int roleId,
            string? searchText)
        {
            using var conn = _db.CreateConnection();

            return await conn.QueryAsync<AuthorizationRowDto>(
                "usp_Authorization_GetByRole",
                new
                {
                    RoleId = roleId,
                    SearchText = searchText
                },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<bool> SaveAllAsync(
       List<AuthorizationItem> items,
       int userId)
        {
            using var conn = _db.CreateConnection();

            foreach (var item in items)
            {
                await conn.ExecuteAsync(
                    "usp_Authorization_SaveNew",
                    new
                    {
                        item.AuthId,
                        item.RoleId,
                        item.SectionId,
                        item.MenuId,
                        item.TabId,

                        item.CanView,
                        item.CanAdd,
                        item.CanEdit,
                        item.CanDelete,

                        //item.IsDisableView,
                        //item.IsDisableEdit,
                        //item.IsDisableAdd,
                        //item.IsDisableDelete,

                        item.CanAction,
                        item.IsDefault,

                        UserId = userId
                    },
                    commandType: CommandType.StoredProcedure);
            }

            return true;
        }
    }

    //public async Task<bool> SaveAsync(
    //AuthorizationSaveDto request,
    //int userId)
    //{
    //    using var conn = _db.CreateConnection();

    //    var result = await conn.QueryFirstAsync<dynamic>(
    //        "usp_Authorization_Save",
    //        new
    //        {
    //            request.RoleId,
    //            request.SectionId,
    //            request.MenuId,
    //            request.TabId,

    //            request.CanView,
    //            request.CanAdd,
    //            request.CanEdit,
    //            request.CanDelete,

    //            //request.IsDisableView,
    //            //request.IsDisableEdit,
    //            //request.IsDisableAdd,
    //            //request.IsDisableDelete,

    //            request.CanAction,
    //            request.IsDisableAction,

    //            UserId = userId
    //        },
    //        commandType: CommandType.StoredProcedure);

    //    return result.Status == 1;
    //}


    //public async Task<bool> UpdateAsync(
    //    AuthorizationUpdateDto request,
    //    int userId)
    //{
    //    using var conn = _db.CreateConnection();

    //    var result = await conn.QueryFirstAsync<dynamic>(
    //        "usp_Authorization_Update",
    //        new
    //        {
    //            request.AuthId,

    //            request.RoleId,
    //            request.SectionId,
    //            request.MenuId,
    //            request.TabId,

    //            request.CanView,
    //            request.CanAdd,
    //            request.CanEdit,
    //            request.CanDelete,

    //            //request.IsDisableView,
    //            //request.IsDisableEdit,
    //            //request.IsDisableAdd,
    //            //request.IsDisableDelete,

    //            request.CanAction,
    //            request.IsDisableAction,

    //            UserId = userId
    //        },
    //        commandType: CommandType.StoredProcedure);

    //    return result.Status == 1;
    //}
}


