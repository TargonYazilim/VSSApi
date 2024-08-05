using Entities.Models;
using Shared.Models.StoreProcedure;

namespace Business.Services.Abstract
{
    public interface IOrderService
    {
        Task<OrderResult?> GetOrderProcedure(int LOGICALREF);
        Task<OrderBarcodeScanResult?> ScanOrderBarcodeProcedure(ScanBarcode scanBarcode);
        Task<List<Order>?> GetAllOrdersByUserId(int userId);
    }
}
