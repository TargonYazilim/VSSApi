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

            services.AddDbContext<DataContext>(opt => opt.UseSqlServer(Configuration.ConnectionString));
        }
    }
}
