using GenXTransitAPI.DataAccess.Interface.IServices;
using GenXTransitAPI.Models;
using GenXTransitAPI.Models.DTO_s;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GenXTransitAPI.Controllers
{
    [Route("api/stop")]
    [ApiController]
    public class StopController : ControllerBase
    {
        private readonly IStopService _svc;

        public StopController(IStopService svc)
        {
            _svc = svc;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] string? searchText,
            [FromQuery] int? routeId,
            [FromQuery] bool? isActive,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var result = await _svc.GetAllAsync(searchText, routeId, isActive, GetCurrentUserId(), pageNumber, pageSize);
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
        public async Task<IActionResult> Insert([FromBody] InsertStopRequest request)
        {
            if (request == null)
                return BadRequest(ApiResponse<int>.Fail("Invalid request data."));

            if (string.IsNullOrWhiteSpace(request.stopName))
                return BadRequest(ApiResponse<int>.Fail("Stop Name is required."));

            if (string.IsNullOrWhiteSpace(request.routeId))
                return BadRequest(ApiResponse<int>.Fail("Route is required."));

            if (request.stopOrder < 1)
                return BadRequest(ApiResponse<int>.Fail("Stop Order must be greater than 0."));

            var entity = new StopDTO
            {
                stopName = request.stopName,
                routeId = request.routeId,
                stopOrder = request.stopOrder,
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
        public async Task<IActionResult> Update([FromBody] UpdateStopRequest request)
        {
            if (request == null)
                return BadRequest(ApiResponse<bool>.Fail("Invalid request data."));

            if (string.IsNullOrEmpty(request.stopId))
                return BadRequest(ApiResponse<bool>.Fail("Stop ID is required."));

            if (!int.TryParse(request.stopId, out int id))
                return BadRequest(ApiResponse<bool>.Fail("Invalid Stop ID format."));

            if (string.IsNullOrWhiteSpace(request.stopName))
                return BadRequest(ApiResponse<bool>.Fail("Stop Name is required."));

            if (string.IsNullOrWhiteSpace(request.routeId))
                return BadRequest(ApiResponse<bool>.Fail("Route is required."));

            if (request.stopOrder < 1)
                return BadRequest(ApiResponse<bool>.Fail("Stop Order must be greater than 0."));

            var entity = new StopDTO
            {
                stopId = request.stopId,
                stopName = request.stopName,
                routeId = request.routeId,
                stopOrder = request.stopOrder,
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
        public async Task<IActionResult> Delete([FromBody] DeleteStopRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.stopId))
                return BadRequest(ApiResponse<bool>.Fail("Stop ID is required."));

            if (!int.TryParse(request.stopId, out int id))
                return BadRequest(ApiResponse<bool>.Fail("Invalid Stop ID format."));

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