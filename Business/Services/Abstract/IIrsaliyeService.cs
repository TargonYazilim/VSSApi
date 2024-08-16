using Shared.Models.StoreProcedure;

namespace Business.Services.Abstract
{
    public interface IIrsaliyeService
    {
        Task<IrsaliyeProcedure?> GetIrsaliye(string siparisNumarasi);
    }
}
