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
    public class OrgDepotRepository : IOrgDepotRepository
    {
        private readonly DBHelper _db;

        public OrgDepotRepository(DBHelper db)
        {
            _db = db;
        }

        public async Task<IEnumerable<OrgDepotDTO>> GetAllAsync(
            string? searchText,
            int? corporationId,
            int? regionId,
            int? divisionId,
            int? zoneId,
            bool? isActive,
            int? scopeToUser,
            int pageNumber = 1,
            int pageSize = 10)
        {
            using var conn = _db.CreateConnection();

            var dbResult = await conn.QueryAsync<OrgDepotDbDTO>(
                "usp_Depot_GetAll",
                new
                {
                    SearchText = searchText,
                    CorporationId = corporationId,
                    RegionId = regionId,
                    DivisionId = divisionId,
                    ZoneId = zoneId,
                    IsActive = isActive,
                    ScopeToUser = scopeToUser,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                },
                commandType: CommandType.StoredProcedure);

            return dbResult.Select(x => new OrgDepotDTO
            {
                depotId = x.Depot_Id?.ToString(),
                depotCode = x.Depot_Code,
                depotName = x.Depot_Name,
                service = x.Service,
                isActive = x.IsActive,
                createdBy = x.Created_By,
                createdDate = x.Created_Date,
                modifiedBy = x.Modified_By,
                modifiedDate = x.Modified_Date,
                // Corporation details
                corpId = x.Corporation_Id?.ToString(),
                corpCode = x.Corp_Code,
                corporationName = x.Corporation_Name,
                // Region details
                regionId = x.Region_Id?.ToString(),
                regionCode = x.Region_Code,
                regionName = x.Region_Name,
                // Division details
                divisionId = x.Division_Id?.ToString(),
                divisionCode = x.Division_Code,
                divisionName = x.Division_Name,
                // Zone details
                zoneId = x.Zone_Id?.ToString(),
                zoneCode = x.Zone_Code,
                zoneName = x.Zone_Name,
                // Counts
                stationCount = x.StationCount,
                workshopCount = x.WorkshopCount,
                parkingYardCount = x.ParkingYardCount,
                totalCount = x.TotalCount
            });
        }

        public async Task<OrgDepotDTO> GetByIdAsync(int depotId)
        {
            using var conn = _db.CreateConnection();

            var dbResult = await conn.QueryFirstOrDefaultAsync<OrgDepotDbDTO>(
                "usp_Depot_GetById",
                new { Depot_Id = depotId },
                commandType: CommandType.StoredProcedure);

            if (dbResult == null)
                return null;

            return new OrgDepotDTO
            {
                depotId = dbResult.Depot_Id?.ToString(),
                depotCode = dbResult.Depot_Code,
                depotName = dbResult.Depot_Name,
                service = dbResult.Service,
                isActive = dbResult.IsActive,
                createdBy = dbResult.Created_By,
                createdDate = dbResult.Created_Date,
                modifiedBy = dbResult.Modified_By,
                modifiedDate = dbResult.Modified_Date,
                // Corporation details
                corpId = dbResult.Corporation_Id?.ToString(),
                corpCode = dbResult.Corp_Code,
                corporationName = dbResult.Corporation_Name,
                // Region details
                regionId = dbResult.Region_Id?.ToString(),
                regionCode = dbResult.Region_Code,
                regionName = dbResult.Region_Name,
                // Division details
                divisionId = dbResult.Division_Id?.ToString(),
                divisionCode = dbResult.Division_Code,
                divisionName = dbResult.Division_Name,
                // Zone details
                zoneId = dbResult.Zone_Id?.ToString(),
                zoneCode = dbResult.Zone_Code,
                zoneName = dbResult.Zone_Name,
                // Counts
                stationCount = dbResult.StationCount,
                workshopCount = dbResult.WorkshopCount,
                parkingYardCount = dbResult.ParkingYardCount,
                totalCount = 0,
                // Parse JSON from database
                stations = !string.IsNullOrEmpty(dbResult.Stations)
                    ? JsonConvert.DeserializeObject(dbResult.Stations)
                    : null,
                workshops = !string.IsNullOrEmpty(dbResult.Workshops)
                    ? JsonConvert.DeserializeObject(dbResult.Workshops)
                    : null,
                parkingYards = !string.IsNullOrEmpty(dbResult.ParkingYards)
                    ? JsonConvert.DeserializeObject(dbResult.ParkingYards)
                    : null
            };
        }

        public async Task<string> GetNextCodeAsync()
        {
            using var conn = _db.CreateConnection();

            var p = new DynamicParameters();
            p.Add("@NextCode", dbType: DbType.String, direction: ParameterDirection.Output, size: 50);

            await conn.ExecuteAsync(
                "usp_Depot_GetNextCode",
                p,
                commandType: CommandType.StoredProcedure);

            return p.Get<string>("@NextCode");
        }

        public async Task<int> InsertAsync(OrgDepotDTO entity, int userId)
        {
            try
            {
                using var conn = _db.CreateConnection();

                var p = new DynamicParameters();

                p.Add("@Depot_Name", entity.depotName);
                p.Add("@Corporation_Id", Convert.ToInt32(entity.corpId));
                p.Add("@Service", entity.service);
                p.Add("@Zone_Id", Convert.ToInt32(entity.zoneId));
                p.Add("@Region_Id", Convert.ToInt32(entity.regionId));
                p.Add("@Division_Id", Convert.ToInt32(entity.divisionId));
                p.Add("@IsActive", entity.isActive);
                p.Add("@UserId", userId);
                p.Add("@NewId", dbType: DbType.Int32, direction: ParameterDirection.Output);

                await conn.ExecuteAsync(
                    "usp_Depot_Insert",
                    p,
                    commandType: CommandType.StoredProcedure);

                return p.Get<int>("@NewId");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<bool> UpdateAsync(OrgDepotDTO entity, int userId)
        {
            try
            {
                using var conn = _db.CreateConnection();

                var p = new DynamicParameters();

                p.Add("@Depot_Id", Convert.ToInt32(entity.depotId));
                p.Add("@Depot_Name", entity.depotName);
                p.Add("@Corporation_Id", Convert.ToInt32(entity.corpId));
                p.Add("@Service", entity.service);
                p.Add("@Zone_Id", Convert.ToInt32(entity.zoneId));
                p.Add("@Region_Id", Convert.ToInt32(entity.regionId));
                p.Add("@Division_Id", Convert.ToInt32(entity.divisionId));
                p.Add("@IsActive", entity.isActive);
                p.Add("@UserId", userId);
                p.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);

                await conn.ExecuteAsync(
                    "usp_Depot_Update",
                    p,
                    commandType: CommandType.StoredProcedure);

                return p.Get<bool>("@Success");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<bool> DeleteAsync(int depotId, int deletedBy)
        {
            try
            {
                using var conn = _db.CreateConnection();

                var p = new DynamicParameters();

                p.Add("@Depot_Id", depotId);
                p.Add("@DeletedBy", deletedBy);
                p.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);

                await conn.ExecuteAsync(
                    "usp_Depot_Delete",
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