using Dapper;
using GenXTransitAPI.DataAccess.Interfaces.Repositories;
using GenXTransitAPI.Models.DTO_s;
using GenXTransitAPI.DataAccess.Data;
using System.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GenXTransitAPI.DataAccess.Repositories
{
    public class OrgCorporationRepository : IOrgCorporationRepository
    {
        private readonly DBHelper _db;

        public OrgCorporationRepository(DBHelper db)
        {
            _db = db;
        }

        public async Task<IEnumerable<OrgCorporationDTO>> GetAllAsync(
            string? searchText,
            string? stateName,
            bool? isActive,
            int? scopeToUser,
            int pageNumber = 1,
            int pageSize = 10)
        {
            using var conn = _db.CreateConnection();

            // ✅ Use OrgCorporationDbDTO to map TotalCount from SP
            var dbResult = await conn.QueryAsync<OrgCorporationDbDTO>(
                "usp_Corporation_GetAll",
                new
                {
                    SearchText = searchText,
                    StateName = stateName,
                    IsActive = isActive,
                    ScopeToUser = scopeToUser,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                },
                commandType: CommandType.StoredProcedure);

            // ✅ Convert from DB DTO to UI DTO with TotalCount
            return dbResult.Select(x => new OrgCorporationDTO
            {
                Corporation_Id = x.Corporation_Id,
                Corp_Code = x.Corp_Code,
                Corporation_Name = x.Corporation_Name,
                State_Name = x.State_Name,
                District_Name = x.District_Name,
                City_Name = x.City_Name,
                IsActive = x.IsActive,
                Created_By = x.Created_By,
                Created_Date = x.Created_Date,
                Modified_By = x.Modified_By,
                Modified_Date = x.Modified_Date,
                IsDeleted = x.IsDeleted,
                Deleted_By = x.Deleted_By,
                Deleted_Date = x.Deleted_Date,
                TotalCount = x.TotalCount  // ✅ Map TotalCount
            });
        }

        public async Task<OrgCorporationDTO> GetByIdAsync(int corporationId)
        {
            using var conn = _db.CreateConnection();

            return await conn.QueryFirstOrDefaultAsync<OrgCorporationDTO>(
                "usp_Corporation_GetById",
                new { Corporation_Id = corporationId },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<string> GetNextCodeAsync()
        {
            using var conn = _db.CreateConnection();

            var p = new DynamicParameters();
            p.Add("@NextCode", dbType: DbType.String, direction: ParameterDirection.Output, size: 50);

            await conn.ExecuteAsync(
                "usp_Corporation_GetNextCode",
                p,
                commandType: CommandType.StoredProcedure);

            return p.Get<string>("@NextCode");
        }

        public async Task<int> InsertAsync(OrgCorporationDTO entity, int userId)
        {
            try
            {
                using var conn = _db.CreateConnection();

                var p = new DynamicParameters();

                p.Add("@Corporation_Name", entity.Corporation_Name);
                p.Add("@State_Name", entity.State_Name);
                p.Add("@District_Name", entity.District_Name);
                p.Add("@City_Name", entity.City_Name);
                p.Add("@IsActive", entity.IsActive);
                p.Add("@UserId", userId);
                p.Add("@NewId", dbType: DbType.Int32, direction: ParameterDirection.Output);

                await conn.ExecuteAsync(
                    "usp_Corporation_Insert",
                    p,
                    commandType: CommandType.StoredProcedure);

                return p.Get<int>("@NewId");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while inserting corporation: {ex.Message}", ex);
            }
        }

        public async Task<bool> UpdateAsync(OrgCorporationDTO entity, int userId)
        {
            try
            {
                using var conn = _db.CreateConnection();

                var p = new DynamicParameters();

                p.Add("@Corporation_Id", entity.Corporation_Id);
                p.Add("@Corporation_Name", entity.Corporation_Name);
                p.Add("@State_Name", entity.State_Name);
                p.Add("@District_Name", entity.District_Name);
                p.Add("@City_Name", entity.City_Name);
                p.Add("@IsActive", entity.IsActive);
                p.Add("@UserId", userId);
                p.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);

                await conn.ExecuteAsync(
                    "usp_Corporation_Update",
                    p,
                    commandType: CommandType.StoredProcedure);

                return p.Get<bool>("@Success");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while updating corporation: {ex.Message}", ex);
            }
        }

        public async Task<bool> DeleteAsync(int corporationId, int deletedBy)
        {
            try
            {
                using var conn = _db.CreateConnection();

                var p = new DynamicParameters();

                p.Add("@Corporation_Id", corporationId);
                p.Add("@DeletedBy", deletedBy);
                p.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);

                await conn.ExecuteAsync(
                    "usp_Corporation_Delete",
                    p,
                    commandType: CommandType.StoredProcedure);

                return p.Get<bool>("@Success");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while deleting corporation: {ex.Message}", ex);
            }
        }
    }
}