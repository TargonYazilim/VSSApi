using DataAccess.Context;
using DataAccess.Dal.Abstract;
using DataAccess.Dal.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccess
{
    public static class ServiceRegistration
    {
        public static void AddDataAccess(this IServiceCollection services)
        {
            services.AddScoped<IUserDal, UserDal>();
            services.AddScoped<IOrderDal, OrderDal>();
            services.AddScoped<IBarcodeDal, BarcodeDal>();
            services.AddScoped<IScanDal, ScanDal>();
            services.AddScoped<IIrsaliyeDal, IrsaliyeDal>();


            services.AddDbContext<DataContext>(opt => opt.UseSqlServer(Configuration.ConnectionString));
        }
    }
}
