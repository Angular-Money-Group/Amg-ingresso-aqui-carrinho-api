using Amg_ingressos_aqui_carrinho_api.Model;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Amg_ingressos_aqui_carrinho_api.Infra
{
    public class DbConnection : IDbConnection
    {
        private IOptions<TransactionDatabaseSettings> _config;
        public DbConnection(IOptions<TransactionDatabaseSettings> transactionDatabaseSettings)
        {
            _config = transactionDatabaseSettings;
        }

        public IMongoCollection<Transaction> GetConnection(){

            var mongoUrl = new MongoUrl(_config.Value.ConnectionString);
            var _mongoClient = new MongoClient(mongoUrl);
            var mongoDatabase = _mongoClient.GetDatabase(_config.Value.DatabaseName);

            return mongoDatabase.GetCollection<Transaction>(_config.Value.TransactionCollectionName);
        }
        public IMongoCollection<TransactionPayment> GetConnectionPayment(){

            var mongoUrl = new MongoUrl(_config.Value.ConnectionString);
            var _mongoClient = new MongoClient(mongoUrl);
            var mongoDatabase = _mongoClient.GetDatabase(_config.Value.DatabaseName);

            return mongoDatabase.GetCollection<TransactionPayment>(_config.Value.TransactionCollectionName);
        }
    }
}