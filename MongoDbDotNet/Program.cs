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
            //InsertOne();
            InsertMany();
            Console.ReadKey();
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