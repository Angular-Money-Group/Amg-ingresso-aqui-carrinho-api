using Amg_ingressos_aqui_carrinho_api.Repository.Interfaces;
using Amg_ingressos_aqui_carrinho_api.Exceptions;
using Amg_ingressos_aqui_carrinho_api.Model;
using System.Diagnostics.CodeAnalysis;
using Amg_ingressos_aqui_carrinho_api.Infra;
using MongoDB.Driver;

namespace Amg_ingressos_aqui_carrinho_api.Repository
{
    [ExcludeFromCodeCoverage]
    public class TransactionRepository<T> : ITransactionRepository
    {
        private readonly IMongoCollection<Transaction> _transactionCollection;
        public TransactionRepository(IDbConnection dbconnection)
        {
            _transactionCollection = dbconnection.GetConnection();
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

        public async Task<object> Save<T>(object transactionComplet)
        {
            try
            {
                await _transactionCollection.InsertOneAsync(transactionComplet as Transaction);
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

        public Task<object> SaveTransactionIten<T1>(object transactionComplet)
        {
            throw new NotImplementedException();
        }

        public Task<object> SaveTransactionPayment<T1>(object transactionPayment)
        {
            throw new NotImplementedException();
        }
    }
}