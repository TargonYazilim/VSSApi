using Entities.Models;
using Shared.Models.CreateUpdate;
using Shared.Models.StoreProcedure;

namespace Business.Services.Abstract
{
    public interface IOrderService
    {
        Task<OrderResult?> GetOrderProcedure(int LOGICALREF);
        Task<List<ScanBarcodeResult>?> ScanOrderBarcodeProcedure(List<CreateUpdateOrder> createUpdateOrders);
        Task<List<Order>?> GetAllOrdersByUserId(int userId);
    }
}
