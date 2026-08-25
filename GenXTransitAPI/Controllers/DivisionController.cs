using GenXTransitAPI.DataAccess.Interfaces.Services;
using GenXTransitAPI.Models;
using GenXTransitAPI.Models.DTO_s;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using static GenXTransitAPI.Models.DTO_s.OrgDivisionDTO;

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
            [FromQuery] int pageNumber = 1,    // ✅ ADD THIS
            [FromQuery] int pageSize = 10)     // ✅ ADD THIS
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
        public async Task<IActionResult> Insert([FromBody] InsertDivisionRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.divisionName))
                return BadRequest(ApiResponse<int>.Fail("Division Name is required."));

            if (string.IsNullOrWhiteSpace(request.regionId))
                return BadRequest(ApiResponse<int>.Fail("Region is required."));

            var entity = new OrgDivisionDTO
            {
                divisionName = request.divisionName,
                regionId = request.regionId,
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
        public async Task<IActionResult> Update([FromBody] UpdateDivisionRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.divisionId))
                return BadRequest(ApiResponse<bool>.Fail("Division ID is required."));

            if (string.IsNullOrWhiteSpace(request.divisionName))
                return BadRequest(ApiResponse<bool>.Fail("Division Name is required."));

            if (string.IsNullOrWhiteSpace(request.regionId))
                return BadRequest(ApiResponse<bool>.Fail("Region is required."));

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
                if (!string.IsNullOrEmpty(result.Message) && result.Message.Contains("not found"))
                    return NotFound(result);
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPost("delete")]
        public async Task<IActionResult> Delete([FromBody] DeleteDivisionRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.divisionId))
                return BadRequest(ApiResponse<bool>.Fail("Division ID is required."));

            if (!int.TryParse(request.divisionId, out int id))
                return BadRequest(ApiResponse<bool>.Fail("Invalid Division ID format."));

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