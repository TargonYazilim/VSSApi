using Business.Services.Abstract;
using DataAccess.Context;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Business.Services.Concrete
{
    public class OrderDetailService : IOrderDetailService
    {
        public async Task<OrderDetail?> GetOrderDetailBySiparisNumarasi(string siparisNumarasi, int Id)
        {
            using (DataContext _context = new DataContext())
            {
                return await _context.Set<OrderDetail>().Include(i => i.Scans).FirstOrDefaultAsync(p => p.Id == Id && p.siparisNumarasi == siparisNumarasi);
            }
        }
    }
}
