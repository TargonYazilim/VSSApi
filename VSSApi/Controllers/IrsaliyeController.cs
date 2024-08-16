using Business.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace VSSApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class IrsaliyeController : ControllerBase
    {
        private readonly IIrsaliyeService _irsaliyeService;

        public IrsaliyeController(IIrsaliyeService irsaliyeService)
        {
            _irsaliyeService = irsaliyeService;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetIrsaliye([FromQuery] string siparisNumarasi)
        {
            var response = await _irsaliyeService.GetIrsaliye(siparisNumarasi);
            return Ok(response);
        }
    }
}
