using Core.DataAccess;
using Entities.Concrete;
using Shared.Models.Login;
using Shared.Models.StoreProcedure;

namespace DataAccess.Dal.Abstract
{
    public interface IUserDal : IEntityRepository<User>
    {
        public Task<LoginStoreProcedureResult?> Login(User user);
        public Task<User?> GetByUsernameAsync(string username);
        public Task<(bool hasMacAddress,int? Id)> CheckMacAddressAsync(string macAddress);
    }
}
