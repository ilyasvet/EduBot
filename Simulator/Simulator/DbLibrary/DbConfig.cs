using System.Configuration;

namespace GoodsDbLibrary
{
    internal static class DbConfig
    {
        public static readonly string serverName = ConfigurationManager.AppSettings["ServerName"];
        public static readonly string dataBaseName = ConfigurationManager.AppSettings["DataBaseName"];
    }
}
