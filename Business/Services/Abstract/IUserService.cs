﻿
using Entities.Dtos;
using Shared.Models.Login;

namespace Business.Services.Abstract
{
    public interface IUserService
    {
        Task<UserDto?> GetByUsername(string username);
        Task<List<UserDto>> GetAllUser();
        Task<UserDto> GetUser(int Id);
        Task<int?> AddUserAsync(UserDto userDto);
        Task<bool> UpdateUserAsync(UserDto userDto);
        Task<bool> DeleteUser(int Id);
        Task<LoginResult?> Login(LoginRequest loginRequest);

    }
}
