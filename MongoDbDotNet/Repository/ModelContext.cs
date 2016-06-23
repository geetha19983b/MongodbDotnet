using MongoDB.Driver;
using MongoDbDotNet.Infrastructure;
using System;

namespace MongoDbDotNet.Repository
{
    public class ModelContext
    {
        private IMongoClient Client { get; set; }
        private IMongoDatabase Database { get; set; }
        private IConfigurationRepository ConfigurationRepository { get; set; }
        private static ModelContext _modelContext;

        private ModelContext() { }

        public static ModelContext Create(IConfigurationRepository configurationRepository,
            IConnectionStringRepository connectionStringRepository)
        {
            if (configurationRepository == null) throw new ArgumentNullException("ConfigurationRepository");
            if (connectionStringRepository == null) throw new ArgumentNullException("ConnectionStringRepository");
            if (_modelContext == null)
            {
                _modelContext = new ModelContext();
                string connectionString = connectionStringRepository.ReadConnectionString("MongoDb");
                _modelContext.Client = new MongoClient(connectionString);
                _modelContext.Database = _modelContext.Client.GetDatabase(configurationRepository.GetConfigurationValue("DemoDatabaseName", "model"));
                _modelContext.ConfigurationRepository = configurationRepository;
            }
            return _modelContext;
        }

        public void TestConnection()
        {
            var dbsCursor = _modelContext.Client.ListDatabases();
            var dbsList = dbsCursor.ToList();
            foreach (var db in dbsList)
            {
                Console.WriteLine(db);
            }
        }

        public IMongoCollection<RestaurantDb> Restaurants
        {
            get { return Database.GetCollection<RestaurantDb>(ConfigurationRepository.GetConfigurationValue("RestaurantsCollectionName", "restaurants")); }
        }
        public IMongoCollection<ZipCodeDb> ZipCodes
        {
            get { return Database.GetCollection<ZipCodeDb>(ConfigurationRepository.GetConfigurationValue("ZipCodesCollectionName", "¨zipcodes")); }
        }
    }
}