using Amg_ingressos_aqui_carrinho_api.Repository.Interfaces;
using Amg_ingressos_aqui_carrinho_api.Exceptions;
using Amg_ingressos_aqui_carrinho_api.Model;
using System.Diagnostics.CodeAnalysis;
using Amg_ingressos_aqui_carrinho_api.Infra;
using MongoDB.Driver;

namespace Amg_ingressos_aqui_carrinho_api.Repository
{
    [ExcludeFromCodeCoverage]
    public class TransactionPaymentRepository<T> : ITransactionPaymentRepository
    {
        private readonly IMongoCollection<TransactionPayment> _transactionCollection;
        public TransactionPaymentRepository(IDbConnection dbconnection)
        {
            _transactionCollection = dbconnection.GetConnectionPayment();
        }

        public async Task<object> Save<T>(object transactionComplet)
        {
            try
            {
                await _transactionCollection.InsertOneAsync(transactionComplet as TransactionPayment);
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
    }
}