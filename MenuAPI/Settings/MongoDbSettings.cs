
namespace MenuAPI.Settings
{
    public class MongoDbSettings : IMongoDbSettings
    {
        public string DataBaseName { get; set; }
        public string ConnectionString { get; set; }
    }
}
