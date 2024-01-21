using Amg_ingressos_aqui_carrinho_api.Consts;
using Amg_ingressos_aqui_carrinho_api.Dto;
using Amg_ingressos_aqui_carrinho_api.Enum;
using Amg_ingressos_aqui_carrinho_api.Exceptions;
using Amg_ingressos_aqui_carrinho_api.Mappers;
using Amg_ingressos_aqui_carrinho_api.Model;
using Amg_ingressos_aqui_carrinho_api.Model.Cielo.Callback;
using Amg_ingressos_aqui_carrinho_api.Model.Querys;
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
        private readonly ITicketService _ticketService;
        private readonly IPaymentService _paymentService;
        private readonly INotificationService _notificationService;
        private readonly IQrCodeService _qrCodeService;
        private readonly ILogger<TransactionService> _logger;

        public CheckoutService(
            ITransactionService transactionService,
            ITransactionItenService transactionItenService,
            ITicketService ticketService,
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
        }

        public async Task<MessageReturn> ProcessSaveAsync(TransactionDto transactionDto)
        {
            try
            {
                transactionDto.IdUser.ValidateIdMongo("Usuário");
                var transactionModel = transactionDto.DtoToModel();
                transactionModel.Status = StatusPaymentEnum.InProgress;
                transactionModel.Stage = StageTransactionEnum.Confirm;

                var listTickets = ReservTicketsAsync(transactionDto);
                await _transactionService.SaveAsync(transactionModel);
                ProcessSaveTransactionItens(transactionModel.Id, transactionDto.TransactionItensDto, listTickets);


                return _messageReturn;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Process, this.GetType().Name, nameof(ProcessSaveAsync)));
                throw;
            }
        }

        public async Task<MessageReturn> UpdateTransactionPersonDataAsync(string idTransaction)
        {
            try
            {

                idTransaction.ValidateIdMongo("idTransaction");
                var transactionService = await _transactionService.GetByIdAsync(idTransaction);
                var transactionDb = (GetTransactionEventData)transactionService.Data;

                if (transactionDb.Stage != StageTransactionEnum.Confirm)
                    throw new RuleException("Estágio fora do padrão");

                var transaction = new Transaction()
                {
                    Id = idTransaction,
                    IdPerson = transactionDb.IdPerson,
                    Stage = StageTransactionEnum.PersonData,
                    TotalValue = transactionDb.TotalValue
                };

                _ = _transactionService.UpdateAsync(transaction);
                return _messageReturn;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Process, this.GetType().Name, nameof(UpdateTransactionPersonDataAsync)));
                throw;
            }
        }

        public async Task<MessageReturn> UpdateTransactionTicketsDataAsync(string idTransaction, StageTicketDataDto transactionDto)
        {
            try
            {
                idTransaction.ValidateIdMongo("idTransaction");
                var transaction = transactionDto.StageTicketDataDtoToTransaction();
                transaction.Id = idTransaction;
                var transactionService = await _transactionService.GetByIdAsync(idTransaction);
                var transactionDb = (GetTransactionEventData)transactionService.Data;

                if (transactionDb.Stage != StageTransactionEnum.PersonData)
                    throw new RuleException("Estágio fora do padrão");

                transaction.IdPerson = transactionDb.IdPerson;
                transaction.TotalValue = transactionDb.TotalValue;

                _ = _transactionService.UpdateAsync(transaction);

                return _messageReturn;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Process, this.GetType().Name, nameof(UpdateTransactionTicketsDataAsync)));
                throw;
            }
        }

        public async Task<MessageReturn> UpdateTransactionPaymentDataAsync(string idTransaction, PaymentMethod transactionDto)
        {
            try
            {
                idTransaction.ValidateIdMongo("idTransaction");
                var transaction = transactionDto.StagePaymentDataDtoToTransaction();
                transaction.Id = idTransaction;
                var transactionService = await _transactionService.GetByIdAsync(idTransaction);
                var transactionDb = (GetTransactionEventData)transactionService.Data;

                if (transactionDb.Stage != StageTransactionEnum.TicketsData &&
                transactionDb.Stage != StageTransactionEnum.PaymentData)
                    throw new RuleException("Estágio fora do padrão");

                transaction.IdPerson = transactionDb.IdPerson;
                transaction.TotalValue = transactionDb.TotalValue;
                transaction.Discount = transactionDb.Discount;
                transaction.Tax = transactionDb.Tax;

                _ = _transactionService.UpdateAsync(transaction);

                return _messageReturn;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Process, this.GetType().Name, nameof(UpdateTransactionTicketsDataAsync)));
                throw;
            }
        }

        public async Task<MessageReturn> PaymentTransactionAsync(string idTransaction)
        {
            try
            {
                idTransaction.ValidateIdMongo("idTransaction");
                var transactionService = await _transactionService.GetByIdAsync(idTransaction);
                var transactionDb = (GetTransactionEventData)transactionService.Data;

                if (transactionDb.Stage != StageTransactionEnum.PaymentData)
                    throw new RuleException("Estágio fora do padrão");

                var transaction = transactionDb.GeTransactionToTransaction();

                var resultPayment = await Payment(transaction);
                transaction.Stage = StageTransactionEnum.PaymentTransaction;
                await _transactionService.UpdateAsync(transaction);
                _messageReturn.Data = "ok";
                return _messageReturn;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Process, this.GetType().Name, nameof(GeneratePixQRcode)));
                throw;
            }
        }

        public async Task<MessageReturn> FinishedTransactionAsync(string idTransaction)
        {
            try
            {
                var transactionService = await _transactionService.GetByIdAsync(idTransaction);
                var transactionDb = (GetTransactionEventData)transactionService.Data;
                var transaction = transactionDb.GeTransactionToTransaction();

                transaction.Id.ValidateIdMongo("Transação");
                transaction.TransactionItens = await _transactionItenService.GetByIdTransaction<TransactionIten>(transaction.Id);
                transaction.TransactionItens.ForEach(async i =>
                {
                    var ticketUserDto = await _ticketService.GetTicketByIdDataUserAsync(i.IdTicket);
                    var nameImagem = await _qrCodeService.GenerateQrCode(i.IdTicket);

                    var ticket = new Ticket()
                    {
                        Id = ticketUserDto.Id,
                        IdLot = ticketUserDto.IdLot,
                        IdUser = ticketUserDto.User.id,
                        IsSold = ticketUserDto.isSold,
                        Position = ticketUserDto.Position,
                        Value = ticketUserDto.Value,
                        Status = (int)StatusTicket.VENDIDO,
                        QrCode = Settings.HostImg + nameImagem
                    };
                    await _ticketService.UpdateTicketsAsync(ticket);
                    await ProcessEmailAsync(ticketUserDto, ticket.QrCode, i.HalfPrice);
                });

                transaction.Status = StatusPaymentEnum.Finished;
                transaction.Stage = StageTransactionEnum.Finished;
                await _transactionService.UpdateAsync(transaction);
                return _messageReturn;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Process, this.GetType().Name, nameof(FinishedTransactionAsync)));
                throw;
            }
        }

        public async Task<MessageReturn> CancelTransactionAsync(string idTransaction)
        {
            try
            {
                idTransaction.ValidateIdMongo("Transação");
                var transactionService = await _transactionService.GetByIdAsync(idTransaction);
                var transactionDb = (GetTransactionEventData)transactionService.Data;
                var transaction = transactionDb.GeTransactionToTransaction();
                transaction.Status = StatusPaymentEnum.Canceled;

                await _transactionService.UpdateAsync(transaction);
                return _messageReturn;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Process, this.GetType().Name, nameof(GeneratePixQRcode)));
                throw;
            }
        }

        public async Task<MessageReturn> GeneratePixQRcode(Transaction transaction)
        {
            try
            {
                _messageReturn = await _paymentService.PaymentCieloPixAsync(transaction);

                if (_messageReturn.Message != null && _messageReturn.Message.Any())
                    throw new PaymentTransactionException(_messageReturn.Message);

                transaction.Stage = StageTransactionEnum.PaymentTransaction;
                transaction.PaymentPix = _messageReturn.Data as CallbackPix;
                transaction.Status = StatusPaymentEnum.Pending;

                await _transactionService.UpdateAsync(transaction);
                return _messageReturn;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Process, this.GetType().Name, nameof(GeneratePixQRcode)));
                throw;
            }
        }

        public async Task<MessageReturn> RefundPaymentPixAsync(string idTransaction, long? amount)
        {
            // try {
            //     idTransaction.ValidateIdMongo("Id Transação");

            //     var transaction = await _transactionRepository.GetById(idTransaction) as List<GetTransaction>;

            //     _messageReturn.Data = _paymentService.RefundPaymentPixAsync(transaction.FirstOrDefault(), amount);
            // }
            // catch (IdMongoException ex)
            // {
            //     _messageReturn.Data = string.Empty;
            //     _messageReturn.Message = ex.Message;
            // }
            // catch (Exception ex)
            // {
            //     _messageReturn.Data = string.Empty;
            //     _messageReturn.Message = ex.Message;
            // }

            return _messageReturn;
        }

        public async Task<MessageReturn> GetStatusPixPaymentAsync(string paymentId)
        {
            try
            {
                var objJson = await _paymentService.GetStatusPayment(paymentId);
                var obj = JsonConvert.DeserializeObject<PaymentTransactionGetStatus>(objJson?.Data?.ToString() ?? throw new RuleException("status pagamento não poder ser vazio."));

                _messageReturn.Data = obj;
                return _messageReturn;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Process, this.GetType().Name, nameof(GetStatusPixPaymentAsync)));
                throw;
            }
        }

        private void ProcessSaveTransactionItens(string idTransaction, List<TransactionItenDto> transactionItensDto, List<Ticket> listTicket)
        {
            try
            {
                idTransaction.ValidateIdMongo("Id Transação");
                transactionItensDto.ForEach(i =>
                {
                    //pra cada compra carimbar o ticket e criar transaction item
                    for (int amount = 0; amount < i.AmountTicket; amount++)
                    {
                        var ticket = listTicket.Find(t => t.IdLot == i.IdLot);
                        var transactionItem = new TransactionIten()
                        {
                            HalfPrice = i.HalfPrice,
                            IdTransaction = idTransaction,
                            IdTicket = ticket.Id,
                            TicketPrice = ticket.Value,
                            Details = i.Details
                        };
                        //cria transaction iten
                        _transactionItenService.Save(transactionItem);
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Process, this.GetType().Name, nameof(ProcessSaveTransactionItens)));
                throw;
            }
        }

        private List<Ticket> ReservTicketsAsync(TransactionDto transactionDto)
        {
            try
            {
                var listTicket = new List<Ticket>();
                transactionDto.TransactionItensDto.ForEach(async i =>
                {

                    //retorna todos tickets para o idLote
                    var lstTickets = _ticketService.GetTicketsByLotAsync(i.IdLot).Result;

                    if (lstTickets != null && lstTickets.Any())
                        throw new RuleException("Erro ao buscar Ingressos");

                    if (lstTickets?.Count == 0 || lstTickets?.Count < i.AmountTicket)
                        throw new SaveException("Número de ingressos inválido");

                    //pra cada compra carimbar o ticket
                    for (int amount = 0; amount < i.AmountTicket; amount++)
                    {
                        var ticket = lstTickets?.Find(i => !i.IsSold) ?? throw new RuleException("ticket não encontrado.");

                        if (ticket?.Value <= 1)
                            throw new SaveException("Valor do Ingresso inválido.");

                        //atualiza Ticket
                        ticket.IdUser = transactionDto.IdUser;
                        ticket.Status = Enum.StatusTicket.RESERVADO;
                        await _ticketService.UpdateTicketsAsync(ticket);
                        listTicket.Add(ticket);
                    }
                });
                return listTicket;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Process, this.GetType().Name, nameof(ProcessSaveTransactionItens)));
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

                transaction.Stage = StageTransactionEnum.PaymentTransaction;
                transaction.Status = StatusPaymentEnum.Aproved;
                return _messageReturn;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Process, this.GetType().Name, nameof(GeneratePixQRcode)));
                throw;
            }
            finally
            {
                await _transactionService.UpdateAsync(transaction);
            }
        }

        private async Task ProcessEmailAsync(TicketUserDataDto ticketUserDto, string urlQrCode, bool halfprice)
        {
            try
            {
                var ticketEventDto = await _ticketService.GetTicketByIdDataEventAsync(ticketUserDto.Id);
                var notification = new NotificationEmailTicketDto()
                {
                    AddressEvent = ticketEventDto.@event.address.addressDescription
                        + " - "
                        + ticketEventDto.@event.address.number
                        + " - "
                        + ticketEventDto.@event.address.neighborhood
                        + " - "
                        + ticketEventDto.@event.address.city
                        + " - "
                        + ticketEventDto.@event.address.state,
                    EndDateEvent = ticketEventDto.@event.endDate.ToString(),
                    EventName = ticketEventDto.@event.name,
                    LocalEvent = ticketEventDto.@event.local,
                    Sender = "suporte@ingressosaqui.com",
                    StartDateEvent = ticketEventDto.@event.startDate.ToString(),
                    Subject = "Ingressos",
                    To = ticketUserDto.User.email,
                    TypeTicket = halfprice ? "Meia Entrada" : "Inteira",
                    UrlQrCode = urlQrCode,
                    UserName = ticketUserDto.User.name,
                    VariantName = ticketEventDto.variant.name,
                };

                _ = _notificationService.SaveAsync(notification);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Process, this.GetType().Name, nameof(FinishedTransactionAsync)));
                throw;
            }
        }

    }
}