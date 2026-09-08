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
    public class TripRepository : ITripRepository
    {
        private readonly DBHelper _db;

        public TripRepository(DBHelper db)
        {
            _db = db;
        }

        public async Task<IEnumerable<TripDTO>> GetAllAsync(
            string? searchText,
            int? depotId,
            int? routeId,
            int? fleetId,
            string? tripStatus,
            DateTime? startDate,
            DateTime? endDate,
            bool? isActive,
            int pageNumber = 1,
            int pageSize = 10)
        {
            using var conn = _db.CreateConnection();

            var dbResult = await conn.QueryAsync<TripDbDTO>(
                "usp_Trip_GetAll",
                new
                {
                    SearchText = searchText,
                    DepotId = depotId,
                    RouteId = routeId,
                    FleetId = fleetId,
                    TripStatus = tripStatus,
                    StartDate = startDate,
                    EndDate = endDate,
                    IsActive = isActive,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                },
                commandType: CommandType.StoredProcedure);

            return dbResult.Select(x => new TripDTO
            {
                tripId = x.Trip_Id?.ToString(),
                tripCode = x.Trip_Code,
                // Depot details
                depotId = x.Depot_Id?.ToString(),
                depotCode = x.Depot_Code,
                depotName = x.Depot_Name,
                // Route details
                routeId = x.Route_Id?.ToString(),
                routeCode = x.Route_Code,
                routeName = x.Route_Name,
                // Fleet details
                fleetId = x.Fleet_Id?.ToString(),
                vehicleNumber = x.Vehicle_Number,
                fleetStatus = x.Fleet_Status,
                // Driver details
                driverId = x.UserId_Driver?.ToString(),
                driverName = x.DriverName,
                // Conductor details
                conductorId = x.UserId_Conductor?.ToString(),
                conductorName = x.ConductorName,
                scheduleTime = x.Schedule_Time?.ToString("yyyy-MM-dd HH:mm:ss"),
                actualTime = x.Actual_Time?.ToString("yyyy-MM-dd HH:mm:ss"),
                tripStatus = x.Trip_Status,
                isActive = x.IsActive,
                createdBy = x.Created_By,
                createdDate = x.Created_Date,
                modifiedBy = x.Modified_By,
                modifiedDate = x.Modified_Date,
                totalCount = x.TotalCount
            });
        }

        public async Task<TripDTO> GetByIdAsync(int tripId)
        {
            using var conn = _db.CreateConnection();

            var dbResult = await conn.QueryFirstOrDefaultAsync<TripDbDTO>(
                "usp_Trip_GetById",
                new { Trip_Id = tripId },
                commandType: CommandType.StoredProcedure);

            if (dbResult == null)
                return null;

            return new TripDTO
            {
                tripId = dbResult.Trip_Id?.ToString(),
                tripCode = dbResult.Trip_Code,
                // Depot details
                depotId = dbResult.Depot_Id?.ToString(),
                depotCode = dbResult.Depot_Code,
                depotName = dbResult.Depot_Name,
                // Route details
                routeId = dbResult.Route_Id?.ToString(),
                routeCode = dbResult.Route_Code,
                routeName = dbResult.Route_Name,
                // Fleet details
                fleetId = dbResult.Fleet_Id?.ToString(),
                vehicleNumber = dbResult.Vehicle_Number,
                fleetStatus = dbResult.Fleet_Status,
                // Driver details
                driverId = dbResult.UserId_Driver?.ToString(),
                driverName = dbResult.DriverName,
                // Conductor details
                conductorId = dbResult.UserId_Conductor?.ToString(),
                conductorName = dbResult.ConductorName,
                scheduleTime = dbResult.Schedule_Time?.ToString("yyyy-MM-dd HH:mm:ss"),
                actualTime = dbResult.Actual_Time?.ToString("yyyy-MM-dd HH:mm:ss"),
                tripStatus = dbResult.Trip_Status,
                isActive = dbResult.IsActive,
                createdBy = dbResult.Created_By,
                createdDate = dbResult.Created_Date,
                modifiedBy = dbResult.Modified_By,
                modifiedDate = dbResult.Modified_Date,
                totalCount = 0
            };
        }

        public async Task<int> InsertAsync(TripDTO entity, int userId)
        {
            try
            {
                using var conn = _db.CreateConnection();

                var p = new DynamicParameters();

                // Parse DateTime from string
                DateTime? scheduleTime = null;
                if (!string.IsNullOrEmpty(entity.scheduleTime))
                {
                    scheduleTime = DateTime.Parse(entity.scheduleTime);
                }

                DateTime? actualTime = null;
                if (!string.IsNullOrEmpty(entity.actualTime))
                {
                    actualTime = DateTime.Parse(entity.actualTime);
                }

                p.Add("@Depot_Id", Convert.ToInt32(entity.depotId));
                p.Add("@Route_Id", Convert.ToInt32(entity.routeId));
                p.Add("@Fleet_Id", Convert.ToInt32(entity.fleetId));
                p.Add("@UserId_Driver", Convert.ToInt32(entity.driverId));
                p.Add("@UserId_Conductor", Convert.ToInt32(entity.conductorId));
                p.Add("@Schedule_Time", scheduleTime);
                p.Add("@Actual_Time", actualTime);
                p.Add("@Trip_Status", entity.tripStatus ?? "Scheduled");
                p.Add("@IsActive", entity.isActive);
                p.Add("@UserId", userId);
                p.Add("@NewId", dbType: DbType.Int32, direction: ParameterDirection.Output);

                await conn.ExecuteAsync(
                    "usp_Trip_Insert",
                    p,
                    commandType: CommandType.StoredProcedure);

                return p.Get<int>("@NewId");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<bool> UpdateAsync(TripDTO entity, int userId)
        {
            try
            {
                using var conn = _db.CreateConnection();

                var p = new DynamicParameters();

                // Parse DateTime from string
                DateTime? scheduleTime = null;
                if (!string.IsNullOrEmpty(entity.scheduleTime))
                {
                    scheduleTime = DateTime.Parse(entity.scheduleTime);
                }

                DateTime? actualTime = null;
                if (!string.IsNullOrEmpty(entity.actualTime))
                {
                    actualTime = DateTime.Parse(entity.actualTime);
                }

                p.Add("@Trip_Id", Convert.ToInt32(entity.tripId));
                p.Add("@Depot_Id", Convert.ToInt32(entity.depotId));
                p.Add("@Route_Id", Convert.ToInt32(entity.routeId));
                p.Add("@Fleet_Id", Convert.ToInt32(entity.fleetId));
                p.Add("@UserId_Driver", Convert.ToInt32(entity.driverId));
                p.Add("@UserId_Conductor", Convert.ToInt32(entity.conductorId));
                p.Add("@Schedule_Time", scheduleTime);
                p.Add("@Actual_Time", actualTime);
                p.Add("@Trip_Status", entity.tripStatus);
                p.Add("@IsActive", entity.isActive);
                p.Add("@UserId", userId);
                p.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);

                await conn.ExecuteAsync(
                    "usp_Trip_Update",
                    p,
                    commandType: CommandType.StoredProcedure);

                return p.Get<bool>("@Success");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<bool> DeleteAsync(int tripId, int deletedBy)
        {
            try
            {
                using var conn = _db.CreateConnection();

                var p = new DynamicParameters();

                p.Add("@Trip_Id", tripId);
                p.Add("@DeletedBy", deletedBy);
                p.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);

                await conn.ExecuteAsync(
                    "usp_Trip_Delete",
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