using Amg_ingressos_aqui_carrinho_api.Repository.Interfaces;
using Amg_ingressos_aqui_carrinho_api.Exceptions;
using Amg_ingressos_aqui_carrinho_api.Model;
using System.Diagnostics.CodeAnalysis;
using Amg_ingressos_aqui_carrinho_api.Infra;
using MongoDB.Driver;

namespace Amg_ingressos_aqui_carrinho_api.Repository
{
    [ExcludeFromCodeCoverage]
    public class TransactionItenRepository<T> : ITransactionItenRepository
    {
        private readonly IMongoCollection<TransactionIten> _transactionItenCollection;
        public TransactionItenRepository(IDbConnection<TransactionIten> dbconnectionIten)
        {
            _transactionItenCollection= dbconnectionIten.GetConnection("transactionIten");
        }

        public async Task<object> Save<T>(object transaction)
        {
            try
            {
                await _transactionItenCollection.InsertOneAsync(transaction as TransactionIten);
                return (transaction as TransactionIten).Id;
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