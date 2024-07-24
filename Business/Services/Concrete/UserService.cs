using AutoMapper;
using Business.Services.Abstract;
using DataAccess.Dal.Abstract;
using Entities;
using Entities.Concrete;
using Entities.Dtos;

namespace Business.Services.Concrete
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly IUserDal _userDal;

        public UserService(IMapper mapper, IUserDal userDal)
        {
            _mapper = mapper;
            _userDal = userDal;
        }
        public async Task<BaseResult?> Login(UserDto userDto)
        {
            return await _userDal.Login(_mapper.Map<User>(userDto));
        }

        public int? AddUser(UserDto userDto)
        {
            return _userDal.Add(_mapper.Map<User>(userDto));
        }

        public async Task<bool> DeleteUser(int Id)
        {
            var userDto = await GetUser(Id);
            return _userDal.Delete(_mapper.Map<User>(userDto));
        }

        public async Task<List<UserDto>> GetAllUser()
        {
            return _mapper.Map<List<UserDto>>(await _userDal.GetAllAsync());
        }

        public async Task<UserDto> GetUser(int Id)
        {
            return _mapper.Map<UserDto>(await _userDal.GetAsync(x => x.Id == Id));
        }

        public bool UpdateUser(UserDto userDto)
        {
            return _userDal.Update(_mapper.Map<User>(userDto));
        }
    }
}
