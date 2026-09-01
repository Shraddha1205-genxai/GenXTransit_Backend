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
    public class OrgDivisionRepository : IOrgDivisionRepository
    {
        private readonly DBHelper _db;

        public OrgDivisionRepository(DBHelper db)
        {
            _db = db;
        }

        public async Task<IEnumerable<OrgDivisionDTO>> GetAllAsync(
            string? searchText,
            int? regionId,
            bool? isActive,
            int? scopeToUser,
            int pageNumber = 1,
            int pageSize = 10)
        {
            using var conn = _db.CreateConnection();

            var dbResult = await conn.QueryAsync<OrgDivisionDbDTO>(
                "usp_Division_GetAll",
                new
                {
                    SearchText = searchText,
                    RegionId = regionId,
                    IsActive = isActive,
                    ScopeToUser = scopeToUser,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                },
                commandType: CommandType.StoredProcedure);

            // Convert from DB format to UI format
            return dbResult.Select(x => new OrgDivisionDTO
            {
                divisionId = x.Division_ID?.ToString(),
                divisionCode = x.Division_Code,
                divisionName = x.Division_Name,
                regionId = x.Region_ID?.ToString(),
                regionCode = x.Region_Code,
                regionName = x.Region_Name,
                isActive = x.IsActive,
                // ✅ Map the actual counts from SP
                depots = x.DepotCount,
                workshops = x.WorkshopCount,
                stations = x.StationCount,
                parkingYards = x.ParkingYardCount,
                createdBy = x.Created_By,
                createdDate = x.Created_Date,
                modifiedBy = x.Modified_By,
                modifiedDate = x.Modified_Date,
                totalCount = x.TotalCount
            });
        }

        public async Task<OrgDivisionDTO> GetByIdAsync(int divisionId)
        {
            using var conn = _db.CreateConnection();

            var dbResult = await conn.QueryFirstOrDefaultAsync<OrgDivisionDbDTO>(
                "usp_Division_GetById",
                new { Division_ID = divisionId },
                commandType: CommandType.StoredProcedure);

            if (dbResult == null)
                return null;

            // Convert from DB format to UI format
            return new OrgDivisionDTO
            {
                divisionId = dbResult.Division_ID?.ToString(),
                divisionCode = dbResult.Division_Code,
                divisionName = dbResult.Division_Name,
                regionId = dbResult.Region_ID?.ToString(),
                regionCode = dbResult.Region_Code,
                regionName = dbResult.Region_Name,
                isActive = dbResult.IsActive,
                // ✅ Map the actual counts from SP
                depots = dbResult.DepotCount,
                workshops = dbResult.WorkshopCount,
                stations = dbResult.StationCount,
                parkingYards = dbResult.ParkingYardCount,
                createdBy = dbResult.Created_By,
                createdDate = dbResult.Created_Date,
                modifiedBy = dbResult.Modified_By,
                modifiedDate = dbResult.Modified_Date,
                totalCount = 0,
                // ✅ Parse JSON from database
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
                "usp_Division_GetNextCode",
                p,
                commandType: CommandType.StoredProcedure);

            return p.Get<string>("@NextCode");
        }

        public async Task<int> InsertAsync(OrgDivisionDTO entity, int userId)
        {
            try
            {
                using var conn = _db.CreateConnection();

                var p = new DynamicParameters();

                p.Add("@Division_Name", entity.divisionName);
                p.Add("@Region_ID", Convert.ToInt32(entity.regionId));
                p.Add("@IsActive", entity.isActive);
                p.Add("@UserId", userId);
                p.Add("@NewId", dbType: DbType.Int32, direction: ParameterDirection.Output);

                await conn.ExecuteAsync(
                    "usp_Division_Insert",
                    p,
                    commandType: CommandType.StoredProcedure);

                return p.Get<int>("@NewId");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<bool> UpdateAsync(OrgDivisionDTO entity, int userId)
        {
            try
            {
                using var conn = _db.CreateConnection();

                var p = new DynamicParameters();

                p.Add("@Division_ID", Convert.ToInt32(entity.divisionId));
                p.Add("@Division_Name", entity.divisionName);
                p.Add("@Region_ID", Convert.ToInt32(entity.regionId));
                p.Add("@IsActive", entity.isActive);
                p.Add("@UserId", userId);
                p.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);

                await conn.ExecuteAsync(
                    "usp_Division_Update",
                    p,
                    commandType: CommandType.StoredProcedure);

                return p.Get<bool>("@Success");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<bool> DeleteAsync(int divisionId, int deletedBy)
        {
            try
            {
                using var conn = _db.CreateConnection();

                var p = new DynamicParameters();

                p.Add("@Division_ID", divisionId);
                p.Add("@DeletedBy", deletedBy);
                p.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);

                await conn.ExecuteAsync(
                    "usp_Division_Delete",
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