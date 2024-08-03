using Core.DataAccess;
using Entities.Dtos;
using Entities.Models;
using Shared.Models.StoreProcedure;

namespace DataAccess.Dal.Abstract
{
    public interface IOrderDal : IEntityRepository<Order>
    {
        public Task<Order?> GetOrderBySiparisNumarasi(string SiparisNumarasi,int userId);
        public Task<OrderResult?> GetOrderProcedure(int LOGICALREF);
        public Task<OrderDetailResult?> GetOrderDetailProcedure(string SiparisNumarasi);
        public Task<OrderBarcodeScanResult?> ScanOrderBarcodeProcedure(string Barkod);
    }
}
