using Dapper;
using GenXTransitAPI.DataAccess.Data;
using GenXTransitAPI.DataAccess.Interfaces.Repositories;
using GenXTransitAPI.Models.DTO_s;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace GenXTransitAPI.DataAccess.Repositories
{
    public class OrgRegionRepository : IOrgRegionRepository
    {
        private readonly DBHelper _db;

        public OrgRegionRepository(DBHelper db)
        {
            _db = db;
        }

        public async Task<IEnumerable<OrgRegionDTO>> GetAllAsync(
            string? searchText,
            bool? isActive,
            int? scopeToUser,
            int pageNumber = 1,
            int pageSize = 10)
        {
            using var conn = _db.CreateConnection();

            var dbResult = await conn.QueryAsync<OrgRegionDbDTO>(
                "usp_Region_GetAll",
                new
                {
                    SearchText = searchText,
                    IsActive = isActive,
                    ScopeToUser = scopeToUser,
                    PageNumber = pageNumber,   
                    PageSize = pageSize        
                },
                commandType: CommandType.StoredProcedure);

            // Convert from DB format to UI format
            return dbResult.Select(x => new OrgRegionDTO
            {
                regionId = x.Region_ID?.ToString(),
                regionCode = x.Region_Code,
                regionName = x.Region_Name,
                isActive = x.IsActive,
                divisions = 0,
                depots = 0,
                stations = 0,
                workshops = 0,
                createdBy = x.Created_By,
                createdDate = x.Created_Date,
                modifiedBy = x.Modified_By,
                modifiedDate = x.Modified_Date,
                isDeleted = x.IsDeleted,
                deletedBy = x.Deleted_By,
                deletedDate = x.Deleted_Date,
                TotalCount = x.TotalCount  
            });
        }

        public async Task<OrgRegionDTO> GetByIdAsync(int regionId)
        {
            using var conn = _db.CreateConnection();

            var dbResult = await conn.QueryFirstOrDefaultAsync<OrgRegionDbDTO>(
                "usp_Region_GetById",
                new { Region_ID = regionId },
                commandType: CommandType.StoredProcedure);

            if (dbResult == null)
                return null;

            // Convert from DB format to UI format
            return new OrgRegionDTO
            {
                regionId = dbResult.Region_ID?.ToString(),
                regionCode = dbResult.Region_Code,
                regionName = dbResult.Region_Name,
                isActive = dbResult.IsActive,
                divisions = 0,
                depots = 0,
                stations = 0,
                workshops = 0,
                createdBy = dbResult.Created_By,
                createdDate = dbResult.Created_Date,
                modifiedBy = dbResult.Modified_By,
                modifiedDate = dbResult.Modified_Date,
                isDeleted = dbResult.IsDeleted,
                deletedBy = dbResult.Deleted_By,
                deletedDate = dbResult.Deleted_Date,
                TotalCount = 0
            };
        }

        public async Task<string> GetNextCodeAsync()
        {
            using var conn = _db.CreateConnection();

            var p = new DynamicParameters();
            p.Add("@NextCode", dbType: DbType.String, direction: ParameterDirection.Output, size: 50);

            await conn.ExecuteAsync(
                "usp_Region_GetNextCode",
                p,
                commandType: CommandType.StoredProcedure);

            return p.Get<string>("@NextCode");
        }

        public async Task<int> InsertAsync(OrgRegionDTO entity, int userId)
        {
            try
            {
                using var conn = _db.CreateConnection();

                var p = new DynamicParameters();

                p.Add("@Region_Name", entity.regionName);
                p.Add("@IsActive", entity.isActive);
                p.Add("@UserId", userId);
                p.Add("@NewId", dbType: DbType.Int32, direction: ParameterDirection.Output);

                await conn.ExecuteAsync(
                    "usp_Region_Insert",
                    p,
                    commandType: CommandType.StoredProcedure);

                return p.Get<int>("@NewId");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while inserting region: {ex.Message}", ex);
            }
        }

        public async Task<bool> UpdateAsync(OrgRegionDTO entity, int userId)
        {
            try
            {
                using var conn = _db.CreateConnection();

                var p = new DynamicParameters();

                p.Add("@Region_ID", Convert.ToInt32(entity.regionId));
                p.Add("@Region_Name", entity.regionName);
                p.Add("@IsActive", entity.isActive);
                p.Add("@UserId", userId);
                p.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);

                await conn.ExecuteAsync(
                    "usp_Region_Update",
                    p,
                    commandType: CommandType.StoredProcedure);

                return p.Get<bool>("@Success");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while updating region: {ex.Message}", ex);
            }
        }

        public async Task<bool> DeleteAsync(int regionId, int deletedBy)
        {
            try
            {
                using var conn = _db.CreateConnection();

                var p = new DynamicParameters();

                p.Add("@Region_ID", regionId);
                p.Add("@DeletedBy", deletedBy);
                p.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);

                await conn.ExecuteAsync(
                    "usp_Region_Delete",
                    p,
                    commandType: CommandType.StoredProcedure);

                return p.Get<bool>("@Success");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while deleting region: {ex.Message}", ex);
            }
        }
    }
}