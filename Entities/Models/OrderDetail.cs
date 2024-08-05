using Core.Entities;

namespace Entities.Models
{
    public class OrderDetail : BaseEntity
    {
        public OrderDetail()
        {
            Scans = new List<Scan>();
        }
        public int siparisId { get; set; }
        public string siparisNumarasi { get; set; }
        public string malzemeKodu { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }
        public ICollection<Scan> Scans { get; set; }
    }
}
