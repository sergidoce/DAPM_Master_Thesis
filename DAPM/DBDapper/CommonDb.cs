using Microsoft.Extensions.Configuration;

namespace DBDapper
{
    /*
     *  lzj Data source operation class
     */

    public abstract class CommonDb
    {
        /// <summary>
        /// Supports multiple database operations
        /// </summary>
        /// <param name="datasource"></param>
        /// <returns></returns>
        public static string CreateConnectionString(string datasource)
        {
            switch (datasource)
            {
                case "1":
                    return GetSettings();
                default:
                    return "";
            }
        }
        public static string GetSettings()
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            IConfigurationRoot configuration = builder.Build();
            return configuration["ConnectionStrings:DefaultConnection"];
        }
    }
}
