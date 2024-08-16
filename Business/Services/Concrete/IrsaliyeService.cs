using Business.Services.Abstract;
using DataAccess.Dal.Abstract;
using Shared.Models.StoreProcedure;

namespace Business.Services.Concrete
{
    public class IrsaliyeService : IIrsaliyeService
    {
        private readonly IIrsaliyeDal _irsaliyeDal;

        public IrsaliyeService(IIrsaliyeDal irsaliyeDal)
        {
            _irsaliyeDal = irsaliyeDal;
        }

        public async Task<IrsaliyeProcedure?> GetIrsaliye(string siparisNumarasi)
        {
            return await _irsaliyeDal.GetIrsaliye(siparisNumarasi);
        }
    }
}
