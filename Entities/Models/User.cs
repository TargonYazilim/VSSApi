using Core.Entities;
using Entities.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Concrete
{
    public class User : BaseEntity
    {
        public string username { get; set; }
        [NotMapped]
        public string password { get; set; }
        [NotMapped]
        public string companyNo { get; set; }
        [NotMapped]
        public string periodNo { get; set; }
        public string? MACADDRESS { get; set; }
        public string? role { get; set; }
        public int LOGICALREF { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}
