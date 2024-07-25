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
            var result = await _userDal.Login(_mapper.Map<User>(userDto));
            if (result != null)
            {
                int.TryParse(result.Error, out int error);
                if (error == 1) return result;
                else
                {
                    /// Db'den kullanıcı adına göre veriyi getir.
                    var user = await _userDal.GetByUsernameAsync(userDto.username);
                    var macAddress = await _userDal.CheckMacAddressAsync(userDto.MACADDRESS);
                    if (user == null && !macAddress.hasMacAddress)
                    {
                        /// Kullanıcı yok ise ve mac adresi de başka yerde yok ise kullanıcıyı kayıt et.
                        await AddUserAsync(userDto);
                    }
                    else if (user == null && macAddress.hasMacAddress)
                    {
                        return new BaseResult()
                        { Error = "1", Result = "Cihaz başka bir hesaba bağlı" };
                    }
                    else if (user != null && user.MACADDRESS == null && !macAddress.hasMacAddress)
                    {
                        /// Mac adresi başka bir hesaba bağlı değilse,
                        /// Hesapta başka bir mac adresi kayıtlı değilse ve,
                        /// Mevcut hesap var ise yeni mac adresini güncelle
                        userDto.Id = user.Id;
                        userDto.CreateDate = user.CreateDate;
                        await UpdateUserAsync(userDto);
                    }
                    else if ((user != null && user.MACADDRESS == null && macAddress.hasMacAddress)
                        || user != null && user.Id != macAddress.Id)
                    {
                        return new BaseResult()
                        { Error = "1", Result = "Hesabınız başka bir cihaza bağlı" };
                    }
                }
            }
            return new BaseResult()
            { Error = "0", Result = "Giriş başarılı" }; ;
        }

        public async Task<int?> AddUserAsync(UserDto userDto)
        {
            return await _userDal.Add(_mapper.Map<User>(userDto));
        }

        public async Task<bool> DeleteUser(int Id)
        {
            var userDto = await GetUser(Id);
            return await _userDal.Delete(_mapper.Map<User>(userDto));
        }

        public async Task<List<UserDto>> GetAllUser()
        {
            return _mapper.Map<List<UserDto>>(await _userDal.GetAllAsync());
        }

        public async Task<UserDto> GetUser(int Id)
        {
            return _mapper.Map<UserDto>(await _userDal.GetAsync(x => x.Id == Id));
        }

        public async Task<bool> UpdateUserAsync(UserDto userDto)
        {
            return await _userDal.Update(_mapper.Map<User>(userDto));
        }
    }
}
