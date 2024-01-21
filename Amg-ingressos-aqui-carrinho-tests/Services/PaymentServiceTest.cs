using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amg_ingressos_aqui_carrinho_api.Services;
using Amg_ingressos_aqui_carrinho_api.Infra;
using Moq;
using NUnit.Framework;
using Amg_ingressos_aqui_carrinho_tests.FactoryServices;
using Amg_ingressos_aqui_carrinho_api.Services.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Amg_ingressos_aqui_carrinho_api.Model;

namespace Amg_ingressos_aqui_carrinho_tests.Services
{
    public class PaymentServiceTest
    {
        private PaymentService _paymentService;
        private readonly UserService _userService;
        private readonly Mock<ITransactionGatewayClient> _cieloClienteMock = new Mock<ITransactionGatewayClient>();
        private readonly Mock<IOptions<PaymentSettings>> _paymentSettings = new Mock<IOptions<PaymentSettings>>();
        private readonly Mock<IUserService> _userServiceMock = new Mock<IUserService>();
        private readonly Mock<ILogger<PaymentService>> _loggerMock = new Mock<ILogger<PaymentService>>();
        private readonly Mock<HttpClient> _httpClienteMock = new Mock<HttpClient>();
        private readonly TestHttpClientFactory HttpClientFactory = new TestHttpClientFactory();

        [SetUp]
        public void SetUp()
        {
            /*_cieloClienteMock.Setup(x => x.CreateClient())
                .Returns(HttpClientFactory.CreateClient());*/
            _paymentService = new PaymentService(_paymentSettings.Object, _userService,_loggerMock.Object);
            
        }

        /*[Test]
        public void Given_Transaction_When_payment_Then_return_Ok()
        {
            //Arrange
            var idPerson = "6442dcb6523d52533aeb1ae4";
            var transaction = FactoryTransaction.SimpleTransactionCreditCard();
            //_transactionRepositoryMock.Setup(x => x.GetByPerson(idPerson))
            //    .Returns(Task.FromResult(FactoryTransaction.SimpleTransaction() as object));

            //Act
            var result = _paymentService.Payment(transaction);

            //Assert
            Assert.IsNotNull(result.Result.Data);
        }*/
    }
}