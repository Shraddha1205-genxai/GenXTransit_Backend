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
    public class SeatLayoutRepository : ISeatLayoutRepository
    {
        private readonly DBHelper _db;

        public SeatLayoutRepository(DBHelper db)
        {
            _db = db;
        }

        public async Task<IEnumerable<SeatLayoutDTO>> GetAllAsync(
            string? searchText,
            int? categoryId,
            bool? isActive,
            int? scopeToUser,
            int pageNumber = 1,
            int pageSize = 10)
        {
            using var conn = _db.CreateConnection();

            var dbResult = await conn.QueryAsync<SeatLayoutDbDTO>(
                "usp_SeatLayout_GetAll",
                new
                {
                    SearchText = searchText,
                    CategoryId = categoryId,
                    IsActive = isActive,
                    ScopeToUser = scopeToUser,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                },
                commandType: CommandType.StoredProcedure);

            return dbResult.Select(x => new SeatLayoutDTO
            {
                layoutId = x.Layout_Id?.ToString(),
                layoutCode = x.Layout_Code,
                description = x.Description,
                isActive = x.IsActive,
                createdBy = x.Created_By,
                createdDate = x.Created_Date,
                modifiedBy = x.Modified_By,
                modifiedDate = x.Modified_Date,
                // Category details
                categoryId = x.Category_Id?.ToString(),
                categoryCode = x.Category_Code,
                categoryName = x.Category_Name,
                totalCount = x.TotalCount
            });
        }

        public async Task<SeatLayoutDTO> GetByIdAsync(int layoutId)
        {
            using var conn = _db.CreateConnection();

            var dbResult = await conn.QueryFirstOrDefaultAsync<SeatLayoutDbDTO>(
                "usp_SeatLayout_GetById",
                new { Layout_Id = layoutId },
                commandType: CommandType.StoredProcedure);

            if (dbResult == null)
                return null;

            return new SeatLayoutDTO
            {
                layoutId = dbResult.Layout_Id?.ToString(),
                layoutCode = dbResult.Layout_Code,
                description = dbResult.Description,
                isActive = dbResult.IsActive,
                createdBy = dbResult.Created_By,
                createdDate = dbResult.Created_Date,
                modifiedBy = dbResult.Modified_By,
                modifiedDate = dbResult.Modified_Date,
                // Category details
                categoryId = dbResult.Category_Id?.ToString(),
                categoryCode = dbResult.Category_Code,
                categoryName = dbResult.Category_Name,
                totalCount = 0
            };
        }

        public async Task<string> GetNextCodeAsync()
        {
            using var conn = _db.CreateConnection();

            var p = new DynamicParameters();
            p.Add("@NextCode", dbType: DbType.String, direction: ParameterDirection.Output, size: 50);

            await conn.ExecuteAsync(
                "usp_SeatLayout_GetNextCode",
                p,
                commandType: CommandType.StoredProcedure);

            return p.Get<string>("@NextCode");
        }

        public async Task<int> InsertAsync(SeatLayoutDTO entity, int userId)
        {
            try
            {
                using var conn = _db.CreateConnection();

                var p = new DynamicParameters();

                p.Add("@Description", entity.description);
                p.Add("@Category_Id", Convert.ToInt32(entity.categoryId));
                p.Add("@IsActive", entity.isActive);
                p.Add("@UserId", userId);
                p.Add("@NewId", dbType: DbType.Int32, direction: ParameterDirection.Output);

                await conn.ExecuteAsync(
                    "usp_SeatLayout_Insert",
                    p,
                    commandType: CommandType.StoredProcedure);

                return p.Get<int>("@NewId");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while inserting seat layout: {ex.Message}", ex);
            }
        }

        public async Task<bool> UpdateAsync(SeatLayoutDTO entity, int userId)
        {
            try
            {
                using var conn = _db.CreateConnection();

                var p = new DynamicParameters();

                p.Add("@Layout_Id", Convert.ToInt32(entity.layoutId));
                p.Add("@Description", entity.description);
                p.Add("@Category_Id", Convert.ToInt32(entity.categoryId));
                p.Add("@IsActive", entity.isActive);
                p.Add("@UserId", userId);
                p.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);

                await conn.ExecuteAsync(
                    "usp_SeatLayout_Update",
                    p,
                    commandType: CommandType.StoredProcedure);

                return p.Get<bool>("@Success");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while updating seat layout: {ex.Message}", ex);
            }
        }

        public async Task<bool> DeleteAsync(int layoutId, int deletedBy)
        {
            try
            {
                using var conn = _db.CreateConnection();

                var p = new DynamicParameters();

                p.Add("@Layout_Id", layoutId);
                p.Add("@DeletedBy", deletedBy);
                p.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);

                await conn.ExecuteAsync(
                    "usp_SeatLayout_Delete",
                    p,
                    commandType: CommandType.StoredProcedure);

                return p.Get<bool>("@Success");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while deleting seat layout: {ex.Message}", ex);
            }
        }
    }
}