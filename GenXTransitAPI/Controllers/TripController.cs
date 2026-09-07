using GenXTransitAPI.DataAccess.Interface.IServices;
using GenXTransitAPI.Models;
using GenXTransitAPI.Models.DTO_s;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace GenXTransitAPI.Controllers
{
    [Route("api/trip")]
    [ApiController]
    [AllowAnonymous]  // Development: No auth required
    public class TripController : BaseController
    {
        private readonly ITripService _svc;

        public TripController(ITripService svc)
        {
            _svc = svc;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] string? searchText,
            [FromQuery] int? depotId,
            [FromQuery] int? routeId,
            [FromQuery] int? fleetId,
            [FromQuery] string? tripStatus,
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate,
            [FromQuery] bool? isActive,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var result = await _svc.GetAllAsync(searchText, depotId, routeId, fleetId, tripStatus, startDate, endDate, isActive, CurrentUserId, pageNumber, pageSize);

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

        [HttpPost("insert")]
        public async Task<IActionResult> Insert([FromBody] InsertTripRequest request)
        {
            if (request == null)
                return BadRequest(new { success = false, message = "Invalid request data." });

            if (string.IsNullOrWhiteSpace(request.depotId))
                return BadRequest(new { success = false, message = "Depot is required." });

            if (string.IsNullOrWhiteSpace(request.routeId))
                return BadRequest(new { success = false, message = "Route is required." });

            if (string.IsNullOrWhiteSpace(request.fleetId))
                return BadRequest(new { success = false, message = "Fleet is required." });

            if (string.IsNullOrWhiteSpace(request.driverId))
                return BadRequest(new { success = false, message = "Driver is required." });

            if (string.IsNullOrWhiteSpace(request.conductorId))
                return BadRequest(new { success = false, message = "Conductor is required." });

            if (string.IsNullOrWhiteSpace(request.scheduleTime))
                return BadRequest(new { success = false, message = "Schedule Time is required." });

            if (!int.TryParse(request.depotId, out int depotId))
                return BadRequest(new { success = false, message = "Invalid Depot ID format." });

            if (!int.TryParse(request.routeId, out int routeId))
                return BadRequest(new { success = false, message = "Invalid Route ID format." });

            if (!int.TryParse(request.fleetId, out int fleetId))
                return BadRequest(new { success = false, message = "Invalid Fleet ID format." });

            if (!int.TryParse(request.driverId, out int driverId))
                return BadRequest(new { success = false, message = "Invalid Driver ID format." });

            if (!int.TryParse(request.conductorId, out int conductorId))
                return BadRequest(new { success = false, message = "Invalid Conductor ID format." });

            if (!DateTime.TryParse(request.scheduleTime, out DateTime scheduleTime))
                return BadRequest(new { success = false, message = "Invalid Schedule Time format." });

            if (scheduleTime < DateTime.Now)
                return BadRequest(new { success = false, message = "Schedule Time cannot be in the past." });

            var validStatuses = new[] { "Scheduled", "OnTime", "Delayed", "Completed", "Cancelled" };
            if (!string.IsNullOrWhiteSpace(request.tripStatus) && !validStatuses.Contains(request.tripStatus))
                return BadRequest(new { success = false, message = "Invalid Trip Status. Valid statuses are: Scheduled, OnTime, Delayed, Completed, Cancelled." });

            var entity = new TripDTO
            {
                depotId = request.depotId,
                routeId = request.routeId,
                fleetId = request.fleetId,
                driverId = request.driverId,
                conductorId = request.conductorId,
                scheduleTime = request.scheduleTime,
                actualTime = request.actualTime,
                tripStatus = request.tripStatus ?? "Scheduled",
                isActive = request.isActive
            };

            var result = await _svc.InsertAsync(entity, CurrentUserId);

            if (!result.Success)
                return BadRequest(new { success = false, message = result.Message });

            return Ok(result);
        }

        [HttpPost("update")]
        public async Task<IActionResult> Update([FromBody] UpdateTripRequest request)
        {
            if (request == null)
                return BadRequest(new { success = false, message = "Invalid request data." });

            if (string.IsNullOrEmpty(request.tripId))
                return BadRequest(new { success = false, message = "Trip ID is required." });

            if (!int.TryParse(request.tripId, out int tripId))
                return BadRequest(new { success = false, message = "Invalid Trip ID format." });

            if (string.IsNullOrWhiteSpace(request.depotId))
                return BadRequest(new { success = false, message = "Depot is required." });

            if (string.IsNullOrWhiteSpace(request.routeId))
                return BadRequest(new { success = false, message = "Route is required." });

            if (string.IsNullOrWhiteSpace(request.fleetId))
                return BadRequest(new { success = false, message = "Fleet is required." });

            if (string.IsNullOrWhiteSpace(request.driverId))
                return BadRequest(new { success = false, message = "Driver is required." });

            if (string.IsNullOrWhiteSpace(request.conductorId))
                return BadRequest(new { success = false, message = "Conductor is required." });

            if (string.IsNullOrWhiteSpace(request.scheduleTime))
                return BadRequest(new { success = false, message = "Schedule Time is required." });

            if (!int.TryParse(request.depotId, out int depotId))
                return BadRequest(new { success = false, message = "Invalid Depot ID format." });

            if (!int.TryParse(request.routeId, out int routeId))
                return BadRequest(new { success = false, message = "Invalid Route ID format." });

            if (!int.TryParse(request.fleetId, out int fleetId))
                return BadRequest(new { success = false, message = "Invalid Fleet ID format." });

            if (!int.TryParse(request.driverId, out int driverId))
                return BadRequest(new { success = false, message = "Invalid Driver ID format." });

            if (!int.TryParse(request.conductorId, out int conductorId))
                return BadRequest(new { success = false, message = "Invalid Conductor ID format." });

            if (!DateTime.TryParse(request.scheduleTime, out DateTime scheduleTime))
                return BadRequest(new { success = false, message = "Invalid Schedule Time format." });

            var validStatuses = new[] { "Scheduled", "OnTime", "Delayed", "Completed", "Cancelled" };
            if (!string.IsNullOrWhiteSpace(request.tripStatus) && !validStatuses.Contains(request.tripStatus))
                return BadRequest(new { success = false, message = "Invalid Trip Status. Valid statuses are: Scheduled, OnTime, Delayed, Completed, Cancelled." });

            var entity = new TripDTO
            {
                tripId = request.tripId,
                depotId = request.depotId,
                routeId = request.routeId,
                fleetId = request.fleetId,
                driverId = request.driverId,
                conductorId = request.conductorId,
                scheduleTime = request.scheduleTime,
                actualTime = request.actualTime,
                tripStatus = request.tripStatus,
                isActive = request.isActive
            };

            var result = await _svc.UpdateAsync(entity, CurrentUserId);

            if (!result.Success)
            {
                if (result.Message != null && result.Message.Contains("not found"))
                    return NotFound(new { success = false, message = result.Message });

                return BadRequest(new { success = false, message = result.Message });
            }

            return Ok(result);
        }

        [HttpPost("delete")]
        public async Task<IActionResult> Delete([FromBody] DeleteTripRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.tripId))
                return BadRequest(new { success = false, message = "Trip ID is required." });

            if (!int.TryParse(request.tripId, out int id))
                return BadRequest(new { success = false, message = "Invalid Trip ID format." });

            var result = await _svc.DeleteAsync(id, CurrentUserId);

            if (!result.Success)
            {
                if (result.Message != null && result.Message.Contains("not found"))
                    return NotFound(new { success = false, message = result.Message });

                return BadRequest(new { success = false, message = result.Message });
            }

            return Ok(result);
        }
    }
}