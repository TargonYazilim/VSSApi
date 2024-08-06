using DataAccess.Context;
using Microsoft.EntityFrameworkCore;

namespace VSSApi.Extension
{
    public static class DbCreator
    {
        public static void  GenerateDb(this IServiceProvider provider)
        {
            var scope = provider.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();
            dbContext.Database.Migrate();
        }
    }
}
