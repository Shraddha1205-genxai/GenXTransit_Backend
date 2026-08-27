using GenXTransitAPI.DataAccess.Interface.IServices;
using GenXTransitAPI.Models;
using GenXTransitAPI.Models.DTO_s;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GenXTransitAPI.Controllers
{
    [Route("api/route")]
    [ApiController]
    public class RouteController : ControllerBase
    {
        private readonly IRouteService _svc;

        public RouteController(IRouteService svc)
        {
            _svc = svc;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] string? searchText,
            [FromQuery] string? service,
            [FromQuery] string? type,
            [FromQuery] bool? isActive,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var result = await _svc.GetAllAsync(searchText, service, type, isActive, GetCurrentUserId(), pageNumber, pageSize);
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
        public async Task<IActionResult> Insert([FromBody] InsertRouteRequest request)
        {
            if (request == null)
                return BadRequest(ApiResponse<int>.Fail("Invalid request data."));

            if (string.IsNullOrWhiteSpace(request.routeName))
                return BadRequest(ApiResponse<int>.Fail("Route Name is required."));

            if (string.IsNullOrWhiteSpace(request.service))
                return BadRequest(ApiResponse<int>.Fail("Service is required."));

            if (string.IsNullOrWhiteSpace(request.fromStationId))
                return BadRequest(ApiResponse<int>.Fail("From Station is required."));

            if (string.IsNullOrWhiteSpace(request.toStationId))
                return BadRequest(ApiResponse<int>.Fail("To Station is required."));

            if (string.IsNullOrWhiteSpace(request.type))
                return BadRequest(ApiResponse<int>.Fail("Type is required."));

            if (request.distance <= 0)
                return BadRequest(ApiResponse<int>.Fail("Distance must be greater than 0."));

            if (string.IsNullOrWhiteSpace(request.fareModel))
                return BadRequest(ApiResponse<int>.Fail("Fare Model is required."));

            if (request.duration == null || request.duration == TimeSpan.Zero)
                return BadRequest(ApiResponse<int>.Fail("Duration is required."));

            var entity = new RouteDTO
            {
                routeName = request.routeName,
                service = request.service,
                fromStationId = request.fromStationId,
                toStationId = request.toStationId,
                type = request.type,
                distance = request.distance,
                fareModel = request.fareModel,
                duration = request.duration,
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
        public async Task<IActionResult> Update([FromBody] UpdateRouteRequest request)
        {
            if (request == null)
                return BadRequest(ApiResponse<bool>.Fail("Invalid request data."));

            if (string.IsNullOrEmpty(request.routeId))
                return BadRequest(ApiResponse<bool>.Fail("Route ID is required."));

            if (!int.TryParse(request.routeId, out int id))
                return BadRequest(ApiResponse<bool>.Fail("Invalid Route ID format."));

            if (string.IsNullOrWhiteSpace(request.routeName))
                return BadRequest(ApiResponse<bool>.Fail("Route Name is required."));

            if (string.IsNullOrWhiteSpace(request.service))
                return BadRequest(ApiResponse<bool>.Fail("Service is required."));

            if (string.IsNullOrWhiteSpace(request.fromStationId))
                return BadRequest(ApiResponse<bool>.Fail("From Station is required."));

            if (string.IsNullOrWhiteSpace(request.toStationId))
                return BadRequest(ApiResponse<bool>.Fail("To Station is required."));

            if (string.IsNullOrWhiteSpace(request.type))
                return BadRequest(ApiResponse<bool>.Fail("Type is required."));

            if (request.distance <= 0)
                return BadRequest(ApiResponse<bool>.Fail("Distance must be greater than 0."));

            if (string.IsNullOrWhiteSpace(request.fareModel))
                return BadRequest(ApiResponse<bool>.Fail("Fare Model is required."));

            if (request.duration == null || request.duration == TimeSpan.Zero)
                return BadRequest(ApiResponse<bool>.Fail("Duration is required."));

            var entity = new RouteDTO
            {
                routeId = request.routeId,
                routeName = request.routeName,
                service = request.service,
                fromStationId = request.fromStationId,
                toStationId = request.toStationId,
                type = request.type,
                distance = request.distance,
                fareModel = request.fareModel,
                duration = request.duration,
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
        public async Task<IActionResult> Delete([FromBody] DeleteRouteRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.routeId))
                return BadRequest(ApiResponse<bool>.Fail("Route ID is required."));

            if (!int.TryParse(request.routeId, out int id))
                return BadRequest(ApiResponse<bool>.Fail("Invalid Route ID format."));

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