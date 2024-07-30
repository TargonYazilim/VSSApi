using Entities.Dtos;
using Shared.Models.StoreProcedure;

namespace Business.Services.Abstract
{
    public interface IOrderService
    {
        Task<OrderResult?> GetOrder(int LOGICALREF);
    }
}
