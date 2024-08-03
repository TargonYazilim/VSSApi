using Core.DataAccess;
using Entities.Models;

namespace DataAccess.Dal.Abstract
{
    public interface IOrderDetailDal : IEntityRepository<OrderDetail>
    {
        public Task<OrderDetail?> GetOrderDetailByOrderId(int orderId);
    }
}
