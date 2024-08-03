
namespace Entities.Dtos
{
    public class OrderDetailDto
    {
        public int Id { get; set; }
        public string siparisNumarasi { get; set; }
        public string malzemeKodu { get; set; }
        public string scanResult { get; set; }
        public int OrderId { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
