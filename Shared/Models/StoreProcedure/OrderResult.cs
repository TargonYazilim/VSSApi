namespace Shared.Models.StoreProcedure
{
    public class Order
    {
        public int SiparisLogicalRef { get; set; }
        public string SiparisNumarasi { get; set; }
        public string SiparisTarihi { get; set; }
        public string SevkiyatYeri { get; set; }
        public string SevkiyatAdresi { get; set; }
        public int CariLogicalRef { get; set; }
        public string cariKodu { get; set; }
        public string cariUnvan { get; set; }
        public string teslimatTarihi { get; set; }
    }

    public class OrderResult
    {
        public int ErrorCode { get; set; }
        public string Result { get; set; }
        public List<Order> Orders { get; set; }
    }

}