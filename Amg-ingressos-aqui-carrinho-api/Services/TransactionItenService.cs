using Amg_ingressos_aqui_carrinho_api.Consts;
using Amg_ingressos_aqui_carrinho_api.Dto;
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
        public Task<MessageReturn> DeleteByIdTransaction(string idTransaction)
        {
            try
            {
                idTransaction.ValidateIdMongo("idTransaction");
                _messageReturn.Data = _transactionItenRepository.DeleteByIdTransaction(idTransaction);
                return Task.FromResult(_messageReturn);
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.Process, this.GetType().Name, nameof(DeleteByIdTransaction)), ex);
                throw;
            }
        }

        public async Task<MessageReturn> GetByIdTransaction(string idTransaction)
        {
            try
            {
                idTransaction.ValidateIdMongo("idTransaction");
                _messageReturn.Data = await _transactionItenRepository.GetByIdTransaction<TransactionIten>(idTransaction);
                return _messageReturn;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.Process, this.GetType().Name, nameof(GetByIdTransaction)), ex);
                throw;
            }
        }

        public Task<MessageReturn> Save(TransactionIten transactionItem)
        {
            try
            {
                ValidateTransactionIten(transactionItem);
                _messageReturn.Data = _transactionItenRepository.Save(transactionItem);
                return Task.FromResult(_messageReturn);
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.Process, this.GetType().Name, nameof(GetByIdTransaction)), ex);
                throw;
            }
        }

        public Task<MessageReturn> ProcessSaveTransactionItens(string idTransaction, List<TransactionItenDto> transactionItensDto, List<Ticket> listTicket)
        {
            try
            {
                idTransaction.ValidateIdMongo("Id Transação");
                transactionItensDto.ForEach(async i =>
                {
                    //pra cada compra carimbar o ticket e criar transaction item
                    for (int amount = 0; amount < i.AmountTicket; amount++)
                    {
                        var ticket = listTicket.Find(t => t.IdLot == i.IdLot) ?? throw new RuleException("Ticket não encontrado.");
                        var transactionItem = new TransactionIten()
                        {
                            HalfPrice = i.HalfPrice,
                            IdTransaction = idTransaction,
                            IdTicket = ticket.Id,
                            TicketPrice = ticket.Value,
                            Details = i.Details
                        };                        
                        //cria transaction iten
                        await Save(transactionItem);
                        listTicket.Remove(ticket); //remove ticket da lista de tickets disponiveis para compra
                    }
                });
                _messageReturn.Data = true;
                return Task.FromResult(_messageReturn);
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.Process, this.GetType().Name, nameof(ProcessSaveTransactionItens)), ex);
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