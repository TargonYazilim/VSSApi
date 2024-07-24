using Microsoft.Extensions.Configuration;

namespace DataAccess
{
    static class Configuration
    {
        static public string ConnectionString
        {
            get
            {
                //Microsoft.Extensions.Configuration alttaki manager için yüklenmesi gerekiyor.
                ConfigurationManager configurationManager = new();
                try
                {
                    //appsettings.json'ın path'ini burada ayarlamamız gerekiyor.
                    configurationManager.SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../VSSApi"));
                    configurationManager.AddJsonFile("appsettings.json");
                }
                catch
                {
                    configurationManager.AddJsonFile("appsettings.Production.json");
                }

                return configurationManager.GetConnectionString("DefaultConnection");
            }
        }
    }
}
