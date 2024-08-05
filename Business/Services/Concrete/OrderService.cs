using Business.Services.Abstract;
using DataAccess.Dal.Abstract;
using Entities.Concrete;
using Entities.Dtos;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Shared.Models.StoreProcedure;

namespace Business.Services.Concrete
{
    public class OrderService : IOrderService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IOrderDal _orderDal;
        private readonly IUserService _userService;
        public OrderService(IOrderDal orderDal, IHttpContextAccessor httpContextAccessor, IUserService userService)
        {
            _orderDal = orderDal;
            _httpContextAccessor = httpContextAccessor;
            _userService = userService;
        }
        public async Task<OrderResult?> GetOrderProcedure(int LOGICALREF)
        {

            var user = await ContextUser();
            var ordersFromProcedure = await _orderDal.GetOrderProcedure(LOGICALREF);
            /// Procedure'dan order detaylarını çekerken eğer ara tabloda scanResult mevcut ise o veriyi 
            /// OrderFromProcedure'e implement et.
            if (ordersFromProcedure != null)
            {
                foreach (var order in ordersFromProcedure.orders)
                {
                    foreach (var orderFromProcedure in order.orderDetails)
                    {

                        var orderFromDb = await _orderDal.GetOrderBySiparisNumarasi(user.Id, order.siparisNumarasi);
                        if (orderFromDb != null)
                        {
                            var hasOrder = orderFromDb.OrderDetails.FirstOrDefault(x => x.siparisId == orderFromProcedure.siparisId);
                            if (hasOrder != null && hasOrder.Scans != null)
                            {
                                orderFromProcedure.scans = hasOrder.Scans.Select(scan => new ScanProcedure
                                {
                                    scanId = scan.Id,
                                    result = scan.result
                                }).ToList();
                            }
                        }
                    }
                }
            }
            return ordersFromProcedure;
        }

        public async Task<OrderBarcodeScanResult?> ScanOrderBarcodeProcedure(ScanBarcode scanBarcode)
        {
            var barcodeResult = await _orderDal.ScanOrderBarcodeProcedure(scanBarcode.barkod);
            if (Convert.ToInt32(barcodeResult?.ErrorCode) == 0 && barcodeResult != null)
            {
                var user = await ContextUser();

                var orderResult = await _orderDal.GetAsync(x => x.UserId == user.Id && x.siparisNumarasi == scanBarcode.siparisNumarasi, includes: i => i.OrderDetails);

                /// Create order
                if (orderResult == null)
                {
                    await AddOrder(new Order()
                    {
                        siparisNumarasi = scanBarcode.siparisNumarasi,
                        status = false,
                        UserId = user.Id,
                        OrderDetails = new List<OrderDetail>
                     {
                         new OrderDetail()
                         {
                             malzemeKodu = scanBarcode.malzemeKodu,
                             siparisNumarasi = scanBarcode.siparisNumarasi,
                             siparisId = scanBarcode.siparisId,
                             Scans = new List<Scan>(){
                             new Scan()
                             {
                                 result =barcodeResult.Items.First().Kilo,
                             }
                             }
                         }
                      }
                    });
                }
                /// Update order
                else if (orderResult != null && orderResult.status == false)
                {
                    var orderDetail = orderResult.OrderDetails.FirstOrDefault(x => x.siparisId == scanBarcode.siparisId);
                    if (orderDetail != null) /// Bir sipariş oluşturulmuşsa yeni bir tarama ekle
                    {
                        /// Var olan order detayını güncelleme
                        orderDetail.Scans.Add(new Scan { result = barcodeResult.Items.FirstOrDefault()?.Kilo ?? "0" });
                        await UpdateOrder(orderResult);
                    }
                    else /// Hiç sipariş oluşturulmadıysa bir sipariş oluştur. 
                    {
                        orderResult.OrderDetails.Add(new OrderDetail()
                        {
                            siparisId = scanBarcode.siparisId,
                            siparisNumarasi = scanBarcode.siparisNumarasi,
                            malzemeKodu = scanBarcode.malzemeKodu,
                            Scans = new List<Scan>(){
                             new Scan()
                             {
                                 result =barcodeResult.Items.First().Kilo,
                             }
                        }
                        });
                        await UpdateOrder(orderResult);
                    }
                }
                else
                {
                    return new OrderBarcodeScanResult()
                    {
                        ErrorCode = 1,
                        Result = "Bu sipariş tamamlandı",
                    };
                }
            }
            return barcodeResult;
        }

        public async Task<int?> AddOrder(Order order)
        {
            return await _orderDal.Add(order);
        }

        public async Task<bool> UpdateOrder(Order order)
        {
            return await _orderDal.Update(order);
        }

        public async Task<List<Order>?> GetAllOrdersByUserId(int userId)
        {
            return await _orderDal.GetAllAsync(x => x.UserId == userId, tracking: false, a => a.OrderDetails);
        }

        private async Task<UserDto> ContextUser()
        {
            var username = _httpContextAccessor.HttpContext?.User?.Identity?.Name;
            if (!string.IsNullOrEmpty(username))
            {
                UserDto? user = await _userService.GetByUsername(username);
                if (user == null) throw new Exception("Kullanıcı bulunamadı");
                return user;
            }
            throw new Exception("Beklenmeyen bir hatayla karşılaşıldı!!!");
        }
    }
}
