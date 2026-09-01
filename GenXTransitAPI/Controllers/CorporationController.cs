using GenXTransitAPI.DataAccess.Interface.IServices;
using GenXTransitAPI.Models;
using GenXTransitAPI.Models.DTO_s;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GenXTransitAPI.Controllers
{
    [Route("api/corporation")]
    [ApiController]
    public class CorporationController : ControllerBase
    {
        private readonly IOrgCorporationService _svc;

        public CorporationController(IOrgCorporationService svc)
        {
            _svc = svc;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] string? searchText,
            [FromQuery] string? stateName,
            [FromQuery] string? districtName,
            [FromQuery] string? cityName,
            [FromQuery] bool? isActive,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var result = await _svc.GetAllAsync(
                searchText,
                stateName,
                districtName,
                cityName,
                isActive,
                GetCurrentUserId(),
                pageNumber,
                pageSize);

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
        public async Task<IActionResult> Insert([FromBody] InsertCorporationRequest request)
        {
            if (request == null)
                return BadRequest(new { success = false, message = "Invalid request data." });

            if (string.IsNullOrWhiteSpace(request.corporationName))
                return BadRequest(new { success = false, message = "Corporation Name is required." });

            if (string.IsNullOrWhiteSpace(request.stateName))
                return BadRequest(new { success = false, message = "State Name is required." });

            if (string.IsNullOrWhiteSpace(request.districtName))
                return BadRequest(new { success = false, message = "District Name is required." });

            if (string.IsNullOrWhiteSpace(request.cityName))
                return BadRequest(new { success = false, message = "City Name is required." });

            var entity = new OrgCorporationDTO
            {
                corporationName = request.corporationName,
                stateName = request.stateName,
                districtName = request.districtName,
                cityName = request.cityName,
                isActive = request.isActive
            };

            var result = await _svc.InsertAsync(entity, GetCurrentUserId());

            if (!result.Success)
                return BadRequest(new { success = false, message = result.Message });

            return Ok(result);
        }

        [HttpPost("update")]
        public async Task<IActionResult> Update([FromBody] UpdateCorporationRequest request)
        {
            if (request == null)
                return BadRequest(new { success = false, message = "Invalid request data." });

            if (string.IsNullOrEmpty(request.corporationId))
                return BadRequest(new { success = false, message = "Corporation ID is required." });

            if (!int.TryParse(request.corporationId, out int id))
                return BadRequest(new { success = false, message = "Invalid Corporation ID format." });

            if (string.IsNullOrWhiteSpace(request.corporationName))
                return BadRequest(new { success = false, message = "Corporation Name is required." });

            if (string.IsNullOrWhiteSpace(request.stateName))
                return BadRequest(new { success = false, message = "State Name is required." });

            if (string.IsNullOrWhiteSpace(request.districtName))
                return BadRequest(new { success = false, message = "District Name is required." });

            if (string.IsNullOrWhiteSpace(request.cityName))
                return BadRequest(new { success = false, message = "City Name is required." });

            var entity = new OrgCorporationDTO
            {
                corpId = request.corporationId,
                corporationName = request.corporationName,
                stateName = request.stateName,
                districtName = request.districtName,
                cityName = request.cityName,
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
        public async Task<IActionResult> Delete([FromBody] DeleteCorporationRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.corporationId))
                return BadRequest(new { success = false, message = "Corporation ID is required." });

            if (!int.TryParse(request.corporationId, out int id))
                return BadRequest(new { success = false, message = "Invalid Corporation ID format." });

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