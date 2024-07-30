using AutoMapper;
using Entities.Concrete;
using Entities.Dtos;
using Shared.Models.Login;

namespace VSSApi.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User,UserDto>().ReverseMap();
            CreateMap<User, LoginRequest>().ReverseMap();
        }
    }
}
