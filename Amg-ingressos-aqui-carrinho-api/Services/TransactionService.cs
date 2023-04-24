using System.Text;
using Amg_ingressos_aqui_carrinho_api.Exceptions;
using Amg_ingressos_aqui_carrinho_api.Model;
using Amg_ingressos_aqui_carrinho_api.Repository.Interfaces;
using Amg_ingressos_aqui_carrinho_api.Services.Interfaces;
using Amg_ingressos_aqui_carrinho_api.Utils;

namespace Amg_ingressos_aqui_carrinho_api.Services
{
    public class TransactionService : ITransactionService
    {
        private ITransactionRepository _transactionRepository;
        private MessageReturn _messageReturn;

        public TransactionService(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
            _messageReturn = new MessageReturn();
        }

        public async Task<MessageReturn> SaveAsync(Transaction transaction)
        {
            try
            {
                transaction.IdEvent.ValidateIdMongo("Evento");
                transaction.IdPerson.ValidateIdMongo("Usuário");

                _messageReturn.Data = await _transactionRepository.Save<object>(transaction);
            }
            catch (IdMongoException ex)
            {
                _messageReturn.Message = ex.Message;
            }
            catch (SaveTransactionException ex)
            {
                _messageReturn.Message = ex.Message;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return _messageReturn;
        }

        public async Task<MessageReturn> SaveTransactionItenAsync(Transaction transaction)
        {
            try
            {
                transaction.Id.ValidateIdMongo("Id Transação");

                transaction.TransactionItens.ForEach(i =>
                {
                    i.IdTransaction = transaction.Id;
                    ValidateTransactionIten(i);

                    _transactionRepository.SaveTransactionIten<object>(i);
                });

                _messageReturn.Data = "Tikets Criados";
            }
            catch (IdMongoException ex)
            {
                _messageReturn.Message = ex.Message;
            }
            catch (SaveTransactionException ex)
            {
                _messageReturn.Message = ex.Message;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return _messageReturn;
        }

        private void ValidateTransactionIten(TransactionIten transactionIten)
        {
            transactionIten.IdLote.ValidateIdMongo("Lote");
            transactionIten.IdVariante.ValidateIdMongo("Variante");

            if (transactionIten.TicketPrice == new decimal(0))
                throw new SaveTransactionException("Valor do Ingresso é obrigatório");
            else if (transactionIten.AmoutTicket == new decimal(0))
                throw new SaveTransactionException("Quantidade de Ingresso é obrigatório");
        }
    }
}