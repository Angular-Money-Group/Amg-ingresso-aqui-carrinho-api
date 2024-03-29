using Amg_ingressos_aqui_carrinho_api.Consts;
using Amg_ingressos_aqui_carrinho_api.Dto;
using Amg_ingressos_aqui_carrinho_api.Dto.Pagbank;
using Amg_ingressos_aqui_carrinho_api.Enum;
using Amg_ingressos_aqui_carrinho_api.Exceptions;
using Amg_ingressos_aqui_carrinho_api.Model;
using Amg_ingressos_aqui_carrinho_api.Model.Cielo.Callback;
using Amg_ingressos_aqui_carrinho_api.Services.Interfaces;
using Amg_ingressos_aqui_carrinho_api.Utils;
using Newtonsoft.Json;

namespace Amg_ingressos_aqui_carrinho_api.Services
{
    public class CheckoutService : ICheckoutService
    {
        private readonly ITransactionService _transactionService;
        private readonly ITransactionItenService _transactionItenService;
        private MessageReturn _messageReturn;
        private readonly IUserService _userService;
        private readonly ITicketService _ticketService;
        private readonly IPaymentService _paymentService;
        private readonly INotificationService _notificationService;
        private readonly IQrCodeService _qrCodeService;
        private readonly ILogger<TransactionService> _logger;

        public CheckoutService(
            ITransactionService transactionService,
            ITransactionItenService transactionItenService,
            ITicketService ticketService,
            IUserService userService,
            IPaymentService paymentService,
            INotificationService notificationService,
            ILogger<TransactionService> logger,
            IQrCodeService qrCodeService
        )
        {
            _transactionService = transactionService;
            _ticketService = ticketService;
            _paymentService = paymentService;
            _transactionItenService = transactionItenService;
            _messageReturn = new MessageReturn();
            _notificationService = notificationService;
            _logger = logger;
            _qrCodeService = qrCodeService;
            _userService = userService;
        }

