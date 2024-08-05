using Core.DataAccess;
using DataAccess.Context;
using DataAccess.Dal.Abstract;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Shared.Models.StoreProcedure;

namespace DataAccess.Dal.Concrete
{
    public class OrderDetailDal : EntityRepository<OrderDetail, DataContext>, IOrderDetailDal
    {
        

        public async Task<OrderDetail?> GetOrderDetailByOrderIdAndId(int orderId,string id)
        {
            using (DataContext _context = new DataContext())
            {
                return await  _context.Set<OrderDetail>().FirstOrDefaultAsync(p => p.OrderId == orderId);
            }
        }
    }
}
