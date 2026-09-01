using GenXTransitAPI.DataAccess.Interface.IServices;
using GenXTransitAPI.Models;
using GenXTransitAPI.Models.DTO_s;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GenXTransitAPI.Controllers
{
    [Route("api/complaintcategory")]
    [ApiController]
    public class ComplaintCategoryController : ControllerBase
    {
        private readonly IComplaintCategoryService _svc;

        public ComplaintCategoryController(IComplaintCategoryService svc)
        {
            _svc = svc;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] string? searchText,
            [FromQuery] string? complaintCategory,
            [FromQuery] string? sla,
            [FromQuery] bool? isActive,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var result = await _svc.GetAllAsync(searchText, complaintCategory, sla, isActive, GetCurrentUserId(), pageNumber, pageSize);

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
        public async Task<IActionResult> Insert([FromBody] InsertComplaintCategoryRequest request)
        {
            if (request == null)
                return BadRequest(new { success = false, message = "Invalid request data." });

            if (string.IsNullOrWhiteSpace(request.complaintTitle))
                return BadRequest(new { success = false, message = "Complaint Title is required." });

            if (string.IsNullOrWhiteSpace(request.complaintCategory))
                return BadRequest(new { success = false, message = "Complaint Category is required." });

            if (string.IsNullOrWhiteSpace(request.sla))
                return BadRequest(new { success = false, message = "SLA is required." });

            // Validate Complaint Category
            var validCategories = new[] { "General", "Technical", "Billing", "Service", "Other" };
            if (!validCategories.Contains(request.complaintCategory))
                return BadRequest(new { success = false, message = "Invalid Complaint Category. Valid categories are: General, Technical, Billing, Service, Other." });

            // Validate SLA
            var validSLAs = new[] { "24 Hours", "48 Hours", "72 Hours", "1 Week", "2 Weeks" };
            if (!validSLAs.Contains(request.sla))
                return BadRequest(new { success = false, message = "Invalid SLA. Valid SLAs are: 24 Hours, 48 Hours, 72 Hours, 1 Week, 2 Weeks." });

            var entity = new ComplaintCategoryDTO
            {
                complaintTitle = request.complaintTitle,
                description = request.description,
                complaintCategory = request.complaintCategory,
                sla = request.sla,
                isActive = request.isActive
            };

            var result = await _svc.InsertAsync(entity, GetCurrentUserId());

            if (!result.Success)
                return BadRequest(new { success = false, message = result.Message });

            return Ok(result);
        }

        [HttpPost("update")]
        public async Task<IActionResult> Update([FromBody] UpdateComplaintCategoryRequest request)
        {
            if (request == null)
                return BadRequest(new { success = false, message = "Invalid request data." });

            if (string.IsNullOrEmpty(request.complaintId))
                return BadRequest(new { success = false, message = "Complaint ID is required." });

            if (!int.TryParse(request.complaintId, out int complaintId))
                return BadRequest(new { success = false, message = "Invalid Complaint ID format." });

            if (string.IsNullOrWhiteSpace(request.complaintTitle))
                return BadRequest(new { success = false, message = "Complaint Title is required." });

            if (string.IsNullOrWhiteSpace(request.complaintCategory))
                return BadRequest(new { success = false, message = "Complaint Category is required." });

            if (string.IsNullOrWhiteSpace(request.sla))
                return BadRequest(new { success = false, message = "SLA is required." });

            // Validate Complaint Category
            var validCategories = new[] { "General", "Technical", "Billing", "Service", "Other" };
            if (!validCategories.Contains(request.complaintCategory))
                return BadRequest(new { success = false, message = "Invalid Complaint Category. Valid categories are: General, Technical, Billing, Service, Other." });

            // Validate SLA
            var validSLAs = new[] { "24 Hours", "48 Hours", "72 Hours", "1 Week", "2 Weeks" };
            if (!validSLAs.Contains(request.sla))
                return BadRequest(new { success = false, message = "Invalid SLA. Valid SLAs are: 24 Hours, 48 Hours, 72 Hours, 1 Week, 2 Weeks." });

            var entity = new ComplaintCategoryDTO
            {
                complaintId = request.complaintId,
                complaintTitle = request.complaintTitle,
                description = request.description,
                complaintCategory = request.complaintCategory,
                sla = request.sla,
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
        public async Task<IActionResult> Delete([FromBody] DeleteComplaintCategoryRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.complaintId))
                return BadRequest(new { success = false, message = "Complaint ID is required." });

            if (!int.TryParse(request.complaintId, out int id))
                return BadRequest(new { success = false, message = "Invalid Complaint ID format." });

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