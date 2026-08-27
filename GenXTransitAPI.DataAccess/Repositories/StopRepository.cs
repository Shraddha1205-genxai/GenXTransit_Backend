using Dapper;
using GenXTransitAPI.DataAccess.Data;
using GenXTransitAPI.DataAccess.Interface.IRepositories;
using GenXTransitAPI.Models.DTO_s;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace GenXTransitAPI.DataAccess.Repositories
{
    public class StopRepository : IStopRepository
    {
        private readonly DBHelper _db;

        public StopRepository(DBHelper db)
        {
            _db = db;
        }

        public async Task<IEnumerable<StopDTO>> GetAllAsync(
            string? searchText,
            int? routeId,
            bool? isActive,
            int? scopeToUser,
            int pageNumber = 1,
            int pageSize = 10)
        {
            using var conn = _db.CreateConnection();

            var dbResult = await conn.QueryAsync<StopDbDTO>(
                "usp_Stop_GetAll",
                new
                {
                    SearchText = searchText,
                    RouteId = routeId,
                    IsActive = isActive,
                    ScopeToUser = scopeToUser,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                },
                commandType: CommandType.StoredProcedure);

            return dbResult.Select(x => new StopDTO
            {
                stopId = x.Stop_Id?.ToString(),
                stopCode = x.Stop_Code,
                stopName = x.Stop_Name,
                stopOrder = x.Sequence,
                isActive = x.IsActive,
                createdBy = x.Created_By,
                createdDate = x.Created_Date,
                modifiedBy = x.Modified_By,
                modifiedDate = x.Modified_Date,
                // Route details
                routeId = x.Route_Id?.ToString(),
                routeCode = x.Route_Code,
                routeName = x.Route_Name,
                totalCount = x.TotalCount
            });
        }

        public async Task<StopDTO> GetByIdAsync(int stopId)
        {
            using var conn = _db.CreateConnection();

            var dbResult = await conn.QueryFirstOrDefaultAsync<StopDbDTO>(
                "usp_Stop_GetById",
                new { Stop_Id = stopId },
                commandType: CommandType.StoredProcedure);

            if (dbResult == null)
                return null;

            return new StopDTO
            {
                stopId = dbResult.Stop_Id?.ToString(),
                stopCode = dbResult.Stop_Code,
                stopName = dbResult.Stop_Name,
                stopOrder = dbResult.Sequence,
                isActive = dbResult.IsActive,
                createdBy = dbResult.Created_By,
                createdDate = dbResult.Created_Date,
                modifiedBy = dbResult.Modified_By,
                modifiedDate = dbResult.Modified_Date,
                // Route details
                routeId = dbResult.Route_Id?.ToString(),
                routeCode = dbResult.Route_Code,
                routeName = dbResult.Route_Name,
                totalCount = 0
            };
        }

        public async Task<string> GetNextCodeAsync()
        {
            using var conn = _db.CreateConnection();

            var p = new DynamicParameters();
            p.Add("@NextCode", dbType: DbType.String, direction: ParameterDirection.Output, size: 50);

            await conn.ExecuteAsync(
                "usp_Stop_GetNextCode",
                p,
                commandType: CommandType.StoredProcedure);

            return p.Get<string>("@NextCode");
        }

        public async Task<int> InsertAsync(StopDTO entity, int userId)
        {
            try
            {
                using var conn = _db.CreateConnection();

                var p = new DynamicParameters();

                p.Add("@Stop_Name", entity.stopName);
                p.Add("@Route_Id", Convert.ToInt32(entity.routeId));
                p.Add("@Sequence", entity.stopOrder);
                p.Add("@IsActive", entity.isActive);
                p.Add("@UserId", userId);
                p.Add("@NewId", dbType: DbType.Int32, direction: ParameterDirection.Output);

                await conn.ExecuteAsync(
                    "usp_Stop_Insert",
                    p,
                    commandType: CommandType.StoredProcedure);

                return p.Get<int>("@NewId");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while inserting stop: {ex.Message}", ex);
            }
        }

        public async Task<bool> UpdateAsync(StopDTO entity, int userId)
        {
            try
            {
                using var conn = _db.CreateConnection();

                var p = new DynamicParameters();

                p.Add("@Stop_Id", Convert.ToInt32(entity.stopId));
                p.Add("@Stop_Name", entity.stopName);
                p.Add("@Route_Id", Convert.ToInt32(entity.routeId));
                p.Add("@Sequence", entity.stopOrder);
                p.Add("@IsActive", entity.isActive);
                p.Add("@UserId", userId);
                p.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);

                await conn.ExecuteAsync(
                    "usp_Stop_Update",
                    p,
                    commandType: CommandType.StoredProcedure);

                return p.Get<bool>("@Success");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while updating stop: {ex.Message}", ex);
            }
        }

        public async Task<bool> DeleteAsync(int stopId, int deletedBy)
        {
            try
            {
                using var conn = _db.CreateConnection();

                var p = new DynamicParameters();

                p.Add("@Stop_Id", stopId);
                p.Add("@DeletedBy", deletedBy);
                p.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);

                await conn.ExecuteAsync(
                    "usp_Stop_Delete",
                    p,
                    commandType: CommandType.StoredProcedure);

                return p.Get<bool>("@Success");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while deleting stop: {ex.Message}", ex);
            }
        }
    }
}