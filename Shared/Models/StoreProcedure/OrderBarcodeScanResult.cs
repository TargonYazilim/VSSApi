﻿namespace Shared.Models.StoreProcedure
{
    public class Item
    {
        public string MalzemeKodu { get; set; }
        public string MalzemeAdi { get; set; }
        public string Birim { get; set; }
        public string Kilo { get; set; }
    }

    public class OrderBarcodeScanResult
    {
        public int ErrorCode { get; set; }
        public string Result { get; set; }
        public List<Item> Items { get; set; }
    }
}
