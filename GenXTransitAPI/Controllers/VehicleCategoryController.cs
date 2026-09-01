using GenXTransitAPI.DataAccess.Interface.IServices;
using GenXTransitAPI.Models;
using GenXTransitAPI.Models.DTO_s;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GenXTransitAPI.Controllers
{
    [Route("api/vehiclecategory")]
    [ApiController]
    public class VehicleCategoryController : ControllerBase
    {
        private readonly IVehicleCategoryService _svc;

        public VehicleCategoryController(IVehicleCategoryService svc)
        {
            _svc = svc;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] string? searchText,
            [FromQuery] string? type,
            [FromQuery] string? vehicleClass,
            [FromQuery] bool? isActive,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var result = await _svc.GetAllAsync(searchText, type, vehicleClass, isActive, GetCurrentUserId(), pageNumber, pageSize);

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
        public async Task<IActionResult> Insert([FromBody] InsertVehicleCategoryRequest request)
        {
            if (request == null)
                return BadRequest(new { success = false, message = "Invalid request data." });

            if (string.IsNullOrWhiteSpace(request.categoryName))
                return BadRequest(new { success = false, message = "Category Name is required." });

            if (request.capacity < 0)
                return BadRequest(new { success = false, message = "Capacity cannot be negative." });

            if (string.IsNullOrWhiteSpace(request.type))
                return BadRequest(new { success = false, message = "Type is required." });

            if (string.IsNullOrWhiteSpace(request.@class))
                return BadRequest(new { success = false, message = "Vehicle Class is required." });

            // Validate Type
            var validTypes = new[] { "AC", "Non-AC" };
            if (!validTypes.Contains(request.type))
                return BadRequest(new { success = false, message = "Invalid Type. Valid types are: AC, Non-AC." });

            var entity = new VehicleCategoryDTO
            {
                categoryName = request.categoryName,
                capacity = request.capacity,
                type = request.type,
                @class = request.@class,
                isActive = request.isActive
            };

            var result = await _svc.InsertAsync(entity, GetCurrentUserId());

            if (!result.Success)
                return BadRequest(new { success = false, message = result.Message });

            return Ok(result);
        }

        [HttpPost("update")]
        public async Task<IActionResult> Update([FromBody] UpdateVehicleCategoryRequest request)
        {
            if (request == null)
                return BadRequest(new { success = false, message = "Invalid request data." });

            if (string.IsNullOrEmpty(request.categoryId))
                return BadRequest(new { success = false, message = "Category ID is required." });

            if (!int.TryParse(request.categoryId, out int categoryId))
                return BadRequest(new { success = false, message = "Invalid Category ID format." });

            if (string.IsNullOrWhiteSpace(request.categoryName))
                return BadRequest(new { success = false, message = "Category Name is required." });

            if (request.capacity < 0)
                return BadRequest(new { success = false, message = "Capacity cannot be negative." });

            if (string.IsNullOrWhiteSpace(request.type))
                return BadRequest(new { success = false, message = "Type is required." });

            if (string.IsNullOrWhiteSpace(request.@class))
                return BadRequest(new { success = false, message = "Vehicle Class is required." });

            // Validate Type
            var validTypes = new[] { "AC", "Non-AC" };
            if (!validTypes.Contains(request.type))
                return BadRequest(new { success = false, message = "Invalid Type. Valid types are: AC, Non-AC." });

            var entity = new VehicleCategoryDTO
            {
                categoryId = request.categoryId,
                categoryName = request.categoryName,
                capacity = request.capacity,
                type = request.type,
                @class = request.@class,
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
        public async Task<IActionResult> Delete([FromBody] DeleteVehicleCategoryRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.categoryId))
                return BadRequest(new { success = false, message = "Category ID is required." });

            if (!int.TryParse(request.categoryId, out int id))
                return BadRequest(new { success = false, message = "Invalid Category ID format." });

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