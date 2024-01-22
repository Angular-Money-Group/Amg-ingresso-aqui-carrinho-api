using Amg_ingressos_aqui_carrinho_api.Dto;
using Amg_ingressos_aqui_carrinho_api.Model;
using Amg_ingressos_aqui_carrinho_api.Repository.Interfaces;
using Amg_ingressos_aqui_carrinho_api.Services.Interfaces;
using Amg_ingressos_aqui_carrinho_api.Utils;
using Amg_ingressos_aqui_carrinho_api.Consts;
using System.Data;
using Amg_ingressos_aqui_carrinho_api.Model.Querys;

namespace Amg_ingressos_aqui_carrinho_api.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly ITransactionItenService _transactionItenService;
        private readonly MessageReturn _messageReturn;
        private readonly ILogger<TransactionService> _logger;

        public TransactionService(
            ITransactionRepository transactionRepository,
            ILogger<TransactionService> logger,
            ITransactionItenService transactionItenService
            )
        {
            _transactionRepository = transactionRepository;
            _messageReturn = new MessageReturn();
            _logger = logger;
            _transactionItenService = transactionItenService;
        }

        public async Task<MessageReturn> GetByIdAsync(string id)
        {
            try
            {
                id.ValidateIdMongo("Transação");
                _messageReturn.Data = await _transactionRepository.GetById<GetTransactionEventData>(id);
                return _messageReturn;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Process, this.GetType().Name, nameof(GetByIdAsync)));
                throw;
            }
        }

        public async Task<MessageReturn> SaveAsync(Transaction transaction)
        {
            try
            {
                _messageReturn.Data = await _transactionRepository.Save(transaction);
                return _messageReturn;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Process, this.GetType().Name, nameof(SaveAsync)));
                throw;
            }
        }

        public async Task<MessageReturn> EditAsync(Transaction transaction)
        {
            try
            {
                transaction.Id.ValidateIdMongo("Transação");
                await _transactionRepository.Edit(transaction.Id, transaction);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Process, this.GetType().Name, nameof(EditAsync)));
                throw;
            }

            return _messageReturn;
        }

        public async Task<MessageReturn> GetByUserActivesAsync(string idUser)
        {
            try
            {
                idUser.ValidateIdMongo("Usuário");
                var data = await _transactionRepository.GetByUserEventData<TransactionComplet>(idUser);
                var listActives = data.Where(x => x.Events[0].StartDate >= DateTime.Now).Select(t => t);
                var list = new TransactionCardDto().ModelListToDtoList(listActives);
                _messageReturn.Data = list;
                return _messageReturn;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Process, this.GetType().Name, nameof(GetByUserActivesAsync)));
                throw;
            }
        }

        public async Task<MessageReturn> GetByUserHistoryAsync(string idUser)
        {
            try
            {
                idUser.ValidateIdMongo("Usuário");
                var data = await _transactionRepository.GetByUserEventData<TransactionComplet>(idUser);
                var listActives = data.Where(x => x.Events[0].StartDate < DateTime.Now).Select(t => t);
                var list = new TransactionCardDto().ModelListToDtoList(listActives);
                _messageReturn.Data = list;
                return _messageReturn;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Process, this.GetType().Name, nameof(GetByUserHistoryAsync)));
                throw;
            }
        }

        public async Task<MessageReturn> GetByUserTicketEventDataAsync(string idUser, string idEvent)
        {
            try
            {
                idUser.ValidateIdMongo("Usuário");
                idUser.ValidateIdMongo("Evento");
                List<TransactionComplet> result = await _transactionRepository
                                                            .GetByUserTicketData<TransactionComplet>(
                                                                idUser,
                                                                idEvent
                                                            );
                _messageReturn.Data = new TransactionTicketDto().ListModelToListDto(result);
                return _messageReturn;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Process, this.GetType().Name, nameof(GetByUserTicketEventDataAsync)));
                throw;
            }
        }

        public async Task<MessageReturn> DeleteAsync(string id)
        {
            try
            {
                id.ValidateIdMongo("Transação");
                _messageReturn.Data = await _transactionItenService.DeleteByIdTransaction(id);
                _messageReturn.Data = await _transactionRepository.Delete(id);
                return _messageReturn;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Process, this.GetType().Name, nameof(DeleteAsync)));
                throw;
            }
        }

        public async Task<MessageReturn> CancelTransaction(Transaction transaction)
        {
            try
            {
                transaction.Status = Enum.StatusPaymentEnum.Canceled;
                _messageReturn.Data = await _transactionRepository.Edit(transaction.Id, transaction);

                return _messageReturn;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Process, this.GetType().Name, nameof(CancelTransaction)));
                throw;
            }
        }
    }
}