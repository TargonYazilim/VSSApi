using AutoMapper;
using Core.DataAccess;
using DataAccess.Context;
using DataAccess.Dal.Abstract;
using Entities.Dtos;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Shared.Models.StoreProcedure;
using System.Linq.Expressions;

namespace DataAccess.Dal.Concrete
{
    public class OrderDetailDal : EntityRepository<OrderDetail, DataContext>, IOrderDetailDal
    {
        

        public async Task<OrderDetail?> GetOrderDetailByOrderId(int orderId)
        {
            using (DataContext _context = new DataContext())
            {
                return await  _context.Set<OrderDetail>().FirstOrDefaultAsync(p => p.OrderId == orderId);
            }
        }
    }
}
