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
    public class HolidayRepository : IHolidayRepository
    {
        private readonly DBHelper _db;

        public HolidayRepository(DBHelper db)
        {
            _db = db;
        }

        public async Task<IEnumerable<HolidayDTO>> GetAllAsync(
            string? searchText,
            string? type,
            DateTime? startDate,
            DateTime? endDate,
            bool? isActive,
            int? scopeToUser,
            int pageNumber = 1,
            int pageSize = 10)
        {
            using var conn = _db.CreateConnection();

            var dbResult = await conn.QueryAsync<HolidayDbDTO>(
                "usp_Holiday_GetAll",
                new
                {
                    SearchText = searchText,
                    Type = type,
                    StartDate = startDate,
                    EndDate = endDate,
                    IsActive = isActive,
                    ScopeToUser = scopeToUser,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                },
                commandType: CommandType.StoredProcedure);

            return dbResult.Select(x => new HolidayDTO
            {
                holidayId = x.Holiday_Id?.ToString(),
                holidayCode = x.Holiday_Code,
                holidayName = x.Holiday_Name,
                occasion = x.Occasion,
                date = x.Date?.ToString("yyyy-MM-dd"),
                description = x.Description,
                type = x.Type,
                isActive = x.IsActive,
                createdBy = x.Created_By,
                createdDate = x.Created_Date,
                modifiedBy = x.Modified_By,
                modifiedDate = x.Modified_Date,
                totalCount = x.TotalCount
            });
        }

        public async Task<HolidayDTO> GetByIdAsync(int holidayId)
        {
            using var conn = _db.CreateConnection();

            var dbResult = await conn.QueryFirstOrDefaultAsync<HolidayDbDTO>(
                "usp_Holiday_GetById",
                new { Holiday_Id = holidayId },
                commandType: CommandType.StoredProcedure);

            if (dbResult == null)
                return null;

            return new HolidayDTO
            {
                holidayId = dbResult.Holiday_Id?.ToString(),
                holidayCode = dbResult.Holiday_Code,
                holidayName = dbResult.Holiday_Name,
                occasion = dbResult.Occasion,
                date = dbResult.Date?.ToString("yyyy-MM-dd"),
                description = dbResult.Description,
                type = dbResult.Type,
                isActive = dbResult.IsActive,
                createdBy = dbResult.Created_By,
                createdDate = dbResult.Created_Date,
                modifiedBy = dbResult.Modified_By,
                modifiedDate = dbResult.Modified_Date,
                totalCount = 0
            };
        }

        public async Task<string> GetNextCodeAsync()
        {
            using var conn = _db.CreateConnection();

            var p = new DynamicParameters();
            p.Add("@NextCode", dbType: DbType.String, direction: ParameterDirection.Output, size: 50);

            await conn.ExecuteAsync(
                "usp_Holiday_GetNextCode",
                p,
                commandType: CommandType.StoredProcedure);

            return p.Get<string>("@NextCode");
        }

        public async Task<int> InsertAsync(HolidayDTO entity, int userId)
        {
            try
            {
                using var conn = _db.CreateConnection();

                var p = new DynamicParameters();

                p.Add("@Holiday_Name", entity.holidayName);
                p.Add("@Occasion", entity.occasion);
                p.Add("@Date", DateTime.Parse(entity.date));
                p.Add("@Description", entity.description);
                p.Add("@Type", entity.type);
                p.Add("@IsActive", entity.isActive);
                p.Add("@UserId", userId);
                p.Add("@NewId", dbType: DbType.Int32, direction: ParameterDirection.Output);

                await conn.ExecuteAsync(
                    "usp_Holiday_Insert",
                    p,
                    commandType: CommandType.StoredProcedure);

                return p.Get<int>("@NewId");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while inserting holiday: {ex.Message}", ex);
            }
        }

        public async Task<bool> UpdateAsync(HolidayDTO entity, int userId)
        {
            try
            {
                using var conn = _db.CreateConnection();

                var p = new DynamicParameters();

                p.Add("@Holiday_Id", Convert.ToInt32(entity.holidayId));
                p.Add("@Holiday_Name", entity.holidayName);
                p.Add("@Occasion", entity.occasion);
                p.Add("@Date", DateTime.Parse(entity.date));
                p.Add("@Description", entity.description);
                p.Add("@Type", entity.type);
                p.Add("@IsActive", entity.isActive);
                p.Add("@UserId", userId);
                p.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);

                await conn.ExecuteAsync(
                    "usp_Holiday_Update",
                    p,
                    commandType: CommandType.StoredProcedure);

                return p.Get<bool>("@Success");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while updating holiday: {ex.Message}", ex);
            }
        }

        public async Task<bool> DeleteAsync(int holidayId, int deletedBy)
        {
            try
            {
                using var conn = _db.CreateConnection();

                var p = new DynamicParameters();

                p.Add("@Holiday_Id", holidayId);
                p.Add("@DeletedBy", deletedBy);
                p.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);

                await conn.ExecuteAsync(
                    "usp_Holiday_Delete",
                    p,
                    commandType: CommandType.StoredProcedure);

                return p.Get<bool>("@Success");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while deleting holiday: {ex.Message}", ex);
            }
        }
    }
}