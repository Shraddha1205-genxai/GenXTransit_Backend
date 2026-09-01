using GenXTransitAPI.DataAccess.Interface.IServices;
using GenXTransitAPI.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GenXTransitAPI.Controllers
{
    [Route("api/tab")]
    [ApiController]
    [AllowAnonymous]
    public class TabController : BaseController
    {
        private readonly ITabService _tabService;

        public TabController(ITabService tabService)
        {
            _tabService = tabService;
        }

        // GET: api/tab
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _tabService.GetAllTabsAsync();

            return Ok(response);
        }

        // GET: api/tab/1
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await _tabService.GetTabByIdAsync(id);

            return Ok(response);
        }

        // POST: api/tab
        [HttpPost("insert")]
        public async Task<IActionResult> Create([FromBody] TabCreateDto request)
        {
            var response = await _tabService.CreateTabAsync(request, CurrentUserId);

            return Ok(response);
        }

        // PUT: api/tab
        [HttpPost("update")]
        public async Task<IActionResult> Update( [FromBody] TabUpdateDto request)
        {
            var response = await _tabService.UpdateTabAsync(request,CurrentUserId);

            return Ok(response);
        }

        // DELETE: api/tab/1?modifiedBy=1
        [HttpPost("delete")]
            public async Task<IActionResult> DeleteTab(DeleteTabRequest request)
        {
            var response = await _tabService.DeleteTabAsync(
                request.TabId,
                CurrentUserId);

            return Ok(response);
        }
    }
}
