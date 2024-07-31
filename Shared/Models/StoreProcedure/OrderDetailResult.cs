
namespace Shared.Models.StoreProcedure
{
    public class OrderLine
    {
        public string MalzemeKodu { get; set; }
        public string MalzemeAdi { get; set; }
        public string Birim { get; set; }
        public double Miktar { get; set; }
    }

    public class OrderDetailResult
    {
        public int ErrorCode { get; set; }
        public string Result { get; set; }
        public List<OrderLine> OrderLines { get; set; }
    }
}
