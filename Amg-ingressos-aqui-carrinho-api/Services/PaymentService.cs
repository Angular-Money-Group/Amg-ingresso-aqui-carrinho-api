using Amg_ingressos_aqui_carrinho_api.Infra;
using Amg_ingressos_aqui_carrinho_api.Model;
using Amg_ingressos_aqui_carrinho_api.Services.Interfaces;
using Amg_ingressos_aqui_carrinho_api.Exceptions;
using Microsoft.Extensions.Options;
using Amg_ingressos_aqui_carrinho_api.Consts;

namespace Amg_ingressos_aqui_carrinho_api.Services
{
    public class PaymentService : IPaymentService
    {
        private MessageReturn _messageReturn;
        private readonly IUserService _userService;
        private readonly ITransactionGatewayClient transactionClient;
        private readonly ILogger<PaymentService> _logger;

        public PaymentService(IOptions<PaymentSettings> transactionDatabaseSettings, IUserService userService, ILogger<PaymentService> logger)
        {
            if (transactionDatabaseSettings.Value.Key.Equals("PAGBANK"))
            {
                transactionClient = new PagBankClient(transactionDatabaseSettings);
            }
            else
                transactionClient = new CieloClient(transactionDatabaseSettings);

            _userService = userService;
            _messageReturn = new Model.MessageReturn();
            _logger = logger;
        }

        public async Task<MessageReturn> Payment(Transaction transaction)
        {
            var userResult = _userService.FindByIdAsync(transaction.IdPerson).Result;
            if (userResult.Message != null && userResult.Message.Any())
                throw new ExternalServiceException(userResult.Message);

            User user = userResult.ToObject<User>();
            try
            {
                if (transaction.PaymentMethod.TypePayment == Enum.TypePayment.CreditCard)
                    _messageReturn = await transactionClient.PaymentCreditCard(transaction, user);
                else if (transaction.PaymentMethod.TypePayment == Enum.TypePayment.DebitCard)
                    _messageReturn = await transactionClient.PaymentDebitCard(transaction, user);
                else if (transaction.PaymentMethod.TypePayment == Enum.TypePayment.PaymentSlip)
                    _messageReturn = await transactionClient.PaymentSlip(transaction, user);
                else if (transaction.PaymentMethod.TypePayment == Enum.TypePayment.Pix)
                    _messageReturn = await transactionClient.PaymentPix(transaction, user);
                else
                    throw new RuleException("Tipo não mapeado");

                return _messageReturn;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Process, this.GetType().Name, nameof(Payment)));
                throw;
            }
        }

        public async Task<MessageReturn> GetStatusPayment(string paymentId)
        {
            try
            {
                _messageReturn.Data = await transactionClient.GetStatusPayment(paymentId);
                return _messageReturn;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, string.Format(MessageLogErrors.Process, this.GetType().Name, nameof(GetStatusPayment)));
                throw;
            }
        }
    }
}