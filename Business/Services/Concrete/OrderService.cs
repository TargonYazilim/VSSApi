
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
            var resu = await _orderDal.GetOrder(LOGICALREF);
            return resu;
        }
    }
}
