using Amg_ingressos_aqui_carrinho_api.Dto;
using Amg_ingressos_aqui_carrinho_api.Exceptions;
using Amg_ingressos_aqui_carrinho_api.Infra;
using Amg_ingressos_aqui_carrinho_api.Model;
using Amg_ingressos_aqui_carrinho_api.Repository.Interfaces;
using Amg_ingressos_aqui_carrinho_api.Services.Interfaces;
using Amg_ingressos_aqui_carrinho_api.Utils;
using Amg_ingressos_aqui_carrinho_api.Model.Cielo.Callback;
using Newtonsoft.Json;
using Amg_ingressos_aqui_carrinho_api.Model.Querys;

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

        public TransactionService(
            ITransactionRepository transactionRepository,
            ITransactionItenRepository transactionItenRepository,
            ITicketService ticketService,
            IPaymentService paymentService,
            INotificationService notificationService
        )
        {
            _transactionRepository = transactionRepository;
            _ticketService = ticketService;
            _paymentService = paymentService;
            _transactionItenRepository = transactionItenRepository;
            _messageReturn = new MessageReturn();
            _notificationService = notificationService;
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
                        .Result.Data;
                    var resultEventData = _ticketService
                        .GetTicketByIdDataEventAsync(i.IdTicket)
                        .Result.Data;
                    var ticketUserDto = (TicketUserDataDto)resultUserData;
                    var ticketEventDto = (TicketEventDataDto)resultEventData;
                    var nameImagem = GenerateQrCode(i.IdTicket).Result;
                    var ticket = new Model.Ticket()
                    {
                        Id = ticketUserDto.Id,
                        IdLot = ticketUserDto.IdLot,
                        IdUser = ticketUserDto.User.id,
                        isSold = ticketUserDto.isSold,
                        Position = ticketUserDto.Position,
                        Value = ticketUserDto.Value,
                        Status = (int)Enum.StatusTicket.VENDIDO,
                        QrCode = "https://api.ingressosaqui.com/imagens/" + nameImagem
                    };
                    _ticketService.UpdateTicketsAsync(ticket);
                    ProcessEmail(ticketUserDto, ticketEventDto, ticket.QrCode, i.HalfPrice);
                });

                transaction.Status = Enum.StatusPaymentEnum.Finished;
                transaction.Stage = Enum.StageTransactionEnum.Finished;
                _transactionRepository.Update<object>(transaction);
            }
            catch (IdMongoException ex)
            {
                _messageReturn.Data = string.Empty;
                _messageReturn.Message = ex.Message;
            }
            catch (UpdateTransactionException ex)
            {
                _messageReturn.Data = string.Empty;
                _messageReturn.Message = ex.Message;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return _messageReturn;
        }

        private void ProcessEmail(
            TicketUserDataDto ticketUserDto,
            TicketEventDataDto ticketEventDto,
            string urlQrCode,
            bool halfprice
        )
        {

            var notification = new NotificationEmailTicketDto(){
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
                    TypeTicket = halfprice ? "Meia Entrada" : "Inteira" ,
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
            }
            catch (IdMongoException ex)
            {
                _messageReturn.Data = string.Empty;
                _messageReturn.Message = ex.Message;
            }
            catch (GetByIdTransactionException ex)
            {
                _messageReturn.Data = string.Empty;
                _messageReturn.Message = ex.Message;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return _messageReturn;
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
            }
            catch (IdMongoException ex)
            {
                _messageReturn.Data = string.Empty;
                _messageReturn.Message = ex.Message;
            }
            catch (GetByIdTransactionException ex)
            {
                _messageReturn.Data = string.Empty;
                _messageReturn.Message = ex.Message;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return _messageReturn;
        }

        public async Task<MessageReturn> Payment(Transaction transaction)
        {
            try
            {
                transaction.Id.ValidateIdMongo("Transação");
                _messageReturn = await _paymentService.Payment(transaction);
                if (_messageReturn.Message != null && _messageReturn.Message.Any())
                    throw new PaymentTransactionException(_messageReturn.Message);

                transaction.Stage = Enum.StageTransactionEnum.PaymentTransaction;
                transaction.Status = Enum.StatusPaymentEnum.Aproved;
            }
            catch (IdMongoException ex)
            {
                transaction.Status = Enum.StatusPaymentEnum.ErrorPayment;
                _messageReturn.Data = string.Empty;
                _messageReturn.Message = ex.Message;
            }
            catch (CreditCardNotValidExeption ex)
            {
                transaction.Status = Enum.StatusPaymentEnum.ErrorPayment;
                _messageReturn.Data = string.Empty;
                _messageReturn.Message = ex.Message;
            }
            catch (PaymentTransactionException ex)
            {
                transaction.Status = Enum.StatusPaymentEnum.ErrorPayment;
                _messageReturn.Data = string.Empty;
                _messageReturn.Message = ex.Message;
            }
            catch (Exception ex)
            {
                transaction.Status = Enum.StatusPaymentEnum.ErrorPayment;
                throw ex;
            }
            finally
            {
                UpdateAsync(transaction);
            }

            return _messageReturn;
        }

        public async Task<MessageReturn> GeneratePixQRcode(Transaction transaction)
        {
            try
            {
                _messageReturn = await _paymentService.PaymentCieloPixAsync(transaction);

                if (_messageReturn.Message != null && _messageReturn.Message.Any())
                    throw new PaymentTransactionException(_messageReturn.Message);

                transaction.Stage = Enum.StageTransactionEnum.PaymentTransaction;
                transaction.PaymentPix = _messageReturn.Data as CallbackPix;
                transaction.Status = Enum.StatusPaymentEnum.Pending;

                await UpdateAsync(transaction);
            }
            catch (IdMongoException ex)
            {
                transaction.Status = Enum.StatusPaymentEnum.ErrorPayment;
                _messageReturn.Data = string.Empty;
                _messageReturn.Message = ex.Message;
            }
            catch (CreditCardNotValidExeption ex)
            {
                transaction.Status = Enum.StatusPaymentEnum.ErrorPayment;
                _messageReturn.Data = string.Empty;
                _messageReturn.Message = ex.Message;
            }
            catch (PaymentTransactionException ex)
            {
                transaction.Status = Enum.StatusPaymentEnum.ErrorPayment;
                _messageReturn.Data = string.Empty;
                _messageReturn.Message = ex.Message;
            }
            catch (Exception ex)
            {
                transaction.Status = Enum.StatusPaymentEnum.ErrorPayment;
                throw ex;
            }
            finally
            {
                // UpdateAsync(transaction);
            }

            return _messageReturn;
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

        public async Task<MessageReturn> SaveAsync(TransactionDto transactionDto)
        {
            try
            {
                transactionDto.IdUser.ValidateIdMongo("Usuário");
                var transaction = new Transaction()
                {
                    IdPerson = transactionDto.IdUser,
                    IdEvent = transactionDto.IdEvent,
                    Status = Enum.StatusPaymentEnum.InProgress,
                    Stage = Enum.StageTransactionEnum.Confirm,
                    DateRegister = DateTime.Now,
                    TotalTicket = transactionDto.TotalTicket,
                    TotalValue = transactionDto.TotalValue
                };

                _messageReturn.Data = await _transactionRepository.Save<object>(transaction);
            }
            catch (IdMongoException ex)
            {
                _messageReturn.Data = string.Empty;
                _messageReturn.Message = ex.Message;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return _messageReturn;
        }

        public async Task<MessageReturn> SaveTransactionItenAsync(
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
                            var ticket = listTickets?.FirstOrDefault(i => i.isSold == false);
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
                            ticket.isSold = true;
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
        }

        public async Task<MessageReturn> UpdateAsync(Transaction transaction)
        {
            try
            {
                transaction.Id.ValidateIdMongo("Transação");

                await _transactionRepository.Update<object>(transaction);
            }
            catch (IdMongoException ex)
            {
                _messageReturn.Data = string.Empty;
                _messageReturn.Message = ex.Message;
            }
            catch (UpdateTransactionException ex)
            {
                _messageReturn.Data = string.Empty;
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
            transactionIten.IdTicket.ValidateIdMongo("Ticket");
            transactionIten.IdTransaction.ValidateIdMongo("Transação");

            if (transactionIten.TicketPrice == new decimal(0))
                throw new SaveTransactionException("Valor do Ingresso é obrigatório");
        }
        public async Task<MessageReturn> GetByUserAsync(string idUser)
        {
            try
            {
                idUser.ValidateIdMongo("Usuário");

                _messageReturn.Data = await _transactionRepository.GetByUser<TransactionComplet>(idUser);
            }
            catch (IdMongoException ex)
            {
                _messageReturn.Data = string.Empty;
                _messageReturn.Message = ex.Message;
            }
            catch (GetByPersonTransactionException ex)
            {
                _messageReturn.Data = string.Empty;
                _messageReturn.Message = ex.Message;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return _messageReturn;
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
            }
            catch (IdMongoException ex)
            {
                _messageReturn.Data = string.Empty;
                _messageReturn.Message = ex.Message;
            }
            catch (GetByPersonTransactionException ex)
            {
                _messageReturn.Data = string.Empty;
                _messageReturn.Message = ex.Message;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return _messageReturn;
        }
        
        private async Task<string> GenerateQrCode(string idTicket)
        {
            HttpClient httpClient = new HttpClient();
            var url = "http://api.ingressosaqui.com:3004/";
            var uri = "v1/generate-qr-code?data=" + idTicket;
            using var httpResponseMessage = await httpClient.GetAsync(url + uri);
            string jsonContent = System.Text.Json.JsonSerializer.Deserialize<string>(
                httpResponseMessage.Content.ReadAsStringAsync().Result
            );
            return jsonContent;
        }
        public async Task<MessageReturn> DeleteAsync(string id)
        {
            try
            {
                id.ValidateIdMongo("Transação");
                _messageReturn.Data = await _transactionItenRepository.DeleteByIdTransaction(id);
                _messageReturn.Data = await _transactionRepository.Delete(id);
            }
            catch (IdMongoException ex)
            {
                _messageReturn.Data = string.Empty;
                _messageReturn.Message = ex.Message;
            }
            catch (UpdateTransactionException ex)
            {
                _messageReturn.Data = string.Empty;
                _messageReturn.Message = ex.Message;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return _messageReturn;
        }
        public async Task<MessageReturn> CancelTransaction(Transaction transaction)
        {
            try
            {
                transaction.Status = Enum.StatusPaymentEnum.Canceled;
                _messageReturn.Data = await _transactionRepository.Update<object>(transaction);

            }
            catch (IdMongoException ex)
            {
                _messageReturn.Data = string.Empty;
                _messageReturn.Message = ex.Message;
            }
            catch (UpdateTransactionException ex)
            {
                _messageReturn.Data = string.Empty;
                _messageReturn.Message = ex.Message;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return _messageReturn;

        }

    }
}