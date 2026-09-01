using Dapper;
using GenXTransitAPI.DataAccess.Data;
using GenXTransitAPI.DataAccess.Interface.IRepositories;
using GenXTransitAPI.Models.DTO_s;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

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

            return dbResult.Select(x => new OrgRegionDTO
            {
                regionId = x.Region_ID?.ToString(),
                regionCode = x.Region_Code,
                regionName = x.Region_Name,
                isActive = x.IsActive,
                divisions = x.DivisionCount,
                zoneCount = x.ZoneCount, // ✅ Added
                depots = x.DepotCount,
                stations = x.StationCount,
                workshops = x.WorkshopCount,
                createdBy = x.Created_By,
                createdDate = x.Created_Date,
                modifiedBy = x.Modified_By,
                modifiedDate = x.Modified_Date,
                totalCount = x.TotalCount
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
                divisions = dbResult.DivisionCount,
                zoneCount = dbResult.ZoneCount, // ✅ Added
                depots = dbResult.DepotCount,
                stations = dbResult.StationCount,
                workshops = dbResult.WorkshopCount,
                createdBy = dbResult.Created_By,
                createdDate = dbResult.Created_Date,
                modifiedBy = dbResult.Modified_By,
                modifiedDate = dbResult.Modified_Date,
                totalCount = 0,
                // ✅ Parse JSON from database
                divisionsList = !string.IsNullOrEmpty(dbResult.Divisions)
                    ? JsonConvert.DeserializeObject(dbResult.Divisions)
                    : null,
                zonesList = !string.IsNullOrEmpty(dbResult.Zones)
                    ? JsonConvert.DeserializeObject(dbResult.Zones)
                    : null,
                depotsList = !string.IsNullOrEmpty(dbResult.Depots)
                    ? JsonConvert.DeserializeObject(dbResult.Depots)
                    : null
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
                throw new Exception(ex.Message, ex);
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
                throw new Exception(ex.Message, ex);
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
                throw new Exception(ex.Message, ex);
            }
        }
    }
}