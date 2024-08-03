using AutoMapper;
using Business.Services.Abstract;
using DataAccess.Dal.Abstract;
using Entities.Dtos;
using Entities.Models;

namespace Business.Services.Concrete
{
    public class OrderDetailService : IOrderDetailService
    {
        private readonly IOrderDetailDal _orderDetailDal;
        private readonly IMapper _mapper;
        public OrderDetailService(IOrderDetailDal orderDetailDal, IMapper mapper)
        {
            _orderDetailDal = orderDetailDal;
            _mapper = mapper;
        }

        public async Task<OrderDetailDto?> GetOrderDetailByOrderId(int orderId)
        {
            return _mapper.Map<OrderDetailDto>(await _orderDetailDal.GetOrderDetailByOrderId(orderId));
        }

        public async Task<bool> UpdateOrderDetail(OrderDetailDto orderDetailDto)
        {
            return await _orderDetailDal.Update(_mapper.Map<OrderDetail>(orderDetailDto));
        }
    }
}
