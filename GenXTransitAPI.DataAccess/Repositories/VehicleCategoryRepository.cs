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
    public class VehicleCategoryRepository : IVehicleCategoryRepository
    {
        private readonly DBHelper _db;

        public VehicleCategoryRepository(DBHelper db)
        {
            _db = db;
        }

        public async Task<IEnumerable<VehicleCategoryDTO>> GetAllAsync(
            string? searchText,
            string? type,
            string? vehicleClass,
            bool? isActive,
            int? scopeToUser,
            int pageNumber = 1,
            int pageSize = 10)
        {
            using var conn = _db.CreateConnection();

            var dbResult = await conn.QueryAsync<VehicleCategoryDbDTO>(
                "usp_VehicleCategory_GetAll",
                new
                {
                    SearchText = searchText,
                    Type = type,
                    VehicleClass = vehicleClass,
                    IsActive = isActive,
                    ScopeToUser = scopeToUser,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                },
                commandType: CommandType.StoredProcedure);

            return dbResult.Select(x => new VehicleCategoryDTO
            {
                categoryId = x.Category_Id?.ToString(),
                categoryCode = x.Category_Code,
                categoryName = x.Category_Name,
                capacity = x.Capacity,
                type = x.Type,
                @class = x.Vehicle_Class,
                isActive = x.IsActive,
                createdBy = x.Created_By,
                createdDate = x.Created_Date,
                modifiedBy = x.Modified_By,
                modifiedDate = x.Modified_Date,
                totalCount = x.TotalCount
            });
        }

        public async Task<VehicleCategoryDTO> GetByIdAsync(int categoryId)
        {
            using var conn = _db.CreateConnection();

            var dbResult = await conn.QueryFirstOrDefaultAsync<VehicleCategoryDbDTO>(
                "usp_VehicleCategory_GetById",
                new { Category_Id = categoryId },
                commandType: CommandType.StoredProcedure);

            if (dbResult == null)
                return null;

            return new VehicleCategoryDTO
            {
                categoryId = dbResult.Category_Id?.ToString(),
                categoryCode = dbResult.Category_Code,
                categoryName = dbResult.Category_Name,
                capacity = dbResult.Capacity,
                type = dbResult.Type,
                @class = dbResult.Vehicle_Class,
                isActive = dbResult.IsActive,
                createdBy = dbResult.Created_By,
                createdDate = dbResult.Created_Date,
                modifiedBy = dbResult.Modified_By,
                modifiedDate = dbResult.Modified_Date,
                totalCount = 0 // ✅ Single record, no pagination needed
            };
        }

        public async Task<string> GetNextCodeAsync()
        {
            using var conn = _db.CreateConnection();

            var p = new DynamicParameters();
            p.Add("@NextCode", dbType: DbType.String, direction: ParameterDirection.Output, size: 50);

            await conn.ExecuteAsync(
                "usp_VehicleCategory_GetNextCode",
                p,
                commandType: CommandType.StoredProcedure);

            return p.Get<string>("@NextCode");
        }

        public async Task<int> InsertAsync(VehicleCategoryDTO entity, int userId)
        {
            try
            {
                using var conn = _db.CreateConnection();

                var p = new DynamicParameters();

                p.Add("@Category_Name", entity.categoryName);
                p.Add("@Capacity", entity.capacity);
                p.Add("@Type", entity.type);
                p.Add("@Vehicle_Class", entity.@class);
                p.Add("@IsActive", entity.isActive);
                p.Add("@UserId", userId);
                p.Add("@NewId", dbType: DbType.Int32, direction: ParameterDirection.Output);

                await conn.ExecuteAsync(
                    "usp_VehicleCategory_Insert",
                    p,
                    commandType: CommandType.StoredProcedure);

                return p.Get<int>("@NewId");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<bool> UpdateAsync(VehicleCategoryDTO entity, int userId)
        {
            try
            {
                using var conn = _db.CreateConnection();

                var p = new DynamicParameters();

                p.Add("@Category_Id", Convert.ToInt32(entity.categoryId));
                p.Add("@Category_Name", entity.categoryName);
                p.Add("@Capacity", entity.capacity);
                p.Add("@Type", entity.type);
                p.Add("@Vehicle_Class", entity.@class);
                p.Add("@IsActive", entity.isActive);
                p.Add("@UserId", userId);
                p.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);

                await conn.ExecuteAsync(
                    "usp_VehicleCategory_Update",
                    p,
                    commandType: CommandType.StoredProcedure);

                return p.Get<bool>("@Success");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<bool> DeleteAsync(int categoryId, int deletedBy)
        {
            try
            {
                using var conn = _db.CreateConnection();

                var p = new DynamicParameters();

                p.Add("@Category_Id", categoryId);
                p.Add("@DeletedBy", deletedBy);
                p.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);

                await conn.ExecuteAsync(
                    "usp_VehicleCategory_Delete",
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