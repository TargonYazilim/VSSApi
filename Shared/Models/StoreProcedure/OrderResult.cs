namespace Shared.Models.StoreProcedure
{
    public class OrderProcedure
    {
        public int Id { get; set; }
        public int siparisLogicalRef { get; set; }
        public string siparisNumarasi { get; set; }
        public string siparisTarihi { get; set; }
        public string sevkiyatYeri { get; set; }
        public string sevkiyatAdresi { get; set; }
        public int cariLogicalRef { get; set; }
        public string cariKodu { get; set; }
        public string cariUnvan { get; set; }
        public string teslimatTarihi { get; set; }
        public List<OrderDetailProcedure> orderDetails { get; set; }
    }

    public class OrderDetailProcedure
    {
        public int Id { get; set; }
        public int siparisId { get; set; }
        public string malzemeKodu { get; set; }
        public string malzemeAdi { get; set; }
        public string birim { get; set; }
        public double miktar { get; set; }
        public List<ScanProcedure> scans { get; set; }
    }

    public class OrderResult
    {
        public int ErrorCode { get; set; }
        public string Result { get; set; }
        public List<OrderProcedure> orders { get; set; }
    }

}
 