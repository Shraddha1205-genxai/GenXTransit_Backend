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
    public class ComplaintCategoryRepository : IComplaintCategoryRepository
    {
        private readonly DBHelper _db;

        public ComplaintCategoryRepository(DBHelper db)
        {
            _db = db;
        }

        public async Task<IEnumerable<ComplaintCategoryDTO>> GetAllAsync(
            string? searchText,
            string? complaintCategory,
            string? sla,
            bool? isActive,
            int? scopeToUser,
            int pageNumber = 1,
            int pageSize = 10)
        {
            using var conn = _db.CreateConnection();

            var dbResult = await conn.QueryAsync<ComplaintCategoryDbDTO>(
                "usp_ComplaintCategory_GetAll",
                new
                {
                    SearchText = searchText,
                    ComplaintCategory = complaintCategory,
                    SLA = sla,
                    IsActive = isActive,
                    ScopeToUser = scopeToUser,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                },
                commandType: CommandType.StoredProcedure);

            return dbResult.Select(x => new ComplaintCategoryDTO
            {
                complaintId = x.Complaint_Id?.ToString(),
                complaintCode = x.Complaint_Code,
                complaintTitle = x.Complaint_Title,
                description = x.Description,
                complaintCategory = x.Complaint_Category,
                sla = x.SLA,
                isActive = x.IsActive,
                createdBy = x.Created_By,
                createdDate = x.Created_Date,
                modifiedBy = x.Modified_By,
                modifiedDate = x.Modified_Date,
                totalCount = x.TotalCount
            });
        }

        public async Task<ComplaintCategoryDTO> GetByIdAsync(int complaintId)
        {
            using var conn = _db.CreateConnection();

            var dbResult = await conn.QueryFirstOrDefaultAsync<ComplaintCategoryDbDTO>(
                "usp_ComplaintCategory_GetById",
                new { Complaint_Id = complaintId },
                commandType: CommandType.StoredProcedure);

            if (dbResult == null)
                return null;

            return new ComplaintCategoryDTO
            {
                complaintId = dbResult.Complaint_Id?.ToString(),
                complaintCode = dbResult.Complaint_Code,
                complaintTitle = dbResult.Complaint_Title,
                description = dbResult.Description,
                complaintCategory = dbResult.Complaint_Category,
                sla = dbResult.SLA,
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
                "usp_ComplaintCategory_GetNextCode",
                p,
                commandType: CommandType.StoredProcedure);

            return p.Get<string>("@NextCode");
        }

        public async Task<int> InsertAsync(ComplaintCategoryDTO entity, int userId)
        {
            try
            {
                using var conn = _db.CreateConnection();

                var p = new DynamicParameters();

                p.Add("@Complaint_Title", entity.complaintTitle);
                p.Add("@Description", entity.description);
                p.Add("@Complaint_Category", entity.complaintCategory);
                p.Add("@SLA", entity.sla);
                p.Add("@IsActive", entity.isActive);
                p.Add("@UserId", userId);
                p.Add("@NewId", dbType: DbType.Int32, direction: ParameterDirection.Output);

                await conn.ExecuteAsync(
                    "usp_ComplaintCategory_Insert",
                    p,
                    commandType: CommandType.StoredProcedure);

                return p.Get<int>("@NewId");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while inserting complaint category: {ex.Message}", ex);
            }
        }

        public async Task<bool> UpdateAsync(ComplaintCategoryDTO entity, int userId)
        {
            try
            {
                using var conn = _db.CreateConnection();

                var p = new DynamicParameters();

                p.Add("@Complaint_Id", Convert.ToInt32(entity.complaintId));
                p.Add("@Complaint_Title", entity.complaintTitle);
                p.Add("@Description", entity.description);
                p.Add("@Complaint_Category", entity.complaintCategory);
                p.Add("@SLA", entity.sla);
                p.Add("@IsActive", entity.isActive);
                p.Add("@UserId", userId);
                p.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);

                await conn.ExecuteAsync(
                    "usp_ComplaintCategory_Update",
                    p,
                    commandType: CommandType.StoredProcedure);

                return p.Get<bool>("@Success");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while updating complaint category: {ex.Message}", ex);
            }
        }

        public async Task<bool> DeleteAsync(int complaintId, int deletedBy)
        {
            try
            {
                using var conn = _db.CreateConnection();

                var p = new DynamicParameters();

                p.Add("@Complaint_Id", complaintId);
                p.Add("@DeletedBy", deletedBy);
                p.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);

                await conn.ExecuteAsync(
                    "usp_ComplaintCategory_Delete",
                    p,
                    commandType: CommandType.StoredProcedure);

                return p.Get<bool>("@Success");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while deleting complaint category: {ex.Message}", ex);
            }
        }
    }
}