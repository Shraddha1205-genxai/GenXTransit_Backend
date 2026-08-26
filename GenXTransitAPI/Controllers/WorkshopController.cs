using GenXTransitAPI.DataAccess.Interface.IServices;
using GenXTransitAPI.Models;
using GenXTransitAPI.Models.DTO_s;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GenXTransitAPI.Controllers
{
    [Route("api/workshop")]
    [ApiController]
    public class WorkshopController : ControllerBase
    {
        private readonly IOrgWorkshopService _svc;

        public WorkshopController(IOrgWorkshopService svc)
        {
            _svc = svc;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] string? searchText,
            [FromQuery] int? regionId,
            [FromQuery] int? divisionId,
            [FromQuery] int? depotId,
            [FromQuery] bool? isActive,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var result = await _svc.GetAllAsync(searchText, regionId, divisionId, depotId, isActive, GetCurrentUserId(), pageNumber, pageSize);
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
        public async Task<IActionResult> Insert([FromBody] InsertWorkshopRequest request)
        {
            if (request == null)
                return BadRequest(ApiResponse<int>.Fail("Invalid request data."));

            if (string.IsNullOrWhiteSpace(request.workShopName))
                return BadRequest(ApiResponse<int>.Fail("Workshop Name is required."));

            if (string.IsNullOrWhiteSpace(request.regionId))
                return BadRequest(ApiResponse<int>.Fail("Region is required."));

            if (string.IsNullOrWhiteSpace(request.divisionId))
                return BadRequest(ApiResponse<int>.Fail("Division is required."));

            if (string.IsNullOrWhiteSpace(request.depotId))
                return BadRequest(ApiResponse<int>.Fail("Depot is required."));

            if (request.workBays < 0)
                return BadRequest(ApiResponse<int>.Fail("Work Bays cannot be negative."));

            var entity = new OrgWorkshopDTO
            {
                workShopName = request.workShopName,
                regionId = request.regionId,
                divisionId = request.divisionId,
                depotId = request.depotId,
                workBays = request.workBays,
                activeRepairJobs = request.activeRepairJobs,
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
        public async Task<IActionResult> Update([FromBody] UpdateWorkshopRequest request)
        {
            if (request == null)
                return BadRequest(ApiResponse<bool>.Fail("Invalid request data."));

            if (string.IsNullOrEmpty(request.workShopId))
                return BadRequest(ApiResponse<bool>.Fail("Workshop ID is required."));

            if (!int.TryParse(request.workShopId, out int id))
                return BadRequest(ApiResponse<bool>.Fail("Invalid Workshop ID format."));

            if (string.IsNullOrWhiteSpace(request.workShopName))
                return BadRequest(ApiResponse<bool>.Fail("Workshop Name is required."));

            if (string.IsNullOrWhiteSpace(request.regionId))
                return BadRequest(ApiResponse<bool>.Fail("Region is required."));

            if (string.IsNullOrWhiteSpace(request.divisionId))
                return BadRequest(ApiResponse<bool>.Fail("Division is required."));

            if (string.IsNullOrWhiteSpace(request.depotId))
                return BadRequest(ApiResponse<bool>.Fail("Depot is required."));

            if (request.workBays < 0)
                return BadRequest(ApiResponse<bool>.Fail("Work Bays cannot be negative."));

            var entity = new OrgWorkshopDTO
            {
                workShopId = request.workShopId,
                workShopName = request.workShopName,
                regionId = request.regionId,
                divisionId = request.divisionId,
                depotId = request.depotId,
                workBays = request.workBays,
                activeRepairJobs = request.activeRepairJobs,
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
        public async Task<IActionResult> Delete([FromBody] DeleteWorkshopRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.workShopId))
                return BadRequest(ApiResponse<bool>.Fail("Workshop ID is required."));

            if (!int.TryParse(request.workShopId, out int id))
                return BadRequest(ApiResponse<bool>.Fail("Invalid Workshop ID format."));

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