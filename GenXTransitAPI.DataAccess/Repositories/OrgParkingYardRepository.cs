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
    public class OrgParkingYardRepository : IOrgParkingYardRepository
    {
        private readonly DBHelper _db;

        public OrgParkingYardRepository(DBHelper db)
        {
            _db = db;
        }

        public async Task<IEnumerable<OrgParkingYardDTO>> GetAllAsync(
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

            var dbResult = await conn.QueryAsync<OrgParkingYardDbDTO>(
                "usp_ParkingYard_GetAll",
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

            return dbResult.Select(x => new OrgParkingYardDTO
            {
                yardId = x.Yard_ID?.ToString(),
                yardCode = x.Yard_Code,
                yardName = x.Yard_Name,
                capacity = x.Capacity,
                occupied = x.Occupied,
                availableSpots = x.Capacity - x.Occupied,
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

        public async Task<OrgParkingYardDTO> GetByIdAsync(int yardId)
        {
            using var conn = _db.CreateConnection();

            var dbResult = await conn.QueryFirstOrDefaultAsync<OrgParkingYardDbDTO>(
                "usp_ParkingYard_GetById",
                new { Yard_ID = yardId },
                commandType: CommandType.StoredProcedure);

            if (dbResult == null)
                return null;

            return new OrgParkingYardDTO
            {
                yardId = dbResult.Yard_ID?.ToString(),
                yardCode = dbResult.Yard_Code,
                yardName = dbResult.Yard_Name,
                capacity = dbResult.Capacity,
                occupied = dbResult.Occupied,
                availableSpots = dbResult.Capacity - dbResult.Occupied,
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
                "usp_ParkingYard_GetNextCode",
                p,
                commandType: CommandType.StoredProcedure);

            return p.Get<string>("@NextCode");
        }

        public async Task<int> InsertAsync(OrgParkingYardDTO entity, int userId)
        {
            try
            {
                using var conn = _db.CreateConnection();

                var p = new DynamicParameters();

                p.Add("@Yard_Name", entity.yardName);
                p.Add("@Region_Id", Convert.ToInt32(entity.regionId));
                p.Add("@Division_Id", Convert.ToInt32(entity.divisionId));
                p.Add("@Depot_Id", Convert.ToInt32(entity.depotId));
                p.Add("@Capacity", entity.capacity);
                p.Add("@Occupied", entity.occupied);
                p.Add("@IsActive", entity.isActive);
                p.Add("@UserId", userId);
                p.Add("@NewId", dbType: DbType.Int32, direction: ParameterDirection.Output);

                await conn.ExecuteAsync(
                    "usp_ParkingYard_Insert",
                    p,
                    commandType: CommandType.StoredProcedure);

                return p.Get<int>("@NewId");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<bool> UpdateAsync(OrgParkingYardDTO entity, int userId)
        {
            try
            {
                using var conn = _db.CreateConnection();

                var p = new DynamicParameters();

                p.Add("@Yard_ID", Convert.ToInt32(entity.yardId));
                p.Add("@Yard_Name", entity.yardName);
                p.Add("@Region_Id", Convert.ToInt32(entity.regionId));
                p.Add("@Division_Id", Convert.ToInt32(entity.divisionId));
                p.Add("@Depot_Id", Convert.ToInt32(entity.depotId));
                p.Add("@Capacity", entity.capacity);
                p.Add("@Occupied", entity.occupied);
                p.Add("@IsActive", entity.isActive);
                p.Add("@UserId", userId);
                p.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);

                await conn.ExecuteAsync(
                    "usp_ParkingYard_Update",
                    p,
                    commandType: CommandType.StoredProcedure);

                return p.Get<bool>("@Success");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<bool> DeleteAsync(int yardId, int deletedBy)
        {
            try
            {
                using var conn = _db.CreateConnection();

                var p = new DynamicParameters();

                p.Add("@Yard_ID", yardId);
                p.Add("@DeletedBy", deletedBy);
                p.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);

                await conn.ExecuteAsync(
                    "usp_ParkingYard_Delete",
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