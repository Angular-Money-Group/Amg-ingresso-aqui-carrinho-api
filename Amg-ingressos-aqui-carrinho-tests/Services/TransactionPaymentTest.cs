using Amg_ingressos_aqui_carrinho_api.Services;
using NUnit.Framework;
using Moq;
using Amg_ingressos_aqui_carrinho_api.Repository.Interfaces;
using Amg_ingressos_aqui_carrinho_api.Model;
using Amg_ingressos_aqui_carrinho_api.Enum;
using Amg_ingressos_aqui_carrinho_api.Infra;
using System.Text.Json;
using System.Text;
using static System.Net.Mime.MediaTypeNames;
using Amg_ingressos_aqui_carrinho_tests.FactoryServices;
using System.Net;

namespace Amg_ingressos_aqui_carrinho_tests.Services
{
    public class TransactionPaymentTest
    {
        private TransactionPaymentService _transactionService;
        private Mock<ITransactionPaymentRepository> _transactionServiceMock = new Mock<ITransactionPaymentRepository>();
        private Mock<ICieloClient> _cieloClienteMock = new Mock<ICieloClient>();
        private Mock<HttpClient> _httpClienteMock = new Mock<HttpClient>();
        private TestHttpClientFactory testHttpClientFactory = new TestHttpClientFactory();

        [SetUp]
        public void SetUp()
        {
            _cieloClienteMock.Setup(x => x.CreateClient())
                .Returns(testHttpClientFactory.CreateClient());

            _transactionService = new TransactionPaymentService(
                _transactionServiceMock.Object,
                _cieloClienteMock.Object);
        }

        [Test]
        public void Given_transaction_with_payment_to_insert_When_SaveTransactionIten_Then_return_message_OK()
        {
            //Arrange
            TransactionPayment transaction = new TransactionPayment()
            {
                IdTransaction = "6442dcb6523d52533aeb1ae4",
                IdPaymentMethod = "6442dcb6523d52533aeb1ae4",
                TotalValue = new decimal(360),
                Status = StatusPayment.InProgress
            };

            var messageReturn = "Transação criada";
            _transactionServiceMock.Setup(x => x.Save<object>(transaction)).Returns(Task.FromResult(messageReturn as object));

            //Act
            var resultMethod = _transactionService.SaveAsync(transaction);

            //Assert
            Assert.AreEqual(messageReturn, resultMethod.Result.Data);
        }

        [Test]
        public void Given_transaction_with_payment_without_totalValue_to_insert_When_SaveTransactionIten_Then_return_message_miss_totalValue()
        {
            //Arrange
            TransactionPayment transaction = new TransactionPayment()
            {
                IdTransaction = "6442dcb6523d52533aeb1ae4",
                IdPaymentMethod = "6442dcb6523d52533aeb1ae4",
                TotalValue = new decimal(0),
                Status = StatusPayment.InProgress
            };

            var messageReturn = "Valor total é obrigatório";
            _transactionServiceMock.Setup(x => x.Save<object>(transaction)).Returns(Task.FromResult(messageReturn as object));

            //Act
            var resultMethod = _transactionService.SaveAsync(transaction);

            //Assert
            Assert.AreEqual(messageReturn, resultMethod.Result.Message);
        }

        [Test]
        public void Given_transaction_with_payment_without_IdTransaction_to_insert_When_SaveTransactionIten_Then_return_message_miss_totalValue()
        {
            //Arrange
            TransactionPayment transaction = new TransactionPayment()
            {
                IdTransaction = "",
                IdPaymentMethod = "6442dcb6523d52533aeb1ae4",
                TotalValue = new decimal(0),
                Status = StatusPayment.InProgress
            };

            var messageReturn = "Id Transação é obrigatório";
            _transactionServiceMock.Setup(x => x.Save<object>(transaction)).Returns(Task.FromResult(messageReturn as object));

            //Act
            var resultMethod = _transactionService.SaveAsync(transaction);

            //Assert
            Assert.AreEqual(messageReturn, resultMethod.Result.Message);
        }

        [Test]
        public void Given_transaction_with_payment_without_IdPayment_to_insert_When_SaveTransactionIten_Then_return_message_miss_totalValue()
        {
            //Arrange
            TransactionPayment transaction = new TransactionPayment()
            {
                IdTransaction = "6442dcb6523d52533aeb1ae4",
                IdPaymentMethod = "",
                TotalValue = new decimal(0),
                Status = StatusPayment.InProgress
            };

            var messageReturn = "Id Meio Pagamento é obrigatório";
            _transactionServiceMock.Setup(x => x.Save<object>(transaction)).Returns(Task.FromResult(messageReturn as object));

            //Act
            var resultMethod = _transactionService.SaveAsync(transaction);

            //Assert
            Assert.AreEqual(messageReturn, resultMethod.Result.Message);
        }

        [Test]
        public void Given_transaction_with__When_SaveTransactionIten_Then_return_message_internal_error()
        {
            //Arrange
            TransactionPayment transaction = new TransactionPayment()
            {
                IdTransaction = "6442dcb6523d52533aeb1ae4",
                IdPaymentMethod = "6442dcb6523d52533aeb1ae4",
                TotalValue = new decimal(360),
                Status = StatusPayment.InProgress
            };

            var messageReturn = "erro ao conectar com base de dados";
            _transactionServiceMock.Setup(x => x.Save<object>(transaction))
            .Throws(new Exception(messageReturn));

            //Act
            var ex = Assert.ThrowsAsync<Exception>(() => _transactionService.SaveAsync(transaction));

            //Assert
            Assert.That(ex?.Message, Is.EqualTo(messageReturn));
        }

        [Test]
        public void Given_transaction_with_payment_When_SendTransaction_Then_return_message_OK()
        {
            //Arrange
            TransactionPayment transaction = new TransactionPayment()
            {
                IdTransaction = "6442dcb6523d52533aeb1ae4",
                IdPaymentMethod = "6442dcb6523d52533aeb1ae4",
                TotalValue = new decimal(360),
                Status = StatusPayment.InProgress
            };

            var messageReturn = "Transação criada";
            _transactionServiceMock.Setup(x => x.Save<object>(transaction)).Returns(Task.FromResult(messageReturn as object));

            //Act
            var resultMethod = _transactionService.SaveAsync(transaction);

            //Assert
            Assert.AreEqual(messageReturn, resultMethod.Result.Data);
        }

        [Test]
        public void Given_transaction_with_payment_When_SendCielo_Then_return_message_OK()
        {
            //Arrange
            TransactionPayment transaction = new TransactionPayment()
            {
                IdTransaction = "6442dcb6523d52533aeb1ae4",
                IdPaymentMethod = "6442dcb6523d52533aeb1ae4",
                TotalValue = new decimal(360),
                Status = StatusPayment.InProgress
            };

            var messageReturn = "Ticket criado";

            //Act
            var resultMethod = _transactionService.Payment(transaction).Result;

            //Assert
            Assert.AreEqual(messageReturn, resultMethod.Data);
        }
    }
}