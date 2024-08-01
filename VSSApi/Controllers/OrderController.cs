using Business.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
            return Ok(await _orderService.GetOrder(LOGICALREF));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetOrderDetail([FromQuery] string SiparisNumarasi)
        {
            return Ok(await _orderService.GetOrderDetail(SiparisNumarasi));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> ScanOrderBarcode([FromQuery] string Barkod)
        {
            return Ok(await _orderService.ScanOrderBarcode(Barkod));
        }
    }
}
