using GenXTransitAPI.DataAccess.Interface.IServices;
using GenXTransitAPI.Models;
using GenXTransitAPI.Models.DTO_s;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GenXTransitAPI.Controllers
{
    [Route("api/service")]
    [ApiController]
    [Authorize]
    public class ServiceController : BaseController
    {
        private readonly IServiceService _svc;

        public ServiceController(IServiceService svc)
        {
            _svc = svc;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] string? searchText,
            [FromQuery] int? fleetId,
            [FromQuery] string? status,
            [FromQuery] bool? isActive,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var result = await _svc.GetAllAsync(searchText, fleetId, status, isActive, pageNumber, pageSize);

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
        public async Task<IActionResult> Insert([FromBody] InsertServiceRequest request)
        {
            if (request == null)
                return BadRequest(new { success = false, message = "Invalid request data." });

            if (string.IsNullOrWhiteSpace(request.fleetId))
                return BadRequest(new { success = false, message = "Fleet is required." });

            if (!int.TryParse(request.fleetId, out int fleetId))
                return BadRequest(new { success = false, message = "Invalid Fleet ID format." });

            // Validate Status
            var validStatuses = new[] { "Pending", "Inprogress", "Completed", "Cancelled" };
            if (!string.IsNullOrWhiteSpace(request.status) && !validStatuses.Contains(request.status))
                return BadRequest(new { success = false, message = "Invalid status. Valid statuses are: Pending, Inprogress, Completed, Cancelled." });

            var entity = new ServiceDTO
            {
                fleetId = request.fleetId,
                status = request.status ?? "Pending",
                isActive = request.isActive
            };

            var result = await _svc.InsertAsync(entity, CurrentUserId);

            if (!result.Success)
                return BadRequest(new { success = false, message = result.Message });

            return Ok(result);
        }

        [HttpPost("update")]
        public async Task<IActionResult> Update([FromBody] UpdateServiceRequest request)
        {
            if (request == null)
                return BadRequest(new { success = false, message = "Invalid request data." });

            if (string.IsNullOrEmpty(request.serviceId))
                return BadRequest(new { success = false, message = "Service ID is required." });

            if (!int.TryParse(request.serviceId, out int serviceId))
                return BadRequest(new { success = false, message = "Invalid Service ID format." });

            if (string.IsNullOrWhiteSpace(request.fleetId))
                return BadRequest(new { success = false, message = "Fleet is required." });

            if (!int.TryParse(request.fleetId, out int fleetId))
                return BadRequest(new { success = false, message = "Invalid Fleet ID format." });

            // Validate Status
            var validStatuses = new[] { "Pending", "Inprogress", "Completed", "Cancelled" };
            if (!string.IsNullOrWhiteSpace(request.status) && !validStatuses.Contains(request.status))
                return BadRequest(new { success = false, message = "Invalid status. Valid statuses are: Pending, Inprogress, Completed, Cancelled." });

            var entity = new ServiceDTO
            {
                serviceId = request.serviceId,
                fleetId = request.fleetId,
                status = request.status,
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
        public async Task<IActionResult> Delete([FromBody] DeleteServiceRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.serviceId))
                return BadRequest(new { success = false, message = "Service ID is required." });

            if (!int.TryParse(request.serviceId, out int id))
                return BadRequest(new { success = false, message = "Invalid Service ID format." });

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