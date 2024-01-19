using Amg_ingressos_aqui_carrinho_api.Repository.Interfaces;
using Amg_ingressos_aqui_carrinho_api.Exceptions;
using Amg_ingressos_aqui_carrinho_api.Model;
using System.Diagnostics.CodeAnalysis;
using Amg_ingressos_aqui_carrinho_api.Infra;
using MongoDB.Driver;
using Amg_ingressos_aqui_carrinho_api.Repository.Querys;
using MongoDB.Bson;
using Amg_ingressos_aqui_carrinho_api.Model.Querys;
using Amg_ingressos_aqui_carrinho_api.Enum;

namespace Amg_ingressos_aqui_carrinho_api.Repository
{
    [ExcludeFromCodeCoverage]
    public class TransactionRepository : ITransactionRepository
    {
        private readonly IMongoCollection<Transaction> _transactionCollection;

        public TransactionRepository(IDbConnection<Transaction> dbconnection)
        {
            _transactionCollection = dbconnection.GetConnection("transaction");
        }

        public async Task<object> GetById(string idTransaction)
        {
            try
            {
                var json = QuerysMongo.GetTransactionQuery;

                BsonDocument documentFilter = BsonDocument.Parse(
                    @"{$addFields:{'_id': { '$toString': '$_id' }}}"
                );
                BsonDocument documentFilter1 = BsonDocument.Parse(
                    @"{ $match: { '$and': [{ '_id': '" + idTransaction.ToString() + "' }] }}"
                );
                BsonDocument document = BsonDocument.Parse(json);
                BsonDocument[] pipeline = new BsonDocument[]
                {
                    documentFilter,
                    documentFilter1,
                    document
                };

                List<GetTransactionEventData> pResults = _transactionCollection
                    .Aggregate<GetTransactionEventData>(pipeline)
                    .ToList();

                //var result = await _eventCollection.FindAsync<Event>(x => x._Id == id as string)
                //    .Result.FirstOrDefaultAsync();


                if (pResults == null)
                    throw new GetException("Evento não encontrado");

                return pResults;
            }
            catch (GetException ex)
            {
                throw ex;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<T>> GetByUser<T>(string idUser)
        {
            try
            {
                var transactions = await _transactionCollection
                    .Find(new BsonDocument { { "IdPerson", ObjectId.Parse(idUser) } })
                    .As<T>()
                    .ToListAsync();
                return transactions;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<T>> GetByUserEventData<T>(string idUser)
        {
            try
            {
                var transactions = await _transactionCollection.Aggregate()
                    .Match(new BsonDocument { { "IdPerson", ObjectId.Parse(idUser) } })
                    .Lookup("events", "IdEvent", "_id", "Events")
                    .As<T>()
                    .ToListAsync();
                return transactions;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<T>> GetByUserTicketData<T>(string idPerson, string idEvent)
        {
            try
            {
                var filters = new List<FilterDefinition<Transaction>>
                {
                    Builders<Transaction>.Filter.Eq("IdPerson", idPerson),
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

                return transactions;
            }
            catch (GetException ex)
            {
                throw ex;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public async Task<object> Save<T>(object transaction)
        {
            try
            {
                await _transactionCollection.InsertOneAsync(transaction as Transaction);
                return transaction as Transaction;
            }
            catch (SaveTransactionException ex)
            {
                throw ex;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public async Task<object> Update<T1>(object transaction)
        {
            try
            {
                var transactionModel = (transaction as Transaction);
                var update = Builders<Transaction>.Update
                    .Set(transactionMongo => transactionMongo.Status, transactionModel.Status)
                    .Set(
                        transactionMongo => transactionMongo.PaymentMethod,
                        transactionModel.PaymentMethod
                    )
                    .Set(
                        transactionMongo => transactionMongo.PaymentMethodPix,
                        transactionModel.PaymentMethodPix
                    )
                    .Set(
                        transactionMongo => transactionMongo.PaymentPix,
                        transactionModel.PaymentPix
                    )
                    .Set(transactionMongo => transactionMongo.IdPerson, transactionModel.IdPerson)
                    .Set(transactionMongo => transactionMongo.Stage, transactionModel.Stage)
                    .Set(transactionMongo => transactionMongo.Tax, transactionModel.Tax)
                    .Set(transactionMongo => transactionMongo.Discount, transactionModel.Discount)
                    .Set(
                        transactionMongo => transactionMongo.PaymentIdService,
                        transactionModel.PaymentIdService
                    )
                    .Set(transactionMongo => transactionMongo.Details, transactionModel.Details)
                    .Set(
                        transactionMongo => transactionMongo.TotalValue,
                        transactionModel.TotalValue
                    );

                var filter = Builders<Transaction>.Filter.Eq(
                    transactionMongo => transactionMongo.Id,
                    transactionModel.Id
                );

                await _transactionCollection.UpdateOneAsync(filter, update);
                return "Atualizado";
            }
            catch (SaveTransactionException ex)
            {
                throw ex;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public async Task<object> Delete(string id)
        {
            try
            {
                // find a person using an equality filter on its id
                var filter = Builders<Transaction>.Filter.Eq(transaction => transaction.Id, id);

                // delete the person
                var transactionDeleteResult = await _transactionCollection.DeleteOneAsync(filter);
                if (transactionDeleteResult.DeletedCount == 1)
                    return "transação deletada com sucesso";
                else
                    return "erro ao deletar transação";
            }
            catch (SaveTransactionException ex)
            {
                throw ex;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
    }
}
