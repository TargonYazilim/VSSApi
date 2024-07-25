using Core.DataAccess;
using Entities;
using Entities.Concrete;

namespace DataAccess.Dal.Abstract
{
    public interface IUserDal : IEntityRepository<User>
    {
        public Task<BaseResult?> Login(User user);
        public Task<User?> GetByUsernameAsync(string username);
        public Task<(bool hasMacAddress,int? Id)> CheckMacAddressAsync(string macAddress);
    }
}
