using GenXTransitAPI.DataAccess.Interfaces.Services;
using GenXTransitAPI.Models;
using GenXTransitAPI.Models.DTO_s;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GenXTransitAPI.Controllers
{
    [Route("api/zone")]
    [ApiController]
    public class ZoneController : ControllerBase
    {
        private readonly IOrgZoneService _svc;

        public ZoneController(IOrgZoneService svc)
        {
            _svc = svc;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] string? searchText,
            [FromQuery] int? regionId,
            [FromQuery] bool? isActive,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var result = await _svc.GetAllAsync(searchText, regionId, isActive, GetCurrentUserId(), pageNumber, pageSize);
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
        public async Task<IActionResult> Insert([FromBody] InsertZoneRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.zoneName))
                return BadRequest(ApiResponse<int>.Fail("Zone Name is required."));

            if (string.IsNullOrWhiteSpace(request.regionId))
                return BadRequest(ApiResponse<int>.Fail("Region is required."));

            var entity = new OrgZoneDTO
            {
                zoneName = request.zoneName,
                regionId = request.regionId,
                districts = request.districts ?? new System.Collections.Generic.List<string>(),
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
        public async Task<IActionResult> Update([FromBody] UpdateZoneRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.zoneId))
                return BadRequest(ApiResponse<bool>.Fail("Zone ID is required."));

            if (string.IsNullOrWhiteSpace(request.zoneName))
                return BadRequest(ApiResponse<bool>.Fail("Zone Name is required."));

            if (string.IsNullOrWhiteSpace(request.regionId))
                return BadRequest(ApiResponse<bool>.Fail("Region is required."));

            var entity = new OrgZoneDTO
            {
                zoneId = request.zoneId,
                zoneName = request.zoneName,
                regionId = request.regionId,
                districts = request.districts ?? new System.Collections.Generic.List<string>(),
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
        public async Task<IActionResult> Delete([FromBody] DeleteZoneRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.zoneId))
                return BadRequest(ApiResponse<bool>.Fail("Zone ID is required."));

            if (!int.TryParse(request.zoneId, out int id))
                return BadRequest(ApiResponse<bool>.Fail("Invalid Zone ID format."));

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