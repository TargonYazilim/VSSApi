using Entities.Models;

namespace Business.Services.Abstract
{
    public interface IScanService
    {
        public Task<bool> DeleteScan(int scanId);
    }
}
