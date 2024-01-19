using Amg_ingressos_aqui_carrinho_api.Dto;
using Amg_ingressos_aqui_carrinho_api.Exceptions;
using Amg_ingressos_aqui_carrinho_api.Model;
using Amg_ingressos_aqui_carrinho_api.Repository.Interfaces;
using Amg_ingressos_aqui_carrinho_api.Services.Interfaces;
using Amg_ingressos_aqui_carrinho_api.Utils;
using Amg_ingressos_aqui_carrinho_api.Model.Cielo.Callback;
using Newtonsoft.Json;
using Amg_ingressos_aqui_carrinho_api.Enum;
using Amg_ingressos_aqui_carrinho_api.Consts;
using System.Data;

namespace Amg_ingressos_aqui_carrinho_api.Services
{
    public class TransactionService : ITransactionService
    {
        private ITransactionRepository _transactionRepository;
        private ITransactionItenRepository _transactionItenRepository;
        private MessageReturn _messageReturn;
        private ITicketService _ticketService;
        private IPaymentService _paymentService;
        private INotificationService _notificationService;
        private ILogger<TransactionService> _logger;

        public TransactionService(
            ITransactionRepository transactionRepository,
            ITransactionItenRepository transactionItenRepository,
            ITicketService ticketService,
            IPaymentService paymentService,
            INotificationService notificationService,
            ILogger<TransactionService> logger
        )
        {
            _transactionRepository = transactionRepository;
            _ticketService = ticketService;
            _paymentService = paymentService;
            _transactionItenRepository = transactionItenRepository;
            _messageReturn = new MessageReturn();
            _notificationService = notificationService;
            _logger = logger;
        }

