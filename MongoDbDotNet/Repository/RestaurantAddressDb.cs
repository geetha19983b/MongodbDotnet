using MongoDB.Bson.Serialization.Attributes;

namespace MongoDbDotNet.Repository
{
    [BsonIgnoreExtraElements]
    public class RestaurantAddressDb
    {
        [BsonElement(elementName: "building")]
        public string BuildingNr { get; set; }
        [BsonElement(elementName: "coord")]
        public double[] Coordinates { get; set; }
        [BsonElement(elementName: "street")]
        public string Street { get; set; }
        [BsonElement(elementName: "zipcode")]
        [BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public int ZipCode { get; set; }
    }
}