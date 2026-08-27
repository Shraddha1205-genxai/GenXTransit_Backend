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
        public async Task<IActionResult> Insert([FromBody] InsertFarePolicyRequest request)
        {
            if (request == null)
                return BadRequest(ApiResponse<int>.Fail("Invalid request data."));

            if (string.IsNullOrWhiteSpace(request.model))
                return BadRequest(ApiResponse<int>.Fail("Model is required."));

            if (string.IsNullOrWhiteSpace(request.policyStatus))
                return BadRequest(ApiResponse<int>.Fail("Policy Status is required."));

            if (string.IsNullOrWhiteSpace(request.categoryId))
                return BadRequest(ApiResponse<int>.Fail("Category is required."));

            if (string.IsNullOrWhiteSpace(request.routeId))
                return BadRequest(ApiResponse<int>.Fail("Route is required."));

            if (request.baseFare <= 0)
                return BadRequest(ApiResponse<int>.Fail("Base Fare must be greater than 0."));

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
            if (result.Success && result.Data > 0)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("update")]
        public async Task<IActionResult> Update([FromBody] UpdateFarePolicyRequest request)
        {
            if (request == null)
                return BadRequest(ApiResponse<bool>.Fail("Invalid request data."));

            if (string.IsNullOrEmpty(request.policyId))
                return BadRequest(ApiResponse<bool>.Fail("Policy ID is required."));

            if (!int.TryParse(request.policyId, out int id))
                return BadRequest(ApiResponse<bool>.Fail("Invalid Policy ID format."));

            if (string.IsNullOrWhiteSpace(request.model))
                return BadRequest(ApiResponse<bool>.Fail("Model is required."));

            if (string.IsNullOrWhiteSpace(request.policyStatus))
                return BadRequest(ApiResponse<bool>.Fail("Policy Status is required."));

            if (string.IsNullOrWhiteSpace(request.categoryId))
                return BadRequest(ApiResponse<bool>.Fail("Category is required."));

            if (string.IsNullOrWhiteSpace(request.routeId))
                return BadRequest(ApiResponse<bool>.Fail("Route is required."));

            if (request.baseFare <= 0)
                return BadRequest(ApiResponse<bool>.Fail("Base Fare must be greater than 0."));

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
                if (!string.IsNullOrEmpty(result.Message) && result.Message.Contains("not found"))
                    return NotFound(result);
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPost("delete")]
        public async Task<IActionResult> Delete([FromBody] DeleteFarePolicyRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.policyId))
                return BadRequest(ApiResponse<bool>.Fail("Policy ID is required."));

            if (!int.TryParse(request.policyId, out int id))
                return BadRequest(ApiResponse<bool>.Fail("Invalid Policy ID format."));

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