using GenXTransitAPI.Models;
using GenXTransitAPI.Models.DTOs;
using GenXTransitAPI.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenXTransitAPI.DataAccess.Interface.IServices
{
    public interface ISectionService
    {
        Task<ApiResponse<Section>> AddSectionAsync(
            SectionRequestDto request, int createdBy);

        Task<ApiResponse<Section>> UpdateSectionAsync(
            SectionUpdateRequestDto request, int modifiedBy);

        Task<ApiResponse<List<Section>>> GetAllSectionsAsync();

        Task<ApiResponse<Section>> GetSectionByIdAsync(
            int sectionId);

        Task<ApiResponse<bool>> DeleteSectionAsync(
            int sectionId, int modifiedBy);
    }
}
