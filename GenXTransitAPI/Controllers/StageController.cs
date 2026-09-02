using GenXTransitAPI.DataAccess.Interface.IServices;
using GenXTransitAPI.Models;
using GenXTransitAPI.Models.DTO_s;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GenXTransitAPI.Controllers
{
    [Route("api/stage")]
    [ApiController]
    [AllowAnonymous]  
    public class StageController : BaseController  
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
            var result = await _svc.GetAllAsync(searchText, routeId, isActive, CurrentUserId, pageNumber, pageSize);  

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
        public async Task<IActionResult> Insert([FromBody] InsertStageRequest request)
        {
            if (request == null)
                return BadRequest(new { success = false, message = "Invalid request data." });

            if (string.IsNullOrWhiteSpace(request.stageName))
                return BadRequest(new { success = false, message = "Stage Name is required." });

            if (string.IsNullOrWhiteSpace(request.routeId))
                return BadRequest(new { success = false, message = "Route is required." });

            if (string.IsNullOrWhiteSpace(request.sectionFromId))
                return BadRequest(new { success = false, message = "Section From is required." });

            if (string.IsNullOrWhiteSpace(request.sectionToId))
                return BadRequest(new { success = false, message = "Section To is required." });

            if (request.distance <= 0)
                return BadRequest(new { success = false, message = "Distance must be greater than 0." });

            // Validate IDs
            if (!int.TryParse(request.routeId, out int routeId))
                return BadRequest(new { success = false, message = "Invalid Route ID format." });

            if (!int.TryParse(request.sectionFromId, out int sectionFromId))
                return BadRequest(new { success = false, message = "Invalid Section From ID format." });

            if (!int.TryParse(request.sectionToId, out int sectionToId))
                return BadRequest(new { success = false, message = "Invalid Section To ID format." });

            // Check if From and To are different
            if (sectionFromId == sectionToId)
                return BadRequest(new { success = false, message = "Section From and Section To cannot be the same." });

            var entity = new StageDTO
            {
                stageName = request.stageName,
                routeId = request.routeId,
                sectionFromId = request.sectionFromId,
                sectionToId = request.sectionToId,
                distance = request.distance,
                isActive = request.isActive
            };

            var result = await _svc.InsertAsync(entity, CurrentUserId);  

            if (!result.Success)
                return BadRequest(new { success = false, message = result.Message });

            return Ok(result);
        }

        [HttpPost("update")]
        public async Task<IActionResult> Update([FromBody] UpdateStageRequest request)
        {
            if (request == null)
                return BadRequest(new { success = false, message = "Invalid request data." });

            if (string.IsNullOrEmpty(request.stageId))
                return BadRequest(new { success = false, message = "Stage ID is required." });

            if (!int.TryParse(request.stageId, out int stageId))
                return BadRequest(new { success = false, message = "Invalid Stage ID format." });

            if (string.IsNullOrWhiteSpace(request.stageName))
                return BadRequest(new { success = false, message = "Stage Name is required." });

            if (string.IsNullOrWhiteSpace(request.routeId))
                return BadRequest(new { success = false, message = "Route is required." });

            if (string.IsNullOrWhiteSpace(request.sectionFromId))
                return BadRequest(new { success = false, message = "Section From is required." });

            if (string.IsNullOrWhiteSpace(request.sectionToId))
                return BadRequest(new { success = false, message = "Section To is required." });

            if (request.distance <= 0)
                return BadRequest(new { success = false, message = "Distance must be greater than 0." });

            // Validate IDs
            if (!int.TryParse(request.routeId, out int routeId))
                return BadRequest(new { success = false, message = "Invalid Route ID format." });

            if (!int.TryParse(request.sectionFromId, out int sectionFromId))
                return BadRequest(new { success = false, message = "Invalid Section From ID format." });

            if (!int.TryParse(request.sectionToId, out int sectionToId))
                return BadRequest(new { success = false, message = "Invalid Section To ID format." });

            // Check if From and To are different
            if (sectionFromId == sectionToId)
                return BadRequest(new { success = false, message = "Section From and Section To cannot be the same." });

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
        public async Task<IActionResult> Delete([FromBody] DeleteStageRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.stageId))
                return BadRequest(new { success = false, message = "Stage ID is required." });

            if (!int.TryParse(request.stageId, out int id))
                return BadRequest(new { success = false, message = "Invalid Stage ID format." });

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