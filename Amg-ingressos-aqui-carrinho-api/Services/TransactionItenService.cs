using Amg_ingressos_aqui_carrinho_api.Consts;
using Amg_ingressos_aqui_carrinho_api.Exceptions;
using Amg_ingressos_aqui_carrinho_api.Model;
using Amg_ingressos_aqui_carrinho_api.Repository.Interfaces;
using Amg_ingressos_aqui_carrinho_api.Services.Interfaces;
using Amg_ingressos_aqui_carrinho_api.Utils;

namespace Amg_ingressos_aqui_carrinho_api.Services
{
    public class TransactionItenService : ITransactionItenService
    {
        private readonly ITransactionItenRepository _transactionItenRepository;
        private readonly MessageReturn _messageReturn;
        private readonly ILogger<TransactionService> _logger;

        public TransactionItenService(
            ITransactionItenRepository transactionItenRepository,
            ILogger<TransactionService> logger)
        {
            _transactionItenRepository = transactionItenRepository;
            _messageReturn = new MessageReturn();
            _logger = logger;
        }
        public Task<bool> DeleteByIdTransaction(string idTransaction)
        {
            try
            {
                idTransaction.ValidateIdMongo("idTransaction");
                var result = _transactionItenRepository.DeleteByIdTransaction(idTransaction);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Process, this.GetType().Name, nameof(DeleteByIdTransaction)));
                throw;
            }
        }

        public Task<List<TransactionIten>> GetByIdTransaction<TransactionIten>(string idTransaction)
        {
            try
            {
                idTransaction.ValidateIdMongo("idTransaction");
                var result = _transactionItenRepository.GetByIdTransaction<TransactionIten>(idTransaction);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Process, this.GetType().Name, nameof(GetByIdTransaction)));
                throw;
            }
        }

        public Task<TransactionIten> Save(TransactionIten transactionItem)
        {
            try
            {
                ValidateTransactionIten(transactionItem);
                var result = _transactionItenRepository.Save(transactionItem);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Process, this.GetType().Name, nameof(GetByIdTransaction)));
                throw;
            }
        }

        private void ValidateTransactionIten(TransactionIten transactionIten)
        {
            transactionIten.IdTicket.ValidateIdMongo("Ticket");
            transactionIten.IdTransaction.ValidateIdMongo("Transação");

            if (transactionIten.TicketPrice == new decimal(0))
                throw new SaveException("Valor do Ingresso é obrigatório");
        }
    }
}