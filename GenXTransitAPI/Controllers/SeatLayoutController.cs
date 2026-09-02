using GenXTransitAPI.DataAccess.Interface.IServices;
using GenXTransitAPI.Models;
using GenXTransitAPI.Models.DTO_s;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GenXTransitAPI.Controllers
{
    [Route("api/seatlayout")]
    [ApiController]
    [AllowAnonymous]  
    public class SeatLayoutController : BaseController  
    {
        private readonly ISeatLayoutService _svc;

        public SeatLayoutController(ISeatLayoutService svc)
        {
            _svc = svc;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] string? searchText,
            [FromQuery] int? categoryId,
            [FromQuery] bool? isActive,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var result = await _svc.GetAllAsync(searchText, categoryId, isActive, CurrentUserId, pageNumber, pageSize);  

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
        public async Task<IActionResult> Insert([FromBody] InsertSeatLayoutRequest request)
        {
            if (request == null)
                return BadRequest(new { success = false, message = "Invalid request data." });

            if (string.IsNullOrWhiteSpace(request.categoryId))
                return BadRequest(new { success = false, message = "Category is required." });

            // Validate Category ID
            if (!int.TryParse(request.categoryId, out int categoryId))
                return BadRequest(new { success = false, message = "Invalid Category ID format." });

            var entity = new SeatLayoutDTO
            {
                description = request.description,
                categoryId = request.categoryId,
                isActive = request.isActive
            };

            var result = await _svc.InsertAsync(entity, CurrentUserId);  

            if (!result.Success)
                return BadRequest(new { success = false, message = result.Message });

            return Ok(result);
        }

        [HttpPost("update")]
        public async Task<IActionResult> Update([FromBody] UpdateSeatLayoutRequest request)
        {
            if (request == null)
                return BadRequest(new { success = false, message = "Invalid request data." });

            if (string.IsNullOrEmpty(request.layoutId))
                return BadRequest(new { success = false, message = "Layout ID is required." });

            if (!int.TryParse(request.layoutId, out int layoutId))
                return BadRequest(new { success = false, message = "Invalid Layout ID format." });

            if (string.IsNullOrWhiteSpace(request.categoryId))
                return BadRequest(new { success = false, message = "Category is required." });

            // Validate Category ID
            if (!int.TryParse(request.categoryId, out int categoryId))
                return BadRequest(new { success = false, message = "Invalid Category ID format." });

            var entity = new SeatLayoutDTO
            {
                layoutId = request.layoutId,
                description = request.description,
                categoryId = request.categoryId,
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
        public async Task<IActionResult> Delete([FromBody] DeleteSeatLayoutRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.layoutId))
                return BadRequest(new { success = false, message = "Layout ID is required." });

            if (!int.TryParse(request.layoutId, out int id))
                return BadRequest(new { success = false, message = "Invalid Layout ID format." });

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