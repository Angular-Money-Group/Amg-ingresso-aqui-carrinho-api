using Amg_ingressos_aqui_carrinho_api.Repository.Interfaces;
using Amg_ingressos_aqui_carrinho_api.Exceptions;
using Amg_ingressos_aqui_carrinho_api.Model;
using System.Diagnostics.CodeAnalysis;
using Amg_ingressos_aqui_carrinho_api.Infra;
using MongoDB.Driver;
using MongoDB.Bson;

namespace Amg_ingressos_aqui_carrinho_api.Repository
{
    [ExcludeFromCodeCoverage]
    public class TransactionRepository : ITransactionRepository
    {
        private readonly IMongoCollection<Transaction> _transactionCollection;

        public TransactionRepository(IDbConnection dbconnection)
        {
            _transactionCollection = dbconnection.GetConnection<Transaction>("transaction");
        }

        public async Task<T> GetById<T>(string id)
        {
            BsonDocument documentFilter = new BsonDocument { { "_id", ObjectId.Parse(id) } };

            var transactionsData = await _transactionCollection.Aggregate()
                .Match(documentFilter)
                .Lookup("transactionIten", "_id", "IdTransaction", "TransactionItens")
                .As<T>()
                .ToListAsync();

            if (transactionsData == null || !transactionsData.Any())
                throw new GetException("Transação não encontrada");

            return transactionsData[0];
        }

        public async Task<List<T>> GetByUser<T>(string idUser)
        {

            var transactions = await _transactionCollection
                .Find(new BsonDocument { { "IdPerson", ObjectId.Parse(idUser) } })
                .As<T>()
                .ToListAsync();

            if (transactions == null)
                throw new GetException("Transações não encontradas");

            return transactions;
        }

        public async Task<List<T>> GetByUserEventData<T>(string idUser)
        {
            var transactions = await _transactionCollection.Aggregate()
                .Match(new BsonDocument { { "IdPerson", ObjectId.Parse(idUser) } })
                .Lookup("events", "IdEvent", "_id", "Events")
                .As<T>()
                .ToListAsync();

            if (transactions == null)
                throw new GetException("Transações não encontradas");

            return transactions;
        }

        public async Task<List<T>> GetByUserTicketData<T>(string idUser, string idEvent)
        {
            var filters = new List<FilterDefinition<Transaction>>
                {
                    Builders<Transaction>.Filter.Eq("IdPerson", idUser),
                    Builders<Transaction>.Filter.Eq("IdEvent", idEvent)
                };
            FilterDefinition<Transaction> filter = Builders<Transaction>.Filter.And(filters);

            var transactions = await _transactionCollection.Aggregate()
                .Match(filter)
                .Lookup("events", "IdEvent", "_id", "Events")
                .Lookup("transactionIten", "_id", "IdTransaction", "TransactionItens")
                .Lookup("tickets", "TransactionItens.IdTicket", "_id", "Tickets")
                .As<T>()
                .ToListAsync();

            if (transactions == null)
                throw new GetException("Transações não encontradas");

            return transactions;
        }

        public async Task<Transaction> Save(Transaction transaction)
        {
            await _transactionCollection.InsertOneAsync(transaction);
            return transaction;
        }

        public async Task<bool> Edit(string id, Transaction transaction)
        {
            var update = Builders<Transaction>.Update
                .Set(transactionMongo => transactionMongo.Status, transaction.Status)
                .Set(
                    transactionMongo => transactionMongo.PaymentMethod,
                    transaction.PaymentMethod
                )
                .Set(transactionMongo => transactionMongo.IdPerson, transaction.IdPerson)
                .Set(transactionMongo => transactionMongo.Stage, transaction.Stage)
                .Set(transactionMongo => transactionMongo.Tax, transaction.Tax)
                .Set(transactionMongo => transactionMongo.Discount, transaction.Discount)
                .Set(
                    transactionMongo => transactionMongo.PaymentIdService,
                    transaction.PaymentIdService
                )
                .Set(transactionMongo => transactionMongo.Details, transaction.Details)
                .Set(
                    transactionMongo => transactionMongo.TotalValue,
                    transaction.TotalValue
                );

            var filter = Builders<Transaction>.Filter.Eq(transactionMongo => transactionMongo.Id, id);

            var result = await _transactionCollection.UpdateOneAsync(filter, update);
            if (result.ModifiedCount <= 0)
                throw new EditException("erro ao editar registro");

            return true;
        }

        public async Task<bool> Delete(string id)
        {
            // find a person using an equality filter on its id
            var filter = Builders<Transaction>.Filter.Eq(transaction => transaction.Id, id);

            // delete the person
            var transactionDeleteResult = await _transactionCollection.DeleteOneAsync(filter);
            if (transactionDeleteResult.DeletedCount <= 1)
                throw new EditException("erro ao deletar transação");

            return true;
        }
    }
}