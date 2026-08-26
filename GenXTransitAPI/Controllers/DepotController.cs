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
        public async Task<IActionResult> Insert([FromBody] InsertDepotRequest request)
        {
            if (request == null)
                return BadRequest(ApiResponse<int>.Fail("Invalid request data."));

            if (string.IsNullOrWhiteSpace(request.depotName))
                return BadRequest(ApiResponse<int>.Fail("Depot Name is required."));

            if (string.IsNullOrWhiteSpace(request.corpId))
                return BadRequest(ApiResponse<int>.Fail("Corporation is required."));

            if (string.IsNullOrWhiteSpace(request.regionId))
                return BadRequest(ApiResponse<int>.Fail("Region is required."));

            if (string.IsNullOrWhiteSpace(request.divisionId))
                return BadRequest(ApiResponse<int>.Fail("Division is required."));

            if (string.IsNullOrWhiteSpace(request.zoneId))
                return BadRequest(ApiResponse<int>.Fail("Zone is required."));

            if (string.IsNullOrWhiteSpace(request.service))
                return BadRequest(ApiResponse<int>.Fail("Service is required."));

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
            if (result.Success && result.Data > 0)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("update")]
        public async Task<IActionResult> Update([FromBody] UpdateDepotRequest request)
        {
            if (request == null)
                return BadRequest(ApiResponse<bool>.Fail("Invalid request data."));

            if (string.IsNullOrEmpty(request.depotId))
                return BadRequest(ApiResponse<bool>.Fail("Depot ID is required."));

            if (!int.TryParse(request.depotId, out int id))
                return BadRequest(ApiResponse<bool>.Fail("Invalid Depot ID format."));

            if (string.IsNullOrWhiteSpace(request.depotName))
                return BadRequest(ApiResponse<bool>.Fail("Depot Name is required."));

            if (string.IsNullOrWhiteSpace(request.corpId))
                return BadRequest(ApiResponse<bool>.Fail("Corporation is required."));

            if (string.IsNullOrWhiteSpace(request.regionId))
                return BadRequest(ApiResponse<bool>.Fail("Region is required."));

            if (string.IsNullOrWhiteSpace(request.divisionId))
                return BadRequest(ApiResponse<bool>.Fail("Division is required."));

            if (string.IsNullOrWhiteSpace(request.zoneId))
                return BadRequest(ApiResponse<bool>.Fail("Zone is required."));

            if (string.IsNullOrWhiteSpace(request.service))
                return BadRequest(ApiResponse<bool>.Fail("Service is required."));

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
                if (!string.IsNullOrEmpty(result.Message) && result.Message.Contains("not found"))
                    return NotFound(result);
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPost("delete")]
        public async Task<IActionResult> Delete([FromBody] DeleteDepotRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.depotId))
                return BadRequest(ApiResponse<bool>.Fail("Depot ID is required."));

            if (!int.TryParse(request.depotId, out int id))
                return BadRequest(ApiResponse<bool>.Fail("Invalid Depot ID format."));

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