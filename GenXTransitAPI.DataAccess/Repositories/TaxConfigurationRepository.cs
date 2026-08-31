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
    public class TaxConfigurationRepository : ITaxConfigurationRepository
    {
        private readonly DBHelper _db;

        public TaxConfigurationRepository(DBHelper db)
        {
            _db = db;
        }

        public async Task<IEnumerable<TaxConfigurationDTO>> GetAllAsync(
            string? searchText,
            string? taxType,
            decimal? rateFrom,
            decimal? rateTo,
            bool? isActive,
            int? scopeToUser,
            int pageNumber = 1,
            int pageSize = 10)
        {
            using var conn = _db.CreateConnection();

            var dbResult = await conn.QueryAsync<TaxConfigurationDbDTO>(
                "usp_TaxConfiguration_GetAll",
                new
                {
                    SearchText = searchText,
                    TaxType = taxType,
                    RateFrom = rateFrom,
                    RateTo = rateTo,
                    IsActive = isActive,
                    ScopeToUser = scopeToUser,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                },
                commandType: CommandType.StoredProcedure);

            return dbResult.Select(x => new TaxConfigurationDTO
            {
                taxId = x.Tax_Id?.ToString(),
                taxCode = x.Tax_Code,
                taxType = x.Tax_Type,
                description = x.Description,
                rate = x.Rate,
                isActive = x.IsActive,
                createdBy = x.Created_By,
                createdDate = x.Created_Date,
                modifiedBy = x.Modified_By,
                modifiedDate = x.Modified_Date,
                totalCount = x.TotalCount
            });
        }

        public async Task<TaxConfigurationDTO> GetByIdAsync(int taxId)
        {
            using var conn = _db.CreateConnection();

            var dbResult = await conn.QueryFirstOrDefaultAsync<TaxConfigurationDbDTO>(
                "usp_TaxConfiguration_GetById",
                new { Tax_Id = taxId },
                commandType: CommandType.StoredProcedure);

            if (dbResult == null)
                return null;

            return new TaxConfigurationDTO
            {
                taxId = dbResult.Tax_Id?.ToString(),
                taxCode = dbResult.Tax_Code,
                taxType = dbResult.Tax_Type,
                description = dbResult.Description,
                rate = dbResult.Rate,
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
                "usp_TaxConfiguration_GetNextCode",
                p,
                commandType: CommandType.StoredProcedure);

            return p.Get<string>("@NextCode");
        }

        public async Task<int> InsertAsync(TaxConfigurationDTO entity, int userId)
        {
            try
            {
                using var conn = _db.CreateConnection();

                var p = new DynamicParameters();

                p.Add("@Tax_Type", entity.taxType);
                p.Add("@Description", entity.description);
                p.Add("@Rate", entity.rate);
                p.Add("@IsActive", entity.isActive);
                p.Add("@UserId", userId);
                p.Add("@NewId", dbType: DbType.Int32, direction: ParameterDirection.Output);

                await conn.ExecuteAsync(
                    "usp_TaxConfiguration_Insert",
                    p,
                    commandType: CommandType.StoredProcedure);

                return p.Get<int>("@NewId");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while inserting tax configuration: {ex.Message}", ex);
            }
        }

        public async Task<bool> UpdateAsync(TaxConfigurationDTO entity, int userId)
        {
            try
            {
                using var conn = _db.CreateConnection();

                var p = new DynamicParameters();

                p.Add("@Tax_Id", Convert.ToInt32(entity.taxId));
                p.Add("@Tax_Type", entity.taxType);
                p.Add("@Description", entity.description);
                p.Add("@Rate", entity.rate);
                p.Add("@IsActive", entity.isActive);
                p.Add("@UserId", userId);
                p.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);

                await conn.ExecuteAsync(
                    "usp_TaxConfiguration_Update",
                    p,
                    commandType: CommandType.StoredProcedure);

                return p.Get<bool>("@Success");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while updating tax configuration: {ex.Message}", ex);
            }
        }

        public async Task<bool> DeleteAsync(int taxId, int deletedBy)
        {
            try
            {
                using var conn = _db.CreateConnection();

                var p = new DynamicParameters();

                p.Add("@Tax_Id", taxId);
                p.Add("@DeletedBy", deletedBy);
                p.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);

                await conn.ExecuteAsync(
                    "usp_TaxConfiguration_Delete",
                    p,
                    commandType: CommandType.StoredProcedure);

                return p.Get<bool>("@Success");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while deleting tax configuration: {ex.Message}", ex);
            }
        }
    }
}