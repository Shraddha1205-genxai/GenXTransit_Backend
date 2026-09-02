using GenXTransitAPI.DataAccess.Interface.IServices;
using GenXTransitAPI.Models;
using GenXTransitAPI.Models.DTO_s;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace GenXTransitAPI.Controllers
{
    [Route("api/zone")]
    [ApiController]
    [AllowAnonymous]
    public class ZoneController : BaseController  
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
            var result = await _svc.GetAllAsync(searchText, regionId, isActive, CurrentUserId, pageNumber, pageSize);  

            if (!result.Success)
                return BadRequest(new { success = false, message = result.Message });

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _svc.GetByIdAsync(id);

            if (!result.Success)
            {
                if (result.Message != null && result.Message.Contains("not found"))
                    return NotFound(new { success = false, message = result.Message });

                return BadRequest(new { success = false, message = result.Message });
            }

            return Ok(result);
        }

        [HttpGet("next-code")]
        public async Task<IActionResult> GetNextCode()
        {
            var result = await _svc.GetNextCodeAsync();

            if (!result.Success)
                return BadRequest(new { success = false, message = result.Message });

            return Ok(result);
        }

        [HttpPost("insert")]
        public async Task<IActionResult> Insert([FromBody] InsertZoneRequest request)
        {
            if (request == null)
                return BadRequest(new { success = false, message = "Invalid request data." });

            if (string.IsNullOrWhiteSpace(request.zoneName))
                return BadRequest(new { success = false, message = "Zone Name is required." });

            if (string.IsNullOrWhiteSpace(request.regionId))
                return BadRequest(new { success = false, message = "Region is required." });

            if (!int.TryParse(request.regionId, out int regionId))
                return BadRequest(new { success = false, message = "Invalid Region ID format." });

            var entity = new OrgZoneDTO
            {
                zoneName = request.zoneName,
                regionId = request.regionId,
                districts = request.districts ?? new List<string>(),
                isActive = request.isActive
            };

            var result = await _svc.InsertAsync(entity, CurrentUserId);  

            if (!result.Success)
                return BadRequest(new { success = false, message = result.Message });

            return Ok(result);
        }

        [HttpPost("update")]
        public async Task<IActionResult> Update([FromBody] UpdateZoneRequest request)
        {
            if (request == null)
                return BadRequest(new { success = false, message = "Invalid request data." });

            if (string.IsNullOrEmpty(request.zoneId))
                return BadRequest(new { success = false, message = "Zone ID is required." });

            if (!int.TryParse(request.zoneId, out int zoneId))
                return BadRequest(new { success = false, message = "Invalid Zone ID format." });

            if (string.IsNullOrWhiteSpace(request.zoneName))
                return BadRequest(new { success = false, message = "Zone Name is required." });

            if (string.IsNullOrWhiteSpace(request.regionId))
                return BadRequest(new { success = false, message = "Region is required." });

            if (!int.TryParse(request.regionId, out int regionId))
                return BadRequest(new { success = false, message = "Invalid Region ID format." });

            var entity = new OrgZoneDTO
            {
                zoneId = request.zoneId,
                zoneName = request.zoneName,
                regionId = request.regionId,
                districts = request.districts ?? new List<string>(),
                isActive = request.isActive
            };

            var result = await _svc.UpdateAsync(entity, CurrentUserId);  

            if (!result.Success)
            {
                if (result.Message != null && result.Message.Contains("not found"))
                    return NotFound(new { success = false, message = result.Message });

                return BadRequest(new { success = false, message = result.Message });
            }

            return Ok(result);
        }

        [HttpPost("delete")]
        public async Task<IActionResult> Delete([FromBody] DeleteZoneRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.zoneId))
                return BadRequest(new { success = false, message = "Zone ID is required." });

            if (!int.TryParse(request.zoneId, out int id))
                return BadRequest(new { success = false, message = "Invalid Zone ID format." });

            var result = await _svc.DeleteAsync(id, CurrentUserId);

            if (!result.Success)
            {
                if (result.Message != null && result.Message.Contains("not found"))
                    return NotFound(new { success = false, message = result.Message });

                return BadRequest(new { success = false, message = result.Message });
            }

            return Ok(result);
        }

    }
}