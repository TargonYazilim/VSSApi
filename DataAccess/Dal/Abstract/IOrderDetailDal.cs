using Core.DataAccess;
using Entities.Models;

namespace DataAccess.Dal.Abstract
{
    public interface IOrderDetailDal : IEntityRepository<OrderDetail>
    {
        public Task<OrderDetail?> GetOrderDetailByOrderIdAndId(int orderId,string id);
    }
}
