using System.Text;
using System.Text.Json;
using Amg_ingressos_aqui_carrinho_api.Exceptions;
using Amg_ingressos_aqui_carrinho_api.Infra;
using Amg_ingressos_aqui_carrinho_api.Model;
using Amg_ingressos_aqui_carrinho_api.Repository.Interfaces;
using Amg_ingressos_aqui_carrinho_api.Services.Interfaces;
using Amg_ingressos_aqui_carrinho_api.Utils;
using static System.Net.Mime.MediaTypeNames;

namespace Amg_ingressos_aqui_carrinho_api.Services
{
    public class TransactionPaymentService : ITransactionPaymentService
    {
        private ITransactionPaymentRepository _transactionPaymentRepository;
        private HttpClient _HttpClient;
        private MessageReturn _messageReturn;

        public TransactionPaymentService(
            ITransactionPaymentRepository transactionRepository,
            ICieloClient cieloClient)
        {
            _transactionPaymentRepository = transactionRepository;
            _HttpClient = cieloClient.CreateClient();
            _messageReturn = new MessageReturn();
        }

        public async Task<MessageReturn> SaveAsync(TransactionPayment transaction)
        {
            try
            {
                ValidateTransactionPayment(transaction);
                await _transactionPaymentRepository.Save<object>(transaction);
                _messageReturn.Data = "Transação criada";
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

        public async Task<MessageReturn> Payment(TransactionPayment transaction)
        {

                var transactionJson = new StringContent(
                JsonSerializer.Serialize(transaction),
                Encoding.UTF8,
                Application.Json); // using static System.Net.Mime.MediaTypeNames;

                using var httpResponseMessage =
                    await _HttpClient.PostAsync("https://apisandbox.cieloecommerce.cielo.com.br/1/sales",
                     transactionJson);

                 var result = httpResponseMessage.EnsureSuccessStatusCode();
                _messageReturn.Data = "Ticket criado";

                return _messageReturn;
        }

        private void ValidateTransactionPayment(TransactionPayment transactionPayment)
        {
            transactionPayment.IdTransaction.ValidateIdMongo("Id Transação");
            transactionPayment.IdPaymentMethod.ValidateIdMongo("Id Meio Pagamento");

            if (transactionPayment.TotalValue == new decimal(0))
                throw new SaveTransactionException("Valor total é obrigatório");
        }
    }
}