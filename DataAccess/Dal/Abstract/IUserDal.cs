using Core.DataAccess;
using Entities.Concrete;
using Shared.Models.Login;

namespace DataAccess.Dal.Abstract
{
    public interface IUserDal : IEntityRepository<User>
    {
        public Task<LoginResult?> Login(User user);
        public Task<User?> GetByUsernameAsync(string username);
        public Task<(bool hasMacAddress,int? Id)> CheckMacAddressAsync(string macAddress);
    }
}
