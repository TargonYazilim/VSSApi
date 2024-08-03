
using Business.Services.Abstract;
using Business.Services.Concrete;
using Microsoft.Extensions.DependencyInjection;

namespace Business
{
    public static class ServiceRegistration
    {
        public static void Business(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IOrderDetailService, OrderDetailService>();
        }
    }
}
