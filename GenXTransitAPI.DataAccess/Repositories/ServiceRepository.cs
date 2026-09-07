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
    public class ServiceRepository : IServiceRepository
    {
        private readonly DBHelper _db;

        public ServiceRepository(DBHelper db)
        {
            _db = db;
        }

        public async Task<IEnumerable<ServiceDTO>> GetAllAsync(
            string? searchText,
            int? fleetId,
            string? status,
            bool? isActive,
            int? scopeToUser,
            int pageNumber = 1,
            int pageSize = 10)
        {
            using var conn = _db.CreateConnection();

            var dbResult = await conn.QueryAsync<ServiceDbDTO>(
                "usp_Service_GetAll",
                new
                {
                    SearchText = searchText,
                    FleetId = fleetId,
                    Status = status,
                    IsActive = isActive,
                    ScopeToUser = scopeToUser,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                },
                commandType: CommandType.StoredProcedure);

            return dbResult.Select(x => new ServiceDTO
            {
                serviceId = x.Service_Id?.ToString(),
                serviceCode = x.Service_Code,
                fleetId = x.Fleet_Id?.ToString(),
                vehicleNumber = x.Vehicle_Number,
                fleetStatus = x.Fleet_Status,
                status = x.Status,
                isActive = x.IsActive,
                createdBy = x.Created_By,
                createdDate = x.Created_Date,
                modifiedBy = x.Modified_By,
                modifiedDate = x.Modified_Date,
                totalCount = x.TotalCount
            });
        }

        public async Task<ServiceDTO> GetByIdAsync(int serviceId)
        {
            using var conn = _db.CreateConnection();

            var dbResult = await conn.QueryFirstOrDefaultAsync<ServiceDbDTO>(
                "usp_Service_GetById",
                new { Service_Id = serviceId },
                commandType: CommandType.StoredProcedure);

            if (dbResult == null)
                return null;

            return new ServiceDTO
            {
                serviceId = dbResult.Service_Id?.ToString(),
                serviceCode = dbResult.Service_Code,
                fleetId = dbResult.Fleet_Id?.ToString(),
                vehicleNumber = dbResult.Vehicle_Number,
                fleetStatus = dbResult.Fleet_Status,
                status = dbResult.Status,
                isActive = dbResult.IsActive,
                createdBy = dbResult.Created_By,
                createdDate = dbResult.Created_Date,
                modifiedBy = dbResult.Modified_By,
                modifiedDate = dbResult.Modified_Date,
                totalCount = 0
            };
        }

        public async Task<int> InsertAsync(ServiceDTO entity, int userId)
        {
            try
            {
                using var conn = _db.CreateConnection();

                var p = new DynamicParameters();

                p.Add("@Fleet_Id", Convert.ToInt32(entity.fleetId));
                p.Add("@Status", entity.status ?? "Pending");
                p.Add("@IsActive", entity.isActive);
                p.Add("@UserId", userId);
                p.Add("@NewId", dbType: DbType.Int32, direction: ParameterDirection.Output);

                await conn.ExecuteAsync(
                    "usp_Service_Insert",
                    p,
                    commandType: CommandType.StoredProcedure);

                return p.Get<int>("@NewId");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<bool> UpdateAsync(ServiceDTO entity, int userId)
        {
            try
            {
                using var conn = _db.CreateConnection();

                var p = new DynamicParameters();

                p.Add("@Service_Id", Convert.ToInt32(entity.serviceId));
                p.Add("@Fleet_Id", Convert.ToInt32(entity.fleetId));
                p.Add("@Status", entity.status);
                p.Add("@IsActive", entity.isActive);
                p.Add("@UserId", userId);
                p.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);

                await conn.ExecuteAsync(
                    "usp_Service_Update",
                    p,
                    commandType: CommandType.StoredProcedure);

                return p.Get<bool>("@Success");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<bool> DeleteAsync(int serviceId, int deletedBy)
        {
            try
            {
                using var conn = _db.CreateConnection();

                var p = new DynamicParameters();

                p.Add("@Service_Id", serviceId);
                p.Add("@DeletedBy", deletedBy);
                p.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);

                await conn.ExecuteAsync(
                    "usp_Service_Delete",
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