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
    public class TicketTypeRepository : ITicketTypeRepository
    {
        private readonly DBHelper _db;

        public TicketTypeRepository(DBHelper db)
        {
            _db = db;
        }

        public async Task<IEnumerable<TicketTypeDTO>> GetAllAsync(
            string? searchText,
            bool? isActive,
            int? scopeToUser,
            int pageNumber = 1,
            int pageSize = 10)
        {
            using var conn = _db.CreateConnection();

            var dbResult = await conn.QueryAsync<TicketTypeDbDTO>(
                "usp_TicketType_GetAll",
                new
                {
                    SearchText = searchText,
                    IsActive = isActive,
                    ScopeToUser = scopeToUser,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                },
                commandType: CommandType.StoredProcedure);

            return dbResult.Select(x => new TicketTypeDTO
            {
                ticketId = x.Ticket_ID?.ToString(),
                ticketCode = x.Ticket_Code,
                ticketName = x.Ticket_Name,
                description = x.Description,
                isActive = x.IsActive,
                createdBy = x.Created_By,
                createdDate = x.Created_Date,
                modifiedBy = x.Modified_By,
                modifiedDate = x.Modified_Date,
                totalCount = x.TotalCount
            });
        }

        public async Task<TicketTypeDTO> GetByIdAsync(int ticketId)
        {
            using var conn = _db.CreateConnection();

            var dbResult = await conn.QueryFirstOrDefaultAsync<TicketTypeDbDTO>(
                "usp_TicketType_GetById",
                new { Ticket_ID = ticketId },
                commandType: CommandType.StoredProcedure);

            if (dbResult == null)
                return null;

            return new TicketTypeDTO
            {
                ticketId = dbResult.Ticket_ID?.ToString(),
                ticketCode = dbResult.Ticket_Code,
                ticketName = dbResult.Ticket_Name,
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
                "usp_TicketType_GetNextCode",
                p,
                commandType: CommandType.StoredProcedure);

            return p.Get<string>("@NextCode");
        }

        public async Task<int> InsertAsync(TicketTypeDTO entity, int userId)
        {
            try
            {
                using var conn = _db.CreateConnection();

                var p = new DynamicParameters();

                p.Add("@Ticket_Name", entity.ticketName);
                p.Add("@Description", entity.description);
                p.Add("@IsActive", entity.isActive);
                p.Add("@UserId", userId);
                p.Add("@NewId", dbType: DbType.Int32, direction: ParameterDirection.Output);

                await conn.ExecuteAsync(
                    "usp_TicketType_Insert",
                    p,
                    commandType: CommandType.StoredProcedure);

                return p.Get<int>("@NewId");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<bool> UpdateAsync(TicketTypeDTO entity, int userId)
        {
            try
            {
                using var conn = _db.CreateConnection();

                var p = new DynamicParameters();

                p.Add("@Ticket_ID", Convert.ToInt32(entity.ticketId));
                p.Add("@Ticket_Name", entity.ticketName);
                p.Add("@Description", entity.description);
                p.Add("@IsActive", entity.isActive);
                p.Add("@UserId", userId);
                p.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);

                await conn.ExecuteAsync(
                    "usp_TicketType_Update",
                    p,
                    commandType: CommandType.StoredProcedure);

                return p.Get<bool>("@Success");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<bool> DeleteAsync(int ticketId, int deletedBy)
        {
            try
            {
                using var conn = _db.CreateConnection();

                var p = new DynamicParameters();

                p.Add("@Ticket_ID", ticketId);
                p.Add("@DeletedBy", deletedBy);
                p.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);

                await conn.ExecuteAsync(
                    "usp_TicketType_Delete",
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