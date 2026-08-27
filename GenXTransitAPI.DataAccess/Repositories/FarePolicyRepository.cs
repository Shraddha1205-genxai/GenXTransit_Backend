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
    public class FarePolicyRepository : IFarePolicyRepository
    {
        private readonly DBHelper _db;

        public FarePolicyRepository(DBHelper db)
        {
            _db = db;
        }

        public async Task<IEnumerable<FarePolicyDTO>> GetAllAsync(
            string? searchText,
            string? model,
            string? policyStatus,
            int? categoryId,
            int? routeId,
            bool? isActive,
            int? scopeToUser,
            int pageNumber = 1,
            int pageSize = 10)
        {
            using var conn = _db.CreateConnection();

            var dbResult = await conn.QueryAsync<FarePolicyDbDTO>(
                "usp_FarePolicy_GetAll",
                new
                {
                    SearchText = searchText,
                    Model = model,
                    PolicyStatus = policyStatus,
                    CategoryId = categoryId,
                    RouteId = routeId,
                    IsActive = isActive,
                    ScopeToUser = scopeToUser,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                },
                commandType: CommandType.StoredProcedure);

            return dbResult.Select(x => new FarePolicyDTO
            {
                policyId = x.Policy_Id?.ToString(),
                policyCode = x.Policy_Code,
                model = x.Model,
                policyStatus = x.Policy_Status,
                baseFare = x.Base_Fare,
                rateDescription = x.Rate_Description,
                isActive = x.IsActive,
                createdBy = x.Created_By,
                createdDate = x.Created_Date,
                modifiedBy = x.Modified_By,
                modifiedDate = x.Modified_Date,
                // Category details
                categoryId = x.Category_Id?.ToString(),
                categoryCode = x.Category_Code,
                categoryName = x.Category_Name,
                // Route details
                routeId = x.Route_Id?.ToString(),
                routeCode = x.Route_Code,
                routeName = x.Route_Name,
                totalCount = x.TotalCount
            });
        }

        public async Task<FarePolicyDTO> GetByIdAsync(int policyId)
        {
            using var conn = _db.CreateConnection();

            var dbResult = await conn.QueryFirstOrDefaultAsync<FarePolicyDbDTO>(
                "usp_FarePolicy_GetById",
                new { Policy_Id = policyId },
                commandType: CommandType.StoredProcedure);

            if (dbResult == null)
                return null;

            return new FarePolicyDTO
            {
                policyId = dbResult.Policy_Id?.ToString(),
                policyCode = dbResult.Policy_Code,
                model = dbResult.Model,
                policyStatus = dbResult.Policy_Status,
                baseFare = dbResult.Base_Fare,
                rateDescription = dbResult.Rate_Description,
                isActive = dbResult.IsActive,
                createdBy = dbResult.Created_By,
                createdDate = dbResult.Created_Date,
                modifiedBy = dbResult.Modified_By,
                modifiedDate = dbResult.Modified_Date,
                // Category details
                categoryId = dbResult.Category_Id?.ToString(),
                categoryCode = dbResult.Category_Code,
                categoryName = dbResult.Category_Name,
                // Route details
                routeId = dbResult.Route_Id?.ToString(),
                routeCode = dbResult.Route_Code,
                routeName = dbResult.Route_Name,
                totalCount = 0
            };
        }

        public async Task<string> GetNextCodeAsync()
        {
            using var conn = _db.CreateConnection();

            var p = new DynamicParameters();
            p.Add("@NextCode", dbType: DbType.String, direction: ParameterDirection.Output, size: 50);

            await conn.ExecuteAsync(
                "usp_FarePolicy_GetNextCode",
                p,
                commandType: CommandType.StoredProcedure);

            return p.Get<string>("@NextCode");
        }

        public async Task<int> InsertAsync(FarePolicyDTO entity, int userId)
        {
            try
            {
                using var conn = _db.CreateConnection();

                var p = new DynamicParameters();

                p.Add("@Model", entity.model);
                p.Add("@Policy_Status", entity.policyStatus);
                p.Add("@Category_Id", Convert.ToInt32(entity.categoryId));
                p.Add("@Route_Id", Convert.ToInt32(entity.routeId));
                p.Add("@Base_Fare", entity.baseFare);
                p.Add("@Rate_Description", entity.rateDescription);
                p.Add("@IsActive", entity.isActive);
                p.Add("@UserId", userId);
                p.Add("@NewId", dbType: DbType.Int32, direction: ParameterDirection.Output);

                await conn.ExecuteAsync(
                    "usp_FarePolicy_Insert",
                    p,
                    commandType: CommandType.StoredProcedure);

                return p.Get<int>("@NewId");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while inserting fare policy: {ex.Message}", ex);
            }
        }

        public async Task<bool> UpdateAsync(FarePolicyDTO entity, int userId)
        {
            try
            {
                using var conn = _db.CreateConnection();

                var p = new DynamicParameters();

                p.Add("@Policy_Id", Convert.ToInt32(entity.policyId));
                p.Add("@Model", entity.model);
                p.Add("@Policy_Status", entity.policyStatus);
                p.Add("@Category_Id", Convert.ToInt32(entity.categoryId));
                p.Add("@Route_Id", Convert.ToInt32(entity.routeId));
                p.Add("@Base_Fare", entity.baseFare);
                p.Add("@Rate_Description", entity.rateDescription);
                p.Add("@IsActive", entity.isActive);
                p.Add("@UserId", userId);
                p.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);

                await conn.ExecuteAsync(
                    "usp_FarePolicy_Update",
                    p,
                    commandType: CommandType.StoredProcedure);

                return p.Get<bool>("@Success");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while updating fare policy: {ex.Message}", ex);
            }
        }

        public async Task<bool> DeleteAsync(int policyId, int deletedBy)
        {
            try
            {
                using var conn = _db.CreateConnection();

                var p = new DynamicParameters();

                p.Add("@Policy_Id", policyId);
                p.Add("@DeletedBy", deletedBy);
                p.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);

                await conn.ExecuteAsync(
                    "usp_FarePolicy_Delete",
                    p,
                    commandType: CommandType.StoredProcedure);

                return p.Get<bool>("@Success");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while deleting fare policy: {ex.Message}", ex);
            }
        }
    }
}