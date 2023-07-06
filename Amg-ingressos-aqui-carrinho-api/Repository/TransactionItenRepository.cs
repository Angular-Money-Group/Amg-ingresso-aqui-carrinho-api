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
                _transactionItenCollection.InsertOneAsync(transaction as TransactionIten);
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
        public async Task<object> GetByIdTransaction(string idTransaction)
        {
            try
            {
                var builder = Builders<TransactionIten>.Filter;
                var filter = builder.Empty;

                if (!string.IsNullOrWhiteSpace(idTransaction))
                {
                    var firstNameFilter = builder.Eq(x => x.IdTransaction, idTransaction);
                    filter &= firstNameFilter;
                }

                var result = _transactionItenCollection.Find(filter).ToListAsync();
                
                
                if (result == null)
                    throw new GetByIdTransactionException("Transacao itens nao encontrados");

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

        public async Task<object> DeleteByIdTransaction(string idTransaction){
            try
            {
                // find a person using an equality filter on its id
                var filter = Builders<TransactionIten>.Filter.Eq(transaction => transaction.IdTransaction, idTransaction);

                // delete the person
                var transactionDeleteResult = await _transactionItenCollection.DeleteOneAsync(filter);
                if (transactionDeleteResult.DeletedCount >= 1)
                    return "transação iten deletado com sucesso";
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