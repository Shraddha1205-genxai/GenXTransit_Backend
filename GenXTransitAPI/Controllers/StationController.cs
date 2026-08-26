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
        public async Task<IActionResult> Insert([FromBody] InsertStationRequest request)
        {
            if (request == null)
                return BadRequest(ApiResponse<int>.Fail("Invalid request data."));

            if (string.IsNullOrWhiteSpace(request.stationName))
                return BadRequest(ApiResponse<int>.Fail("Station Name is required."));

            if (string.IsNullOrWhiteSpace(request.regionId))
                return BadRequest(ApiResponse<int>.Fail("Region is required."));

            if (string.IsNullOrWhiteSpace(request.divisionId))
                return BadRequest(ApiResponse<int>.Fail("Division is required."));

            if (string.IsNullOrWhiteSpace(request.depotId))
                return BadRequest(ApiResponse<int>.Fail("Depot is required."));

            if (request.platforms < 0)
                return BadRequest(ApiResponse<int>.Fail("Platforms cannot be negative."));

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
            if (result.Success && result.Data > 0)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("update")]
        public async Task<IActionResult> Update([FromBody] UpdateStationRequest request)
        {
            if (request == null)
                return BadRequest(ApiResponse<bool>.Fail("Invalid request data."));

            if (string.IsNullOrEmpty(request.stationId))
                return BadRequest(ApiResponse<bool>.Fail("Station ID is required."));

            if (!int.TryParse(request.stationId, out int id))
                return BadRequest(ApiResponse<bool>.Fail("Invalid Station ID format."));

            if (string.IsNullOrWhiteSpace(request.stationName))
                return BadRequest(ApiResponse<bool>.Fail("Station Name is required."));

            if (string.IsNullOrWhiteSpace(request.regionId))
                return BadRequest(ApiResponse<bool>.Fail("Region is required."));

            if (string.IsNullOrWhiteSpace(request.divisionId))
                return BadRequest(ApiResponse<bool>.Fail("Division is required."));

            if (string.IsNullOrWhiteSpace(request.depotId))
                return BadRequest(ApiResponse<bool>.Fail("Depot is required."));

            if (request.platforms < 0)
                return BadRequest(ApiResponse<bool>.Fail("Platforms cannot be negative."));

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
                if (!string.IsNullOrEmpty(result.Message) && result.Message.Contains("not found"))
                    return NotFound(result);
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPost("delete")]
        public async Task<IActionResult> Delete([FromBody] DeleteStationRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.stationId))
                return BadRequest(ApiResponse<bool>.Fail("Station ID is required."));

            if (!int.TryParse(request.stationId, out int id))
                return BadRequest(ApiResponse<bool>.Fail("Invalid Station ID format."));

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