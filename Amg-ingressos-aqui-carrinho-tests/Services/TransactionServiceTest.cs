using Amg_ingressos_aqui_carrinho_tests.FactoryServices;
using Amg_ingressos_aqui_carrinho_api.Services;
using NUnit.Framework;
using Moq;
using Amg_ingressos_aqui_carrinho_api.Repository.Interfaces;
using Amg_ingressos_aqui_carrinho_api.Model;
using Amg_ingressos_aqui_carrinho_api.Services.Interfaces;
using Amg_ingressos_aqui_carrinho_api.Exceptions;
using Microsoft.Extensions.Logging;

namespace Prime.UnitTests.Services
{
    public class TransactionServiceTest
    {
        private TransactionService _transactionService;
        private readonly Mock<ITransactionRepository> _transactionRepositoryMock = new Mock<ITransactionRepository>();
        private readonly Mock<ITransactionItenService> _transactionItenServiceMock = new Mock<ITransactionItenService>();
        private readonly Mock<ILogger<TransactionService>> _logger = new Mock<ILogger<TransactionService>>();

        [SetUp]
        public void SetUp()
        {

            _transactionService = new TransactionService(
                _transactionRepositoryMock.Object,
                _logger.Object,
                _transactionItenServiceMock.Object
                );
        }

        [Test]
        public void Given_transaction_When_UpdateTransaction_Then_return_Ok()
        {
            //Arrange
            var transaction = FactoryTransaction.SimpleTransactionPersonData();
            
                       var messageReturn = "Transação alterada";
            _transactionRepositoryMock.Setup(x => x.Edit(It.IsAny<string>(),It.IsAny<Transaction>()))
                .Returns(Task.FromResult(true));

            //Act
            var result = _transactionService.EditAsync(transaction);

            //Assert
            Assert.AreEqual(messageReturn, result.Result.Data);
        }

        [Test]
        public void Given_transaction_without_idTransaction_When_UpdateTransaction_Then_return_Miss_TransactionId()
        {
            //Arrange
            var transaction = FactoryTransaction.SimpleTransactionPersonData();
            transaction.Id= string.Empty;
            var messageReturn = "Transação é obrigatório";
            _transactionRepositoryMock.Setup(x => x.Edit(It.IsAny<string>(),It.IsAny<Transaction>()))
                .Returns(Task.FromResult(true));

            //Act
            var result = _transactionService.EditAsync(transaction);

            //Assert
            Assert.AreEqual(messageReturn, result.Result.Message);
        }

        [Test]
        public void Given_transaction_When_UpdateTransaction_Then_return_Miss_TransactionId_in_Db()
        {
            //Arrange
            var transaction = FactoryTransaction.SimpleTransactionPersonData();
            var messageReturn = "Transação não encontrada";
            _transactionRepositoryMock.Setup(x => x.Edit(It.IsAny<string>(),It.IsAny<Transaction>()))
                .Throws(new UpdateTransactionException(messageReturn));

            //Act
            var result = _transactionService.EditAsync(transaction);

            //Assert
            Assert.AreEqual(messageReturn, result.Result.Message);
        }

        [Test]
        public void Given_transaction_When_UpdateTransaction_Then_return_internal_error()
        {
            //Arrange
            var transaction = FactoryTransaction.SimpleTransactionPersonData();
            _transactionRepositoryMock.Setup(x => x.Edit(It.IsAny<string>(),It.IsAny<Transaction>()))
                .Throws(new Exception("erro ao conectar na base de dados"));

            //Act
            var result = _transactionService.EditAsync(transaction);

            //Assert
            Assert.IsNotEmpty(result?.Exception?.Message);
        }

        [Test]
        public void Given_idtransaction_When_GetByIdTransaction_Then_return_Ok()
        {
            //Arrange
            var idTransaction = "6442dcb6523d52533aeb1ae4";
            _transactionRepositoryMock.Setup(x => x.GetById<TransactionComplet>(idTransaction))
                .Returns(Task.FromResult(new TransactionComplet()));

            //Act
            var result = _transactionService.GetByIdAsync(idTransaction);

            //Assert
            Assert.IsNotNull(result.Result.Data);
        }

        [Test]
        public void Given_idTransaction_When_GetById_Then_return_Miss_TransactionId()
        {
            //Arrange
            var idTransaction = string.Empty;
            var messageReturn = "Transação é obrigatório";
            _transactionRepositoryMock.Setup(x => x.GetById<TransactionComplet>(idTransaction))
                .Returns(Task.FromResult(new TransactionComplet()));

            //Act
            var result = _transactionService.GetByIdAsync(idTransaction);

            //Assert
            Assert.AreEqual(messageReturn, result.Result.Message);
        }

        [Test]
        public void Given_idTransaction_When_GetById_Then_return_Miss_TransactionId_in_Db()
        {
            //Arrange
            var idTransaction = "6442dcb6523d52533aeb1ae4";
            var messageReturn = "Transação não encontrada";
            _transactionRepositoryMock.Setup(x => x.GetById<TransactionComplet>(idTransaction))
                .Returns(Task.FromResult(new TransactionComplet()));

            //Act
            var result = _transactionService.GetByIdAsync(idTransaction);
            //Assert
            Assert.AreEqual(messageReturn, result.Result.Message);
        }

        [Test]
        public void Given_idtransaction_When_GetById_Then_return_internal_error()
        {
            //Arrange
            var idTransaction = "6442dcb6523d52533aeb1ae4";
            _transactionRepositoryMock.Setup(x => x.GetById<TransactionComplet>(idTransaction))
                .Returns(Task.FromResult(new TransactionComplet()));

            //Act
            var result = _transactionService.GetByIdAsync(idTransaction);

            //Assert
            Assert.IsNotEmpty(result?.Exception?.Message);
        }

        [Test]
        public void Given_idPerson_When_GetByPerson_Then_return_internal_error()
        {
            //Arrange
            var idTransaction = "6442dcb6523d52533aeb1ae4";
             _transactionRepositoryMock.Setup(x => x.GetById<TransactionComplet>(idTransaction))
                .Throws(new Exception("erro ao conectar na base de dados"));

            //Act
            var result = _transactionService.GetByIdAsync(idTransaction);

            //Assert
            Assert.IsNotEmpty(result?.Exception?.Message);
        }
    }
}
