using System.Text;
using System.Text.Json;
using Amg_ingressos_aqui_carrinho_api.Dtos;
using Amg_ingressos_aqui_carrinho_api.Exceptions;
using Amg_ingressos_aqui_carrinho_api.Infra;
using Amg_ingressos_aqui_carrinho_api.Model;
using Amg_ingressos_aqui_carrinho_api.Repository.Interfaces;
using Amg_ingressos_aqui_carrinho_api.Services.Interfaces;
using Amg_ingressos_aqui_carrinho_api.Utils;
using Amg_ingressos_aqui_eventos_api.Model;
using Newtonsoft.Json;
using static System.Net.Mime.MediaTypeNames;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Amg_ingressos_aqui_carrinho_api.Services
{
    public class TransactionService : ITransactionService
    {
        private ITransactionRepository _transactionRepository;
        private MessageReturn _messageReturn;
        private HttpClient _HttpClient;

        public TransactionService(ITransactionRepository transactionRepository,
            ICieloClient cieloClient)
        {
            _transactionRepository = transactionRepository;
            _HttpClient = cieloClient.CreateClient();
            _messageReturn = new MessageReturn();
        }

        public async Task<MessageReturn> SaveAsync(TransactionDto transactionDto)
        {
            try
            {
                transactionDto.IdCustomer.ValidateIdMongo("Usuário");
                var transaction = new Transaction()
                {
                    IdPerson = transactionDto.IdCustomer,
                    Status = Enum.StatusPayment.InProgress,
                };

                _messageReturn.Data = await _transactionRepository.Save<object>(transaction);

                await SaveTransactionItenAsync(_messageReturn.Data.ToString(),
                                        transactionDto.IdCustomer,
                                         transactionDto.TransactionItensDto);
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

        public async Task<MessageReturn> SaveTransactionItenAsync(string IdTransaction,string IdCustomer, List<TransactionItenDto> transactionItens)
        {
            try
            {
                IdTransaction.ValidateIdMongo("Id Transação");
                transactionItens.ForEach(async i =>
                {
                    //retorna todos tickets para o idLote
                    var listTickets = GetTicketsAsync(i.IdLot);

                    //pra cada compra carimbar o ticket e criar transactio item
                    for (int amount = 0; amount < i.AmountTicket; amount++)
                    {
                        var ticket = listTickets.Result.FirstOrDefault(i => i.isSold == false);
                        var valueTicket = i.HalfPrice == true ? (ticket.Value / 2) : ticket.Value;
                        var transactionItem = new TransactionIten()
                        {
                            HalfPrice = i.HalfPrice,
                            IdTransaction = IdTransaction,
                            IdTicket = ticket.Id,
                            TicketPrice = valueTicket
                        };
                        //cria transaction iten
                        ValidateTransactionIten(transactionItem);
                        _transactionRepository.SaveTransactionIten<object>(transactionItem);

                        //atualiza Ticket
                        ticket.isSold = true;
                        ticket.IdUser = IdCustomer;
                        ticket.Value = valueTicket;
                        UpdateTicketsAsync(ticket);
                    }
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
        public async Task<MessageReturn> Payment(Transaction transaction)
        {

            var transactionJson = new StringContent(JsonSerializer.Serialize(transaction),
            Encoding.UTF8, Application.Json); // using static System.Net.Mime.MediaTypeNames;

            using var httpResponseMessage =
                await _HttpClient.PostAsync("https://apisandbox.cieloecommerce.cielo.com.br/1/sales",
                 transactionJson);

            var result = httpResponseMessage.EnsureSuccessStatusCode();
            _messageReturn.Data = "Ticket criado";

            return _messageReturn;
        }

        private void ValidateTransactionIten(TransactionIten transactionIten)
        {
            transactionIten.IdTicket.ValidateIdMongo("Ticket");
            transactionIten.IdTransaction.ValidateIdMongo("Transação");

            if (transactionIten.TicketPrice == new decimal(0))
                throw new SaveTransactionException("Valor do Ingresso é obrigatório");
        }

        private async Task<List<Ticket>> GetTicketsAsync(string idLote)
        {
            var url = new Uri("api.ingressosaqui.com.br:3002/v1/eventos/lote/" + idLote);
            using var httpResponseMessage = await _HttpClient.GetAsync(url);
            var result = httpResponseMessage.EnsureSuccessStatusCode();
            string jsonContent = result.Content.ReadAsStringAsync().Result;

            return JsonConvert.DeserializeObject<List<Ticket>>(jsonContent);
        }
        private async Task UpdateTicketsAsync(Ticket ticket)
        {
            var transactionJson = new StringContent(JsonSerializer.Serialize(ticket),
            Encoding.UTF8, Application.Json); // using static System.Net.Mime.MediaTypeNames;

            using var httpResponseMessage =
                await _HttpClient.PostAsync("api.ingressosaqui.com.br:3002/v1/eventos/lote/" + ticket.Id,
                 transactionJson);

            var result = httpResponseMessage.EnsureSuccessStatusCode();
        }
    }
}