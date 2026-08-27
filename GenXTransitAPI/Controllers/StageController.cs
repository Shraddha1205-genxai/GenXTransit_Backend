using GenXTransitAPI.DataAccess.Interface.IServices;
using GenXTransitAPI.Models;
using GenXTransitAPI.Models.DTO_s;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GenXTransitAPI.Controllers
{
    [Route("api/stage")]
    [ApiController]
    public class StageController : ControllerBase
    {
        private readonly IStageService _svc;

        public StageController(IStageService svc)
        {
            _svc = svc;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] string? searchText,
            [FromQuery] int? routeId,
            [FromQuery] bool? isActive,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var result = await _svc.GetAllAsync(searchText, routeId, isActive, GetCurrentUserId(), pageNumber, pageSize);
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
        public async Task<IActionResult> Insert([FromBody] InsertStageRequest request)
        {
            if (request == null)
                return BadRequest(ApiResponse<int>.Fail("Invalid request data."));

            if (string.IsNullOrWhiteSpace(request.stageName))
                return BadRequest(ApiResponse<int>.Fail("Stage Name is required."));

            if (string.IsNullOrWhiteSpace(request.routeId))
                return BadRequest(ApiResponse<int>.Fail("Route is required."));

            if (string.IsNullOrWhiteSpace(request.sectionFromId))
                return BadRequest(ApiResponse<int>.Fail("Section From is required."));

            if (string.IsNullOrWhiteSpace(request.sectionToId))
                return BadRequest(ApiResponse<int>.Fail("Section To is required."));

            if (request.distance <= 0)
                return BadRequest(ApiResponse<int>.Fail("Distance must be greater than 0."));

            var entity = new StageDTO
            {
                stageName = request.stageName,
                routeId = request.routeId,
                sectionFromId = request.sectionFromId,
                sectionToId = request.sectionToId,
                distance = request.distance,
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
        public async Task<IActionResult> Update([FromBody] UpdateStageRequest request)
        {
            if (request == null)
                return BadRequest(ApiResponse<bool>.Fail("Invalid request data."));

            if (string.IsNullOrEmpty(request.stageId))
                return BadRequest(ApiResponse<bool>.Fail("Stage ID is required."));

            if (!int.TryParse(request.stageId, out int id))
                return BadRequest(ApiResponse<bool>.Fail("Invalid Stage ID format."));

            if (string.IsNullOrWhiteSpace(request.stageName))
                return BadRequest(ApiResponse<bool>.Fail("Stage Name is required."));

            if (string.IsNullOrWhiteSpace(request.routeId))
                return BadRequest(ApiResponse<bool>.Fail("Route is required."));

            if (string.IsNullOrWhiteSpace(request.sectionFromId))
                return BadRequest(ApiResponse<bool>.Fail("Section From is required."));

            if (string.IsNullOrWhiteSpace(request.sectionToId))
                return BadRequest(ApiResponse<bool>.Fail("Section To is required."));

            if (request.distance <= 0)
                return BadRequest(ApiResponse<bool>.Fail("Distance must be greater than 0."));

            var entity = new StageDTO
            {
                stageId = request.stageId,
                stageName = request.stageName,
                routeId = request.routeId,
                sectionFromId = request.sectionFromId,
                sectionToId = request.sectionToId,
                distance = request.distance,
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
        public async Task<IActionResult> Delete([FromBody] DeleteStageRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.stageId))
                return BadRequest(ApiResponse<bool>.Fail("Stage ID is required."));

            if (!int.TryParse(request.stageId, out int id))
                return BadRequest(ApiResponse<bool>.Fail("Invalid Stage ID format."));

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