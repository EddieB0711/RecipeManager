using System.Configuration;

namespace RecipeManager.ConnectionBuilding
{
    public static class ConnectionBuilder
    {
        public static string GetConnectionString(string connectionName)
        {
            return ConfigurationManager.ConnectionStrings[connectionName].ConnectionString;
        }
    }
}