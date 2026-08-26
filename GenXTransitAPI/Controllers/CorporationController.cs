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
            [FromQuery] bool? isActive,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var result = await _svc.GetAllAsync(searchText, stateName, isActive, GetCurrentUserId(), pageNumber, pageSize);
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
        public async Task<IActionResult> Insert([FromBody] InsertCorporationRequest request)
        {
            if (request == null)
                return BadRequest(ApiResponse<int>.Fail("Invalid request data."));

            if (string.IsNullOrWhiteSpace(request.corporationName))
                return BadRequest(ApiResponse<int>.Fail("Corporation Name is required."));

            if (string.IsNullOrWhiteSpace(request.stateName))
                return BadRequest(ApiResponse<int>.Fail("State Name is required."));

            if (string.IsNullOrWhiteSpace(request.districtName))
                return BadRequest(ApiResponse<int>.Fail("District Name is required."));

            if (string.IsNullOrWhiteSpace(request.cityName))
                return BadRequest(ApiResponse<int>.Fail("City Name is required."));

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
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost("update")]
        public async Task<IActionResult> Update([FromBody] UpdateCorporationRequest request)
        {
            if (request == null)
                return BadRequest(ApiResponse<bool>.Fail("Invalid request data."));

            if (string.IsNullOrEmpty(request.corporationId))
                return BadRequest(ApiResponse<bool>.Fail("Corporation ID is required."));

            if (!int.TryParse(request.corporationId, out int id))
                return BadRequest(ApiResponse<bool>.Fail("Invalid Corporation ID format."));

            if (string.IsNullOrWhiteSpace(request.corporationName))
                return BadRequest(ApiResponse<bool>.Fail("Corporation Name is required."));

            if (string.IsNullOrWhiteSpace(request.stateName))
                return BadRequest(ApiResponse<bool>.Fail("State Name is required."));

            if (string.IsNullOrWhiteSpace(request.districtName))
                return BadRequest(ApiResponse<bool>.Fail("District Name is required."));

            if (string.IsNullOrWhiteSpace(request.cityName))
                return BadRequest(ApiResponse<bool>.Fail("City Name is required."));

            var entity = new OrgCorporationDTO
            {
                corpId = id.ToString(),
                corporationName = request.corporationName,
                stateName = request.stateName,
                districtName = request.districtName,
                cityName = request.cityName,
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
        public async Task<IActionResult> Delete([FromBody] DeleteCorporationRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.corporationId))
                return BadRequest(ApiResponse<bool>.Fail("Corporation ID is required."));

            if (!int.TryParse(request.corporationId, out int id))
                return BadRequest(ApiResponse<bool>.Fail("Invalid Corporation ID format."));

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