using Amg_ingressos_aqui_carrinho_api.Controllers;
using NUnit.Framework;
using Moq;
using Amg_ingressos_aqui_carrinho_api.Repository.Interfaces;
using Microsoft.Extensions.Logging;
using Amg_ingressos_aqui_carrinho_tests.FactoryServices;
using Amg_ingressos_aqui_carrinho_api.Services;
using Microsoft.AspNetCore.Mvc;
using Amg_ingressos_aqui_carrinho_api.Consts;
using Amg_ingressos_aqui_carrinho_api.Services.Interfaces;
using Amg_ingressos_aqui_carrinho_api.Infra;

namespace Amg_ingressos_aqui_carrinho_tests.Controllers
{
    public class TransactionControllerTest
    {
        private TransactionController _transactionController;
        private Mock<ITransactionRepository> _transactionRepositoryMock = new Mock<ITransactionRepository>();
        private Mock<ILogger<TransactionController>> _loggerMock = new Mock<ILogger<TransactionController>>();
        private Mock<ICieloClient> _cieloClienteMock = new Mock<ICieloClient>();
        private Mock<HttpClient> _httpClienteMock = new Mock<HttpClient>();
        private TestHttpClientFactory HttpClientFactory = new TestHttpClientFactory();

        [SetUp]
        public void Setup()
        {
            _cieloClienteMock.Setup(x => x.CreateClient())
                .Returns(HttpClientFactory.CreateClient());

            _transactionController = new TransactionController(
                _loggerMock.Object,
                new TransactionService(_transactionRepositoryMock.Object,
                _cieloClienteMock.Object)
            );
        }

        [Test]
        public async Task Given_transaction_CreditCard_When_Save_Then_return_message_created_Async()
        {
            // Arrange
            var messageReturn = "Transação criada";
            var transactionSave = FactoryTransaction.SimpleTransactionDto();
            _transactionRepositoryMock.Setup(x => x.Save<object>(transactionSave)).Returns(Task.FromResult(messageReturn as object));

            // Act
            var result = (await _transactionController.SaveTransactionAsync(transactionSave) as OkObjectResult);

            // Assert
            Assert.AreEqual(messageReturn, result?.Value);
        }

        [Test]
        public async Task Given_transaction_When_Save_and_has_internal_error_Then_return_status_code_500_Async()
        {
            // Arrange
            var transactionSave = FactoryTransaction.SimpleTransactionDto();
            var espectedReturn = MessageLogErrors.saveTransactionMessage;
            _transactionRepositoryMock.Setup(x => x.Save<object>(transactionSave)).Throws(new Exception("error conection database"));

            // Act
            var result = (await _transactionController.SaveTransactionAsync(transactionSave) as ObjectResult);
            var teste = (await _transactionController.SaveTransactionAsync(transactionSave));

            // Assert
            Assert.AreEqual(500, result.StatusCode);
            Assert.AreEqual(espectedReturn, result.Value);
        }

        [Test]
        public async Task Given_transaction_withOut_id_event_When_Save_and_has_internal_error_Then_return_miss_Id_Transaction_Async()
        {
            // Arrange
            var transactionSave = FactoryTransaction.SimpleTransactionDto();
            var espectedReturn = "";
            _transactionRepositoryMock.Setup(x => x.Save<object>(transactionSave)).Throws(new Exception("error conection database"));

            // Act
            var result = (await _transactionController.SaveTransactionAsync(transactionSave) as ObjectResult);
            var teste = (await _transactionController.SaveTransactionAsync(transactionSave));

            // Assert
            Assert.AreEqual(500, result.StatusCode);
            Assert.AreEqual(espectedReturn, result.Value);
        }
    }
}