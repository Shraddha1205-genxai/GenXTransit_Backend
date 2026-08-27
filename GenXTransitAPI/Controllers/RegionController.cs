using GenXTransitAPI.DataAccess.Interface.IServices;
using GenXTransitAPI.Models;
using GenXTransitAPI.Models.DTO_s;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GenXTransitAPI.Controllers
{
    [Route("api/region")]
    [ApiController]
    public class RegionController : ControllerBase
    {
        private readonly IOrgRegionService _svc;

        public RegionController(IOrgRegionService svc)
        {
            _svc = svc;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] string? searchText,
            [FromQuery] bool? isActive,
            [FromQuery] int pageNumber = 1,    
            [FromQuery] int pageSize = 10)     
        {
            var result = await _svc.GetAllAsync(searchText, isActive, GetCurrentUserId(), pageNumber, pageSize);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _svc.GetByIdAsync(id);
            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        [HttpGet("next-code")]
        public async Task<IActionResult> GetNextCode()
        {
            var result = await _svc.GetNextCodeAsync();
            return Ok(result);
        }

        [HttpPost("insert")]
        public async Task<IActionResult> Insert([FromBody] InsertRegionRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.regionName))
                return BadRequest(ApiResponse<int>.Fail("Region Name is required."));

            var entity = new OrgRegionDTO
            {
                regionName = request.regionName,
                isActive = request.isActive
            };

            var result = await _svc.InsertAsync(entity, GetCurrentUserId());
            if (result.Success && result.Data > 0)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("update")]
        public async Task<IActionResult> Update([FromBody] UpdateRegionRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.regionId))
                return BadRequest(ApiResponse<bool>.Fail("Region ID is required."));

            if (!int.TryParse(request.regionId, out int id))
                return BadRequest(ApiResponse<bool>.Fail("Invalid Region ID format."));

            if (string.IsNullOrWhiteSpace(request.regionName))
                return BadRequest(ApiResponse<bool>.Fail("Region Name is required."));

            var entity = new OrgRegionDTO
            {
                regionId = request.regionId,
                regionName = request.regionName,
                isActive = request.isActive
            };

            var result = await _svc.UpdateAsync(entity, GetCurrentUserId());
            if (!result.Success)
            {
                if (!string.IsNullOrEmpty(result.Message) && result.Message.Contains("not found"))
                    return NotFound(result);
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPost("delete")]
        public async Task<IActionResult> Delete([FromBody] DeleteRegionRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.regionId))
                return BadRequest(ApiResponse<bool>.Fail("Region ID is required."));

            if (!int.TryParse(request.regionId, out int id))
                return BadRequest(ApiResponse<bool>.Fail("Invalid Region ID format."));

            var result = await _svc.DeleteAsync(id, GetCurrentUserId());
            if (!result.Success)
            {
                if (!string.IsNullOrEmpty(result.Message) && result.Message.Contains("not found"))
                    return NotFound(result);
                return BadRequest(result);
            }
            return Ok(result);
        }

        private int GetCurrentUserId()
        {
            return 1;
        }
    }
}