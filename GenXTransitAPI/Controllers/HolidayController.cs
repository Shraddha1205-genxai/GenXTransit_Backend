using GenXTransitAPI.DataAccess.Interface.IServices;
using GenXTransitAPI.Models;
using GenXTransitAPI.Models.DTO_s;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace GenXTransitAPI.Controllers
{
    [Route("api/holiday")]
    [ApiController]
    [AllowAnonymous]  
    public class HolidayController : BaseController  
    {
        private readonly IHolidayService _svc;

        public HolidayController(IHolidayService svc)
        {
            _svc = svc;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] string? searchText,
            [FromQuery] string? type,
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate,
            [FromQuery] bool? isActive,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var result = await _svc.GetAllAsync(searchText, type, startDate, endDate, isActive, CurrentUserId, pageNumber, pageSize);  

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
        public async Task<IActionResult> Insert([FromBody] InsertHolidayRequest request)
        {
            if (request == null)
                return BadRequest(new { success = false, message = "Invalid request data." });

            if (string.IsNullOrWhiteSpace(request.holidayName))
                return BadRequest(new { success = false, message = "Holiday Name is required." });

            if (string.IsNullOrWhiteSpace(request.occasion))
                return BadRequest(new { success = false, message = "Occasion is required." });

            if (string.IsNullOrWhiteSpace(request.date))
                return BadRequest(new { success = false, message = "Date is required." });

            if (string.IsNullOrWhiteSpace(request.type))
                return BadRequest(new { success = false, message = "Type is required." });

            if (!DateTime.TryParse(request.date, out _))
                return BadRequest(new { success = false, message = "Invalid date format. Please use yyyy-MM-dd." });

            // Validate Holiday Type
            var validTypes = new[] { "National", "Regional", "Festival", "Optional" };
            if (!validTypes.Contains(request.type))
                return BadRequest(new { success = false, message = "Invalid Type. Valid types are: National, Regional, Festival, Optional." });

            var entity = new HolidayDTO
            {
                holidayName = request.holidayName,
                occasion = request.occasion,
                date = request.date,
                description = request.description,
                type = request.type,
                isActive = request.isActive
            };

            var result = await _svc.InsertAsync(entity, CurrentUserId);  

            if (!result.Success)
                return BadRequest(new { success = false, message = result.Message });

            return Ok(result);
        }

        [HttpPost("update")]
        public async Task<IActionResult> Update([FromBody] UpdateHolidayRequest request)
        {
            if (request == null)
                return BadRequest(new { success = false, message = "Invalid request data." });

            if (string.IsNullOrEmpty(request.holidayId))
                return BadRequest(new { success = false, message = "Holiday ID is required." });

            if (!int.TryParse(request.holidayId, out int holidayId))
                return BadRequest(new { success = false, message = "Invalid Holiday ID format." });

            if (string.IsNullOrWhiteSpace(request.holidayName))
                return BadRequest(new { success = false, message = "Holiday Name is required." });

            if (string.IsNullOrWhiteSpace(request.occasion))
                return BadRequest(new { success = false, message = "Occasion is required." });

            if (string.IsNullOrWhiteSpace(request.date))
                return BadRequest(new { success = false, message = "Date is required." });

            if (string.IsNullOrWhiteSpace(request.type))
                return BadRequest(new { success = false, message = "Type is required." });

            if (!DateTime.TryParse(request.date, out _))
                return BadRequest(new { success = false, message = "Invalid date format. Please use yyyy-MM-dd." });

            // Validate Holiday Type
            var validTypes = new[] { "National", "Regional", "Festival", "Optional" };
            if (!validTypes.Contains(request.type))
                return BadRequest(new { success = false, message = "Invalid Type. Valid types are: National, Regional, Festival, Optional." });

            var entity = new HolidayDTO
            {
                holidayId = request.holidayId,
                holidayName = request.holidayName,
                occasion = request.occasion,
                date = request.date,
                description = request.description,
                type = request.type,
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
        public async Task<IActionResult> Delete([FromBody] DeleteHolidayRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.holidayId))
                return BadRequest(new { success = false, message = "Holiday ID is required." });

            if (!int.TryParse(request.holidayId, out int id))
                return BadRequest(new { success = false, message = "Invalid Holiday ID format." });

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