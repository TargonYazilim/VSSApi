﻿using AutoMapper;
using Business.Services.Abstract;
using DataAccess.Dal.Abstract;
using Entities;
using Entities.Concrete;
using Entities.Dtos;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Shared.Models;
using Shared.Models.Login;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Business.Services.Concrete
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly IUserDal _userDal;
        private readonly JwtSettings _jwtSettings;
        public UserService(IMapper mapper, IUserDal userDal, IOptions<JwtSettings> jwtSettings)
        {
            _mapper = mapper;
            _userDal = userDal;
            _jwtSettings = jwtSettings.Value;
        }
        public async Task<LoginResult?> Login(UserDto userDto)
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
                        return new LoginResult()
                        { Error = "1", Result = "Cihaz başka bir hesaba bağlı" };
                    }
                    else if (user != null && user.MACADDRESS == null && !macAddress.hasMacAddress)
                    {
                        /// Mac adresi başka bir hesaba bağlı değilse,
                        /// Hesapta başka bir mac adresi kayıtlı değilse ve,
                        /// Mevcut hesap var ise yeni mac adresini güncelle
                        await UpdateUserAsync(_mapper.Map<UserDto>(user));
                    }
                    else if ((user != null && user.MACADDRESS == null && macAddress.hasMacAddress)
                        || user != null && user.Id != macAddress.Id)
                    {
                        return new LoginResult()
                        { Error = "1", Result = "Hesabınız başka bir cihaza bağlı" };
                    }
                }
            }
            var token = CreateToken(userDto);
            return new LoginResult()
            { Error = "0", Result = "Giriş başarılı" ,Token = token}; ;
        }

        private string CreateToken(UserDto userDto)
        {
            if (_jwtSettings.Key == null) throw new Exception("Jwt ayarlarındaki key değeri null olamaz");

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var credantials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userDto.username),
                new Claim(ClaimTypes.Role, userDto.role ?? string.Empty),
            };

            var token = new JwtSecurityToken(_jwtSettings.Issuer,
                _jwtSettings.Audience,
                claims,
                expires: DateTime.Now.AddDays(Convert.ToInt32(_jwtSettings.ExpireDay)),
                signingCredentials: credantials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<int?> AddUserAsync(UserDto userDto)
        {
            var result = await _userDal.Add(_mapper.Map<User>(userDto));
            return result;
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
