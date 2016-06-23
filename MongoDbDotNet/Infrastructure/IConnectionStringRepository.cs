using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDbDotNet.Infrastructure
{
    public interface IConnectionStringRepository
    {
        string ReadConnectionString(string connectionStringName);
    }
}
