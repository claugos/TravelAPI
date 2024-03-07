using MongoDB.Driver;

namespace TravelAPI.Repositories
{
    public class MongoDBRepository
    {
        private static IConfiguration _config;
        private static MongoDBRepository? instance;
        private static readonly object lockObj = new();

        private readonly IMongoClient client;
        public IMongoDatabase db { get; }

        private MongoDBRepository(IConfiguration config)
        {
            _config = config;

            string connectionString = _config.GetSection("MongoDBConnection").Value ?? throw new ArgumentNullException("MongoDBConnection");
            string databaseName = _config.GetSection("DatabaseName").Value ?? throw new ArgumentNullException("DatabaseName");
            client = new MongoClient(connectionString);
            db = client.GetDatabase(databaseName);
        }

        public static MongoDBRepository GetInstance(IConfiguration config)
        {
            if (instance == null)
            {
                lock (lockObj)
                {
                    instance ??= new MongoDBRepository(config);
                }
            }
            return instance;
        }
    }
}
