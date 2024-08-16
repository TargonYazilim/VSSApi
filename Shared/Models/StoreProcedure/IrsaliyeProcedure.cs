
namespace Shared.Models.StoreProcedure
{
    public class IrsaliyeProcedure
    {
        public int ErrorCode { get; set; }
        public string Result { get; set; }
        public IrsaliyeDetail Baslik { get; set; }
    }
    public class IrsaliyeDetail
    {
        public string CariKodu { get; set; }
        public string CariUnvan { get; set; }
        public string CariAdresi { get; set; }
        public string CariVergiDairesi { get; set; }
        public string CariVKN { get; set; }
        public string SiparisNumarasi { get; set; }
        public DateTime SiparisTarihi { get; set; }
    }

   
}
