using GenXTransitAPI.DataAccess.Interface.IServices;
using GenXTransitAPI.Models;
using GenXTransitAPI.Models.DTO_s;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GenXTransitAPI.Controllers
{
    [Route("api/farepolicy")]
    [ApiController]
    public class FarePolicyController : ControllerBase
    {
        private readonly IFarePolicyService _svc;

        public FarePolicyController(IFarePolicyService svc)
        {
            _svc = svc;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] string? searchText,
            [FromQuery] string? model,
            [FromQuery] string? policyStatus,
            [FromQuery] int? categoryId,
            [FromQuery] int? routeId,
            [FromQuery] bool? isActive,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var result = await _svc.GetAllAsync(searchText, model, policyStatus, categoryId, routeId, isActive, GetCurrentUserId(), pageNumber, pageSize);

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
        public async Task<IActionResult> Insert([FromBody] InsertFarePolicyRequest request)
        {
            if (request == null)
                return BadRequest(new { success = false, message = "Invalid request data." });

            if (string.IsNullOrWhiteSpace(request.model))
                return BadRequest(new { success = false, message = "Model is required." });

            if (string.IsNullOrWhiteSpace(request.policyStatus))
                return BadRequest(new { success = false, message = "Policy Status is required." });

            if (string.IsNullOrWhiteSpace(request.categoryId))
                return BadRequest(new { success = false, message = "Category is required." });

            if (string.IsNullOrWhiteSpace(request.routeId))
                return BadRequest(new { success = false, message = "Route is required." });

            if (request.baseFare <= 0)
                return BadRequest(new { success = false, message = "Base Fare must be greater than 0." });

            // Validate IDs
            if (!int.TryParse(request.categoryId, out int categoryId))
                return BadRequest(new { success = false, message = "Invalid Category ID format." });

            if (!int.TryParse(request.routeId, out int routeId))
                return BadRequest(new { success = false, message = "Invalid Route ID format." });

            // Validate Model
            var validModels = new[] { "Fixed", "Distance", "Zone" };
            if (!validModels.Contains(request.model))
                return BadRequest(new { success = false, message = "Invalid Model. Valid models are: Fixed, Distance, Zone." });

            // Validate Policy Status
            var validStatuses = new[] { "Published", "Simulated", "Draft" };
            if (!validStatuses.Contains(request.policyStatus))
                return BadRequest(new { success = false, message = "Invalid Policy Status. Valid statuses are: Published, Simulated, Draft." });

            var entity = new FarePolicyDTO
            {
                model = request.model,
                policyStatus = request.policyStatus,
                categoryId = request.categoryId,
                routeId = request.routeId,
                baseFare = request.baseFare,
                rateDescription = request.rateDescription,
                isActive = request.isActive
            };

            var result = await _svc.InsertAsync(entity, GetCurrentUserId());

            if (!result.Success)
                return BadRequest(new { success = false, message = result.Message });

            return Ok(result);
        }

        [HttpPost("update")]
        public async Task<IActionResult> Update([FromBody] UpdateFarePolicyRequest request)
        {
            if (request == null)
                return BadRequest(new { success = false, message = "Invalid request data." });

            if (string.IsNullOrEmpty(request.policyId))
                return BadRequest(new { success = false, message = "Policy ID is required." });

            if (!int.TryParse(request.policyId, out int policyId))
                return BadRequest(new { success = false, message = "Invalid Policy ID format." });

            if (string.IsNullOrWhiteSpace(request.model))
                return BadRequest(new { success = false, message = "Model is required." });

            if (string.IsNullOrWhiteSpace(request.policyStatus))
                return BadRequest(new { success = false, message = "Policy Status is required." });

            if (string.IsNullOrWhiteSpace(request.categoryId))
                return BadRequest(new { success = false, message = "Category is required." });

            if (string.IsNullOrWhiteSpace(request.routeId))
                return BadRequest(new { success = false, message = "Route is required." });

            if (request.baseFare <= 0)
                return BadRequest(new { success = false, message = "Base Fare must be greater than 0." });

            // Validate IDs
            if (!int.TryParse(request.categoryId, out int categoryId))
                return BadRequest(new { success = false, message = "Invalid Category ID format." });

            if (!int.TryParse(request.routeId, out int routeId))
                return BadRequest(new { success = false, message = "Invalid Route ID format." });

            // Validate Model
            var validModels = new[] { "Fixed", "Distance", "Zone" };
            if (!validModels.Contains(request.model))
                return BadRequest(new { success = false, message = "Invalid Model. Valid models are: Fixed, Distance, Zone." });

            // Validate Policy Status
            var validStatuses = new[] { "Published", "Simulated", "Draft" };
            if (!validStatuses.Contains(request.policyStatus))
                return BadRequest(new { success = false, message = "Invalid Policy Status. Valid statuses are: Published, Simulated, Draft." });

            var entity = new FarePolicyDTO
            {
                policyId = request.policyId,
                model = request.model,
                policyStatus = request.policyStatus,
                categoryId = request.categoryId,
                routeId = request.routeId,
                baseFare = request.baseFare,
                rateDescription = request.rateDescription,
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
        public async Task<IActionResult> Delete([FromBody] DeleteFarePolicyRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.policyId))
                return BadRequest(new { success = false, message = "Policy ID is required." });

            if (!int.TryParse(request.policyId, out int id))
                return BadRequest(new { success = false, message = "Invalid Policy ID format." });

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