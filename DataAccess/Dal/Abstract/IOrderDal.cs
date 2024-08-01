﻿using Shared.Models.StoreProcedure;

namespace DataAccess.Dal.Abstract
{
    public interface IOrderDal
    {
        public Task<OrderResult?> GetOrder(int LOGICALREF);
        public Task<OrderDetailResult?> GetOrderDetail(string SiparisNumarasi);
        public Task<OrderBarcodeScanResult?> ScanOrderBarcode(string Barkod);
    }
}
