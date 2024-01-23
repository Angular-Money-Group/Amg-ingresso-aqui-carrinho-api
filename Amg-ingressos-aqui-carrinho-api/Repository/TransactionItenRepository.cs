using Amg_ingressos_aqui_carrinho_api.Repository.Interfaces;
using Amg_ingressos_aqui_carrinho_api.Exceptions;
using Amg_ingressos_aqui_carrinho_api.Model;
using System.Diagnostics.CodeAnalysis;
using Amg_ingressos_aqui_carrinho_api.Infra;
using MongoDB.Driver;

namespace Amg_ingressos_aqui_carrinho_api.Repository
{
    [ExcludeFromCodeCoverage]
    public class TransactionItenRepository : ITransactionItenRepository
    {
        private readonly IMongoCollection<TransactionIten> _transactionItenCollection;
        public TransactionItenRepository(IDbConnection dbconnectionIten)
        {
            _transactionItenCollection = dbconnectionIten.GetConnection<TransactionIten>("transactionIten");
        }

        public async Task<TransactionIten> Save(TransactionIten transaction)
        {
            await _transactionItenCollection.InsertOneAsync(transaction);
            return transaction;

        }

        public async Task<List<T>> GetByIdTransaction<T>(string idTransaction)
        {
            var builder = Builders<TransactionIten>.Filter;
            var filter = builder.Empty;

            if (string.IsNullOrWhiteSpace(idTransaction))
                throw new GetException("idTransactin é necessário.");

            var firstNameFilter = builder.Eq(x => x.IdTransaction, idTransaction);
                filter &= firstNameFilter;

            var result = await _transactionItenCollection.Find(filter)
            .As<T>()
            .ToListAsync();


            if (result == null)
                throw new GetException("Itens da transação não encontrados");

            return result;
        }

        public async Task<bool> DeleteByIdTransaction(string idTransaction)
        {
            // find a person using an equality filter on its id
            var filter = Builders<TransactionIten>.Filter.Eq(transaction => transaction.IdTransaction, idTransaction);

            // delete the person
            var transactionDeleteResult = await _transactionItenCollection.DeleteOneAsync(filter);
            if (transactionDeleteResult.DeletedCount < 1)
                return false;

            return true;
        }
    }
}