using Business.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace VSSApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BarcodeController : ControllerBase
    {
        private readonly IBarcodeService _barcodeService;

        public BarcodeController(IBarcodeService barcodeService)
        {
            _barcodeService = barcodeService;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetBarcodes()
        {
            return Ok(await _barcodeService.GetAllBarcodes());
        }
    }
}
