using Core.DataAccess;
using DataAccess.Context;
using DataAccess.Dal.Abstract;
using Entities.Models;

namespace DataAccess.Dal.Concrete
{
    public class ScanDal : EntityRepository<Scan, DataContext>, IScanDal
    {
    }
}
