using GenXTransitAPI.DataAccess.Interface.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GenXTransitAPI.Controllers
{
    [Route("api/driverconductoravailability")]
    [ApiController]
    public class DriverConductorController : BaseController
    {
        private readonly IDriverConductorService _driverConductorService;

        public DriverConductorController(IDriverConductorService driverConductorService)
        {
            _driverConductorService = driverConductorService;
        }

        [HttpGet("driver-conductor")]
        public async Task<IActionResult> GetDriverConductor(
    [FromQuery] int? roleId = null,
    [FromQuery] string? roleName = null,
    [FromQuery] int? depotId = null)
        {
            var response = await _driverConductorService.GetDriverConductorAsync(roleId,roleName, depotId);

            return Ok(response);
        }
    }
}
