using GenXTransitAPI.DataAccess.Interface.IServices;
using GenXTransitAPI.Models;
using GenXTransitAPI.Models.DTO_s;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GenXTransitAPI.Controllers
{
    [Route("api/tickettype")]
    [ApiController]
    public class TicketTypeController : ControllerBase
    {
        private readonly ITicketTypeService _svc;

        public TicketTypeController(ITicketTypeService svc)
        {
            _svc = svc;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] string? searchText,
            [FromQuery] bool? isActive,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var result = await _svc.GetAllAsync(searchText, isActive, GetCurrentUserId(), pageNumber, pageSize);

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
        public async Task<IActionResult> Insert([FromBody] InsertTicketTypeRequest request)
        {
            if (request == null)
                return BadRequest(new { success = false, message = "Invalid request data." });

            if (string.IsNullOrWhiteSpace(request.ticketName))
                return BadRequest(new { success = false, message = "Ticket Name is required." });

            var entity = new TicketTypeDTO
            {
                ticketName = request.ticketName,
                description = request.description,
                isActive = request.isActive
            };

            var result = await _svc.InsertAsync(entity, GetCurrentUserId());

            if (!result.Success)
                return BadRequest(new { success = false, message = result.Message });

            return Ok(result);
        }

        [HttpPost("update")]
        public async Task<IActionResult> Update([FromBody] UpdateTicketTypeRequest request)
        {
            if (request == null)
                return BadRequest(new { success = false, message = "Invalid request data." });

            if (string.IsNullOrEmpty(request.ticketId))
                return BadRequest(new { success = false, message = "Ticket ID is required." });

            if (!int.TryParse(request.ticketId, out int ticketId))
                return BadRequest(new { success = false, message = "Invalid Ticket ID format." });

            if (string.IsNullOrWhiteSpace(request.ticketName))
                return BadRequest(new { success = false, message = "Ticket Name is required." });

            var entity = new TicketTypeDTO
            {
                ticketId = request.ticketId,
                ticketName = request.ticketName,
                description = request.description,
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
        public async Task<IActionResult> Delete([FromBody] DeleteTicketTypeRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.ticketId))
                return BadRequest(new { success = false, message = "Ticket ID is required." });

            if (!int.TryParse(request.ticketId, out int id))
                return BadRequest(new { success = false, message = "Invalid Ticket ID format." });

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