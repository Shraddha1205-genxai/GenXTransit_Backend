using GenXTransitAPI.DataAccess.Interface.IServices;
using GenXTransitAPI.Models;
using GenXTransitAPI.Models.DTO_s;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GenXTransitAPI.Controllers
{
    [Route("api/division")]
    [ApiController]
    public class DivisionController : ControllerBase
    {
        private readonly IOrgDivisionService _svc;

        public DivisionController(IOrgDivisionService svc)
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
        public async Task<IActionResult> Insert([FromBody] InsertDivisionRequest request)
        {
            if (request == null)
                return BadRequest(new { success = false, message = "Invalid request data." });

            if (string.IsNullOrWhiteSpace(request.divisionName))
                return BadRequest(new { success = false, message = "Division Name is required." });

            if (string.IsNullOrWhiteSpace(request.regionId))
                return BadRequest(new { success = false, message = "Region is required." });

            if (!int.TryParse(request.regionId, out int regionId))
                return BadRequest(new { success = false, message = "Invalid Region ID format." });

            var entity = new OrgDivisionDTO
            {
                divisionName = request.divisionName,
                regionId = request.regionId,
                isActive = request.isActive
            };

            var result = await _svc.InsertAsync(entity, GetCurrentUserId());

            if (!result.Success)
                return BadRequest(new { success = false, message = result.Message });

            return Ok(result);
        }

        [HttpPost("update")]
        public async Task<IActionResult> Update([FromBody] UpdateDivisionRequest request)
        {
            if (request == null)
                return BadRequest(new { success = false, message = "Invalid request data." });

            if (string.IsNullOrEmpty(request.divisionId))
                return BadRequest(new { success = false, message = "Division ID is required." });

            if (!int.TryParse(request.divisionId, out int divisionId))
                return BadRequest(new { success = false, message = "Invalid Division ID format." });

            if (string.IsNullOrWhiteSpace(request.divisionName))
                return BadRequest(new { success = false, message = "Division Name is required." });

            if (string.IsNullOrWhiteSpace(request.regionId))
                return BadRequest(new { success = false, message = "Region is required." });

            if (!int.TryParse(request.regionId, out int regionId))
                return BadRequest(new { success = false, message = "Invalid Region ID format." });

            var entity = new OrgDivisionDTO
            {
                divisionId = request.divisionId,
                divisionName = request.divisionName,
                regionId = request.regionId,
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
        public async Task<IActionResult> Delete([FromBody] DeleteDivisionRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.divisionId))
                return BadRequest(new { success = false, message = "Division ID is required." });

            if (!int.TryParse(request.divisionId, out int id))
                return BadRequest(new { success = false, message = "Invalid Division ID format." });

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