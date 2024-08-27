using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Models.Login
{
    public class LoginRequest
    {
        public string username { get; set; }
        public string password { get; set; }
        public string MACADDRESS { get; set; }
        public string companyNo { get; set; }
        public string periodNo { get; set; }
    }
}
