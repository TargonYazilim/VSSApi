
using Shared.Models.StoreProcedure;

namespace DataAccess.Dal.Abstract
{
    public interface IIrsaliyeDal
    {
        public Task<IrsaliyeProcedure?> GetIrsaliye(string siparisNumarasi);
    }
}
