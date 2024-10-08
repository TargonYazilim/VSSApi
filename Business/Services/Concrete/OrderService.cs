﻿using Business.Services.Abstract;
using DataAccess.Dal.Abstract;
using Entities.Dtos;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using Shared.Models.CreateUpdate;
using Shared.Models.StoreProcedure;

namespace Business.Services.Concrete
{
    public class OrderService : IOrderService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IOrderDal _orderDal;
        private readonly IUserService _userService;
        private readonly IScanService _scanService;

        public OrderService(IOrderDal orderDal, IHttpContextAccessor httpContextAccessor, IUserService userService, IScanService scanService)
        {
            _orderDal = orderDal;
            _httpContextAccessor = httpContextAccessor;
            _userService = userService;
            _scanService = scanService;
        }
        public async Task<OrderResult?> GetOrderProcedure(int LOGICALREF)
        {

            var user = await ContextUser();
            var ordersFromProcedure = await _orderDal.GetOrderProcedure(LOGICALREF);
            /// Procedure'dan order detaylarını çekerken eğer ara tabloda scanResult mevcut ise o veriyi 
            /// OrderFromProcedure'e implement et.
            if (ordersFromProcedure != null && ordersFromProcedure.orders != null)
            {
                foreach (var order in ordersFromProcedure.orders)
                {
                    if (order.orderDetails != null)
                    {
                        foreach (var orderFromProcedure in order.orderDetails)
                        {
                            var orderFromDb = await _orderDal.GetOrderBySiparisNumarasi(user.Id, order.siparisNumarasi);
                            if (orderFromDb != null)
                            {
                                var hasOrderDetail = orderFromDb.OrderDetails.FirstOrDefault(x => x.siparisId == orderFromProcedure.siparisId);
                                if (hasOrderDetail != null && hasOrderDetail.Scans != null)
                                {
                                    order.Id = orderFromDb.Id;
                                    orderFromProcedure.Id = hasOrderDetail.Id;
                                    orderFromProcedure.scans = hasOrderDetail.Scans.Select(scan => new ScanProcedure
                                    {
                                        Id = scan.Id,
                                        scanId = scan.scanId,
                                        result = scan.result
                                    }).ToList();
                                }
                            }
                        }
                    }
                }
            }
            return ordersFromProcedure;
        }

        public async Task<List<ScanBarcodeResult>?> ScanOrderBarcodeProcedure(List<CreateUpdateOrder> createUpdateOrders)
        {
            var user = await ContextUser();
            List<ScanBarcodeResult> synchronizedOrders = new List<ScanBarcodeResult>();
            foreach (var createUpdateOrder in createUpdateOrders)
            {
                await CheckRemovedScans(createUpdateOrder);

                var orderResult = await _orderDal.GetOrderBySiparisNumarasiAndOrderId(user.Id, createUpdateOrder.siparisNumarasi, createUpdateOrder.Id);

                /// Create order
                if (orderResult == null)
                {
                    await CreateOrder(createUpdateOrder, user.Id);
                    synchronizedOrders.Add(new ScanBarcodeResult { siparisNumarasi = createUpdateOrder.siparisNumarasi });
                }
                /// Update order
                else if (orderResult != null && orderResult.status == false)
                {
                    await UpdateOrderWithDetails(orderResult, createUpdateOrder);
                    synchronizedOrders.Add(new ScanBarcodeResult { siparisNumarasi = createUpdateOrder.siparisNumarasi });
                }
            }
            return synchronizedOrders;
        }

        private async Task CreateOrder(CreateUpdateOrder createUpdateOrder, int userId)
        {
            if (createUpdateOrder.orderDetails == null || !createUpdateOrder.orderDetails.Any())
            {
                return;
            }

            var orderDetails = createUpdateOrder.orderDetails
            .Where(od => od.scans != null && od.scans.Any())
            .Select(od => new OrderDetail
            {
                malzemeKodu = od.malzemeKodu,
                siparisNumarasi = createUpdateOrder.siparisNumarasi,
                siparisId = od.siparisId,
                Scans = od.scans!.Select(sc => new Scan
                {
                    result = sc.result,
                    scanId = sc.scanId
                }).ToList()
            }).ToList();


            if (!orderDetails.Any())
            {
                return;
            }


            var order = new Order
            {
                siparisNumarasi = createUpdateOrder.siparisNumarasi,
                status = false,
                UserId = userId,
                synchronized = true,
                OrderDetails = orderDetails
            };

            await AddOrder(order);
        }

        private async Task UpdateOrderWithDetails(Order order, CreateUpdateOrder createUpdateOrder)
        {

            foreach (var orderDetail in createUpdateOrder.orderDetails)
            {
                var orderDetailResult = order.OrderDetails.FirstOrDefault(x => x.siparisId == orderDetail.siparisId);

                if (orderDetailResult != null && orderDetail.scans != null && orderDetail.scans.Any())
                {
                    foreach (var scan in orderDetail.scans)
                    {
                        var scanExists = orderDetailResult.Scans?.FirstOrDefault(x => x.scanId == scan.scanId);

                        if (scanExists == null)
                        {
                            orderDetailResult.Scans!.Add(new Scan
                            {
                                result = scan.result,
                                scanId = scan.scanId
                            });
                            await UpdateOrder(order); // Burada her yeni scan için UpdateOrder çağrısı yapılıyor
                        }
                    }
                }
                else
                {
                    if (orderDetail.scans != null && orderDetail.scans.Any())
                    {
                        order.OrderDetails.Add(new OrderDetail
                        {
                            siparisId = orderDetail.siparisId,
                            siparisNumarasi = createUpdateOrder.siparisNumarasi,
                            malzemeKodu = orderDetail.malzemeKodu,
                            Scans = orderDetail.scans.Select(sc => new Scan
                            {
                                result = sc.result,
                                scanId = sc.scanId
                            }).ToList()
                        });
                        await UpdateOrder(order);

                    }
                }
            }
        }

        private async Task CheckRemovedScans(CreateUpdateOrder createUpdateOrder)
        {
            /// Id varsa bu sipariş daha önce oluşturulmuştur.
            if (createUpdateOrder.Id != null)
            {
                var user = await ContextUser();
                var order = await _orderDal.GetOrderBySiparisNumarasi(user.Id, createUpdateOrder.siparisNumarasi);

                if (order?.OrderDetails == null || order.status == true) return;
                foreach (var orderDetail in order.OrderDetails)
                {
                    foreach (var createUpdateOrderDetail in createUpdateOrder.orderDetails)
                    {
                        if (orderDetail.siparisId == createUpdateOrderDetail.siparisId)
                        {
                            if (orderDetail?.Scans == null) return;
                            foreach (var scan in orderDetail.Scans)
                            {
                                var existsScanInClient = createUpdateOrderDetail.scans?.Any(x => x.scanId == scan.scanId) ?? false;
                                if (!existsScanInClient)
                                {
                                    await _scanService.DeleteScan(scan.scanId);
                                }
                            }

                        }
                    }
                }
            }
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
