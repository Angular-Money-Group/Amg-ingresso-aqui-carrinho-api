using Amg_ingressos_aqui_carrinho_api.Services;
using Amg_ingressos_aqui_carrinho_api.Infra;
using Moq;
using NUnit.Framework;
using Amg_ingressos_aqui_carrinho_api.Services.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;

namespace Amg_ingressos_aqui_carrinho_tests.Services
{
    public class PaymentServiceTest
    {
        private PaymentService _paymentService;
        private readonly Mock<IUserService> _userService = new Mock<IUserService>();
        private readonly Mock<IOptions<PaymentSettings>> _paymentSettings = new Mock<IOptions<PaymentSettings>>();
        private readonly Mock<ILogger<PaymentService>> _loggerMock = new Mock<ILogger<PaymentService>>();
        private readonly Mock<ILogger<CieloClient>> _loggerCieloMock = new Mock<ILogger<CieloClient>>();
        private readonly Mock<ILogger<PagBankClient>> _loggerPagBankMock = new Mock<ILogger<PagBankClient>>();
        private readonly Mock<ILogger<OperatorRest>> _loggerOperatorRest = new Mock<ILogger<OperatorRest>>();

        [SetUp]
        public void SetUp()
        {
            _paymentService = new PaymentService(
                _paymentSettings.Object, 
                _userService.Object,
                _loggerMock.Object,
                _loggerCieloMock.Object,
                _loggerPagBankMock.Object,
                _loggerOperatorRest.Object
                );
        }
    }
}