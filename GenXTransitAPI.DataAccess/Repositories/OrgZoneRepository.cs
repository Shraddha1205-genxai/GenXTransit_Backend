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
    public class OrgZoneRepository : IOrgZoneRepository
    {
        private readonly DBHelper _db;

        public OrgZoneRepository(DBHelper db)
        {
            _db = db;
        }

        public async Task<IEnumerable<OrgZoneDTO>> GetAllAsync(
            string? searchText,
            int? regionId,
            bool? isActive,
            int? scopeToUser,
            int pageNumber = 1,
            int pageSize = 10)
        {
            using var conn = _db.CreateConnection();

            var dbResult = await conn.QueryAsync<OrgZoneDbDTO>(
                "usp_Zone_GetAll",
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

            // ✅ Convert from DB format to UI format (camelCase)
            return dbResult.Select(x => new OrgZoneDTO
            {
                zoneId = x.Zone_ID?.ToString(),
                zoneCode = x.Zone_Code,
                zoneName = x.Zone_Name,
                regionId = x.Region_ID?.ToString(),
                regionCode = x.Region_Code,
                regionName = x.Region_Name,
                // ✅ Convert comma-separated string to List<string>
                districts = !string.IsNullOrEmpty(x.Districts)
                    ? x.Districts.Split(',').Select(d => d.Trim()).ToList()
                    : new List<string>(),
                isActive = x.IsActive,
                createdBy = x.Created_By,
                createdDate = x.Created_Date,
                modifiedBy = x.Modified_By,
                modifiedDate = x.Modified_Date,
                isDeleted = x.IsDeleted,
                deletedBy = x.Deleted_By,
                deletedDate = x.Deleted_Date,
                TotalCount = x.TotalCount
            });
        }

        public async Task<OrgZoneDTO> GetByIdAsync(int zoneId)
        {
            using var conn = _db.CreateConnection();

            var dbResult = await conn.QueryFirstOrDefaultAsync<OrgZoneDbDTO>(
                "usp_Zone_GetById",
                new { Zone_ID = zoneId },
                commandType: CommandType.StoredProcedure);

            if (dbResult == null)
                return null;

            // ✅ Convert from DB format to UI format (camelCase)
            return new OrgZoneDTO
            {
                zoneId = dbResult.Zone_ID?.ToString(),
                zoneCode = dbResult.Zone_Code,
                zoneName = dbResult.Zone_Name,
                regionId = dbResult.Region_ID?.ToString(),
                regionCode = dbResult.Region_Code,
                regionName = dbResult.Region_Name,
                // ✅ Convert comma-separated string to List<string>
                districts = !string.IsNullOrEmpty(dbResult.Districts)
                    ? dbResult.Districts.Split(',').Select(d => d.Trim()).ToList()
                    : new List<string>(),
                isActive = dbResult.IsActive,
                createdBy = dbResult.Created_By,
                createdDate = dbResult.Created_Date,
                modifiedBy = dbResult.Modified_By,
                modifiedDate = dbResult.Modified_Date,
                isDeleted = dbResult.IsDeleted,
                deletedBy = dbResult.Deleted_By,
                deletedDate = dbResult.Deleted_Date,
                TotalCount = dbResult.TotalCount
            };
        }

        public async Task<string> GetNextCodeAsync()
        {
            using var conn = _db.CreateConnection();

            var p = new DynamicParameters();
            p.Add("@NextCode", dbType: DbType.String, direction: ParameterDirection.Output, size: 50);

            await conn.ExecuteAsync(
                "usp_Zone_GetNextCode",
                p,
                commandType: CommandType.StoredProcedure);

            return p.Get<string>("@NextCode");
        }

        public async Task<int> InsertAsync(OrgZoneDTO entity, int userId)
        {
            try
            {
                using var conn = _db.CreateConnection();

                var p = new DynamicParameters();

                // ✅ Convert List<string> to comma-separated string for DB
                string? districts = entity.districts != null && entity.districts.Count > 0
                    ? string.Join(",", entity.districts)
                    : null;

                p.Add("@Zone_Name", entity.zoneName);
                p.Add("@Region_ID", Convert.ToInt32(entity.regionId));
                p.Add("@Districts", districts);
                p.Add("@IsActive", entity.isActive);
                p.Add("@UserId", userId);
                p.Add("@NewId", dbType: DbType.Int32, direction: ParameterDirection.Output);

                await conn.ExecuteAsync(
                    "usp_Zone_Insert",
                    p,
                    commandType: CommandType.StoredProcedure);

                return p.Get<int>("@NewId");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while inserting zone: {ex.Message}", ex);
            }
        }

        public async Task<bool> UpdateAsync(OrgZoneDTO entity, int userId)
        {
            try
            {
                using var conn = _db.CreateConnection();

                var p = new DynamicParameters();

                // ✅ Convert List<string> to comma-separated string for DB
                string? districts = entity.districts != null && entity.districts.Count > 0
                    ? string.Join(",", entity.districts)
                    : null;

                p.Add("@Zone_ID", Convert.ToInt32(entity.zoneId));
                p.Add("@Zone_Name", entity.zoneName);
                p.Add("@Region_ID", Convert.ToInt32(entity.regionId));
                p.Add("@Districts", districts);
                p.Add("@IsActive", entity.isActive);
                p.Add("@UserId", userId);
                p.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);

                await conn.ExecuteAsync(
                    "usp_Zone_Update",
                    p,
                    commandType: CommandType.StoredProcedure);

                return p.Get<bool>("@Success");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while updating zone: {ex.Message}", ex);
            }
        }

        public async Task<bool> DeleteAsync(int zoneId, int deletedBy)
        {
            try
            {
                using var conn = _db.CreateConnection();

                var p = new DynamicParameters();

                p.Add("@Zone_ID", zoneId);
                p.Add("@DeletedBy", deletedBy);
                p.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);

                await conn.ExecuteAsync(
                    "usp_Zone_Delete",
                    p,
                    commandType: CommandType.StoredProcedure);

                return p.Get<bool>("@Success");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while deleting zone: {ex.Message}", ex);
            }
        }
    }
}