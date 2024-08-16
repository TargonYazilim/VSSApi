using Business.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace VSSApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ScanController : ControllerBase
    {
        private readonly IScanService _scanService;

        public ScanController(IScanService scanService)
        {
            _scanService = scanService;
        }

        [HttpDelete("[action]")]
        public async Task<IActionResult> DeleteScan([FromQuery] string scanId)
        {
            var response = await _scanService.DeleteScan(scanId);
            return Ok(response);
        }

    }
}
