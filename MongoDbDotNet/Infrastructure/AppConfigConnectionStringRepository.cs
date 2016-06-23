using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace MongoDbDotNet.Infrastructure
{
    public class AppConfigConnectionStringRepository : IConnectionStringRepository
    {
        public string ReadConnectionString(string connectionStringName)
        {
            return ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;
        }
    }
}
