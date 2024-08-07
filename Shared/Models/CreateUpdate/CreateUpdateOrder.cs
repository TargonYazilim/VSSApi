namespace Shared.Models.CreateUpdate
{
    public class CreateUpdateOrder
    {
        public int Id { get; set; }
        public string siparisNumarasi { get; set; }
        public List<CreateUpdateOrderDetail> orderDetails { get; set; }
    }
}
