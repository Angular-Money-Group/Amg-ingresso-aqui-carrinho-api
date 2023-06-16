using System.Text.Json;
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
                var emailUser= string.Empty;

                transaction.TransactionItens.ForEach(i =>
                {
                    var result = _ticketService.GetTicketsByIdAsync(i.IdTicket).Result.Data;
                    var ticketDto = (TicketUserDto)result;
                    emailUser = ticketDto.User.email;
                    var ticket = new Ticket()
                    {
                        Id = ticketDto.Id,
                        IdLot = ticketDto.IdLot,
                        IdUser = ticketDto.IdUser,
                        isSold = ticketDto.isSold,
                        Position = ticketDto.Position,
                        Value = ticketDto.Value
                    };
                    var nameImagem = GenerateQrCode(ticket?.Id).Result;
                    ticket.QrCode = "https://api.ingressosaqui.com/imagens/qrcodes/"+nameImagem;
                    _ticketService.UpdateTicketsAsync(ticket);
                });
                var email = new Email
                {
                    Body = _emailService.GenerateBody(),
                    Subject = "Ingressos",
                    Sender = "suporte@ingressosaqui.com",
                    To = emailUser 
                };
                _ = _emailService.SaveAsync(email);
                
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

        public async Task<MessageReturn> GetByIdAsync(string idTransaction)
        {
            try
            {
                idTransaction.ValidateIdMongo("Transação");

                _messageReturn.Data = await _transactionRepository.GetById(idTransaction);

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
                var resultPayment = await _paymentService.Payment(transaction);
                if (resultPayment.Message != null && resultPayment.Message.Any())
                    throw new PaymentTransactionException(resultPayment.Message);

                transaction.Stage = Enum.StageTransactionEnum.PaymentTransaction;
                transaction.Status = Enum.StatusPaymentEnum.Aproved;
                UpdateAsync(transaction);
                _messageReturn.Data = "Transação Efetivada";

            }
            catch (IdMongoException ex)
            {
                _messageReturn.Data = string.Empty;
                _messageReturn.Message = ex.Message;
            }
            catch (PaymentTransactionException ex)
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

        public async Task<MessageReturn> SaveAsync(TransactionDto transactionDto)
        {
            try
            {
                transactionDto.IdUser.ValidateIdMongo("Usuário");
                var transaction = new Transaction()
                {
                    IdPerson = transactionDto.IdUser,
                    Status = Enum.StatusPaymentEnum.InProgress,
                    Stage = Enum.StageTransactionEnum.Confirm
                };

                _messageReturn.Data = await _transactionRepository.Save<object>(transaction);
                //_messageReturn.Data.ToString().ValidateIdMongo("Transação");
                transaction.Id = (string)_messageReturn.Data;

                var totalTransaction = SaveTransactionItenAsync(_messageReturn?.Data?.ToString(),
                                        transactionDto.IdUser,
                                         transactionDto.TransactionItensDto).Result.Data;
                if (_messageReturn?.Message != null &&
                    !string.IsNullOrEmpty(_messageReturn?.Message?.ToString()))
                    return _messageReturn;
                transaction.TotalValue = Convert.ToDecimal(totalTransaction);
                UpdateAsync(transaction);
                _messageReturn.Data = transaction.Id;
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
                var valueTransaction = new decimal(0);
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

                            var valueTicket = i.HalfPrice == true ? (ticket.Value / 2) : ticket.Value;
                            valueTransaction = valueTransaction + valueTicket;
                            var transactionItem = new TransactionIten()
                            {
                                HalfPrice = i.HalfPrice,
                                IdTransaction = IdTransaction,
                                IdTicket = ticket?.Id,
                                TicketPrice = valueTicket
                            };
                            //cria transaction iten
                            ValidateTransactionIten(transactionItem);
                            _transactionItenRepository.Save<object>(transactionItem);

                            //atualiza Ticket
                            ticket.isSold = true;
                            ticket.IdUser = IdUser;
                            ticket.Value = valueTicket;
                            _ticketService.UpdateTicketsAsync(ticket);
                        }
                    }
                    catch (SaveTransactionException ex)
                    {
                        _messageReturn.Data = string.Empty;
                        throw ex;
                    }
                    catch (System.Exception ex)
                    {
                        _messageReturn.Data = string.Empty;
                        throw ex;
                    }

                });
                _messageReturn.Data = valueTransaction;
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
            //var url = new Uri(@);
            var url = "http://api.ingressosaqui.com:3004/";
            var uri = "v1/generate-qr-code?data=" + idTicket;
            using var httpResponseMessage = await _HttpClient.GetAsync(url + uri);
            string jsonContent =JsonSerializer.Deserialize<string>
                                    ( httpResponseMessage.Content.ReadAsStringAsync().Result);
            return jsonContent;
        }
    }
}