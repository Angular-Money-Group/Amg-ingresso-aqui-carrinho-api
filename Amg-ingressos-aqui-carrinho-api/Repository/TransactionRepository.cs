using Amg_ingressos_aqui_carrinho_api.Repository.Interfaces;
using Amg_ingressos_aqui_carrinho_api.Exceptions;
using Amg_ingressos_aqui_carrinho_api.Model;
using System.Diagnostics.CodeAnalysis;
using Amg_ingressos_aqui_carrinho_api.Infra;
using MongoDB.Driver;
using Amg_ingressos_aqui_carrinho_api.Repository.Querys;
using MongoDB.Bson;
using Amg_ingressos_aqui_carrinho_api.Model.Querys;

namespace Amg_ingressos_aqui_carrinho_api.Repository
{
    [ExcludeFromCodeCoverage]
    public class TransactionRepository<T> : ITransactionRepository
    {
        private readonly IMongoCollection<Transaction> _transactionCollection;
        private readonly IMongoCollection<TransactionIten> _transactionItenCollection;
        public TransactionRepository(IDbConnection<Transaction> dbconnection)
        {
            _transactionCollection = dbconnection.GetConnection("transaction");
        }

        public async Task<object> GetById(string idTransaction)
        {
            try
            {
                var json = QuerysMongo.GetTransactionQuery;

                BsonDocument documentFilter = BsonDocument.Parse(@"{$addFields:{'_id': { '$toString': '$_id' }}}");
                BsonDocument documentFilter1 = BsonDocument.Parse(@"{ $match: { '$and': [{ '_id': '" + idTransaction.ToString() + "' }] }}");
                BsonDocument document = BsonDocument.Parse(json);
                BsonDocument[] pipeline = new BsonDocument[] {
                    documentFilter,
                    documentFilter1,
                    document
                };
                var result = _transactionCollection
                                                .Aggregate<object>(pipeline).ToList();

                List<GetTransaction> pResults = _transactionCollection
                                                .Aggregate<GetTransaction>(pipeline).ToList();

                
                //var result = await _eventCollection.FindAsync<Event>(x => x._Id == id as string)
                //    .Result.FirstOrDefaultAsync();


                if (pResults == null)
                    throw new GetByIdTransactionException("Evento não encontrado");

                return pResults;
            }
            catch (GetByIdTransactionException ex)
            {
                throw ex;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public async Task<object> GetByUser(string idPerson)
        {
            try
            {
                var builder = Builders<Transaction>.Filter;
                var filter = builder.Empty;

                if (!string.IsNullOrWhiteSpace(idPerson))
                {
                    var idPersonFilter = builder.Eq(x => x.IdPerson, idPerson);
                    var statusFilter = builder.Eq(x => x.Status, Enum.StatusPaymentEnum.InProgress);
                    filter &= idPersonFilter;
                    filter &= statusFilter;
                }

                var result = await _transactionCollection.Find(filter).ToListAsync();
                
                
                if (result == null)
                    throw new GetByIdTransactionException("Transação não encontrada");

                return result;
            }
            catch (GetByIdTransactionException ex)
            {
                throw ex;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public async Task<object> Post<T1>(object transactionComplet)
        {
            try
            {
                var arrayFilter = Builders<Transaction>.Filter.Eq("student_id", 10000)
                    & Builders<Transaction>.Filter.Eq("scores.type", "quiz");
                var arrayUpdate = Builders<Transaction>.Update.Set("scores.$.score", 84.92381029342834);
                await _transactionCollection.UpdateOneAsync(arrayFilter, arrayUpdate);
                return "Transaction criado";
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

        public async Task<object> Save<T>(object transaction)
        {
            try
            {
                await _transactionCollection.InsertOneAsync(transaction as Transaction);
                return (transaction as Transaction).Id;

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
                   .Set(transactionMongo => transactionMongo.PaymentMethod, transactionModel.PaymentMethod)
                   .Set(transactionMongo => transactionMongo.IdPerson, transactionModel.IdPerson)
                   .Set(transactionMongo => transactionMongo.Stage, transactionModel.Stage)
                   .Set(transactionMongo => transactionMongo.Tax, transactionModel.Tax)
                   .Set(transactionMongo => transactionMongo.Discount, transactionModel.Discount)
                   .Set(transactionMongo => transactionMongo.PaymentIdService, transactionModel.PaymentIdService)
                   .Set(transactionMongo => transactionMongo.Details, transactionModel.Details)
                   .Set(transactionMongo => transactionMongo.TotalValue, transactionModel.TotalValue);

                var filter = Builders<Transaction>.Filter
                    .Eq(transactionMongo => transactionMongo.Id, transactionModel.Id);

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
    }
}