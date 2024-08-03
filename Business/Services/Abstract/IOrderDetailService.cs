using Entities.Dtos;

namespace Business.Services.Abstract
{
    public interface IOrderDetailService
    {
        public Task<bool> UpdateOrderDetail(OrderDetailDto orderDetailDto);
        public Task<OrderDetailDto?> GetOrderDetailByOrderId(int orderId);
    }
}
