namespace Shared.Models.StoreProcedure
{
    public class BarcodeItem
    {
        public string MalzemeKodu { get; set; }
        public string Barkod { get; set; }
        public string Kilo { get; set; }
        public string Birim { get; set; }
    }

    public class Barcode
    {
        public List<BarcodeItem> Items { get; set; }
    }
}
