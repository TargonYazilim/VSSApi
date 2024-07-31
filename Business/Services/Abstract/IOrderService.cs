using Shared.Models.StoreProcedure;

namespace Business.Services.Abstract
{
    public interface IOrderService
    {
        Task<OrderResult?> GetOrder(int LOGICALREF);
        Task<OrderDetailResult?> GetOrderDetail(string SiparisNumarasi);
    }
}
