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
    public class StageRepository : IStageRepository
    {
        private readonly DBHelper _db;

        public StageRepository(DBHelper db)
        {
            _db = db;
        }

        public async Task<IEnumerable<StageDTO>> GetAllAsync(
            string? searchText,
            int? routeId,
            bool? isActive,
            int? scopeToUser,
            int pageNumber = 1,
            int pageSize = 10)
        {
            using var conn = _db.CreateConnection();

            var dbResult = await conn.QueryAsync<StageDbDTO>(
                "usp_Stage_GetAll",
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

            return dbResult.Select(x => new StageDTO
            {
                stageId = x.Stage_Id?.ToString(),
                stageCode = x.Stage_Code,
                stageName = x.Stage_Name,
                distance = x.Distance,
                isActive = x.IsActive,
                createdBy = x.Created_By,
                createdDate = x.Created_Date,
                modifiedBy = x.Modified_By,
                modifiedDate = x.Modified_Date,
                // Route details
                routeId = x.Route_Id?.ToString(),
                routeCode = x.Route_Code,
                routeName = x.Route_Name,
                // Section From details
                sectionFromId = x.Section_From_Id?.ToString(),
                sectionFromCode = x.SectionFromCode,
                sectionFromName = x.SectionFromName,
                // Section To details
                sectionToId = x.Section_To_Id?.ToString(),
                sectionToCode = x.SectionToCode,
                sectionToName = x.SectionToName,
                totalCount = x.TotalCount
            });
        }

        public async Task<StageDTO> GetByIdAsync(int stageId)
        {
            using var conn = _db.CreateConnection();

            var dbResult = await conn.QueryFirstOrDefaultAsync<StageDbDTO>(
                "usp_Stage_GetById",
                new { Stage_Id = stageId },
                commandType: CommandType.StoredProcedure);

            if (dbResult == null)
                return null;

            return new StageDTO
            {
                stageId = dbResult.Stage_Id?.ToString(),
                stageCode = dbResult.Stage_Code,
                stageName = dbResult.Stage_Name,
                distance = dbResult.Distance,
                isActive = dbResult.IsActive,
                createdBy = dbResult.Created_By,
                createdDate = dbResult.Created_Date,
                modifiedBy = dbResult.Modified_By,
                modifiedDate = dbResult.Modified_Date,
                // Route details
                routeId = dbResult.Route_Id?.ToString(),
                routeCode = dbResult.Route_Code,
                routeName = dbResult.Route_Name,
                // Section From details
                sectionFromId = dbResult.Section_From_Id?.ToString(),
                sectionFromCode = dbResult.SectionFromCode,
                sectionFromName = dbResult.SectionFromName,
                // Section To details
                sectionToId = dbResult.Section_To_Id?.ToString(),
                sectionToCode = dbResult.SectionToCode,
                sectionToName = dbResult.SectionToName,
                totalCount = 0
            };
        }

        public async Task<string> GetNextCodeAsync()
        {
            using var conn = _db.CreateConnection();

            var p = new DynamicParameters();
            p.Add("@NextCode", dbType: DbType.String, direction: ParameterDirection.Output, size: 50);

            await conn.ExecuteAsync(
                "usp_Stage_GetNextCode",
                p,
                commandType: CommandType.StoredProcedure);

            return p.Get<string>("@NextCode");
        }

        public async Task<int> InsertAsync(StageDTO entity, int userId)
        {
            try
            {
                using var conn = _db.CreateConnection();

                var p = new DynamicParameters();

                p.Add("@Stage_Name", entity.stageName);
                p.Add("@Route_Id", Convert.ToInt32(entity.routeId));
                p.Add("@Section_From_Id", Convert.ToInt32(entity.sectionFromId));
                p.Add("@Section_To_Id", Convert.ToInt32(entity.sectionToId));
                p.Add("@Distance", entity.distance);
                p.Add("@IsActive", entity.isActive);
                p.Add("@UserId", userId);
                p.Add("@NewId", dbType: DbType.Int32, direction: ParameterDirection.Output);

                await conn.ExecuteAsync(
                    "usp_Stage_Insert",
                    p,
                    commandType: CommandType.StoredProcedure);

                return p.Get<int>("@NewId");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while inserting stage: {ex.Message}", ex);
            }
        }

        public async Task<bool> UpdateAsync(StageDTO entity, int userId)
        {
            try
            {
                using var conn = _db.CreateConnection();

                var p = new DynamicParameters();

                p.Add("@Stage_Id", Convert.ToInt32(entity.stageId));
                p.Add("@Stage_Name", entity.stageName);
                p.Add("@Route_Id", Convert.ToInt32(entity.routeId));
                p.Add("@Section_From_Id", Convert.ToInt32(entity.sectionFromId));
                p.Add("@Section_To_Id", Convert.ToInt32(entity.sectionToId));
                p.Add("@Distance", entity.distance);
                p.Add("@IsActive", entity.isActive);
                p.Add("@UserId", userId);
                p.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);

                await conn.ExecuteAsync(
                    "usp_Stage_Update",
                    p,
                    commandType: CommandType.StoredProcedure);

                return p.Get<bool>("@Success");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while updating stage: {ex.Message}", ex);
            }
        }

        public async Task<bool> DeleteAsync(int stageId, int deletedBy)
        {
            try
            {
                using var conn = _db.CreateConnection();

                var p = new DynamicParameters();

                p.Add("@Stage_Id", stageId);
                p.Add("@DeletedBy", deletedBy);
                p.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);

                await conn.ExecuteAsync(
                    "usp_Stage_Delete",
                    p,
                    commandType: CommandType.StoredProcedure);

                return p.Get<bool>("@Success");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while deleting stage: {ex.Message}", ex);
            }
        }
    }
}