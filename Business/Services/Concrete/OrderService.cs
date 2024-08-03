
using AutoMapper;
using Business.Services.Abstract;
using DataAccess.Dal.Abstract;
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
        private readonly IMapper _mapper;
        public OrderService(IOrderDal orderDal, IMapper mapper, IHttpContextAccessor httpContextAccessor, IUserService userService)
        {
            _orderDal = orderDal;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _userService = userService;
        }
        public async Task<OrderResult?> GetOrderProcedure(int LOGICALREF)
        {
            return await _orderDal.GetOrderProcedure(LOGICALREF);
        }

        public async Task<OrderDetailResult?> GetOrderDetailProcedure(string SiparisNumarasi)
        {
            return await _orderDal.GetOrderDetailProcedure(SiparisNumarasi);
        }

        public async Task<OrderBarcodeScanResult?> ScanOrderBarcodeProcedure(string Barkod, string siparisNumarasi, string malzemeKodu)
        {
            var barcodeResult = await _orderDal.ScanOrderBarcodeProcedure(Barkod);
            if (Convert.ToInt32(barcodeResult?.ErrorCode) == 0 && barcodeResult != null)
            {
                var user = await ContextUser();

                var orderResult = await GetOrderBySiparisNumarasi(siparisNumarasi, user.Id);

                /// Create order
                if (orderResult == null)
                {
                    await AddOrder(new Order()
                    {
                        siparisNumarasi = siparisNumarasi,
                        status = false,
                        UserId = user.Id,
                        OrderDetails = new List<OrderDetail>
                     {
                         new OrderDetail()
                         {
                             malzemeKodu =malzemeKodu,
                             scanResult = barcodeResult.Items.First().Kilo,
                             siparisNumarasi = siparisNumarasi,
                          }
                      }
                    });
                }
                /// Update order
                else if (orderResult != null && orderResult.status == false)
                {
                    var orderDetail = orderResult.OrderDetails.FirstOrDefault(x => x.malzemeKodu == malzemeKodu);
                    if (orderDetail != null)
                    {
                        /// Var olan order detayını güncelleme
                        orderDetail.scanResult = barcodeResult.Items.FirstOrDefault()?.Kilo ?? "0";
                        await UpdateOrder(orderResult);
                    }
                    else
                    {
                        orderResult.OrderDetails.Add(new OrderDetail()
                        {
                            siparisNumarasi = siparisNumarasi,
                            malzemeKodu = malzemeKodu,
                            scanResult = barcodeResult.Items.First().Kilo,
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

        public async Task<Order?> GetOrderBySiparisNumarasi(string siparisNumarasi, int userId)
        {
            return await _orderDal.GetOrderBySiparisNumarasi(siparisNumarasi, userId);
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
