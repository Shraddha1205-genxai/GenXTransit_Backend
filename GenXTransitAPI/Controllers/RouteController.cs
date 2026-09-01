using GenXTransitAPI.DataAccess.Interface.IServices;
using GenXTransitAPI.Models;
using GenXTransitAPI.Models.DTO_s;
using Microsoft.AspNetCore.Mvc;
using System;
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
        public async Task<IActionResult> Insert([FromBody] InsertRouteRequest request)
        {
            if (request == null)
                return BadRequest(new { success = false, message = "Invalid request data." });

            if (string.IsNullOrWhiteSpace(request.routeName))
                return BadRequest(new { success = false, message = "Route Name is required." });

            if (string.IsNullOrWhiteSpace(request.service))
                return BadRequest(new { success = false, message = "Service is required." });

            if (string.IsNullOrWhiteSpace(request.fromStationId))
                return BadRequest(new { success = false, message = "From Station is required." });

            if (string.IsNullOrWhiteSpace(request.toStationId))
                return BadRequest(new { success = false, message = "To Station is required." });

            if (string.IsNullOrWhiteSpace(request.type))
                return BadRequest(new { success = false, message = "Type is required." });

            if (request.distance <= 0)
                return BadRequest(new { success = false, message = "Distance must be greater than 0." });

            if (string.IsNullOrWhiteSpace(request.fareModel))
                return BadRequest(new { success = false, message = "Fare Model is required." });

            if (request.duration == null || request.duration == TimeSpan.Zero)
                return BadRequest(new { success = false, message = "Duration is required." });

            // Validate IDs
            if (!int.TryParse(request.fromStationId, out int fromStationId))
                return BadRequest(new { success = false, message = "Invalid From Station ID format." });

            if (!int.TryParse(request.toStationId, out int toStationId))
                return BadRequest(new { success = false, message = "Invalid To Station ID format." });

            // Check if From and To stations are different
            if (fromStationId == toStationId)
                return BadRequest(new { success = false, message = "From and To stations cannot be the same." });

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

            if (!result.Success)
                return BadRequest(new { success = false, message = result.Message });

            return Ok(result);
        }

        [HttpPost("update")]
        public async Task<IActionResult> Update([FromBody] UpdateRouteRequest request)
        {
            if (request == null)
                return BadRequest(new { success = false, message = "Invalid request data." });

            if (string.IsNullOrEmpty(request.routeId))
                return BadRequest(new { success = false, message = "Route ID is required." });

            if (!int.TryParse(request.routeId, out int routeId))
                return BadRequest(new { success = false, message = "Invalid Route ID format." });

            if (string.IsNullOrWhiteSpace(request.routeName))
                return BadRequest(new { success = false, message = "Route Name is required." });

            if (string.IsNullOrWhiteSpace(request.service))
                return BadRequest(new { success = false, message = "Service is required." });

            if (string.IsNullOrWhiteSpace(request.fromStationId))
                return BadRequest(new { success = false, message = "From Station is required." });

            if (string.IsNullOrWhiteSpace(request.toStationId))
                return BadRequest(new { success = false, message = "To Station is required." });

            if (string.IsNullOrWhiteSpace(request.type))
                return BadRequest(new { success = false, message = "Type is required." });

            if (request.distance <= 0)
                return BadRequest(new { success = false, message = "Distance must be greater than 0." });

            if (string.IsNullOrWhiteSpace(request.fareModel))
                return BadRequest(new { success = false, message = "Fare Model is required." });

            if (request.duration == null || request.duration == TimeSpan.Zero)
                return BadRequest(new { success = false, message = "Duration is required." });

            // Validate IDs
            if (!int.TryParse(request.fromStationId, out int fromStationId))
                return BadRequest(new { success = false, message = "Invalid From Station ID format." });

            if (!int.TryParse(request.toStationId, out int toStationId))
                return BadRequest(new { success = false, message = "Invalid To Station ID format." });

            // Check if From and To stations are different
            if (fromStationId == toStationId)
                return BadRequest(new { success = false, message = "From and To stations cannot be the same." });

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
                if (result.Message != null && result.Message.Contains("not found"))
                    return NotFound(new { success = false, message = result.Message });

                return BadRequest(new { success = false, message = result.Message });
            }

            return Ok(result);
        }

        [HttpPost("delete")]
        public async Task<IActionResult> Delete([FromBody] DeleteRouteRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.routeId))
                return BadRequest(new { success = false, message = "Route ID is required." });

            if (!int.TryParse(request.routeId, out int id))
                return BadRequest(new { success = false, message = "Invalid Route ID format." });

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