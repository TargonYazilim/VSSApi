
using Shared.Models.StoreProcedure;

namespace DataAccess.Dal.Abstract
{
    public interface IBarcodeDal
    {
        public Task<Barcode?> GetAllBarcodes();
    }
}
