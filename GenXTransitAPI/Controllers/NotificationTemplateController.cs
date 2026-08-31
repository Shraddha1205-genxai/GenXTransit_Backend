using GenXTransitAPI.DataAccess.Interface.IServices;
using GenXTransitAPI.Models;
using GenXTransitAPI.Models.DTO_s;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GenXTransitAPI.Controllers
{
    [Route("api/notificationtemplate")]
    [ApiController]
    public class NotificationTemplateController : ControllerBase
    {
        private readonly INotificationTemplateService _svc;

        public NotificationTemplateController(INotificationTemplateService svc)
        {
            _svc = svc;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] string? searchText,
            [FromQuery] string? channel,
            [FromQuery] bool? isActive,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var result = await _svc.GetAllAsync(searchText, channel, isActive, GetCurrentUserId(), pageNumber, pageSize);
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
        public async Task<IActionResult> Insert([FromBody] InsertNotificationTemplateRequest request)
        {
            if (request == null)
                return BadRequest(ApiResponse<int>.Fail("Invalid request data."));

            if (string.IsNullOrWhiteSpace(request.notificationTitle))
                return BadRequest(ApiResponse<int>.Fail("Notification Title is required."));

            if (string.IsNullOrWhiteSpace(request.channel))
                return BadRequest(ApiResponse<int>.Fail("Channel is required."));

            // Validate channel
            var validChannels = new[] { "Email", "SMS", "Push", "InApp" };
            if (!validChannels.Contains(request.channel))
                return BadRequest(ApiResponse<int>.Fail("Invalid Channel. Valid channels are: Email, SMS, Push, InApp."));

            var entity = new NotificationTemplateDTO
            {
                notificationTitle = request.notificationTitle,
                channel = request.channel,
                description = request.description,
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
        public async Task<IActionResult> Update([FromBody] UpdateNotificationTemplateRequest request)
        {
            if (request == null)
                return BadRequest(ApiResponse<bool>.Fail("Invalid request data."));

            if (string.IsNullOrEmpty(request.notificationId))
                return BadRequest(ApiResponse<bool>.Fail("Notification ID is required."));

            if (!int.TryParse(request.notificationId, out int id))
                return BadRequest(ApiResponse<bool>.Fail("Invalid Notification ID format."));

            if (string.IsNullOrWhiteSpace(request.notificationTitle))
                return BadRequest(ApiResponse<bool>.Fail("Notification Title is required."));

            if (string.IsNullOrWhiteSpace(request.channel))
                return BadRequest(ApiResponse<bool>.Fail("Channel is required."));

            // Validate channel
            var validChannels = new[] { "Email", "SMS", "Push", "InApp" };
            if (!validChannels.Contains(request.channel))
                return BadRequest(ApiResponse<bool>.Fail("Invalid Channel. Valid channels are: Email, SMS, Push, InApp."));

            var entity = new NotificationTemplateDTO
            {
                notificationId = request.notificationId,
                notificationTitle = request.notificationTitle,
                channel = request.channel,
                description = request.description,
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
        public async Task<IActionResult> Delete([FromBody] DeleteNotificationTemplateRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.notificationId))
                return BadRequest(ApiResponse<bool>.Fail("Notification ID is required."));

            if (!int.TryParse(request.notificationId, out int id))
                return BadRequest(ApiResponse<bool>.Fail("Invalid Notification ID format."));

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