using Business.Services.Abstract;
using DataAccess.Dal.Abstract;
using Shared.Models.StoreProcedure;

namespace Business.Services.Concrete
{
    public class BarcodeService : IBarcodeService
    {
        private readonly IBarcodeDal _barcodeDal;

        public BarcodeService(IBarcodeDal barcodeDal)
        {
            _barcodeDal = barcodeDal;
        }

        public async Task<Barcode?> GetAllBarcodes()
        {
            return await _barcodeDal.GetAllBarcodes();
        }
    }
}
