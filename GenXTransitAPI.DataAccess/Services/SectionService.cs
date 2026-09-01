using GenXTransitAPI.DataAccess.Interface.IRepositories;
using GenXTransitAPI.DataAccess.Interface.IServices;
using GenXTransitAPI.Models;
using GenXTransitAPI.Models.DTOs;
using GenXTransitAPI.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenXTransitAPI.DataAccess.Services
{
    public class SectionService: ISectionService
    {
        private readonly ISectionRepository _sectionRepository;

        public SectionService(
            ISectionRepository sectionRepository)
        {
            _sectionRepository = sectionRepository;
        }

        public async Task<ApiResponse<Section>> AddSectionAsync(
            SectionRequestDto request, int createdBy)
        {
            return await _sectionRepository.AddSectionAsync(request, createdBy);
        }

        public async Task<ApiResponse<Section>> UpdateSectionAsync(
            SectionUpdateRequestDto request, int modifiedBy)
        {
            return await _sectionRepository.UpdateSectionAsync(request, modifiedBy);
        }

        public async Task<ApiResponse<List<Section>>> GetAllSectionsAsync()
        {
            return await _sectionRepository.GetAllSectionsAsync();
        }

        public async Task<ApiResponse<Section>> GetSectionByIdAsync(
            int sectionId)
        {
            return await _sectionRepository.GetSectionByIdAsync(sectionId);
        }

        public async Task<ApiResponse<bool>> DeleteSectionAsync(
            int sectionId, int modifiedBy)
        {
            return await _sectionRepository.DeleteSectionAsync(sectionId,modifiedBy);
        }
    }
}