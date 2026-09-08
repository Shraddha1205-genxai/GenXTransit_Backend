using GenXTransitAPI.DataAccess.Interface.IServices;
using GenXTransitAPI.Models;
using GenXTransitAPI.Models.DTO_s;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GenXTransitAPI.Controllers
{
    [Route("api/fleet")]
    [ApiController]
    [Authorize]  // ✅ Development: No auth required
    public class FleetController : BaseController
    {
        private readonly IFleetService _svc;

        public FleetController(IFleetService svc)
        {
            _svc = svc;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] string? searchText,
            [FromQuery] int? categoryId,
            [FromQuery] int? depotId,
            [FromQuery] string? fleetStatus,
            [FromQuery] bool? isActive,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var result = await _svc.GetAllAsync(searchText, categoryId, depotId, fleetStatus, isActive, pageNumber, pageSize);

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
        public async Task<IActionResult> Insert([FromBody] InsertFleetRequest request)
        {
            if (request == null)
                return BadRequest(new { success = false, message = "Invalid request data." });

            if (string.IsNullOrWhiteSpace(request.vehicleNumber))
                return BadRequest(new { success = false, message = "Vehicle Number is required." });

            if (!Regex.IsMatch(request.vehicleNumber, @"^[A-Z]{2}-[0-9]{2}-[A-Z]{2}-[0-9]{4}$"))
                return BadRequest(new { success = false, message = "Invalid vehicle number format. Expected format: XX-99-XX-9999 (e.g., MH-12-AB-1234)" });

            if (string.IsNullOrWhiteSpace(request.categoryId))
                return BadRequest(new { success = false, message = "Vehicle Category is required." });

            if (string.IsNullOrWhiteSpace(request.seriesType))
                return BadRequest(new { success = false, message = "Series Type is required." });

            if (string.IsNullOrWhiteSpace(request.depotId))
                return BadRequest(new { success = false, message = "Depot is required." });

            // Validate Series Type
            var validSeriesTypes = new[] { "BH", "State" };
            if (!validSeriesTypes.Contains(request.seriesType))
                return BadRequest(new { success = false, message = "Invalid Series Type. Valid types are: BH, State." });

            if (!int.TryParse(request.categoryId, out int categoryId))
                return BadRequest(new { success = false, message = "Invalid Category ID format." });

            if (!int.TryParse(request.depotId, out int depotId))
                return BadRequest(new { success = false, message = "Invalid Depot ID format." });

            var entity = new FleetDTO
            {
                vehicleNumber = request.vehicleNumber,
                categoryId = request.categoryId,
                seriesType = request.seriesType,
                depotId = request.depotId,
                fleetStatus = request.fleetStatus,  
                docExpiry = request.docExpiry,
                isActive = request.isActive
            };

            var result = await _svc.InsertAsync(entity, CurrentUserId);

            if (!result.Success)
                return BadRequest(new { success = false, message = result.Message });

            return Ok(result);
        }

        [HttpPost("update")]
        public async Task<IActionResult> Update([FromBody] UpdateFleetRequest request)
        {
            if (request == null)
                return BadRequest(new { success = false, message = "Invalid request data." });

            if (string.IsNullOrEmpty(request.fleetId))
                return BadRequest(new { success = false, message = "Fleet ID is required." });

            if (!int.TryParse(request.fleetId, out int fleetId))
                return BadRequest(new { success = false, message = "Invalid Fleet ID format." });

            if (string.IsNullOrWhiteSpace(request.vehicleNumber))
                return BadRequest(new { success = false, message = "Vehicle Number is required." });

            if (!Regex.IsMatch(request.vehicleNumber, @"^[A-Z]{2}-[0-9]{2}-[A-Z]{2}-[0-9]{4}$"))
                return BadRequest(new { success = false, message = "Invalid vehicle number format. Expected format: XX-99-XX-9999 (e.g., MH-12-AB-1234)" });

            if (string.IsNullOrWhiteSpace(request.categoryId))
                return BadRequest(new { success = false, message = "Vehicle Category is required." });

            if (string.IsNullOrWhiteSpace(request.seriesType))
                return BadRequest(new { success = false, message = "Series Type is required." });

            if (string.IsNullOrWhiteSpace(request.depotId))
                return BadRequest(new { success = false, message = "Depot is required." });

            // Validate Series Type
            var validSeriesTypes = new[] { "BH", "State" };
            if (!validSeriesTypes.Contains(request.seriesType))
                return BadRequest(new { success = false, message = "Invalid Series Type. Valid types are: BH, State." });

            if (!int.TryParse(request.categoryId, out int categoryId))
                return BadRequest(new { success = false, message = "Invalid Category ID format." });

            if (!int.TryParse(request.depotId, out int depotId))
                return BadRequest(new { success = false, message = "Invalid Depot ID format." });

            var entity = new FleetDTO
            {
                fleetId = request.fleetId,
                vehicleNumber = request.vehicleNumber,
                categoryId = request.categoryId,
                seriesType = request.seriesType,
                depotId = request.depotId,
                fleetStatus = request.fleetStatus,  
                docExpiry = request.docExpiry,
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
        public async Task<IActionResult> Delete([FromBody] DeleteFleetRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.fleetId))
                return BadRequest(new { success = false, message = "Fleet ID is required." });

            if (!int.TryParse(request.fleetId, out int id))
                return BadRequest(new { success = false, message = "Invalid Fleet ID format." });

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