using Amg_ingressos_aqui_carrinho_api.Controllers;
using NUnit.Framework;
using Moq;
using Amg_ingressos_aqui_carrinho_api.Repository.Interfaces;
using Microsoft.Extensions.Logging;
using Amg_ingressos_aqui_carrinho_api.Services;
using Microsoft.AspNetCore.Mvc;
using Amg_ingressos_aqui_carrinho_api.Consts;
using Amg_ingressos_aqui_carrinho_api.Services.Interfaces;
using Amg_ingressos_aqui_carrinho_api.Model;

namespace Amg_ingressos_aqui_carrinho_tests.Controllers
{
    public class TransactionControllerTest
    {
        private TransactionController _transactionController;
        private readonly Mock<ITransactionRepository> _transactionRepositoryMock = new Mock<ITransactionRepository>();
        private readonly Mock<ITransactionItenService> _transactionItenServiceMock = new Mock<ITransactionItenService>();
        private readonly Mock<ILogger<TransactionController>> _loggerMock = new Mock<ILogger<TransactionController>>();
        private readonly Mock<ILogger<TransactionService>> _logger = new Mock<ILogger<TransactionService>>();

        [SetUp]
        public void Setup()
        {
            _transactionController = new TransactionController(
                _loggerMock.Object,
                new TransactionService(
                _transactionRepositoryMock.Object,
                _logger.Object,
                _transactionItenServiceMock.Object)
            );
        }

        [Test]
        public async Task Given_idTransaction_When_GetById_Then_return_transaction_Async()
        {
            // Arrange
            var idTransaction = "6442dcb6523d52533aeb1ae4";
            _transactionRepositoryMock.Setup(x => x.GetById<TransactionComplet>(idTransaction))
                .Returns(Task.FromResult(new TransactionComplet()));

            // Act
            var result = (await _transactionController.GetByIdTransactionAsync(idTransaction) as OkObjectResult);

            // Assert
            Assert.AreEqual(200, result?.StatusCode);
            Assert.IsNotNull(result?.Value);
        }

        [Test]
        public async Task Given_idTransaction_When_GetById_Then_return_status_code_500_Async()
        {
            // Arrange
            var idTransaction = "6442dcb6523d52533aeb1ae4";
            var espectedReturn = MessageLogErrors.getByIdTransactionMessage;
            _transactionRepositoryMock.Setup(x => x.GetById<TransactionComplet>(idTransaction))
                .Throws(new Exception("error conection database"));

            // Act
            var result = (await _transactionController.GetByIdTransactionAsync(idTransaction) as ObjectResult);

            // Assert
            Assert.AreEqual(500, result?.StatusCode);
            Assert.AreEqual(espectedReturn, result?.Value);
        }

        [Test]
        public async Task Given_idtransaction_When_GetById_Then_return_NotFound_Async()
        {
            // Arrange

            var idTransaction= string.Empty;
            var espectedReturn = "Transação é obrigatório";

            // Act
            var result = (await _transactionController.GetByIdTransactionAsync(idTransaction) as ObjectResult);

            // Assert
            Assert.AreEqual(404, result?.StatusCode);
            Assert.AreEqual(espectedReturn, result?.Value);
        }

        [Test]
        public async Task Given_idPerson_When_GetByPerson_Then_return_NotFound_Async()
        {
            // Arrange
            var idTransaction= string.Empty;
            var espectedReturn = "Usuário é obrigatório";

            // Act
            var result = (await _transactionController.GetByUserActivesAsync(idTransaction) as ObjectResult);

            // Assert
            Assert.AreEqual(404, result?.StatusCode);
            Assert.AreEqual(espectedReturn, result?.Value);
        }
    }
}