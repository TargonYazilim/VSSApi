using Business.Services.Abstract;
using DataAccess.Dal.Abstract;
using Entities.Models;

namespace Business.Services.Concrete
{
    public class ScanService : IScanService
    {
        private readonly IScanDal _scanDal;

        public ScanService(IScanDal scanDal)
        {
            _scanDal = scanDal;
        }

        public async Task<bool> DeleteScan(string scanId)
        {
            Scan? scan = await _scanDal.GetAsync(x => x.scanId == scanId);
            if (scan != null)
            {
                return await _scanDal.Delete(scan);
            }
            return false;
        }
    }
}
