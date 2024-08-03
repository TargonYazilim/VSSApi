using Core.DataAccess;
using Entities.Dtos;
using Shared.Models.StoreProcedure;

namespace Business.Services.Abstract
{
    public interface IOrderService
    {
        Task<OrderResult?> GetOrderProcedure(int LOGICALREF);
        Task<OrderDetailResult?> GetOrderDetailProcedure(string SiparisNumarasi);
        Task<OrderBarcodeScanResult?> ScanOrderBarcodeProcedure(string Barkod,string siparisNumarasi, string malzemeKodu);
    }
}
