using Business.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Models.StoreProcedure;

namespace VSSApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetOrder([FromQuery] int LOGICALREF)
        {
            return Ok(await _orderService.GetOrderProcedure(LOGICALREF));
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> ScanOrderBarcode([FromBody] ScanBarcode scanBarcode)
        {
            return Ok(await _orderService.ScanOrderBarcodeProcedure(scanBarcode));
        }
    }
}
