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
    public class OrgWorkshopRepository : IOrgWorkshopRepository
    {
        private readonly DBHelper _db;

        public OrgWorkshopRepository(DBHelper db)
        {
            _db = db;
        }

        public async Task<IEnumerable<OrgWorkshopDTO>> GetAllAsync(
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

            var dbResult = await conn.QueryAsync<OrgWorkshopDbDTO>(
                "usp_Workshop_GetAll",
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

            return dbResult.Select(x => new OrgWorkshopDTO
            {
                workShopId = x.WorkShop_ID?.ToString(),
                workShopCode = x.WorkShop_Code,
                workShopName = x.WorkShop_Name,
                workBays = x.Work_Bays,
                activeRepairJobs = x.Active_Repair_Jobs,
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

        public async Task<OrgWorkshopDTO> GetByIdAsync(int workShopId)
        {
            using var conn = _db.CreateConnection();

            var dbResult = await conn.QueryFirstOrDefaultAsync<OrgWorkshopDbDTO>(
                "usp_Workshop_GetById",
                new { WorkShop_ID = workShopId },
                commandType: CommandType.StoredProcedure);

            if (dbResult == null)
                return null;

            return new OrgWorkshopDTO
            {
                workShopId = dbResult.WorkShop_ID?.ToString(),
                workShopCode = dbResult.WorkShop_Code,
                workShopName = dbResult.WorkShop_Name,
                workBays = dbResult.Work_Bays,
                activeRepairJobs = dbResult.Active_Repair_Jobs,
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
                "usp_Workshop_GetNextCode",
                p,
                commandType: CommandType.StoredProcedure);

            return p.Get<string>("@NextCode");
        }

        public async Task<int> InsertAsync(OrgWorkshopDTO entity, int userId)
        {
            try
            {
                using var conn = _db.CreateConnection();

                var p = new DynamicParameters();

                p.Add("@WorkShop_Name", entity.workShopName);
                p.Add("@Region_Id", Convert.ToInt32(entity.regionId));
                p.Add("@Division_Id", Convert.ToInt32(entity.divisionId));
                p.Add("@Depot_Id", Convert.ToInt32(entity.depotId));
                p.Add("@Work_Bays", entity.workBays);
                p.Add("@Active_Repair_Jobs", entity.activeRepairJobs);
                p.Add("@IsActive", entity.isActive);
                p.Add("@UserId", userId);
                p.Add("@NewId", dbType: DbType.Int32, direction: ParameterDirection.Output);

                await conn.ExecuteAsync(
                    "usp_Workshop_Insert",
                    p,
                    commandType: CommandType.StoredProcedure);

                return p.Get<int>("@NewId");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<bool> UpdateAsync(OrgWorkshopDTO entity, int userId)
        {
            try
            {
                using var conn = _db.CreateConnection();

                var p = new DynamicParameters();

                p.Add("@WorkShop_ID", Convert.ToInt32(entity.workShopId));
                p.Add("@WorkShop_Name", entity.workShopName);
                p.Add("@Region_Id", Convert.ToInt32(entity.regionId));
                p.Add("@Division_Id", Convert.ToInt32(entity.divisionId));
                p.Add("@Depot_Id", Convert.ToInt32(entity.depotId));
                p.Add("@Work_Bays", entity.workBays);
                p.Add("@Active_Repair_Jobs", entity.activeRepairJobs);
                p.Add("@IsActive", entity.isActive);
                p.Add("@UserId", userId);
                p.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);

                await conn.ExecuteAsync(
                    "usp_Workshop_Update",
                    p,
                    commandType: CommandType.StoredProcedure);

                return p.Get<bool>("@Success");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<bool> DeleteAsync(int workShopId, int deletedBy)
        {
            try
            {
                using var conn = _db.CreateConnection();

                var p = new DynamicParameters();

                p.Add("@WorkShop_ID", workShopId);
                p.Add("@DeletedBy", deletedBy);
                p.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);

                await conn.ExecuteAsync(
                    "usp_Workshop_Delete",
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