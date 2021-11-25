

namespace MenuAPI.Settings
{
   public interface IMongoDbSettings
    {
        public string DataBaseName { get; set; }
        public string ConnectionString { get; set; }
    }
}
