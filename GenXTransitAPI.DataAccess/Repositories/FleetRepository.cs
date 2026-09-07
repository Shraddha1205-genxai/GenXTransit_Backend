using Dapper;
using GenXTransitAPI.DataAccess.Data;
using GenXTransitAPI.DataAccess.Interface.IRepositories;
using GenXTransitAPI.Models.DTO_s;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace GenXTransitAPI.DataAccess.Repositories
{
    public class FleetRepository : IFleetRepository
    {
        private readonly DBHelper _db;

        public FleetRepository(DBHelper db)
        {
            _db = db;
        }

        public async Task<IEnumerable<FleetDTO>> GetAllAsync(
            string? searchText,
            int? categoryId,
            int? depotId,
            string? fleetStatus,
            bool? isActive,
            int? scopeToUser,
            int pageNumber = 1,
            int pageSize = 10)
        {
            using var conn = _db.CreateConnection();

            var dbResult = await conn.QueryAsync<FleetDbDTO>(
                "usp_Fleet_GetAll",
                new
                {
                    SearchText = searchText,
                    CategoryId = categoryId,
                    DepotId = depotId,
                    FleetStatus = fleetStatus,
                    IsActive = isActive,
                    ScopeToUser = scopeToUser,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                },
                commandType: CommandType.StoredProcedure);

            return dbResult.Select(x => new FleetDTO
            {
                fleetId = x.Fleet_Id?.ToString(),
                vehicleNumber = x.Vehicle_Number,
                seriesType = x.Series_Type,
                fleetStatus = x.Fleet_Status,
                isActive = x.IsActive,
                createdBy = x.Created_By,
                createdDate = x.Created_Date,
                modifiedBy = x.Modified_By,
                modifiedDate = x.Modified_Date,
                categoryId = x.Category_Id?.ToString(),
                categoryCode = x.Category_Code,
                categoryName = x.Category_Name,
                depotId = x.Depot_Id?.ToString(),
                depotCode = x.Depot_Code,
                depotName = x.Depot_Name,
                docExpiry = !string.IsNullOrEmpty(x.Doc_Expiry)
                    ? JsonConvert.DeserializeObject<List<FleetDocumentDTO>>(x.Doc_Expiry)
                    : new List<FleetDocumentDTO>(),
                totalCount = x.TotalCount
            });
        }

        public async Task<FleetDTO> GetByIdAsync(int fleetId)
        {
            using var conn = _db.CreateConnection();

            var dbResult = await conn.QueryFirstOrDefaultAsync<FleetDbDTO>(
                "usp_Fleet_GetById",
                new { Fleet_Id = fleetId },
                commandType: CommandType.StoredProcedure);

            if (dbResult == null)
                return null;

            return new FleetDTO
            {
                fleetId = dbResult.Fleet_Id?.ToString(),
                vehicleNumber = dbResult.Vehicle_Number,
                seriesType = dbResult.Series_Type,
                fleetStatus = dbResult.Fleet_Status,
                isActive = dbResult.IsActive,
                createdBy = dbResult.Created_By,
                createdDate = dbResult.Created_Date,
                modifiedBy = dbResult.Modified_By,
                modifiedDate = dbResult.Modified_Date,
                categoryId = dbResult.Category_Id?.ToString(),
                categoryCode = dbResult.Category_Code,
                categoryName = dbResult.Category_Name,
                depotId = dbResult.Depot_Id?.ToString(),
                depotCode = dbResult.Depot_Code,
                depotName = dbResult.Depot_Name,
                docExpiry = !string.IsNullOrEmpty(dbResult.Doc_Expiry)
                    ? JsonConvert.DeserializeObject<List<FleetDocumentDTO>>(dbResult.Doc_Expiry)
                    : new List<FleetDocumentDTO>(),
                totalCount = 0
            };
        }

        public async Task<int> InsertAsync(FleetDTO entity, int userId)
        {
            try
            {
                using var conn = _db.CreateConnection();

                // Convert documents to JSON
                string? docExpiryJson = null;
                if (entity.docExpiry != null && entity.docExpiry.Count > 0)
                {
                    docExpiryJson = JsonConvert.SerializeObject(entity.docExpiry);
                }

                var p = new DynamicParameters();

                p.Add("@Vehicle_Number", entity.vehicleNumber);
                p.Add("@Category_Id", Convert.ToInt32(entity.categoryId));
                p.Add("@Series_Type", entity.seriesType);
                p.Add("@Depot_Id", Convert.ToInt32(entity.depotId));
                p.Add("@Fleet_Status", entity.fleetStatus ?? "Available"); 
                p.Add("@Doc_Expiry", docExpiryJson);
                p.Add("@IsActive", entity.isActive);
                p.Add("@UserId", userId);
                p.Add("@NewId", dbType: DbType.Int32, direction: ParameterDirection.Output);

                await conn.ExecuteAsync(
                    "usp_Fleet_Insert",
                    p,
                    commandType: CommandType.StoredProcedure);

                return p.Get<int>("@NewId");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<bool> UpdateAsync(FleetDTO entity, int userId)
        {
            try
            {
                using var conn = _db.CreateConnection();

                // Convert documents to JSON
                string? docExpiryJson = null;
                if (entity.docExpiry != null && entity.docExpiry.Count > 0)
                {
                    docExpiryJson = JsonConvert.SerializeObject(entity.docExpiry);
                }

                var p = new DynamicParameters();

                p.Add("@Fleet_Id", Convert.ToInt32(entity.fleetId));
                p.Add("@Vehicle_Number", entity.vehicleNumber);
                p.Add("@Category_Id", Convert.ToInt32(entity.categoryId));
                p.Add("@Series_Type", entity.seriesType);
                p.Add("@Depot_Id", Convert.ToInt32(entity.depotId));
                p.Add("@Fleet_Status", entity.fleetStatus);  
                p.Add("@Doc_Expiry", docExpiryJson);
                p.Add("@IsActive", entity.isActive);
                p.Add("@UserId", userId);
                p.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);

                await conn.ExecuteAsync(
                    "usp_Fleet_Update",
                    p,
                    commandType: CommandType.StoredProcedure);

                return p.Get<bool>("@Success");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<bool> DeleteAsync(int fleetId, int deletedBy)
        {
            try
            {
                using var conn = _db.CreateConnection();

                var p = new DynamicParameters();

                p.Add("@Fleet_Id", fleetId);
                p.Add("@DeletedBy", deletedBy);
                p.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);

                await conn.ExecuteAsync(
                    "usp_Fleet_Delete",
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