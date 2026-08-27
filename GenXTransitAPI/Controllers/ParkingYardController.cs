using GenXTransitAPI.DataAccess.Interface.IServices;
using GenXTransitAPI.Models;
using GenXTransitAPI.Models.DTO_s;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GenXTransitAPI.Controllers
{
    [Route("api/parkingyard")]
    [ApiController]
    public class ParkingYardController : ControllerBase
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
            var result = await _svc.GetAllAsync(searchText, regionId, divisionId, depotId, isActive, GetCurrentUserId(), pageNumber, pageSize);
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
        public async Task<IActionResult> Insert([FromBody] InsertParkingYardRequest request)
        {
            if (request == null)
                return BadRequest(ApiResponse<int>.Fail("Invalid request data."));

            if (string.IsNullOrWhiteSpace(request.yardName))
                return BadRequest(ApiResponse<int>.Fail("Parking Yard Name is required."));

            if (string.IsNullOrWhiteSpace(request.regionId))
                return BadRequest(ApiResponse<int>.Fail("Region is required."));

            if (string.IsNullOrWhiteSpace(request.divisionId))
                return BadRequest(ApiResponse<int>.Fail("Division is required."));

            if (string.IsNullOrWhiteSpace(request.depotId))
                return BadRequest(ApiResponse<int>.Fail("Depot is required."));

            if (request.capacity < 0)
                return BadRequest(ApiResponse<int>.Fail("Capacity cannot be negative."));

            if (request.occupied < 0)
                return BadRequest(ApiResponse<int>.Fail("Occupied cannot be negative."));

            if (request.occupied > request.capacity)
                return BadRequest(ApiResponse<int>.Fail("Occupied cannot exceed Capacity."));

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

            var result = await _svc.InsertAsync(entity, GetCurrentUserId());
            if (result.Success && result.Data > 0)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("update")]
        public async Task<IActionResult> Update([FromBody] UpdateParkingYardRequest request)
        {
            if (request == null)
                return BadRequest(ApiResponse<bool>.Fail("Invalid request data."));

            if (string.IsNullOrEmpty(request.yardId))
                return BadRequest(ApiResponse<bool>.Fail("Parking Yard ID is required."));

            if (!int.TryParse(request.yardId, out int id))
                return BadRequest(ApiResponse<bool>.Fail("Invalid Parking Yard ID format."));

            if (string.IsNullOrWhiteSpace(request.yardName))
                return BadRequest(ApiResponse<bool>.Fail("Parking Yard Name is required."));

            if (string.IsNullOrWhiteSpace(request.regionId))
                return BadRequest(ApiResponse<bool>.Fail("Region is required."));

            if (string.IsNullOrWhiteSpace(request.divisionId))
                return BadRequest(ApiResponse<bool>.Fail("Division is required."));

            if (string.IsNullOrWhiteSpace(request.depotId))
                return BadRequest(ApiResponse<bool>.Fail("Depot is required."));

            if (request.capacity < 0)
                return BadRequest(ApiResponse<bool>.Fail("Capacity cannot be negative."));

            if (request.occupied < 0)
                return BadRequest(ApiResponse<bool>.Fail("Occupied cannot be negative."));

            if (request.occupied > request.capacity)
                return BadRequest(ApiResponse<bool>.Fail("Occupied cannot exceed Capacity."));

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
        public async Task<IActionResult> Delete([FromBody] DeleteParkingYardRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.yardId))
                return BadRequest(ApiResponse<bool>.Fail("Parking Yard ID is required."));

            if (!int.TryParse(request.yardId, out int id))
                return BadRequest(ApiResponse<bool>.Fail("Invalid Parking Yard ID format."));

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