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
    public class PaymentModeRepository : IPaymentModeRepository
    {
        private readonly DBHelper _db;

        public PaymentModeRepository(DBHelper db)
        {
            _db = db;
        }

        public async Task<IEnumerable<PaymentModeDTO>> GetAllAsync(
            string? searchText,
            string? modeStatus,
            bool? isActive,
            int? scopeToUser,
            int pageNumber = 1,
            int pageSize = 10)
        {
            using var conn = _db.CreateConnection();

            var dbResult = await conn.QueryAsync<PaymentModeDbDTO>(
                "usp_PaymentMode_GetAll",
                new
                {
                    SearchText = searchText,
                    ModeStatus = modeStatus,
                    IsActive = isActive,
                    ScopeToUser = scopeToUser,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                },
                commandType: CommandType.StoredProcedure);

            return dbResult.Select(x => new PaymentModeDTO
            {
                modeId = x.Mode_Id?.ToString(),
                modeCode = x.Mode_Code,
                modeName = x.Mode_Name,
                modeStatus = x.Mode_Status,
                description = x.Description,
                isActive = x.IsActive,
                createdBy = x.Created_By,
                createdDate = x.Created_Date,
                modifiedBy = x.Modified_By,
                modifiedDate = x.Modified_Date,
                totalCount = x.TotalCount
            });
        }

        public async Task<PaymentModeDTO> GetByIdAsync(int modeId)
        {
            using var conn = _db.CreateConnection();

            var dbResult = await conn.QueryFirstOrDefaultAsync<PaymentModeDbDTO>(
                "usp_PaymentMode_GetById",
                new { Mode_Id = modeId },
                commandType: CommandType.StoredProcedure);

            if (dbResult == null)
                return null;

            return new PaymentModeDTO
            {
                modeId = dbResult.Mode_Id?.ToString(),
                modeCode = dbResult.Mode_Code,
                modeName = dbResult.Mode_Name,
                modeStatus = dbResult.Mode_Status,
                description = dbResult.Description,
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
                "usp_PaymentMode_GetNextCode",
                p,
                commandType: CommandType.StoredProcedure);

            return p.Get<string>("@NextCode");
        }

        public async Task<int> InsertAsync(PaymentModeDTO entity, int userId)
        {
            try
            {
                using var conn = _db.CreateConnection();

                var p = new DynamicParameters();

                p.Add("@Mode_Name", entity.modeName);
                p.Add("@Mode_Status", entity.modeStatus);
                p.Add("@Description", entity.description);
                p.Add("@IsActive", entity.isActive);
                p.Add("@UserId", userId);
                p.Add("@NewId", dbType: DbType.Int32, direction: ParameterDirection.Output);

                await conn.ExecuteAsync(
                    "usp_PaymentMode_Insert",
                    p,
                    commandType: CommandType.StoredProcedure);

                return p.Get<int>("@NewId");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while inserting payment mode: {ex.Message}", ex);
            }
        }

        public async Task<bool> UpdateAsync(PaymentModeDTO entity, int userId)
        {
            try
            {
                using var conn = _db.CreateConnection();

                var p = new DynamicParameters();

                p.Add("@Mode_Id", Convert.ToInt32(entity.modeId));
                p.Add("@Mode_Name", entity.modeName);
                p.Add("@Mode_Status", entity.modeStatus);
                p.Add("@Description", entity.description);
                p.Add("@IsActive", entity.isActive);
                p.Add("@UserId", userId);
                p.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);

                await conn.ExecuteAsync(
                    "usp_PaymentMode_Update",
                    p,
                    commandType: CommandType.StoredProcedure);

                return p.Get<bool>("@Success");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while updating payment mode: {ex.Message}", ex);
            }
        }

        public async Task<bool> DeleteAsync(int modeId, int deletedBy)
        {
            try
            {
                using var conn = _db.CreateConnection();

                var p = new DynamicParameters();

                p.Add("@Mode_Id", modeId);
                p.Add("@DeletedBy", deletedBy);
                p.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);

                await conn.ExecuteAsync(
                    "usp_PaymentMode_Delete",
                    p,
                    commandType: CommandType.StoredProcedure);

                return p.Get<bool>("@Success");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while deleting payment mode: {ex.Message}", ex);
            }
        }
    }
}