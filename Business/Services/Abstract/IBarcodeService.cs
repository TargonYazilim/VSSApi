using Shared.Models.StoreProcedure;

namespace Business.Services.Abstract
{
    public interface IBarcodeService
    {
        public Task<Barcode?> GetAllBarcodes();
    }
}
