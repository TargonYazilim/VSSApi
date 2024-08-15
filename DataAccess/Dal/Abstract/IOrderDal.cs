using Core.DataAccess;
using Entities.Models;
using Shared.Models.StoreProcedure;

namespace DataAccess.Dal.Abstract
{
    public interface IOrderDal : IEntityRepository<Order>
    {
        public Task<Order?> GetOrderBySiparisNumarasiAndOrderId(int userId,string siparisNumarasi, int? orderId);
        public Task<Order?> GetOrderBySiparisNumarasi(int userId,string siparisNumarasi);
        public Task<OrderResult?> GetOrderProcedure(int LOGICALREF);
        public Task<OrderBarcodeScanResult?> ScanOrderBarcodeProcedure(string Barkod);
    }
}
