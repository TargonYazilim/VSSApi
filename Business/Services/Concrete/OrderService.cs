using Business.Services.Abstract;
using DataAccess.Dal.Abstract;
using Entities.Concrete;
using Entities.Dtos;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Shared.Models.CreateUpdate;
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
                                order.Id = orderFromDb.Id;
                                orderFromProcedure.Id = hasOrder.Id;
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

        public async Task<List<string>?> ScanOrderBarcodeProcedure(List<CreateUpdateOrder> createUpdateOrders)
        {
            var user = await ContextUser();
            List<string> synchronizedOrders = new List<string>();
            foreach (var createUpdateOrder in createUpdateOrders)
            {

                var orderResult = await _orderDal.GetAsync(x => x.UserId == user.Id && (x.siparisNumarasi == createUpdateOrder.siparisNumarasi || (createUpdateOrder.Id != null && x.Id == createUpdateOrder.Id)), includes: i => i.OrderDetails);

                /// Create order
                if (orderResult == null)
                {
                    await CreateOrder(createUpdateOrder, user.Id);
                    synchronizedOrders.Add(createUpdateOrder.siparisNumarasi);
                }
                /// Update order
                else if (orderResult != null && orderResult.status == false)
                {
                    await UpdateOrderWithDetails(orderResult, createUpdateOrder);
                    synchronizedOrders.Add(createUpdateOrder.siparisNumarasi);
                }
            }
            return synchronizedOrders;
        }

        private async Task CreateOrder(CreateUpdateOrder createUpdateOrder, int userId)
        {
            var order = new Order
            {
                siparisNumarasi = createUpdateOrder.siparisNumarasi,
                status = false,
                UserId = userId,
                synchronized = true,
                OrderDetails = createUpdateOrder.orderDetails.Select(od => new OrderDetail
                {
                    malzemeKodu = od.malzemeKodu,
                    siparisNumarasi = createUpdateOrder.siparisNumarasi,
                    siparisId = od.siparisId,
                    Scans = od.scans != null ? od.scans.Select(sc => new Scan
                    {
                        result = sc.result,
                        scanId = sc.scanId
                    }).ToList() : null
                }).ToList()
            };
            await AddOrder(order);
        }

        private async Task UpdateOrderWithDetails(Order order, CreateUpdateOrder createUpdateOrder)
        {
            foreach (var orderDetail in createUpdateOrder.orderDetails)
            {
                var orderDetailResult = order.OrderDetails.FirstOrDefault(x => x.siparisId == orderDetail.siparisId);

                if (orderDetailResult != null)
                {
                    if (orderDetail.scans != null)
                    {
                        foreach (var scan in orderDetail.scans)
                        {
                            orderDetailResult.Scans.Add(new Scan
                            {
                                result = scan.result,
                                scanId = scan.scanId
                            });
                        }
                    }
                }
                else
                {
                    order.OrderDetails.Add(new OrderDetail
                    {
                        siparisId = orderDetail.siparisId,
                        siparisNumarasi = createUpdateOrder.siparisNumarasi,
                        malzemeKodu = orderDetail.malzemeKodu,
                        Scans = orderDetail.scans != null ? orderDetail.scans.Select(sc => new Scan
                        {
                            result = sc.result,
                            scanId = sc.scanId
                        }).ToList() : null
                    });
                }
            }
            await UpdateOrder(order);
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
