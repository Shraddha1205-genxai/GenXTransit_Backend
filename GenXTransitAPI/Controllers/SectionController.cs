using GenXTransitAPI.DataAccess.Interface.IServices;
using GenXTransitAPI.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GenXTransitAPI.Controllers
{
    [Route("api/section")]
    [ApiController]
    [AllowAnonymous]
    public class SectionController : BaseController
    {
        private readonly ISectionService _sectionService;

        public SectionController(
            ISectionService sectionService)
        {
            _sectionService = sectionService;
        }

        // POST: api/section
        [HttpPost("insert")]
        public async Task<IActionResult> AddSection(
            [FromBody] SectionRequestDto request)
        {
            var response = await _sectionService.AddSectionAsync(request,CurrentUserId);

            return Ok(response);
        }

        // PUT: api/section
        [HttpPost("update")]
        public async Task<IActionResult> UpdateSection(
            [FromBody] SectionUpdateRequestDto request)
        {
            var response =
                await _sectionService.UpdateSectionAsync(request,CurrentUserId);

            return Ok(response);
        }

        // GET: api/section
        [HttpGet]
        public async Task<IActionResult> GetAllSections()
        {
            var response =
                await _sectionService.GetAllSectionsAsync();

            return Ok(response);
        }

        // GET: api/section/5
        [HttpGet("{sectionId}")]
        public async Task<IActionResult> GetSectionById(
            int sectionId)
        {
            var response =
                await _sectionService.GetSectionByIdAsync(sectionId);

            return Ok(response);
        }

        [HttpPost("delete")]
        public async Task<IActionResult> DeleteSection(
            DeleteSectionRequest request)
        {
            var response =
                await _sectionService.DeleteSectionAsync(request.SectionId,CurrentUserId);

            return Ok(response);
        }
    }
}
    