using GenXTransitAPI.DataAccess.Interface.IServices;
using GenXTransitAPI.Models;
using GenXTransitAPI.Models.DTO_s;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GenXTransitAPI.Controllers
{
    [Route("api/parkingyard")]
    [ApiController]
    [AllowAnonymous]  
    public class ParkingYardController : BaseController  
    {
        private readonly IOrgParkingYardService _svc;

        public ParkingYardController(IOrgParkingYardService svc)
        {
            _svc = svc;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] string? searchText,
            [FromQuery] int? regionId,
            [FromQuery] int? divisionId,
            [FromQuery] int? depotId,
            [FromQuery] bool? isActive,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var result = await _svc.GetAllAsync(searchText, regionId, divisionId, depotId, isActive, CurrentUserId, pageNumber, pageSize);  

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
        public async Task<IActionResult> Insert([FromBody] InsertParkingYardRequest request)
        {
            if (request == null)
                return BadRequest(new { success = false, message = "Invalid request data." });

            if (string.IsNullOrWhiteSpace(request.yardName))
                return BadRequest(new { success = false, message = "Parking Yard Name is required." });

            if (string.IsNullOrWhiteSpace(request.regionId))
                return BadRequest(new { success = false, message = "Region is required." });

            if (string.IsNullOrWhiteSpace(request.divisionId))
                return BadRequest(new { success = false, message = "Division is required." });

            if (string.IsNullOrWhiteSpace(request.depotId))
                return BadRequest(new { success = false, message = "Depot is required." });

            if (request.capacity < 0)
                return BadRequest(new { success = false, message = "Capacity cannot be negative." });

            if (request.occupied < 0)
                return BadRequest(new { success = false, message = "Occupied cannot be negative." });

            if (request.occupied > request.capacity)
                return BadRequest(new { success = false, message = "Occupied cannot exceed Capacity." });

            // Validate IDs
            if (!int.TryParse(request.regionId, out int regionId))
                return BadRequest(new { success = false, message = "Invalid Region ID format." });

            if (!int.TryParse(request.divisionId, out int divisionId))
                return BadRequest(new { success = false, message = "Invalid Division ID format." });

            if (!int.TryParse(request.depotId, out int depotId))
                return BadRequest(new { success = false, message = "Invalid Depot ID format." });

            var entity = new OrgParkingYardDTO
            {
                yardName = request.yardName,
                regionId = request.regionId,
                divisionId = request.divisionId,
                depotId = request.depotId,
                capacity = request.capacity,
                occupied = request.occupied,
                isActive = request.isActive
            };

            var result = await _svc.InsertAsync(entity, CurrentUserId);  

            if (!result.Success)
                return BadRequest(new { success = false, message = result.Message });

            return Ok(result);
        }

        [HttpPost("update")]
        public async Task<IActionResult> Update([FromBody] UpdateParkingYardRequest request)
        {
            if (request == null)
                return BadRequest(new { success = false, message = "Invalid request data." });

            if (string.IsNullOrEmpty(request.yardId))
                return BadRequest(new { success = false, message = "Parking Yard ID is required." });

            if (!int.TryParse(request.yardId, out int yardId))
                return BadRequest(new { success = false, message = "Invalid Parking Yard ID format." });

            if (string.IsNullOrWhiteSpace(request.yardName))
                return BadRequest(new { success = false, message = "Parking Yard Name is required." });

            if (string.IsNullOrWhiteSpace(request.regionId))
                return BadRequest(new { success = false, message = "Region is required." });

            if (string.IsNullOrWhiteSpace(request.divisionId))
                return BadRequest(new { success = false, message = "Division is required." });

            if (string.IsNullOrWhiteSpace(request.depotId))
                return BadRequest(new { success = false, message = "Depot is required." });

            if (request.capacity < 0)
                return BadRequest(new { success = false, message = "Capacity cannot be negative." });

            if (request.occupied < 0)
                return BadRequest(new { success = false, message = "Occupied cannot be negative." });

            if (request.occupied > request.capacity)
                return BadRequest(new { success = false, message = "Occupied cannot exceed Capacity." });

            // Validate IDs
            if (!int.TryParse(request.regionId, out int regionId))
                return BadRequest(new { success = false, message = "Invalid Region ID format." });

            if (!int.TryParse(request.divisionId, out int divisionId))
                return BadRequest(new { success = false, message = "Invalid Division ID format." });

            if (!int.TryParse(request.depotId, out int depotId))
                return BadRequest(new { success = false, message = "Invalid Depot ID format." });

            var entity = new OrgParkingYardDTO
            {
                yardId = request.yardId,
                yardName = request.yardName,
                regionId = request.regionId,
                divisionId = request.divisionId,
                depotId = request.depotId,
                capacity = request.capacity,
                occupied = request.occupied,
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
        public async Task<IActionResult> Delete([FromBody] DeleteParkingYardRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.yardId))
                return BadRequest(new { success = false, message = "Parking Yard ID is required." });

            if (!int.TryParse(request.yardId, out int id))
                return BadRequest(new { success = false, message = "Invalid Parking Yard ID format." });

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