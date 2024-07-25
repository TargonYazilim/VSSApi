using Core.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Concrete
{
    public class User : BaseEntity
    {
        public string username { get; set; }
        [NotMapped]
        public string password { get; set; }
        public string? MACADDRESS { get; set; }
    }
}
