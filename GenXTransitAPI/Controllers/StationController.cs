using GenXTransitAPI.DataAccess.Interface.IServices;
using GenXTransitAPI.Models;
using GenXTransitAPI.Models.DTO_s;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GenXTransitAPI.Controllers
{
    [Route("api/station")]
    [ApiController]
    public class StationController : ControllerBase
    {
        private readonly IOrgStationService _svc;

        public StationController(IOrgStationService svc)
        {
            _svc = svc;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] string? searchText,
            [FromQuery] int? regionId,
            [FromQuery] int? divisionId,
            [FromQuery] int? depotId,
            [FromQuery] bool? isActive,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var result = await _svc.GetAllAsync(searchText, regionId, divisionId, depotId, isActive, GetCurrentUserId(), pageNumber, pageSize);

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
        public async Task<IActionResult> Insert([FromBody] InsertStationRequest request)
        {
            if (request == null)
                return BadRequest(new { success = false, message = "Invalid request data." });

            if (string.IsNullOrWhiteSpace(request.stationName))
                return BadRequest(new { success = false, message = "Station Name is required." });

            if (string.IsNullOrWhiteSpace(request.regionId))
                return BadRequest(new { success = false, message = "Region is required." });

            if (string.IsNullOrWhiteSpace(request.divisionId))
                return BadRequest(new { success = false, message = "Division is required." });

            if (string.IsNullOrWhiteSpace(request.depotId))
                return BadRequest(new { success = false, message = "Depot is required." });

            if (request.platforms < 0)
                return BadRequest(new { success = false, message = "Platforms cannot be negative." });

            if (request.dailyFootfall < 0)
                return BadRequest(new { success = false, message = "Daily Footfall cannot be negative." });

            // Validate IDs
            if (!int.TryParse(request.regionId, out int regionId))
                return BadRequest(new { success = false, message = "Invalid Region ID format." });

            if (!int.TryParse(request.divisionId, out int divisionId))
                return BadRequest(new { success = false, message = "Invalid Division ID format." });

            if (!int.TryParse(request.depotId, out int depotId))
                return BadRequest(new { success = false, message = "Invalid Depot ID format." });

            var entity = new OrgStationDTO
            {
                stationName = request.stationName,
                regionId = request.regionId,
                divisionId = request.divisionId,
                depotId = request.depotId,
                platforms = request.platforms,
                dailyFootfall = request.dailyFootfall,
                isActive = request.isActive
            };

            var result = await _svc.InsertAsync(entity, GetCurrentUserId());

            if (!result.Success)
                return BadRequest(new { success = false, message = result.Message });

            return Ok(result);
        }

        [HttpPost("update")]
        public async Task<IActionResult> Update([FromBody] UpdateStationRequest request)
        {
            if (request == null)
                return BadRequest(new { success = false, message = "Invalid request data." });

            if (string.IsNullOrEmpty(request.stationId))
                return BadRequest(new { success = false, message = "Station ID is required." });

            if (!int.TryParse(request.stationId, out int stationId))
                return BadRequest(new { success = false, message = "Invalid Station ID format." });

            if (string.IsNullOrWhiteSpace(request.stationName))
                return BadRequest(new { success = false, message = "Station Name is required." });

            if (string.IsNullOrWhiteSpace(request.regionId))
                return BadRequest(new { success = false, message = "Region is required." });

            if (string.IsNullOrWhiteSpace(request.divisionId))
                return BadRequest(new { success = false, message = "Division is required." });

            if (string.IsNullOrWhiteSpace(request.depotId))
                return BadRequest(new { success = false, message = "Depot is required." });

            if (request.platforms < 0)
                return BadRequest(new { success = false, message = "Platforms cannot be negative." });

            if (request.dailyFootfall < 0)
                return BadRequest(new { success = false, message = "Daily Footfall cannot be negative." });

            // Validate IDs
            if (!int.TryParse(request.regionId, out int regionId))
                return BadRequest(new { success = false, message = "Invalid Region ID format." });

            if (!int.TryParse(request.divisionId, out int divisionId))
                return BadRequest(new { success = false, message = "Invalid Division ID format." });

            if (!int.TryParse(request.depotId, out int depotId))
                return BadRequest(new { success = false, message = "Invalid Depot ID format." });

            var entity = new OrgStationDTO
            {
                stationId = request.stationId,
                stationName = request.stationName,
                regionId = request.regionId,
                divisionId = request.divisionId,
                depotId = request.depotId,
                platforms = request.platforms,
                dailyFootfall = request.dailyFootfall,
                isActive = request.isActive
            };

            var result = await _svc.UpdateAsync(entity, GetCurrentUserId());

            if (!result.Success)
            {
                if (result.Message != null && result.Message.Contains("not found"))
                    return NotFound(new { success = false, message = result.Message });

                return BadRequest(new { success = false, message = result.Message });
            }

            return Ok(result);
        }

        [HttpPost("delete")]
        public async Task<IActionResult> Delete([FromBody] DeleteStationRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.stationId))
                return BadRequest(new { success = false, message = "Station ID is required." });

            if (!int.TryParse(request.stationId, out int id))
                return BadRequest(new { success = false, message = "Invalid Station ID format." });

            var result = await _svc.DeleteAsync(id, GetCurrentUserId());

            if (!result.Success)
            {
                if (result.Message != null && result.Message.Contains("not found"))
                    return NotFound(new { success = false, message = result.Message });

                return BadRequest(new { success = false, message = result.Message });
            }

            return Ok(result);
        }

        private int GetCurrentUserId()
        {
            return 1;
        }
    }
}