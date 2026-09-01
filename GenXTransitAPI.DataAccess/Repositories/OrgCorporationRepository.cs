using Dapper;
using GenXTransitAPI.DataAccess.Interface.IRepositories;
using GenXTransitAPI.Models.DTO_s;
using GenXTransitAPI.DataAccess.Data;
using System.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

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
            string? districtName,
            string? cityName,
            bool? isActive,
            int? scopeToUser,
            int pageNumber = 1,
            int pageSize = 10)
        {
            using var conn = _db.CreateConnection();

            var dbResult = await conn.QueryAsync<OrgCorporationDbDTO>(
                "usp_Corporation_GetAll",
                new
                {
                    SearchText = searchText,
                    StateName = stateName,
                    DistrictName = districtName,
                    CityName = cityName,
                    IsActive = isActive,
                    ScopeToUser = scopeToUser,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                },
                commandType: CommandType.StoredProcedure);

            return dbResult.Select(x => new OrgCorporationDTO
            {
                corpId = x.Corporation_Id?.ToString(),
                corpCode = x.Corp_Code,
                corporationName = x.Corporation_Name,
                stateName = x.State_Name,
                districtName = x.District_Name,
                cityName = x.City_Name,
                isActive = x.IsActive,
                createdBy = x.Created_By,
                createdDate = x.Created_Date,
                modifiedBy = x.Modified_By,
                modifiedDate = x.Modified_Date,
                totalCount = x.TotalCount,
                depotCount = x.DepotCount
            });
        }

        public async Task<OrgCorporationDTO> GetByIdAsync(int corporationId)
        {
            using var conn = _db.CreateConnection();

            var dbResult = await conn.QueryFirstOrDefaultAsync<OrgCorporationDbDTO>(
                "usp_Corporation_GetById",
                new { Corporation_Id = corporationId },
                commandType: CommandType.StoredProcedure);

            if (dbResult == null)
                return null;

            return new OrgCorporationDTO
            {
                corpId = dbResult.Corporation_Id?.ToString(),
                corpCode = dbResult.Corp_Code,
                corporationName = dbResult.Corporation_Name,
                stateName = dbResult.State_Name,
                districtName = dbResult.District_Name,
                cityName = dbResult.City_Name,
                isActive = dbResult.IsActive,
                createdBy = dbResult.Created_By,
                createdDate = dbResult.Created_Date,
                modifiedBy = dbResult.Modified_By,
                modifiedDate = dbResult.Modified_Date,
                totalCount = 0,
                depotCount = dbResult.DepotCount 
            };
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

                p.Add("@Corporation_Name", entity.corporationName);
                p.Add("@State_Name", entity.stateName);
                p.Add("@District_Name", entity.districtName);
                p.Add("@City_Name", entity.cityName);
                p.Add("@IsActive", entity.isActive);
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
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<bool> UpdateAsync(OrgCorporationDTO entity, int userId)
        {
            try
            {
                using var conn = _db.CreateConnection();

                var p = new DynamicParameters();

                p.Add("@Corporation_Id", Convert.ToInt32(entity.corpId));
                p.Add("@Corporation_Name", entity.corporationName);
                p.Add("@State_Name", entity.stateName);
                p.Add("@District_Name", entity.districtName);
                p.Add("@City_Name", entity.cityName);
                p.Add("@IsActive", entity.isActive);
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
                throw new Exception(ex.Message, ex);
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
                throw new Exception(ex.Message, ex);
            }
        }
    }
}