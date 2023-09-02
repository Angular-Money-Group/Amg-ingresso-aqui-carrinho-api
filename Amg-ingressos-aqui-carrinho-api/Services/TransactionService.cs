using System.Text.Json;
using System;
using Amg_ingressos_aqui_carrinho_api.Dto;
using Amg_ingressos_aqui_carrinho_api.Exceptions;
using Amg_ingressos_aqui_carrinho_api.Infra;
using Amg_ingressos_aqui_carrinho_api.Model;
using Amg_ingressos_aqui_carrinho_api.Repository.Interfaces;
using Amg_ingressos_aqui_carrinho_api.Services.Interfaces;
using Amg_ingressos_aqui_carrinho_api.Utils;

namespace Amg_ingressos_aqui_carrinho_api.Services
{
    public class TransactionService : ITransactionService
    {
        private ITransactionRepository _transactionRepository;
        private ITransactionItenRepository _transactionItenRepository;
        private MessageReturn _messageReturn;
        private ITicketService _ticketService;
        private IPaymentService _paymentService;
        private IEmailService _emailService;
        private HttpClient _HttpClient;

        public TransactionService(
            ITransactionRepository transactionRepository,
            ITransactionItenRepository transactionItenRepository,
            ITicketService ticketService,
            IPaymentService paymentService,
            ICieloClient cieloClient,
            IEmailService emailService)
        {
            _transactionRepository = transactionRepository;
            _ticketService = ticketService;
            _paymentService = paymentService;
            _transactionItenRepository = transactionItenRepository;
            _HttpClient = cieloClient.CreateClient();
            _messageReturn = new MessageReturn();
            _emailService = emailService;
        }

        public async Task<MessageReturn> FinishedTransactionAsync(Transaction transaction)
        {
            try
            {
                transaction.Id.ValidateIdMongo("Transação");
                transaction.TransactionItens = (List<TransactionIten>)_transactionItenRepository.GetByIdTransaction(transaction.Id).Result;
                transaction.TransactionItens.ForEach(i =>
                {
                    var resultUserData = _ticketService.GetTicketByIdDataUserAsync(i.IdTicket).Result.Data;
                    var resultEventData = _ticketService.GetTicketByIdDataEventAsync(i.IdTicket).Result.Data;
                    var ticketUserDto = (TicketUserDataDto)resultUserData;
                    var ticketEventDto = (TicketEventDataDto)resultEventData;
                    var nameImagem = GenerateQrCode(i.IdTicket).Result;
                    var ticket = new Ticket()
                    {
                        Id = ticketUserDto.Id,
                        IdLot = ticketUserDto.IdLot,
                        IdUser = ticketUserDto.User._id,
                        isSold = ticketUserDto.isSold,
                        Position = ticketUserDto.Position,
                        Value = ticketUserDto.Value,
                        QrCode = "https://api.ingressosaqui.com/imagens/" + nameImagem
                    };
                    _ticketService.UpdateTicketsAsync(ticket);
                    ProcessEmail(ticketUserDto,ticketEventDto,ticket.QrCode, i.HalfPrice);
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

        private void ProcessEmail(TicketUserDataDto ticketUserDto,TicketEventDataDto ticketEventDto, string urlQrCode, bool halfprice)
        {
            var email = new Email
            {
                Body = _emailService.GenerateBody(),
                Subject = "Ingressos",
                Sender = "suporte@ingressosaqui.com",
                To = ticketUserDto.User.email,
                DataCadastro = DateTime.Now
            };
            //alterar pra urlQrCode 
            email.Body = email.Body.Replace("{nome_usuario}",ticketUserDto.User.name);
            email.Body = email.Body.Replace("{nome_evento}",ticketEventDto.@event.name);
            email.Body = email.Body.Replace("{data_evento}",ticketEventDto.@event.startDate +" as "+ ticketEventDto.@event.endDate);
            email.Body = email.Body.Replace("{local_evento}",ticketEventDto.@event.local);
            email.Body = email.Body.Replace("{endereco_evento}",ticketEventDto.@event.address.addressDescription 
                + " - "+ ticketEventDto.@event.address.number
                + " - "+ ticketEventDto.@event.address.neighborhood
                + " - "+ ticketEventDto.@event.address.city
                + " - "+ ticketEventDto.@event.address.state);
            email.Body = email.Body.Replace("{area_evento}",ticketEventDto.variant.name);
            email.Body = email.Body.Replace("{tipo_ingresso}", halfprice? "Meia Entrada":"Inteira");
            email.Body = email.Body.Replace("{qr_code}", urlQrCode);
            
            
            _ = _emailService.SaveAsync(email);
            _ = _emailService.Send(email.id);
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
            finally{
                UpdateAsync(transaction);
            }

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
                    DateRegister= DateTime.Now,
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

        public async Task<MessageReturn> SaveTransactionItenAsync(string IdTransaction, string IdUser, List<TransactionItenDto> transactionItens)
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

                        var listTickets = (List<Ticket>)messageTicket.Data;
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
                                Details= i.Details
                                
                            };
                            //cria transaction iten
                            ValidateTransactionIten(transactionItem);
                            _transactionItenRepository.Save<object>(transactionItem);

                            //atualiza Ticket
                            ticket.isSold = true;
                            ticket.IdUser = IdUser;
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

                _messageReturn.Data = await _transactionRepository.GetByUser(idUser);

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
            var url = "http://api.ingressosaqui.com:3004/";
            var uri = "v1/generate-qr-code?data=" + idTicket;
            using var httpResponseMessage = await _HttpClient.GetAsync(url + uri);
            string jsonContent =JsonSerializer.Deserialize<string>
                                    ( httpResponseMessage.Content.ReadAsStringAsync().Result);
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
    }
}