        public async Task<MessageReturn> ProcessSaveAsync(TransactionDto transactionDto)
        {
            try
            {
                transactionDto.IdUser.ValidateIdMongo("Usuário");
                var transactionModel = transactionDto.DtoToModel();
                transactionModel.Status = StatusPayment.InProgress;
                transactionModel.Stage = StageTransaction.Confirm;

                var listTickets = await _ticketService.ReservTicketsAsync(transactionDto);
                await _transactionService.SaveAsync(transactionModel);
                await _transactionItenService.ProcessSaveTransactionItens(transactionModel.Id, transactionDto.TransactionItensDto, listTickets);

                _messageReturn.Data = transactionModel;
                return _messageReturn;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.Process, this.GetType().Name, nameof(ProcessSaveAsync)), ex);
                throw;
            }
        }

        public async Task<MessageReturn> UpdateTransactionPersonDataAsync(string idTransaction)
        {
            try
            {

                idTransaction.ValidateIdMongo("idTransaction");
                var transactionDb = _transactionService.GetByIdAsync(idTransaction).Result.ToObject<TransactionComplet>();

                if (transactionDb.Stage != StageTransaction.Confirm)
                    throw new RuleException("Estágio fora do padrão");

                var transaction = new Transaction()
                {
                    Id = idTransaction,
                    IdPerson = transactionDb.IdPerson,
                    Stage = StageTransaction.PersonData,
                    TotalValue = transactionDb.TotalValue
                };

                _ = await _transactionService.EditAsync(transaction);
                return _messageReturn;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.Process, this.GetType().Name, nameof(UpdateTransactionPersonDataAsync)), ex);
                throw;
            }
        }

        public async Task<MessageReturn> UpdateTransactionTicketsDataAsync(string idTransaction, StageTicketDataDto transactionDto)
        {
            try
            {
                idTransaction.ValidateIdMongo("idTransaction");
                var transaction = transactionDto.DtoToModel();
                transaction.Id = idTransaction;
                var transactionDb = _transactionService.GetByIdAsync(idTransaction).Result.ToObject<TransactionComplet>();

                if (transactionDb.Stage != StageTransaction.PersonData)
                    throw new RuleException("Estágio fora do padrão");

                transaction.IdPerson = transactionDb.IdPerson;
                transaction.TotalValue = transactionDb.TotalValue;

                _ = await _transactionService.EditAsync(transaction);

                return _messageReturn;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.Process, this.GetType().Name, nameof(UpdateTransactionTicketsDataAsync)), ex);
                throw;
            }
        }

        public async Task<MessageReturn> UpdateTransactionPaymentDataAsync(string idTransaction, PaymentMethod transactionDto)
        {
            try
            {
                idTransaction.ValidateIdMongo("idTransaction");
                var transaction = transactionDto.ToModelTransaction();
                transaction.Id = idTransaction;
                var transactionDb = _transactionService.GetByIdAsync(idTransaction).Result.ToObject<TransactionComplet>();

                if (transactionDb.Stage != StageTransaction.TicketsData &&
                transactionDb.Stage != StageTransaction.PaymentData)
                    throw new RuleException("Estágio fora do padrão");

                transaction.IdPerson = transactionDb.IdPerson;
                transaction.TotalValue = transactionDb.TotalValue;
                transaction.Discount = transactionDb.Discount;
                transaction.Tax = transactionDb.Tax;

                _ = await _transactionService.EditAsync(transaction);

                return _messageReturn;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.Process, this.GetType().Name, nameof(UpdateTransactionPaymentDataAsync)), ex);
                throw;
            }
        }

        public async Task<MessageReturn> PaymentTransactionAsync(string idTransaction)
        {
            try
            {
                idTransaction.ValidateIdMongo("idTransaction");
                var transactionDb = _transactionService.GetByIdAsync(idTransaction).Result.ToObject<TransactionComplet>();

                //if (transactionDb.Stage != StageTransaction.PaymentData)
                //    throw new RuleException("Estágio fora do padrão");

                var transaction = new TransactionCompletDto().ModelToDto(transactionDb);

                await Payment(transaction);
                transaction.Stage = StageTransaction.PaymentTransaction;
                transaction.Status = StatusPayment.Aproved;
                await _transactionService.EditAsync(transaction);
                _messageReturn.Data = "ok";
                return _messageReturn;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.Process, this.GetType().Name, nameof(PaymentTransactionAsync)), ex);
                throw;
            }
        }

        public async Task<MessageReturn> FinishedTransactionAsync(string idTransaction)
        {
            try
            {
                idTransaction.ValidateIdMongo("Transação");
                var transactionDb = _transactionService.GetByIdAsync(idTransaction).Result.ToObject<TransactionComplet>();
                var transaction = new TransactionCompletDto().ModelToDto(transactionDb);


                transaction.TransactionItens = _transactionItenService.GetByIdTransaction(transaction.Id).Result.ToListObject<TransactionIten>();
                foreach (var i in transaction.TransactionItens)
                {
                    var ticketUserDto = await _ticketService.GetTicketByIdDataUserAsync(i.IdTicket);
                    var nameImagem = await _qrCodeService.GenerateQrCode(i.IdTicket);

                    var ticket = new Ticket()
                    {
                        Id = ticketUserDto.Id,
                        IdLot = ticketUserDto.IdLot,
                        IdUser = string.IsNullOrEmpty(ticketUserDto.User.Id) ? null : ticketUserDto.User.Id,
                        IsSold = true,
                        Position = ticketUserDto.Position,
                        Value = ticketUserDto.Value,
                        Status = (int)StatusTicket.Vendido,
                        QrCode = Settings.HostImg + nameImagem
                    };
                    if (!await _ticketService.EditTicketsAsync(ticket))
                        throw new RuleException("Erro ao editar ticket");

                    var ticketEventDto = await _ticketService.GetTicketByIdDataEventAsync(ticketUserDto.Id);
                    await _notificationService.ProcessEmailTicketAsync(ticketUserDto, ticketEventDto, ticket.QrCode, i.HalfPrice);
                }

                transaction.Status = StatusPayment.Finished;
                transaction.Stage = StageTransaction.Finished;
                await _transactionService.EditAsync(transaction);
                return _messageReturn;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.Process, this.GetType().Name, nameof(FinishedTransactionAsync)), ex);
                throw;
            }
        }

        public async Task<MessageReturn> CancelTransactionAsync(string idTransaction)
        {
            try
            {
                idTransaction.ValidateIdMongo("Transação");
                var transactionDb = _transactionService.GetByIdAsync(idTransaction).Result.ToObject<TransactionComplet>();
                var transaction = new TransactionCompletDto().ModelToDto(transactionDb);
                transaction.Status = StatusPayment.Canceled;

                await _transactionService.EditAsync(transaction);
                return _messageReturn;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.Process, this.GetType().Name, nameof(CancelTransactionAsync)), ex);
                throw;
            }
        }

        public async Task<MessageReturn> GeneratePixQRcode(Transaction transaction)
        {
            try
            {
                _messageReturn.Data = string.Empty;
                await _paymentService.Payment(transaction);

                if (_messageReturn.Message != null && _messageReturn.Message.Any())
                    throw new PaymentTransactionException(_messageReturn.Message);

                transaction.Stage = StageTransaction.PaymentTransaction;
                transaction.Status = StatusPayment.Pending;

                await _transactionService.EditAsync(transaction);
                return _messageReturn;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.Process, this.GetType().Name, nameof(GeneratePixQRcode)), ex);
                throw;
            }
        }

        public async Task<MessageReturn> GetStatusPixPaymentAsync(string paymentId)
        {
            try
            {
                MessageReturn objJson = await _paymentService.GetStatusPayment(paymentId);

                if (objJson.Data == null || objJson.Data.ToString() == string.Empty)
                    throw new RuleException("status pagamento não poder ser vazio.");

                _messageReturn.Data = objJson.JsonToModel<PaymentTransactionGetStatus>();

                return _messageReturn;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.Process, this.GetType().Name, nameof(GetStatusPixPaymentAsync)), ex);
                throw;
            }
        }

        private async Task<MessageReturn> Payment(Transaction transaction)
        {
            try
            {
                transaction.Id.ValidateIdMongo("Transação");
                _messageReturn = await _paymentService.Payment(transaction);

                if (_messageReturn.Message != null && _messageReturn.Message.Any())
                    throw new PaymentTransactionException(_messageReturn.Message);


                return _messageReturn;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(MessageLogErrors.Process, this.GetType().Name, nameof(Payment)), ex);
                throw;
            }
        }

        public Task<MessageReturn> GetDataPagbank(string idTransaction)
        {
            try
            {
                idTransaction.ValidateIdMongo("Transação");
                var transaction = _transactionService.GetByIdAsync(idTransaction).Result.ToObject<TransactionComplet>();

                var userResult = _userService.FindByIdAsync(transaction.IdPerson).Result.ToObject<User>();

                RequestDto request = new RequestDto().TransactionToRequest(transaction, userResult);

                _messageReturn.Data = request;

                return Task.FromResult(_messageReturn);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Get, this.GetType().Name, nameof(GetDataPagbank)), idTransaction);
                throw;
            }
        }
    }
}