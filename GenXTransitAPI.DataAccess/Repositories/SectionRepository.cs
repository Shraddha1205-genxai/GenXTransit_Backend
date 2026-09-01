using Dapper;
using GenXTransitAPI.DataAccess.Data;
using GenXTransitAPI.DataAccess.Interface.IRepositories;
using GenXTransitAPI.Models;
using GenXTransitAPI.Models.DTOs;
using GenXTransitAPI.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenXTransitAPI.DataAccess.Repositories
{
    public class SectionRepository : ISectionRepository
    {
        private readonly DBHelper _db;

        public SectionRepository(DBHelper dbHelper)
        {
            _db = dbHelper;
        }

        public async Task<ApiResponse<Section>> AddSectionAsync(
            SectionRequestDto request, int createdBy)
        {
            try
            {
                using var conn = _db.CreateConnection();
                var parameters = new DynamicParameters();

                parameters.Add(
                    "@SectionName",
                    request.SectionName,
                    DbType.String);

                parameters.Add(
                    "@CreatedBy",
                    createdBy,
                    DbType.Int32);

                var result = await conn.QueryFirstOrDefaultAsync<dynamic>(
                    "usp_Section_Insert",
                    parameters,
                      commandType: CommandType.StoredProcedure);

                if (result == null)
                {
                    return new ApiResponse<Section>
                    {
                        Success = false,
                        Message = "Unable to add section."
                    };
                }

                if (result.Status == 0)
                {
                    return new ApiResponse<Section>
                    {
                        Success = false,
                        Message = result.Message
                    };
                }

                var sectionId = Convert.ToInt32(result.SectionId);

                var section = await GetSectionByIdAsync(sectionId);

                return new ApiResponse<Section>
                {
                    Success = true,
                    Message = result.Message,
                    Data = section.Data
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ApiResponse<Section>> UpdateSectionAsync(
            SectionUpdateRequestDto request, int modifiedBy)
        {
            try
            {
                using var conn = _db.CreateConnection();
                var parameters = new DynamicParameters();

                parameters.Add(
                    "@SectionId",
                    request.SectionId,
                    DbType.Int32);

                parameters.Add(
                    "@SectionName",
                    request.SectionName,
                    DbType.String);

                parameters.Add(
                    "@ModifiedBy",
                    modifiedBy,
                    DbType.Int32);

                var result = await conn.QueryFirstOrDefaultAsync<dynamic>(
                    "usp_Section_Update",
                    parameters,
                   commandType: CommandType.StoredProcedure);

                if (result == null)
                {
                    return new ApiResponse<Section>
                    {
                        Success=false,
                        Message = "Unable to update section."
                    };
                }

                if (result.Status == 0)
                {
                    return new ApiResponse<Section>
                    {
                        Success = false,
                        Message = result.Message
                    };
                }

                var section = await GetSectionByIdAsync(
                    request.SectionId);

                return new ApiResponse<Section>
                {
                    Success = true,
                    Message = result.Message,
                    Data = section.Data
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ApiResponse<List<Section>>> GetAllSectionsAsync()
        {
            try
            {
                using var conn = _db.CreateConnection();
                var result = await conn.QueryAsync<Section>(
                    "usp_Section_GetAll",
                    null,
                    commandType: CommandType.StoredProcedure);

                return new ApiResponse<List<Section>>
                {
                    Success = true,
                    Message = "Sections retrieved successfully.",
                    Data = result.ToList()
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ApiResponse<Section>> GetSectionByIdAsync(
            int sectionId)
        {
            try
            {
                using var conn = _db.CreateConnection();
                var parameters = new DynamicParameters();

                parameters.Add(
                    "@SectionId",
                    sectionId,
                    DbType.Int32);

                var result = await conn.QueryFirstOrDefaultAsync<Section>(
                    "usp_Section_GetById",
                    parameters,
                   commandType: CommandType.StoredProcedure);

                if (result == null)
                {
                    return new ApiResponse<Section>
                    {
                        Success = false,
                        Message = "Section not found."
                    };
                }

                return new ApiResponse<Section>
                {
                    Success = true,
                    Message = "Section retrieved successfully.",
                    Data = result
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ApiResponse<bool>> DeleteSectionAsync(
            int sectionId, int modifiedBy)
        {
            try
            {
                using var conn = _db.CreateConnection();
                var parameters = new DynamicParameters();

                parameters.Add(
                    "@SectionId",
                    sectionId,
                    DbType.Int32);
                parameters.Add(
                   "@ModifiedBy",
                   sectionId,
                   DbType.Int32);

                var result = await conn.QueryFirstOrDefaultAsync<dynamic>(
                    "usp_Section_Delete",
                    parameters,
                    commandType: CommandType.StoredProcedure);

                if (result == null)
                {
                    return new ApiResponse<bool>
                    {
                        Success = false,
                        Message = "Unable to delete section.",
                        Data = false
                    };
                }

                return new ApiResponse<bool>
                {
                    Success = true,
                    Message = result.Message,
                    Data = Convert.ToInt32(result.Status) == 1
                };
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
