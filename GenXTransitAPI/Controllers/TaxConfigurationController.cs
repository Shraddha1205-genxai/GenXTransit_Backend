using GenXTransitAPI.DataAccess.Interface.IServices;
using GenXTransitAPI.Models;
using GenXTransitAPI.Models.DTO_s;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GenXTransitAPI.Controllers
{
    [Route("api/taxconfiguration")]
    [ApiController]
    [AllowAnonymous]  
    public class TaxConfigurationController : BaseController  
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
            var result = await _svc.GetAllAsync(searchText, taxType, rateFrom, rateTo, isActive, CurrentUserId, pageNumber, pageSize);  

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
        public async Task<IActionResult> Insert([FromBody] InsertTaxConfigurationRequest request)
        {
            if (request == null)
                return BadRequest(new { success = false, message = "Invalid request data." });

            if (string.IsNullOrWhiteSpace(request.taxType))
                return BadRequest(new { success = false, message = "Tax Type is required." });

            if (request.rate < 0)
                return BadRequest(new { success = false, message = "Tax rate cannot be negative." });

            // Validate Tax Type
            var validTaxTypes = new[] { "GST", "Service Tax", "VAT", "Cess", "Others" };
            if (!validTaxTypes.Contains(request.taxType))
                return BadRequest(new { success = false, message = "Invalid Tax Type. Valid types are: GST, Service Tax, VAT, Cess, Others." });

            var entity = new TaxConfigurationDTO
            {
                taxType = request.taxType,
                description = request.description,
                rate = request.rate,
                isActive = request.isActive
            };

            var result = await _svc.InsertAsync(entity, CurrentUserId);  

            if (!result.Success)
                return BadRequest(new { success = false, message = result.Message });

            return Ok(result);
        }

        [HttpPost("update")]
        public async Task<IActionResult> Update([FromBody] UpdateTaxConfigurationRequest request)
        {
            if (request == null)
                return BadRequest(new { success = false, message = "Invalid request data." });

            if (string.IsNullOrEmpty(request.taxId))
                return BadRequest(new { success = false, message = "Tax ID is required." });

            if (!int.TryParse(request.taxId, out int taxId))
                return BadRequest(new { success = false, message = "Invalid Tax ID format." });

            if (string.IsNullOrWhiteSpace(request.taxType))
                return BadRequest(new { success = false, message = "Tax Type is required." });

            if (request.rate < 0)
                return BadRequest(new { success = false, message = "Tax rate cannot be negative." });

            // Validate Tax Type
            var validTaxTypes = new[] { "GST", "Service Tax", "VAT", "Cess", "Others" };
            if (!validTaxTypes.Contains(request.taxType))
                return BadRequest(new { success = false, message = "Invalid Tax Type. Valid types are: GST, Service Tax, VAT, Cess, Others." });

            var entity = new TaxConfigurationDTO
            {
                taxId = request.taxId,
                taxType = request.taxType,
                description = request.description,
                rate = request.rate,
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
        public async Task<IActionResult> Delete([FromBody] DeleteTaxConfigurationRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.taxId))
                return BadRequest(new { success = false, message = "Tax ID is required." });

            if (!int.TryParse(request.taxId, out int id))
                return BadRequest(new { success = false, message = "Invalid Tax ID format." });

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