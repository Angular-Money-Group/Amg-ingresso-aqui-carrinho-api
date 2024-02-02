using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Amg_ingressos_aqui_carrinho_api.Infra
{
    public class DbConnection : IDbConnection
    {
        private readonly ILogger<DbConnection> _logger;
        private readonly IConfiguration _configuration;
        public DbConnection(ILogger<DbConnection> logger,IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _logger.LogInformation(_configuration["DatabaseName"]);
            _logger.LogInformation(_configuration["DbMongoConcectionString"]);
        }

        public IMongoCollection<T> GetConnection<T>(string colletionName)
        {
            var mongoUrl = new MongoUrl(_configuration["DbMongoConcectionString"]);
            var mongoClient = new MongoClient(mongoUrl);
            var mongoDatabase = mongoClient.GetDatabase(_configuration["DatabaseName"]);

            return mongoDatabase.GetCollection<T>(colletionName);
        }

        public IMongoCollection<T> GetConnection<T>()
        {
            var colletionName = GetCollectionName<T>();
            var mongoUrl = new MongoUrl(_configuration["DbMongoConcectionString"]);
            var mongoClient = new MongoClient(mongoUrl);
            var mongoDatabase = mongoClient.GetDatabase(_configuration["DatabaseName"]);

            return mongoDatabase.GetCollection<T>(colletionName);
        }

        private static string GetCollectionName<T>()
        {
            return typeof(T).Name.ToLower() ?? string.Empty;
        }
    }
}