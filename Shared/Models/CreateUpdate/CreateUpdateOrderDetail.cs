
namespace Shared.Models.CreateUpdate
{
    public class CreateUpdateOrderDetail
    {
        public int Id { get; set; }
        public string malzemeKodu { get; set; }
        public int siparisId { get; set; }
        public List<CreateUpdateScan> scans { get; set; }
    }
}
