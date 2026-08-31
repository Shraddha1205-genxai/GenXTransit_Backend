using GenXTransitAPI.DataAccess.Interface.IServices;
using GenXTransitAPI.Models;
using GenXTransitAPI.Models.DTO_s;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace GenXTransitAPI.Controllers
{
    [Route("api/holiday")]
    [ApiController]
    public class HolidayController : ControllerBase
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
            var result = await _svc.GetAllAsync(searchText, type, startDate, endDate, isActive, GetCurrentUserId(), pageNumber, pageSize);
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
        public async Task<IActionResult> Insert([FromBody] InsertHolidayRequest request)
        {
            if (request == null)
                return BadRequest(ApiResponse<int>.Fail("Invalid request data."));

            if (string.IsNullOrWhiteSpace(request.holidayName))
                return BadRequest(ApiResponse<int>.Fail("Holiday Name is required."));

            if (string.IsNullOrWhiteSpace(request.occasion))
                return BadRequest(ApiResponse<int>.Fail("Occasion is required."));

            if (string.IsNullOrWhiteSpace(request.date))
                return BadRequest(ApiResponse<int>.Fail("Date is required."));

            if (string.IsNullOrWhiteSpace(request.type))
                return BadRequest(ApiResponse<int>.Fail("Type is required."));

            if (!DateTime.TryParse(request.date, out _))
                return BadRequest(ApiResponse<int>.Fail("Invalid date format. Please use yyyy-MM-dd."));

            var entity = new HolidayDTO
            {
                holidayName = request.holidayName,
                occasion = request.occasion,
                date = request.date,
                description = request.description,
                type = request.type,
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
        public async Task<IActionResult> Update([FromBody] UpdateHolidayRequest request)
        {
            if (request == null)
                return BadRequest(ApiResponse<bool>.Fail("Invalid request data."));

            if (string.IsNullOrEmpty(request.holidayId))
                return BadRequest(ApiResponse<bool>.Fail("Holiday ID is required."));

            if (!int.TryParse(request.holidayId, out int id))
                return BadRequest(ApiResponse<bool>.Fail("Invalid Holiday ID format."));

            if (string.IsNullOrWhiteSpace(request.holidayName))
                return BadRequest(ApiResponse<bool>.Fail("Holiday Name is required."));

            if (string.IsNullOrWhiteSpace(request.occasion))
                return BadRequest(ApiResponse<bool>.Fail("Occasion is required."));

            if (string.IsNullOrWhiteSpace(request.date))
                return BadRequest(ApiResponse<bool>.Fail("Date is required."));

            if (string.IsNullOrWhiteSpace(request.type))
                return BadRequest(ApiResponse<bool>.Fail("Type is required."));

            if (!DateTime.TryParse(request.date, out _))
                return BadRequest(ApiResponse<bool>.Fail("Invalid date format. Please use yyyy-MM-dd."));

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
        public async Task<IActionResult> Delete([FromBody] DeleteHolidayRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.holidayId))
                return BadRequest(ApiResponse<bool>.Fail("Holiday ID is required."));

            if (!int.TryParse(request.holidayId, out int id))
                return BadRequest(ApiResponse<bool>.Fail("Invalid Holiday ID format."));

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