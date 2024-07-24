using AutoMapper;
using Entities.Concrete;
using Entities.Dtos;

namespace VSSApi.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User,UserDto>().ReverseMap();
        }
    }
}
