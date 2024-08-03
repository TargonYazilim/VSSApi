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
            return Ok(await _orderService.GetOrderProcedure(LOGICALREF));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetOrderDetail([FromQuery] string SiparisNumarasi)
        {
            return Ok(await _orderService.GetOrderDetailProcedure(SiparisNumarasi));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> ScanOrderBarcode([FromQuery] string Barkod, string siparisNumarasi, string malzemeKodu)
        {
            return Ok(await _orderService.ScanOrderBarcodeProcedure(Barkod, siparisNumarasi, malzemeKodu));
        }
    }
}
