using Entities;
using Entities.Dtos;

namespace Business.Services.Abstract
{
    public interface IUserService
    {
        Task<List<UserDto>> GetAllUser();
        Task<UserDto> GetUser(int Id);
        int? AddUser(UserDto userDto);
        bool UpdateUser(UserDto userDto);
        Task<bool> DeleteUser(int Id);
        Task<BaseResult?> Login(UserDto userDto);

    }
}
