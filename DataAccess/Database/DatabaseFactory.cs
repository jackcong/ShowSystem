using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Data.Common;

namespace DataAccess.Database
{
    public static class DatabaseFactory
    {
        public static Database CreateDatabase()
        {
            DbProviderFactory dbProviderFactory = DbProviderFactories.GetFactory(ConfigurationManager.ConnectionStrings["DC"].ProviderName);
            Database db = new Database(ConfigurationManager.ConnectionStrings["DC"].ConnectionString, dbProviderFactory);
            return db;
        }

        public static Database CreateDatabase(string name, string connectString)
        {
            DbProviderFactory dbProviderFactory = DbProviderFactories.GetFactory(name);
            Database db = new Database(connectString, dbProviderFactory);
            return db;
        }

        public static Database CreateDatabase(string connectName)
        {
            DbProviderFactory dbProviderFactory = DbProviderFactories.GetFactory(ConfigurationManager.ConnectionStrings[connectName].ProviderName);
            Database db = new Database(ConfigurationManager.ConnectionStrings[connectName].ConnectionString, dbProviderFactory);
            return db;
        }
    }
}
