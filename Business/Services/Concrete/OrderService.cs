
using Business.Services.Abstract;
using DataAccess.Dal.Abstract;
using Shared.Models.StoreProcedure;

namespace Business.Services.Concrete
{
    public class OrderService : IOrderService
    {
        private readonly IOrderDal _orderDal;
        public OrderService(IOrderDal orderDal)
        {
            _orderDal = orderDal;
        }
        public async Task<OrderResult?> GetOrder(int LOGICALREF)
        {
            return await _orderDal.GetOrder(LOGICALREF);
        }

        public async Task<OrderDetailResult?> GetOrderDetail(string SiparisNumarasi)
        {
            return await _orderDal.GetOrderDetail(SiparisNumarasi);
        }
    }
}
