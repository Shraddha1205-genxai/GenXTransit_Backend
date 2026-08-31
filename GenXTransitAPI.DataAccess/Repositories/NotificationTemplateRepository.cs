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
    public class NotificationTemplateRepository : INotificationTemplateRepository
    {
        private readonly DBHelper _db;

        public NotificationTemplateRepository(DBHelper db)
        {
            _db = db;
        }

        public async Task<IEnumerable<NotificationTemplateDTO>> GetAllAsync(
            string? searchText,
            string? channel,
            bool? isActive,
            int? scopeToUser,
            int pageNumber = 1,
            int pageSize = 10)
        {
            using var conn = _db.CreateConnection();

            var dbResult = await conn.QueryAsync<NotificationTemplateDbDTO>(
                "usp_NotificationTemplate_GetAll",
                new
                {
                    SearchText = searchText,
                    Channel = channel,
                    IsActive = isActive,
                    ScopeToUser = scopeToUser,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                },
                commandType: CommandType.StoredProcedure);

            return dbResult.Select(x => new NotificationTemplateDTO
            {
                notificationId = x.Notification_Id?.ToString(),
                notificationCode = x.Notification_Code,
                notificationTitle = x.Notification_Title,
                channel = x.Channel,
                description = x.Notification_Description,
                isActive = x.IsActive,
                createdBy = x.Created_By,
                createdDate = x.Created_Date,
                modifiedBy = x.Modified_By,
                modifiedDate = x.Modified_Date,
                totalCount = x.TotalCount
            });
        }

        public async Task<NotificationTemplateDTO> GetByIdAsync(int notificationId)
        {
            using var conn = _db.CreateConnection();

            var dbResult = await conn.QueryFirstOrDefaultAsync<NotificationTemplateDbDTO>(
                "usp_NotificationTemplate_GetById",
                new { Notification_Id = notificationId },
                commandType: CommandType.StoredProcedure);

            if (dbResult == null)
                return null;

            return new NotificationTemplateDTO
            {
                notificationId = dbResult.Notification_Id?.ToString(),
                notificationCode = dbResult.Notification_Code,
                notificationTitle = dbResult.Notification_Title,
                channel = dbResult.Channel,
                description = dbResult.Notification_Description,
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
                "usp_NotificationTemplate_GetNextCode",
                p,
                commandType: CommandType.StoredProcedure);

            return p.Get<string>("@NextCode");
        }

        public async Task<int> InsertAsync(NotificationTemplateDTO entity, int userId)
        {
            try
            {
                using var conn = _db.CreateConnection();

                var p = new DynamicParameters();

                p.Add("@Notification_Title", entity.notificationTitle);
                p.Add("@Channel", entity.channel);
                p.Add("@Notification_Description", entity.description);
                p.Add("@IsActive", entity.isActive);
                p.Add("@UserId", userId);
                p.Add("@NewId", dbType: DbType.Int32, direction: ParameterDirection.Output);

                await conn.ExecuteAsync(
                    "usp_NotificationTemplate_Insert",
                    p,
                    commandType: CommandType.StoredProcedure);

                return p.Get<int>("@NewId");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while inserting notification template: {ex.Message}", ex);
            }
        }

        public async Task<bool> UpdateAsync(NotificationTemplateDTO entity, int userId)
        {
            try
            {
                using var conn = _db.CreateConnection();

                var p = new DynamicParameters();

                p.Add("@Notification_Id", Convert.ToInt32(entity.notificationId));
                p.Add("@Notification_Title", entity.notificationTitle);
                p.Add("@Channel", entity.channel);
                p.Add("@Notification_Description", entity.description);
                p.Add("@IsActive", entity.isActive);
                p.Add("@UserId", userId);
                p.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);

                await conn.ExecuteAsync(
                    "usp_NotificationTemplate_Update",
                    p,
                    commandType: CommandType.StoredProcedure);

                return p.Get<bool>("@Success");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while updating notification template: {ex.Message}", ex);
            }
        }

        public async Task<bool> DeleteAsync(int notificationId, int deletedBy)
        {
            try
            {
                using var conn = _db.CreateConnection();

                var p = new DynamicParameters();

                p.Add("@Notification_Id", notificationId);
                p.Add("@DeletedBy", deletedBy);
                p.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);

                await conn.ExecuteAsync(
                    "usp_NotificationTemplate_Delete",
                    p,
                    commandType: CommandType.StoredProcedure);

                return p.Get<bool>("@Success");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while deleting notification template: {ex.Message}", ex);
            }
        }
    }
}