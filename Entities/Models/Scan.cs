using Core.Entities;

namespace Entities.Models
{
    public class Scan : BaseEntity
    {
        public string scanId { get; set; }
        public string result { get; set; }
        public int orderDetailId { get; set; }
        public OrderDetail OrderDetail { get; set; }
    }
}
