using Core.Entities;

namespace Entities.Models
{
    public class OrderDetail : BaseEntity
    {
        public string siparisNumarasi { get; set; }
        public string malzemeKodu { get; set; }
        public string scanResult { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }
    }
}
