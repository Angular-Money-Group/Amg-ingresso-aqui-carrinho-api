using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Amg_ingressos_aqui_carrinho_api.Infra
{
    public class DbConnection : IDbConnection
    {
        private readonly IOptions<TransactionDatabaseSettings> _config;
        private readonly ILogger<DbConnection> logger;
        public DbConnection(IOptions<TransactionDatabaseSettings> eventDatabaseSettings, ILogger<DbConnection> logger1)
        {
            logger = logger1;
            _config = eventDatabaseSettings;
            logger.LogInformation("Conectando ao banco de dados");
            logger.LogInformation(_config.Value.DatabaseName);
        }

        public IMongoCollection<T> GetConnection<T>(string colletionName)
        {
            var mongoUrl = new MongoUrl(_config.Value.ConnectionString);
            var mongoClient = new MongoClient(mongoUrl);
            var mongoDatabase = mongoClient.GetDatabase(_config.Value.DatabaseName);

            return mongoDatabase.GetCollection<T>(colletionName);
        }

        public IMongoCollection<T> GetConnection<T>()
        {
            var colletionName = GetCollectionName<T>();
            var mongoUrl = new MongoUrl(_config.Value.ConnectionString);
            var mongoClient = new MongoClient(mongoUrl);
            var mongoDatabase = mongoClient.GetDatabase(_config.Value.DatabaseName);

            return mongoDatabase.GetCollection<T>(colletionName);
        }
        private static string GetCollectionName<T>()
        {

            return typeof(T).Name.ToLower() ?? string.Empty;
        }
    }
}