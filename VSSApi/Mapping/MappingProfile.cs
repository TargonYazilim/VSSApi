using AutoMapper;
using Entities.Concrete;
using Entities.Dtos;
using Entities.Models;
using Shared.Models.Login;

namespace VSSApi.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User,UserDto>().ReverseMap();
            CreateMap<User, LoginRequest>().ReverseMap();
            CreateMap<Order, OrderDto>().ReverseMap();
            CreateMap<OrderDetail, OrderDetailDto>().ReverseMap();
        }
    }
}
