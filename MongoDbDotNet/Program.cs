using MongoDB.Bson;
using MongoDB.Driver;
using MongoDbDotNet.Infrastructure;
using MongoDbDotNet.Repository;
using System;
using System.Collections.Generic;

namespace MongoDbDotNet
{
    class Program
    {
        static void Main(string[] args)
        {
            BulkWrite();
            //findonedel();
            //Deletone();
            //updateOne();
            //findonereplace();
            //UpdateOne();
            //InsertOne();
            //InsertMany();
            Console.ReadKey();
        }
        static void BulkWrite()
        {
            ModelContext modelContext = ModelContext.Create(new ConfigFileConfigurationRepository(), new AppConfigConnectionStringRepository());
            RestaurantDb newThaiRestaurant = new RestaurantDb();
            newThaiRestaurant.Address = new RestaurantAddressDb()
            {
                BuildingNr = "150",
                Coordinates = new double[] { 22.82, 99.12 },
                Street = "Old Street",
                ZipCode = 876654
            };
            newThaiRestaurant.Borough = "Somewhere in Thailand";
            newThaiRestaurant.Cuisine = "Thai";
            newThaiRestaurant.Grades = new List<RestaurantGradeDb>()
            {
                new RestaurantGradeDb() {Grade = "A", InsertedUtc = DateTime.UtcNow, Score = "7" },
                new RestaurantGradeDb() {Grade = "B", InsertedUtc = DateTime.UtcNow, Score = "4" },
                new RestaurantGradeDb() {Grade = "B", InsertedUtc = DateTime.UtcNow, Score = "10" },
                new RestaurantGradeDb() {Grade = "B", InsertedUtc = DateTime.UtcNow, Score = "4" }
            };
            newThaiRestaurant.Id = 463456435;
            newThaiRestaurant.Name = "FavThai";

            RestaurantDb newThaiRestaurantTwo = new RestaurantDb();
            newThaiRestaurantTwo.Address = new RestaurantAddressDb()
            {
                BuildingNr = "13",
                Coordinates = new double[] { 22.82, 99.12 },
                Street = "Wide Street",
                ZipCode = 345456
            };
            newThaiRestaurantTwo.Borough = "Somewhere in Thailand";
            newThaiRestaurantTwo.Cuisine = "Thai";
            newThaiRestaurantTwo.Grades = new List<RestaurantGradeDb>()
            {               
                new RestaurantGradeDb() {Grade = "A", InsertedUtc = DateTime.UtcNow, Score = "4" },
                new RestaurantGradeDb() {Grade = "B", InsertedUtc = DateTime.UtcNow, Score = "10" },
                new RestaurantGradeDb() {Grade = "C", InsertedUtc = DateTime.UtcNow, Score = "4" }
            };
            newThaiRestaurantTwo.Id = 463456436;
            newThaiRestaurantTwo.Name = "ThaiDinner";

            var updateDefinitionBorough = Builders<RestaurantDb>.Update.Set(r => r.Borough, "The middle of Thailand");

            FilterDefinition<RestaurantDb> deletionFilterDefinition = Builders<RestaurantDb>.Filter.Eq(r => r.Name, "PakistaniKing");

            BulkWriteResult bulkWriteResult = modelContext.Restaurants.BulkWrite(new WriteModel<RestaurantDb>[]
            {
                new InsertOneModel<RestaurantDb>(newThaiRestaurant),
                new InsertOneModel<RestaurantDb>(newThaiRestaurantTwo),
                new UpdateOneModel<RestaurantDb>(Builders<RestaurantDb>.Filter.Eq(r => r.Name, "RandomThai"), updateDefinitionBorough),
                new DeleteOneModel<RestaurantDb>(deletionFilterDefinition)
            }, new BulkWriteOptions() { IsOrdered = false });

            Console.WriteLine(string.Concat("Deleted count: ", bulkWriteResult.DeletedCount));
            Console.WriteLine(string.Concat("Inserted count: ", bulkWriteResult.InsertedCount));
            Console.WriteLine(string.Concat("Acknowledged: ", bulkWriteResult.IsAcknowledged));
            Console.WriteLine(string.Concat("Matched count: ", bulkWriteResult.MatchedCount));
            Console.WriteLine(string.Concat("Modified count: ", bulkWriteResult.ModifiedCount));
            Console.WriteLine(string.Concat("Request count: ", bulkWriteResult.RequestCount));
            Console.WriteLine(string.Concat("Upsert count: ", bulkWriteResult.Upserts));
        }
        static void findonedel()
        {
            ModelContext modelContext = ModelContext.Create(new ConfigFileConfigurationRepository(), new AppConfigConnectionStringRepository());
            FindOneAndDeleteOptions<RestaurantDb, RestaurantDb> findOneAndDeleteOptions = new FindOneAndDeleteOptions<RestaurantDb, RestaurantDb>()
            {
                Sort = Builders<RestaurantDb>.Sort.Descending(r => r.Id)
            };
            RestaurantDb deleted = modelContext.Restaurants.FindOneAndDelete(Builders<RestaurantDb>.Filter.Eq(r => r.Name, "BrandNewMexicanKing"), findOneAndDeleteOptions);
            Console.WriteLine(deleted == null);
        }
        static void Deletone()
        {
            ModelContext modelContext = ModelContext.Create(new ConfigFileConfigurationRepository(), new AppConfigConnectionStringRepository());
            DeleteResult deleteResult = modelContext.Restaurants.DeleteOne(Builders<RestaurantDb>.Filter.Eq(r => r.Name, "BrandNewMexicanKing"));
            Console.WriteLine(deleteResult.DeletedCount);
        }
        static void updateOne()
        {
            ModelContext modelContext = ModelContext.Create(new ConfigFileConfigurationRepository(), new AppConfigConnectionStringRepository());
            var updateDefinitionBorough = Builders<RestaurantDb>.Update.Set(r => r.Borough, "New Borough");
            var updateDefinitionGrades = Builders<RestaurantDb>.Update.Push(r => r.Grades, new RestaurantGradeDb() { Grade = "A", InsertedUtc = DateTime.UtcNow, Score = "6" });
            var combinedUpdateDefinition = Builders<RestaurantDb>.Update.Combine(updateDefinitionBorough, updateDefinitionGrades);
            UpdateResult updateResult = modelContext.Restaurants.UpdateOne(r => r.Name == "BrandNewMexicanKing", combinedUpdateDefinition, new UpdateOptions() { IsUpsert = true });
        }
        static void findonereplace()
        {
            ModelContext modelContext = ModelContext.Create(new ConfigFileConfigurationRepository(), new AppConfigConnectionStringRepository());
            RestaurantDb mexicanReplacement = new RestaurantDb();
            mexicanReplacement.Address = new RestaurantAddressDb()
            {
                BuildingNr = "4/D",
                Coordinates = new double[] { 24.68, -100.9 },
                Street = "New Mexico Street",
                ZipCode = 768324865
            };
            mexicanReplacement.Borough = "In the middle of Mexico";
            mexicanReplacement.Cuisine = "Mexican";
            mexicanReplacement.Grades = new List<RestaurantGradeDb>()
{
    new RestaurantGradeDb() {Grade = "B", InsertedUtc = DateTime.UtcNow, Score = "10" },
    new RestaurantGradeDb() {Grade = "B", InsertedUtc = DateTime.UtcNow, Score = "4" }
};
            mexicanReplacement.Id = 457656745;
            mexicanReplacement.Name = "BrandNewMexicanKing";
            mexicanReplacement.MongoDbId = ObjectId.Parse("576cfc66067b0044fc4d5c28");

            RestaurantDb replaced = modelContext.Restaurants.FindOneAndReplace
                (Builders<RestaurantDb>.Filter.Eq(r => r.Name, "NewMexicanKing"),
                mexicanReplacement,
                new FindOneAndReplaceOptions<RestaurantDb, RestaurantDb>()
                {
                    IsUpsert = true,
                    ReturnDocument = ReturnDocument.After,
                    Sort = Builders<RestaurantDb>.Sort.Descending(r => r.Name)
                });
        }
        static void UpdateOne()
        {
            ModelContext modelContext = ModelContext.Create(new ConfigFileConfigurationRepository(), new AppConfigConnectionStringRepository());
            RestaurantDb mexicanReplacement = new RestaurantDb();
            mexicanReplacement.Address = new RestaurantAddressDb()
            {
                BuildingNr = "3/D",
                Coordinates = new double[] { 24.68, -100.9 },
                Street = "Mexico Street",
                ZipCode = 768324865
            };
            mexicanReplacement.Borough = "Somewhere in Mexico";
            mexicanReplacement.Cuisine = "Mexican";
            mexicanReplacement.Grades = new List<RestaurantGradeDb>()
            {
                new RestaurantGradeDb() {Grade = "B", InsertedUtc = DateTime.UtcNow, Score = "10" },
                new RestaurantGradeDb() {Grade = "B", InsertedUtc = DateTime.UtcNow, Score = "4" }
            };
            mexicanReplacement.Id = 457656745;
            mexicanReplacement.Name = "NewMexicanKing";
            mexicanReplacement.MongoDbId = ObjectId.Parse("576cfc66067b0044fc4d5c28");
            ReplaceOneResult replaceOneResult = modelContext.Restaurants.ReplaceOne(Builders<RestaurantDb>.Filter.Eq(r => r.Name, "MexicanKing"), mexicanReplacement, new UpdateOptions() { IsUpsert = true });
            Console.WriteLine(replaceOneResult.IsAcknowledged);
            Console.WriteLine(replaceOneResult.MatchedCount);
            Console.WriteLine(replaceOneResult.ModifiedCount);
            Console.WriteLine(replaceOneResult.UpsertedId);
        }
        static void InsertMany()
        {
            ModelContext modelContext = ModelContext.Create(new ConfigFileConfigurationRepository(), new AppConfigConnectionStringRepository());
            RestaurantDb newPakistaniRestaurant = new RestaurantDb();
            newPakistaniRestaurant.Address = new RestaurantAddressDb()
            {
                BuildingNr = "12A",
                Coordinates = new double[] { 31.135, 71.24 },
                Street = "New Street",
                ZipCode = 9877654
            };
            newPakistaniRestaurant.Borough = "Somewhere in Pakistan";
            newPakistaniRestaurant.Cuisine = "Pakistani";
            newPakistaniRestaurant.Grades = new List<RestaurantGradeDb>()
            {
                new RestaurantGradeDb() {Grade = "A", InsertedUtc = DateTime.UtcNow, Score = "9" },
                new RestaurantGradeDb() {Grade = "C", InsertedUtc = DateTime.UtcNow, Score = "3" }
            };
            newPakistaniRestaurant.Id = 457656745;
            newPakistaniRestaurant.Name = "PakistaniKing";

            RestaurantDb newMexicanRestaurant = new RestaurantDb();
            newMexicanRestaurant.Address = new RestaurantAddressDb()
            {
                BuildingNr = "2/C",
                Coordinates = new double[] { 24.68, -100.9 },
                Street = "Mexico Street",
                ZipCode = 768324523
            };
            newMexicanRestaurant.Borough = "Somewhere in Mexico";
            newMexicanRestaurant.Cuisine = "Mexican";
            newMexicanRestaurant.Grades = new List<RestaurantGradeDb>()
            {
                new RestaurantGradeDb() {Grade = "B", InsertedUtc = DateTime.UtcNow, Score = "10" }
            };
            newMexicanRestaurant.Id = 457656745;
            newMexicanRestaurant.Name = "MexicanKing";

            List<RestaurantDb> newRestaurants = new List<RestaurantDb>()
            {
                newPakistaniRestaurant,
                newMexicanRestaurant
            };

            modelContext.Restaurants.InsertMany(newRestaurants);
        }
        static void InsertOne()
        {
            ModelContext modelContext = ModelContext.Create(new ConfigFileConfigurationRepository(), new AppConfigConnectionStringRepository());
            RestaurantDb newRestaurant = new RestaurantDb();
            newRestaurant.Address = new RestaurantAddressDb()
            {
                BuildingNr = "120",
                Coordinates = new double[] { 22.82, 99.12 },
                Street = "Whatever",
                ZipCode = 123456
            };
            newRestaurant.Borough = "Somewhere in Thailand";
            newRestaurant.Cuisine = "Thai";
            newRestaurant.Grades = new List<RestaurantGradeDb>()
            {
                new RestaurantGradeDb() {Grade = "A", InsertedUtc = DateTime.UtcNow, Score = "7" },
                new RestaurantGradeDb() {Grade = "B", InsertedUtc = DateTime.UtcNow, Score = "4" }
            };
            newRestaurant.Id = 883738291;
            newRestaurant.Name = "RandomThai";

            modelContext.Restaurants.InsertOne(newRestaurant);
        }
        static void BasicOperation()
        {
            ModelContext modelContext = ModelContext.Create(new ConfigFileConfigurationRepository(), new AppConfigConnectionStringRepository());
            var filter = Builders<RestaurantDb>.Filter.Eq(r => r.Borough, "Brooklyn");
            var restaurants = modelContext.Restaurants.FindSync<RestaurantDb>(filter).ToList();
            var restaurant = modelContext.Restaurants.FindSync<RestaurantDb>(filter).FirstOrDefault();
            Console.WriteLine(restaurant);
            Console.WriteLine();
            ZipCodeDb firstInMassachusetts = modelContext.ZipCodes.FindSync(z => z.State == "MA").FirstOrDefault();
            Console.WriteLine(firstInMassachusetts);
            Console.WriteLine();
            ZipCodeDb firstZip = modelContext.ZipCodes.Find(z => true).FirstOrDefault();
            Console.WriteLine(firstZip);
            RestaurantDb firstRestaurant = modelContext.Restaurants.Find(r => true).FirstOrDefault();
            Console.WriteLine();

            var boroughFilter = Builders<RestaurantDb>.Filter.Eq(r => r.Borough, "Brooklyn");
            var cuisineFilter = Builders<RestaurantDb>.Filter.Eq(r => r.Cuisine, "Delicatessen");
            var cuisineAndBoroughFilter = boroughFilter & cuisineFilter;
            var firstRes = modelContext.Restaurants.Find(cuisineAndBoroughFilter).First();
            Console.WriteLine(firstRes);
            Console.WriteLine();


            var firstResWithLinq = modelContext.Restaurants.Find(r => r.Borough == "Brooklyn" && r.Cuisine == "Delicatessen").Project(x => x.Name).FirstOrDefault();

            Console.WriteLine(firstResWithLinq);
        }
    }
}