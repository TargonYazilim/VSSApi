using Core.Entities;
using Entities.Concrete;

namespace Entities.Models
{
    public class Order : BaseEntity
    {
        public string siparisNumarasi { get; set; }
        public bool status { get; set; }
        public bool synchronized { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
