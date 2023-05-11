using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amg_ingressos_aqui_carrinho_api.Services;
using Amg_ingressos_aqui_carrinho_api.Infra;
using Moq;
using NUnit.Framework;
using Amg_ingressos_aqui_carrinho_tests.FactoryServices;

namespace Amg_ingressos_aqui_carrinho_tests.Services
{
    public class PaymentServiceTest
    {
        private PaymentService _paymentService;
        private Mock<ICieloClient> _cieloClienteMock = new Mock<ICieloClient>();
        private Mock<HttpClient> _httpClienteMock = new Mock<HttpClient>();
        private FactoryServices.TestHttpClientFactory HttpClientFactory = new FactoryServices.TestHttpClientFactory();

        [SetUp]
        public void SetUp()
        {
            _cieloClienteMock.Setup(x => x.CreateClient())
                .Returns(HttpClientFactory.CreateClient());
            _paymentService = new PaymentService(_cieloClienteMock.Object);
            
        }

        [Test]
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
        }
    }
}