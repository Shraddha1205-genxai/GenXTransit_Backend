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
    public class RouteRepository : IRouteRepository
    {
        private readonly DBHelper _db;

        public RouteRepository(DBHelper db)
        {
            _db = db;
        }

        public async Task<IEnumerable<RouteDTO>> GetAllAsync(
            string? searchText,
            string? service,
            string? type,
            bool? isActive,
            int? scopeToUser,
            int pageNumber = 1,
            int pageSize = 10)
        {
            using var conn = _db.CreateConnection();

            var dbResult = await conn.QueryAsync<RouteDbDTO>(
                "usp_Route_GetAll",
                new
                {
                    SearchText = searchText,
                    Service = service,
                    Type = type,
                    IsActive = isActive,
                    ScopeToUser = scopeToUser,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                },
                commandType: CommandType.StoredProcedure);

            return dbResult.Select(x => new RouteDTO
            {
                routeId = x.Route_Id?.ToString(),
                routeCode = x.Route_Code,
                routeName = x.Route_Name,
                service = x.Service,
                type = x.Type,
                distance = x.Distance,
                fareModel = x.Fare_Model,
                duration = x.Duration,
                isActive = x.IsActive,
                createdBy = x.Created_By,
                createdDate = x.Created_Date,
                modifiedBy = x.Modified_By,
                modifiedDate = x.Modified_Date,
                // From Station details
                fromStationId = x.FromStationId?.ToString(),
                fromStationCode = x.FromStationCode,
                fromStationName = x.FromStationName,
                // To Station details
                toStationId = x.ToStationId?.ToString(),
                toStationCode = x.ToStationCode,
                toStationName = x.ToStationName,
                // Region details
                regionId = x.RegionId?.ToString(),
                regionCode = x.RegionCode,
                regionName = x.RegionName,
                totalCount = x.TotalCount
            });
        }

        public async Task<RouteDTO> GetByIdAsync(int routeId)
        {
            using var conn = _db.CreateConnection();

            var dbResult = await conn.QueryFirstOrDefaultAsync<RouteDbDTO>(
                "usp_Route_GetById",
                new { Route_Id = routeId },
                commandType: CommandType.StoredProcedure);

            if (dbResult == null)
                return null;

            return new RouteDTO
            {
                routeId = dbResult.Route_Id?.ToString(),
                routeCode = dbResult.Route_Code,
                routeName = dbResult.Route_Name,
                service = dbResult.Service,
                type = dbResult.Type,
                distance = dbResult.Distance,
                fareModel = dbResult.Fare_Model,
                duration = dbResult.Duration,
                isActive = dbResult.IsActive,
                createdBy = dbResult.Created_By,
                createdDate = dbResult.Created_Date,
                modifiedBy = dbResult.Modified_By,
                modifiedDate = dbResult.Modified_Date,
                // From Station details
                fromStationId = dbResult.FromStationId?.ToString(),
                fromStationCode = dbResult.FromStationCode,
                fromStationName = dbResult.FromStationName,
                // To Station details
                toStationId = dbResult.ToStationId?.ToString(),
                toStationCode = dbResult.ToStationCode,
                toStationName = dbResult.ToStationName,
                // Region details
                regionId = dbResult.RegionId?.ToString(),
                regionCode = dbResult.RegionCode,
                regionName = dbResult.RegionName,
                totalCount = 0
            };
        }

        public async Task<string> GetNextCodeAsync()
        {
            using var conn = _db.CreateConnection();

            var p = new DynamicParameters();
            p.Add("@NextCode", dbType: DbType.String, direction: ParameterDirection.Output, size: 50);

            await conn.ExecuteAsync(
                "usp_Route_GetNextCode",
                p,
                commandType: CommandType.StoredProcedure);

            return p.Get<string>("@NextCode");
        }

        public async Task<int> InsertAsync(RouteDTO entity, int userId)
        {
            try
            {
                using var conn = _db.CreateConnection();

                var p = new DynamicParameters();

                p.Add("@Route_Name", entity.routeName);
                p.Add("@Service", entity.service);
                p.Add("@From_Location", Convert.ToInt32(entity.fromStationId));
                p.Add("@To_Location", Convert.ToInt32(entity.toStationId));
                p.Add("@Type", entity.type);
                p.Add("@Distance", entity.distance);
                p.Add("@Fare_Model", entity.fareModel);
                p.Add("@Duration", entity.duration);
                p.Add("@IsActive", entity.isActive);
                p.Add("@UserId", userId);
                p.Add("@NewId", dbType: DbType.Int32, direction: ParameterDirection.Output);

                await conn.ExecuteAsync(
                    "usp_Route_Insert",
                    p,
                    commandType: CommandType.StoredProcedure);

                return p.Get<int>("@NewId");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<bool> UpdateAsync(RouteDTO entity, int userId)
        {
            try
            {
                using var conn = _db.CreateConnection();

                var p = new DynamicParameters();

                p.Add("@Route_Id", Convert.ToInt32(entity.routeId));
                p.Add("@Route_Name", entity.routeName);
                p.Add("@Service", entity.service);
                p.Add("@From_Location", Convert.ToInt32(entity.fromStationId));
                p.Add("@To_Location", Convert.ToInt32(entity.toStationId));
                p.Add("@Type", entity.type);
                p.Add("@Distance", entity.distance);
                p.Add("@Fare_Model", entity.fareModel);
                p.Add("@Duration", entity.duration);
                p.Add("@IsActive", entity.isActive);
                p.Add("@UserId", userId);
                p.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);

                await conn.ExecuteAsync(
                    "usp_Route_Update",
                    p,
                    commandType: CommandType.StoredProcedure);

                return p.Get<bool>("@Success");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<bool> DeleteAsync(int routeId, int deletedBy)
        {
            try
            {
                using var conn = _db.CreateConnection();

                var p = new DynamicParameters();

                p.Add("@Route_Id", routeId);
                p.Add("@DeletedBy", deletedBy);
                p.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);

                await conn.ExecuteAsync(
                    "usp_Route_Delete",
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