        public async Task<MessageReturn> FinishedTransactionAsync(Transaction transaction)
        {
            try
            {
                transaction.Id.ValidateIdMongo("Transação");
                transaction.TransactionItens =
                    (List<TransactionIten>)
                        _transactionItenRepository.GetByIdTransaction(transaction.Id).Result;
                transaction.TransactionItens.ForEach(i =>
                {
                    var resultUserData = _ticketService
                        .GetTicketByIdDataUserAsync(i.IdTicket)
                        .Result;
                    var resultEventData = _ticketService
                        .GetTicketByIdDataEventAsync(i.IdTicket)
                        .Result;
                    var ticketUserDto = resultUserData;
                    var ticketEventDto = resultEventData;
                    var nameImagem = GenerateQrCode(i.IdTicket).Result;
                    var ticket = new Ticket()
                    {
                        Id = ticketUserDto.Id,
                        IdLot = ticketUserDto.IdLot,
                        IdUser = ticketUserDto.User.id,
                        IsSold = ticketUserDto.isSold,
                        Position = ticketUserDto.Position,
                        Value = ticketUserDto.Value,
                        Status = (int)StatusTicket.VENDIDO,
                        QrCode = "https://api.ingressosaqui.com/imagens/" + nameImagem
                    };
                    _ticketService.UpdateTicketsAsync(ticket);
                    ProcessEmail(ticketUserDto, ticketEventDto, ticket.QrCode, i.HalfPrice);
                });

                transaction.Status = StatusPaymentEnum.Finished;
                transaction.Stage = StageTransactionEnum.Finished;
                _transactionRepository.Update<object>(transaction);
                return _messageReturn;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Process, this.GetType().Name, nameof(FinishedTransactionAsync)));
                throw;
            }
        }

        private void ProcessEmail(TicketUserDataDto ticketUserDto, TicketEventDataDto ticketEventDto, string urlQrCode, bool halfprice)
        {

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

        public async Task<MessageReturn> GetByIdAsync(string id)
        {
            try
            {
                id.ValidateIdMongo("Transação");

                _messageReturn.Data = await _transactionRepository.GetById(id);
                return _messageReturn;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Process, this.GetType().Name, nameof(GetStatusPixPaymentAsync)));
                throw;
            }
        }

        public async Task<MessageReturn> GetStatusPixPaymentAsync(string paymentId)
        {
            try
            {
                var objJson = _paymentService.GetStatusPayment(paymentId);
                var obj = JsonConvert.DeserializeObject<PaymentTransactionGetStatus>(
                    objJson
                );

                _messageReturn.Data = obj;
                return _messageReturn;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Process, this.GetType().Name, nameof(GetStatusPixPaymentAsync)));
                throw;
            }
        }

        public async Task<MessageReturn> Payment(Transaction transaction)
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
                UpdateAsync(transaction);
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

                await UpdateAsync(transaction);
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

        public async Task<MessageReturn> ProcessSaveAsync(TransactionDto transactionDto)
        {
            try
            {
                transactionDto.IdUser.ValidateIdMongo("Usuário");
                var transactionModel = transactionDto.DtoToModel();
                transactionModel.Status = StatusPaymentEnum.InProgress;
                transactionModel.Stage = StageTransactionEnum.Confirm;

                var listTickets = ProcessTicketsAsync(transactionDto);
                await SaveAsync(transactionModel);
                ProcessSaveTransactionItens(transactionModel.Id, transactionDto.TransactionItensDto, listTickets);


                return _messageReturn;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Process, this.GetType().Name, nameof(ProcessSaveAsync)));
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
                    try
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
                            ValidateTransactionIten(transactionItem);
                            _transactionItenRepository.Save<object>(transactionItem);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, string.Format(MessageLogErrors.Process, this.GetType().Name, nameof(ProcessSaveTransactionItens)));
                        throw;
                    }

                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Process, this.GetType().Name, nameof(ProcessSaveTransactionItens)));
                throw;
            }
        }

        private List<Ticket> ProcessTicketsAsync(TransactionDto transactionDto)
        {
            try
            {
                var listTicket = new List<Ticket>();
                transactionDto.TransactionItensDto.ForEach(i =>
                {
                    try
                    {
                        //retorna todos tickets para o idLote
                        var lstTickets = _ticketService.GetTicketsByLotAsync(i.IdLot).Result;

                        if (lstTickets != null && lstTickets.Any())
                            throw new RuleException("Erro ao buscar Ingressos");

                        if (lstTickets?.Count == 0 || lstTickets?.Count < i.AmountTicket)
                            throw new SaveTransactionException("Número de ingressos inválido");

                        //pra cada compra carimbar o ticket
                        for (int amount = 0; amount < i.AmountTicket; amount++)
                        {
                            var ticket = lstTickets?.FirstOrDefault(i => !i.IsSold) ?? throw new RuleException("ticket não encontrado.");
                            if (ticket?.Value <= 1)
                                throw new SaveTransactionException("Valor do Ingresso inválido.");

                            //atualiza Ticket
                            ticket.IdUser = transactionDto.IdUser;
                            ticket.Status = Enum.StatusTicket.RESERVADO;
                            _ticketService.UpdateTicketsAsync(ticket);
                            listTicket.Add(ticket);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, string.Format(MessageLogErrors.Process, this.GetType().Name, nameof(ProcessSaveTransactionItens)));
                        throw;
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

        private async Task SaveAsync(Transaction transactionModel)
        {
            try
            {
                _messageReturn.Data = await _transactionRepository.Save<object>(transactionModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Process, this.GetType().Name, nameof(ProcessSaveAsync)));
                throw;
            }
        }

        /*public async Task<MessageReturn> SaveTransactionItenAsync(
            string IdTransaction,
            string IdUser,
            List<TransactionItenDto> transactionItens
        )
        {
            try
            {
                IdTransaction.ValidateIdMongo("Id Transação");
                transactionItens.ForEach(i =>
                {
                    try
                    {
                        //retorna todos tickets para o idLote
                        var messageTicket = _ticketService.GetTicketsByLotAsync(i.IdLot).Result;

                        if (messageTicket.Message != null && messageTicket.Message.Any())
                            throw new Exception("Erro ao buscar Ingressos");

                        var listTickets = (List<Model.Ticket>)messageTicket.Data;
                        if (listTickets?.Count == 0 || listTickets?.Count < i.AmountTicket)
                            throw new SaveTransactionException("Número de ingressos inválido");

                        //pra cada compra carimbar o ticket e criar transaction item
                        for (int amount = 0; amount < i.AmountTicket; amount++)
                        {
                            var ticket = listTickets?.FirstOrDefault(i => i.IsSold == false);
                            if (ticket.Value == 0)
                                throw new SaveTransactionException("Valor do Ingresso inválido.");

                            var transactionItem = new TransactionIten()
                            {
                                HalfPrice = i.HalfPrice,
                                IdTransaction = IdTransaction,
                                IdTicket = ticket.Id,
                                TicketPrice = ticket.Value,
                                Details = i.Details
                            };
                            //cria transaction iten
                            ValidateTransactionIten(transactionItem);
                            _transactionItenRepository.Save<object>(transactionItem);

                            //atualiza Ticket
                            ticket.IsSold = true;
                            ticket.IdUser = IdUser;
                            ticket.Status = (int)Enum.StatusTicket.VENDIDO;
                            _ticketService.UpdateTicketsAsync(ticket);
                        }
                    }
                    catch (SaveTransactionException ex)
                    {
                        _transactionItenRepository.DeleteByIdTransaction(IdTransaction);
                        _transactionRepository.Delete(IdTransaction);
                        _messageReturn.Data = string.Empty;
                        throw ex;
                    }
                    catch (System.Exception ex)
                    {
                        _transactionItenRepository.DeleteByIdTransaction(IdTransaction);
                        _transactionRepository.Delete(IdTransaction);
                        _messageReturn.Data = string.Empty;
                        throw ex;
                    }
                });
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
        }*/

        public async Task<MessageReturn> UpdateAsync(Transaction transaction)
        {
            try
            {
                transaction.Id.ValidateIdMongo("Transação");
                await _transactionRepository.Update<object>(transaction);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Process, this.GetType().Name, nameof(UpdateAsync)));
                throw;
            }

            return _messageReturn;
        }
        private void ValidateTransactionIten(TransactionIten transactionIten)
        {
            transactionIten.IdTicket.ValidateIdMongo("Ticket");
            transactionIten.IdTransaction.ValidateIdMongo("Transação");

            if (transactionIten.TicketPrice == new decimal(0))
                throw new SaveTransactionException("Valor do Ingresso é obrigatório");
        }

        public async Task<MessageReturn> GetByUserActivesAsync(string idUser)
        {
            try
            {
                idUser.ValidateIdMongo("Usuário");
                var data = await _transactionRepository.GetByUserEventData<TransactionComplet>(idUser);
                var listActives = data.Where(x => x.Events.FirstOrDefault().StartDate >= DateTime.Now).Select(t => t);
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
                var listActives = data.Where(x => x.Events.FirstOrDefault().StartDate < DateTime.Now).Select(t => t);
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
                _logger.LogError(ex, string.Format(MessageLogErrors.Process, this.GetType().Name, nameof(GetByUserActivesAsync)));
                throw;
            }
        }

        private async Task<string> GenerateQrCode(string idTicket)
        {
            HttpClient httpClient = new HttpClient();
            var url = "http://api.ingressosaqui.com:3004/";
            var uri = "v1/generate-qr-code?data=" + idTicket;
            using var httpResponseMessage = await httpClient.GetAsync(url + uri);
            string jsonContent = System.Text.Json.JsonSerializer.Deserialize<string>(
                httpResponseMessage.Content.ReadAsStringAsync().Result
            ) ?? string.Empty;
            return jsonContent;
        }

        public async Task<MessageReturn> DeleteAsync(string id)
        {
            try
            {
                id.ValidateIdMongo("Transação");
                _messageReturn.Data = await _transactionItenRepository.DeleteByIdTransaction(id);
                _messageReturn.Data = await _transactionRepository.Delete(id);
                return _messageReturn;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Process, this.GetType().Name, nameof(GetByUserActivesAsync)));
                throw;
            }
        }

        public async Task<MessageReturn> CancelTransaction(Transaction transaction)
        {
            try
            {
                transaction.Status = Enum.StatusPaymentEnum.Canceled;
                _messageReturn.Data = await _transactionRepository.Update<object>(transaction);

                return _messageReturn;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Process, this.GetType().Name, nameof(GetByUserActivesAsync)));
                throw;
            }
        }
    }
}