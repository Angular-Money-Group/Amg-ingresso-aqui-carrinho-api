using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Amg_ingressos_aqui_carrinho_api.Infra;
using Amg_ingressos_aqui_carrinho_api.Model;
using Amg_ingressos_aqui_carrinho_api.Model.Cielo.CreditCard;
using Amg_ingressos_aqui_carrinho_api.Model.Cielo.DebitCard;
using Amg_ingressos_aqui_carrinho_api.Services.Interfaces;
using static System.Net.Mime.MediaTypeNames;

namespace Amg_ingressos_aqui_carrinho_api.Services
{
    public class PaymentService : IPaymentService
    {
        private MessageReturn _messageReturn;
        private HttpClient _HttpClient;
        public PaymentService(ICieloClient cieloClient)
        {
            _HttpClient = cieloClient.CreateClient();
            _messageReturn = new Model.MessageReturn();
        }
        public async Task<MessageReturn> Payment(Transaction transaction)
        {
            StringContent transactionJson;
            if (transaction.PaymentMethod.TypePayment == Enum.TypePaymentEnum.CreditCard)
            {
                var transactionToJson = new PaymentCieloCreditCard()
                {
                    MerchantOrderId = transaction.Id,
                    Payment = new Model.Cielo.CreditCard.Payment()
                    {
                        Amount = 500,//(int)transaction.TransactionItens.Sum(i => i.TicketPrice),
                        CreditCard = new CreditCard()
                        {
                            Brand = transaction.PaymentMethod.Brand,
                            CardNumber = transaction.PaymentMethod.CardNumber,
                            ExpirationDate = transaction.PaymentMethod.ExpirationDate,
                            Holder = transaction.PaymentMethod.Holder,
                            SecurityCode = transaction.PaymentMethod.SecurityCode
                        },
                        Type = transaction.PaymentMethod.TypePayment.ToString(),
                        Installments = 1,
                        SoftDescriptor = "Tiquetera"
                    }
                };
                transactionJson = new StringContent(JsonSerializer.Serialize(transactionToJson), Encoding.UTF8, Application.Json);
            }
            else if (transaction.PaymentMethod.TypePayment == Enum.TypePaymentEnum.DebitCard)
            {
                var transactionToJson = new PaymentCieloDebitCard()
                {
                    MerchantOrderId = transaction.Id,
                    Customer = new Customer() { Name = "usuario sobrenome" },
                    Payment = new Model.Cielo.DebitCard.Payment()
                    {
                        Amount = (int)transaction.TransactionItens.Sum(i => i.TicketPrice),
                        DebitCard = new DebitCard()
                        {
                            Brand = transaction.PaymentMethod.Brand,
                            CardNumber = transaction.PaymentMethod.CardNumber,
                            ExpirationDate = transaction.PaymentMethod.ExpirationDate,
                            Holder = transaction.PaymentMethod.Holder,
                            SecurityCode = transaction.PaymentMethod.SecurityCode
                        },
                        Type = transaction.PaymentMethod.TypePayment.ToString(),
                        Provider = "Simulado",
                        ReturnUrl = "retorno url"
                    }
                };
                transactionJson = new StringContent(JsonSerializer.Serialize(transactionToJson), Encoding.UTF8, Application.Json);
            }
            else
                throw new Exception("Tipo nao mapeado");

            //var transactionJson = new StringContent(JsonSerializer.Serialize(transaction),
            //Encoding.UTF8, Application.Json); // using static System.Net.Mime.MediaTypeNames;
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, "https://apisandbox.cieloecommerce.cielo.com.br/1/sales");
            requestMessage.Headers.Add("Accept", "*/*");
            requestMessage.Headers.Add("Accept-Encoding", "gzip, deflate, br");
            requestMessage.Headers.Add("Connection", "keep-alive");
            requestMessage.Headers.Add("MerchantId", "e3169e28-d8e5-4f90-8db2-3c54af7d361a");
            requestMessage.Headers.Add("MerchantKey", "GNOWRLOKZITAJZTNKLDTZNEAUDHFQTRCTAKMWQEP");
            requestMessage.Content = transactionJson;

            //_HttpClient.DefaultRequestHeaders.Add("Accept", "*/*");
            //_HttpClient.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");
            //_HttpClient.DefaultRequestHeaders.Add("Connection", "keep-alive");
            //_HttpClient.DefaultRequestHeaders.Add("MerchantId", "e3169e28-d8e5-4f90-8db2-3c54af7d361a");
            //_HttpClient.DefaultRequestHeaders.Add("MerchantKey", "GNOWRLOKZITAJZTNKLDTZNEAUDHFQTRCTAKMWQEP");

            using var httpResponseMessage = 
             await _HttpClient.SendAsync(requestMessage);
                //await _HttpClient.PostAsync("https://apisandbox.cieloecommerce.cielo.com.br/1/sales",
                 //transactionJson);
            
            
            string jsonContent = httpResponseMessage.Content.ReadAsStringAsync().Result;
            var result = httpResponseMessage.EnsureSuccessStatusCode();
            _messageReturn.Data = "Ticket criado";

            return _messageReturn;
        }
    }
}