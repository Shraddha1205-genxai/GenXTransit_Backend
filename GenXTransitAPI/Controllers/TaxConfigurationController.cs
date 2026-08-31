using GenXTransitAPI.DataAccess.Interface.IServices;
using GenXTransitAPI.Models;
using GenXTransitAPI.Models.DTO_s;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GenXTransitAPI.Controllers
{
    [Route("api/taxconfiguration")]
    [ApiController]
    public class TaxConfigurationController : ControllerBase
    {
        private readonly ITaxConfigurationService _svc;

        public TaxConfigurationController(ITaxConfigurationService svc)
        {
            _svc = svc;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] string? searchText,
            [FromQuery] string? taxType,
            [FromQuery] decimal? rateFrom,
            [FromQuery] decimal? rateTo,
            [FromQuery] bool? isActive,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var result = await _svc.GetAllAsync(searchText, taxType, rateFrom, rateTo, isActive, GetCurrentUserId(), pageNumber, pageSize);
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
        public async Task<IActionResult> Insert([FromBody] InsertTaxConfigurationRequest request)
        {
            if (request == null)
                return BadRequest(ApiResponse<int>.Fail("Invalid request data."));

            if (string.IsNullOrWhiteSpace(request.taxType))
                return BadRequest(ApiResponse<int>.Fail("Tax Type is required."));

            if (request.rate < 0)
                return BadRequest(ApiResponse<int>.Fail("Tax rate cannot be negative."));

            // Validate Tax Type
            var validTaxTypes = new[] { "GST", "Service Tax", "VAT", "Cess", "Others" };
            if (!validTaxTypes.Contains(request.taxType))
                return BadRequest(ApiResponse<int>.Fail("Invalid Tax Type. Valid types are: GST, Service Tax, VAT, Cess, Others."));

            var entity = new TaxConfigurationDTO
            {
                taxType = request.taxType,
                description = request.description,
                rate = request.rate,
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
        public async Task<IActionResult> Update([FromBody] UpdateTaxConfigurationRequest request)
        {
            if (request == null)
                return BadRequest(ApiResponse<bool>.Fail("Invalid request data."));

            if (string.IsNullOrEmpty(request.taxId))
                return BadRequest(ApiResponse<bool>.Fail("Tax ID is required."));

            if (!int.TryParse(request.taxId, out int id))
                return BadRequest(ApiResponse<bool>.Fail("Invalid Tax ID format."));

            if (string.IsNullOrWhiteSpace(request.taxType))
                return BadRequest(ApiResponse<bool>.Fail("Tax Type is required."));

            if (request.rate < 0)
                return BadRequest(ApiResponse<bool>.Fail("Tax rate cannot be negative."));

            // Validate Tax Type
            var validTaxTypes = new[] { "GST", "Service Tax", "VAT", "Cess", "Others" };
            if (!validTaxTypes.Contains(request.taxType))
                return BadRequest(ApiResponse<bool>.Fail("Invalid Tax Type. Valid types are: GST, Service Tax, VAT, Cess, Others."));

            var entity = new TaxConfigurationDTO
            {
                taxId = request.taxId,
                taxType = request.taxType,
                description = request.description,
                rate = request.rate,
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
        public async Task<IActionResult> Delete([FromBody] DeleteTaxConfigurationRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.taxId))
                return BadRequest(ApiResponse<bool>.Fail("Tax ID is required."));

            if (!int.TryParse(request.taxId, out int id))
                return BadRequest(ApiResponse<bool>.Fail("Invalid Tax ID format."));

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