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
    public class OrgStationRepository : IOrgStationRepository
    {
        private readonly DBHelper _db;

        public OrgStationRepository(DBHelper db)
        {
            _db = db;
        }

        public async Task<IEnumerable<OrgStationDTO>> GetAllAsync(
            string? searchText,
            int? regionId,
            int? divisionId,
            int? depotId,
            bool? isActive,
            int? scopeToUser,
            int pageNumber = 1,
            int pageSize = 10)
        {
            using var conn = _db.CreateConnection();

            var dbResult = await conn.QueryAsync<OrgStationDbDTO>(
                "usp_Station_GetAll",
                new
                {
                    SearchText = searchText,
                    RegionId = regionId,
                    DivisionId = divisionId,
                    DepotId = depotId,
                    IsActive = isActive,
                    ScopeToUser = scopeToUser,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                },
                commandType: CommandType.StoredProcedure);

            return dbResult.Select(x => new OrgStationDTO
            {
                stationId = x.Station_Id?.ToString(),
                stationCode = x.Station_Code,
                stationName = x.Station_Name,
                platforms = x.Platforms,
                dailyFootfall = x.Daily_Footfall,
                isActive = x.IsActive,
                createdBy = x.Created_By,
                createdDate = x.Created_Date,
                modifiedBy = x.Modified_By,
                modifiedDate = x.Modified_Date,
                // Region details
                regionId = x.Region_Id?.ToString(),
                regionCode = x.Region_Code,
                regionName = x.Region_Name,
                // Division details
                divisionId = x.Division_Id?.ToString(),
                divisionCode = x.Division_Code,
                divisionName = x.Division_Name,
                // Depot details
                depotId = x.Depot_Id?.ToString(),
                depotCode = x.Depot_Code,
                depotName = x.Depot_Name,
                totalCount = x.TotalCount
            });
        }

        public async Task<OrgStationDTO> GetByIdAsync(int stationId)
        {
            using var conn = _db.CreateConnection();

            var dbResult = await conn.QueryFirstOrDefaultAsync<OrgStationDbDTO>(
                "usp_Station_GetById",
                new { Station_Id = stationId },
                commandType: CommandType.StoredProcedure);

            if (dbResult == null)
                return null;

            return new OrgStationDTO
            {
                stationId = dbResult.Station_Id?.ToString(),
                stationCode = dbResult.Station_Code,
                stationName = dbResult.Station_Name,
                platforms = dbResult.Platforms,
                dailyFootfall = dbResult.Daily_Footfall,
                isActive = dbResult.IsActive,
                createdBy = dbResult.Created_By,
                createdDate = dbResult.Created_Date,
                modifiedBy = dbResult.Modified_By,
                modifiedDate = dbResult.Modified_Date,
                // Region details
                regionId = dbResult.Region_Id?.ToString(),
                regionCode = dbResult.Region_Code,
                regionName = dbResult.Region_Name,
                // Division details
                divisionId = dbResult.Division_Id?.ToString(),
                divisionCode = dbResult.Division_Code,
                divisionName = dbResult.Division_Name,
                // Depot details
                depotId = dbResult.Depot_Id?.ToString(),
                depotCode = dbResult.Depot_Code,
                depotName = dbResult.Depot_Name,
                totalCount = 0
            };
        }

        public async Task<string> GetNextCodeAsync()
        {
            using var conn = _db.CreateConnection();

            var p = new DynamicParameters();
            p.Add("@NextCode", dbType: DbType.String, direction: ParameterDirection.Output, size: 50);

            await conn.ExecuteAsync(
                "usp_Station_GetNextCode",
                p,
                commandType: CommandType.StoredProcedure);

            return p.Get<string>("@NextCode");
        }

        public async Task<int> InsertAsync(OrgStationDTO entity, int userId)
        {
            try
            {
                using var conn = _db.CreateConnection();

                var p = new DynamicParameters();

                p.Add("@Station_Name", entity.stationName);
                p.Add("@Region_Id", Convert.ToInt32(entity.regionId));
                p.Add("@Division_Id", Convert.ToInt32(entity.divisionId));
                p.Add("@Depot_Id", Convert.ToInt32(entity.depotId));
                p.Add("@Platforms", entity.platforms);
                p.Add("@Daily_Footfall", entity.dailyFootfall);
                p.Add("@IsActive", entity.isActive);
                p.Add("@UserId", userId);
                p.Add("@NewId", dbType: DbType.Int32, direction: ParameterDirection.Output);

                await conn.ExecuteAsync(
                    "usp_Station_Insert",
                    p,
                    commandType: CommandType.StoredProcedure);

                return p.Get<int>("@NewId");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while inserting station: {ex.Message}", ex);
            }
        }

        public async Task<bool> UpdateAsync(OrgStationDTO entity, int userId)
        {
            try
            {
                using var conn = _db.CreateConnection();

                var p = new DynamicParameters();

                p.Add("@Station_Id", Convert.ToInt32(entity.stationId));
                p.Add("@Station_Name", entity.stationName);
                p.Add("@Region_Id", Convert.ToInt32(entity.regionId));
                p.Add("@Division_Id", Convert.ToInt32(entity.divisionId));
                p.Add("@Depot_Id", Convert.ToInt32(entity.depotId));
                p.Add("@Platforms", entity.platforms);
                p.Add("@Daily_Footfall", entity.dailyFootfall);
                p.Add("@IsActive", entity.isActive);
                p.Add("@UserId", userId);
                p.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);

                await conn.ExecuteAsync(
                    "usp_Station_Update",
                    p,
                    commandType: CommandType.StoredProcedure);

                return p.Get<bool>("@Success");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while updating station: {ex.Message}", ex);
            }
        }

        public async Task<bool> DeleteAsync(int stationId, int deletedBy)
        {
            try
            {
                using var conn = _db.CreateConnection();

                var p = new DynamicParameters();

                p.Add("@Station_Id", stationId);
                p.Add("@DeletedBy", deletedBy);
                p.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);

                await conn.ExecuteAsync(
                    "usp_Station_Delete",
                    p,
                    commandType: CommandType.StoredProcedure);

                return p.Get<bool>("@Success");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while deleting station: {ex.Message}", ex);
            }
        }
    }
}