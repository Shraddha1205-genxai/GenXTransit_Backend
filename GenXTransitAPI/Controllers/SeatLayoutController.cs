using GenXTransitAPI.DataAccess.Interface.IServices;
using GenXTransitAPI.Models;
using GenXTransitAPI.Models.DTO_s;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GenXTransitAPI.Controllers
{
    [Route("api/seatlayout")]
    [ApiController]
    public class SeatLayoutController : ControllerBase
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
            var result = await _svc.GetAllAsync(searchText, categoryId, isActive, GetCurrentUserId(), pageNumber, pageSize);
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
        public async Task<IActionResult> Insert([FromBody] InsertSeatLayoutRequest request)
        {
            if (request == null)
                return BadRequest(ApiResponse<int>.Fail("Invalid request data."));

            if (string.IsNullOrWhiteSpace(request.categoryId))
                return BadRequest(ApiResponse<int>.Fail("Category is required."));

            var entity = new SeatLayoutDTO
            {
                description = request.description,
                categoryId = request.categoryId,
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
        public async Task<IActionResult> Update([FromBody] UpdateSeatLayoutRequest request)
        {
            if (request == null)
                return BadRequest(ApiResponse<bool>.Fail("Invalid request data."));

            if (string.IsNullOrEmpty(request.layoutId))
                return BadRequest(ApiResponse<bool>.Fail("Layout ID is required."));

            if (!int.TryParse(request.layoutId, out int id))
                return BadRequest(ApiResponse<bool>.Fail("Invalid Layout ID format."));

            if (string.IsNullOrWhiteSpace(request.categoryId))
                return BadRequest(ApiResponse<bool>.Fail("Category is required."));

            var entity = new SeatLayoutDTO
            {
                layoutId = request.layoutId,
                description = request.description,
                categoryId = request.categoryId,
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
        public async Task<IActionResult> Delete([FromBody] DeleteSeatLayoutRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.layoutId))
                return BadRequest(ApiResponse<bool>.Fail("Layout ID is required."));

            if (!int.TryParse(request.layoutId, out int id))
                return BadRequest(ApiResponse<bool>.Fail("Invalid Layout ID format."));

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