using GenXTransitAPI.DataAccess.Interface.IServices;
using GenXTransitAPI.Models;
using GenXTransitAPI.Models.DTO_s;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GenXTransitAPI.Controllers
{
    [Route("api/depot")]
    [ApiController]
    public class DepotController : ControllerBase
    {
        private readonly IOrgDepotService _svc;

        public DepotController(IOrgDepotService svc)
        {
            _svc = svc;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] string? searchText,
            [FromQuery] int? corporationId,
            [FromQuery] int? regionId,
            [FromQuery] int? divisionId,
            [FromQuery] int? zoneId,
            [FromQuery] bool? isActive,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var result = await _svc.GetAllAsync(searchText, corporationId, regionId, divisionId, zoneId, isActive, GetCurrentUserId(), pageNumber, pageSize);

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
        public async Task<IActionResult> Insert([FromBody] InsertDepotRequest request)
        {
            if (request == null)
                return BadRequest(new { success = false, message = "Invalid request data." });

            if (string.IsNullOrWhiteSpace(request.depotName))
                return BadRequest(new { success = false, message = "Depot Name is required." });

            if (string.IsNullOrWhiteSpace(request.corpId))
                return BadRequest(new { success = false, message = "Corporation is required." });

            if (string.IsNullOrWhiteSpace(request.regionId))
                return BadRequest(new { success = false, message = "Region is required." });

            if (string.IsNullOrWhiteSpace(request.divisionId))
                return BadRequest(new { success = false, message = "Division is required." });

            if (string.IsNullOrWhiteSpace(request.zoneId))
                return BadRequest(new { success = false, message = "Zone is required." });

            if (string.IsNullOrWhiteSpace(request.service))
                return BadRequest(new { success = false, message = "Service is required." });

            // Validate IDs
            if (!int.TryParse(request.corpId, out int corpId))
                return BadRequest(new { success = false, message = "Invalid Corporation ID format." });

            if (!int.TryParse(request.regionId, out int regionId))
                return BadRequest(new { success = false, message = "Invalid Region ID format." });

            if (!int.TryParse(request.divisionId, out int divisionId))
                return BadRequest(new { success = false, message = "Invalid Division ID format." });

            if (!int.TryParse(request.zoneId, out int zoneId))
                return BadRequest(new { success = false, message = "Invalid Zone ID format." });

            var entity = new OrgDepotDTO
            {
                depotName = request.depotName,
                corpId = request.corpId,
                regionId = request.regionId,
                divisionId = request.divisionId,
                zoneId = request.zoneId,
                service = request.service,
                isActive = request.isActive
            };

            var result = await _svc.InsertAsync(entity, GetCurrentUserId());

            if (!result.Success)
                return BadRequest(new { success = false, message = result.Message });

            return Ok(result);
        }

        [HttpPost("update")]
        public async Task<IActionResult> Update([FromBody] UpdateDepotRequest request)
        {
            if (request == null)
                return BadRequest(new { success = false, message = "Invalid request data." });

            if (string.IsNullOrEmpty(request.depotId))
                return BadRequest(new { success = false, message = "Depot ID is required." });

            if (!int.TryParse(request.depotId, out int depotId))
                return BadRequest(new { success = false, message = "Invalid Depot ID format." });

            if (string.IsNullOrWhiteSpace(request.depotName))
                return BadRequest(new { success = false, message = "Depot Name is required." });

            if (string.IsNullOrWhiteSpace(request.corpId))
                return BadRequest(new { success = false, message = "Corporation is required." });

            if (string.IsNullOrWhiteSpace(request.regionId))
                return BadRequest(new { success = false, message = "Region is required." });

            if (string.IsNullOrWhiteSpace(request.divisionId))
                return BadRequest(new { success = false, message = "Division is required." });

            if (string.IsNullOrWhiteSpace(request.zoneId))
                return BadRequest(new { success = false, message = "Zone is required." });

            if (string.IsNullOrWhiteSpace(request.service))
                return BadRequest(new { success = false, message = "Service is required." });

            // Validate IDs
            if (!int.TryParse(request.corpId, out int corpId))
                return BadRequest(new { success = false, message = "Invalid Corporation ID format." });

            if (!int.TryParse(request.regionId, out int regionId))
                return BadRequest(new { success = false, message = "Invalid Region ID format." });

            if (!int.TryParse(request.divisionId, out int divisionId))
                return BadRequest(new { success = false, message = "Invalid Division ID format." });

            if (!int.TryParse(request.zoneId, out int zoneId))
                return BadRequest(new { success = false, message = "Invalid Zone ID format." });

            var entity = new OrgDepotDTO
            {
                depotId = request.depotId,
                depotName = request.depotName,
                corpId = request.corpId,
                regionId = request.regionId,
                divisionId = request.divisionId,
                zoneId = request.zoneId,
                service = request.service,
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
        public async Task<IActionResult> Delete([FromBody] DeleteDepotRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.depotId))
                return BadRequest(new { success = false, message = "Depot ID is required." });

            if (!int.TryParse(request.depotId, out int id))
                return BadRequest(new { success = false, message = "Invalid Depot ID format." });

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