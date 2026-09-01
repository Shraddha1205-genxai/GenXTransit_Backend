using GenXTransitAPI.DataAccess.Interface.IServices;
using GenXTransitAPI.Models;
using GenXTransitAPI.Models.DTO_s;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GenXTransitAPI.Controllers
{
    [Route("api/paymentmode")]
    [ApiController]
    public class PaymentModeController : ControllerBase
    {
        private readonly IPaymentModeService _svc;

        public PaymentModeController(IPaymentModeService svc)
        {
            _svc = svc;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] string? searchText,
            [FromQuery] string? modeStatus,
            [FromQuery] bool? isActive,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var result = await _svc.GetAllAsync(searchText, modeStatus, isActive, GetCurrentUserId(), pageNumber, pageSize);

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
        public async Task<IActionResult> Insert([FromBody] InsertPaymentModeRequest request)
        {
            if (request == null)
                return BadRequest(new { success = false, message = "Invalid request data." });

            if (string.IsNullOrWhiteSpace(request.modeName))
                return BadRequest(new { success = false, message = "Mode Name is required." });

            if (string.IsNullOrWhiteSpace(request.modeStatus))
                return BadRequest(new { success = false, message = "Mode Status is required." });

            // Validate Mode Status
            var validStatuses = new[] { "Live", "Under Maintenance", "Disabled" };
            if (!validStatuses.Contains(request.modeStatus))
                return BadRequest(new { success = false, message = "Invalid Mode Status. Valid statuses are: Live , Under Maintenance , Disabled." });

            var entity = new PaymentModeDTO
            {
                modeName = request.modeName,
                modeStatus = request.modeStatus,
                description = request.description,
                isActive = request.isActive
            };

            var result = await _svc.InsertAsync(entity, GetCurrentUserId());

            if (!result.Success)
                return BadRequest(new { success = false, message = result.Message });

            return Ok(result);
        }

        [HttpPost("update")]
        public async Task<IActionResult> Update([FromBody] UpdatePaymentModeRequest request)
        {
            if (request == null)
                return BadRequest(new { success = false, message = "Invalid request data." });

            if (string.IsNullOrEmpty(request.modeId))
                return BadRequest(new { success = false, message = "Mode ID is required." });

            if (!int.TryParse(request.modeId, out int modeId))
                return BadRequest(new { success = false, message = "Invalid Mode ID format." });

            if (string.IsNullOrWhiteSpace(request.modeName))
                return BadRequest(new { success = false, message = "Mode Name is required." });

            if (string.IsNullOrWhiteSpace(request.modeStatus))
                return BadRequest(new { success = false, message = "Mode Status is required." });

            // Validate Mode Status
            var validStatuses = new[] { "Live", "Under Maintenance", "Disabled" };
            if (!validStatuses.Contains(request.modeStatus))
                return BadRequest(new { success = false, message = "Invalid Mode Status. Valid statuses are: Live , Under Maintenance , Disabled." });

            var entity = new PaymentModeDTO
            {
                modeId = request.modeId,
                modeName = request.modeName,
                modeStatus = request.modeStatus,
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
        public async Task<IActionResult> Delete([FromBody] DeletePaymentModeRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.modeId))
                return BadRequest(new { success = false, message = "Mode ID is required." });

            if (!int.TryParse(request.modeId, out int id))
                return BadRequest(new { success = false, message = "Invalid Mode ID format." });

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