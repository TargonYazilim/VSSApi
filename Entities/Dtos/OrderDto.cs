namespace Entities.Dtos
{
    public class OrderDto
    {
        public int Id { get; set; }
        public string siparisNumarasi { get; set; }
        public bool status { get; set; }
        public int UserId { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
