using Entities.Models;

namespace Business.Services.Abstract
{
    public interface IOrderDetailService
    {
        Task<OrderDetail?> GetOrderDetailBySiparisNumarasi(string siparisNumarasi,int Id);
    }
}
