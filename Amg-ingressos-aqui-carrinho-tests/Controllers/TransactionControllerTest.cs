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
using Amg_ingressos_aqui_carrinho_api.Model;

namespace Amg_ingressos_aqui_carrinho_tests.Controllers
{
    public class TransactionControllerTest
    {
        private TransactionController _transactionController;
        private Mock<ITransactionRepository> _transactionRepositoryMock = new Mock<ITransactionRepository>();
        private Mock<ITransactionItenRepository> _transactionItenRepositoryMock = new Mock<ITransactionItenRepository>();
        private Mock<ILogger<TransactionController>> _loggerMock = new Mock<ILogger<TransactionController>>();
        private Mock<ITicketService> _ticketServiceMock = new Mock<ITicketService>();
        private Mock<IPaymentService> _paymentServiceMock = new Mock<IPaymentService>();
        private TestHttpClientFactory HttpClientFactory = new TestHttpClientFactory();
        private Mock<ITransactionRepository> _transactionServiceMock = new Mock<ITransactionRepository>();

        [SetUp]
        public void Setup()
        {
            _transactionController = new TransactionController(
                _loggerMock.Object,
                new TransactionService(
                _transactionRepositoryMock.Object,
                _transactionItenRepositoryMock.Object,
                _ticketServiceMock.Object,
                _paymentServiceMock.Object)
            );
        }

        [Test]
        public async Task Given_transaction_CreditCard_When_Save_Then_return_message_created_Async()
        {
            // Arrange
            var transactionDto = FactoryTransaction.SimpleTransactionDtoStageConfirm();
            var messageReturn = "6442dcb6523d52533aeb1ae4";
            _transactionRepositoryMock.Setup(x => x.Save<object>(It.IsAny<Transaction>()))
                .Returns(Task.FromResult(messageReturn as object));
            _ticketServiceMock.Setup(x => x.GetTicketsAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(new MessageReturn(){ Data=FactoryTicketService.SimpleListTicketNotSold()}));
            _ticketServiceMock.Setup(x => x.UpdateTicketsAsync(It.IsAny<Ticket>()))
                .Returns(Task.FromResult(new MessageReturn(){ Data="Ticket alterado"}));

            // Act
            var result = (await _transactionController.SaveTransactionAsync(transactionDto) as OkObjectResult);

            // Assert
            Assert.AreEqual(messageReturn, result?.Value);
        }

        [Test]
        public async Task Given_transaction_When_Save_and_has_internal_error_Then_return_status_code_500_Async()
        {
            // Arrange
            var transactionSave = FactoryTransaction.SimpleTransactionDtoStageConfirm();
            var espectedReturn = MessageLogErrors.saveTransactionMessage;
            _transactionRepositoryMock.Setup(x => x.Save<object>(transactionSave)).Throws(new Exception("error conection database"));

            // Act
            var result = (await _transactionController.SaveTransactionAsync(transactionSave) as ObjectResult);

            // Assert
            Assert.AreEqual(500, result.StatusCode);
            Assert.AreEqual(espectedReturn, result.Value);
        }

        [Test]
        public async Task Given_transaction_When_Save_Then_return_NotFound_Async()
        {
            // Arrange
            var transactionSave = FactoryTransaction.SimpleTransactionDtoStageConfirm();
            transactionSave.IdCustomer= string.Empty;
            var espectedReturn = "Usuário é obrigatório";

            // Act
            var result = (await _transactionController.SaveTransactionAsync(transactionSave) as ObjectResult);

            // Assert
            Assert.AreEqual(404, result.StatusCode);
            Assert.AreEqual(espectedReturn, result.Value);
        }

        [Test]
        public async Task Given_transaction_When_UpdateStage_Then_return_message_updated_Async()
        {
            // Arrange
            var idTransaction = "6442dcb6523d52533aeb1ae4";
            var messageReturn = "Transação alterada";
            _transactionRepositoryMock.Setup(x => x.Update<object>(It.IsAny<Transaction>()))
                .Returns(Task.FromResult(messageReturn as object));

            // Act
            var result = (await _transactionController.UpdateTransactionPersonDataAsync(idTransaction) as OkObjectResult);

            // Assert
            Assert.AreEqual(messageReturn, result?.Value);
        }

        [Test]
        public async Task Given_transaction_When_UpdateStage_Then_return_status_code_500_Async()
        {
            // Arrange
            var espectedReturn = MessageLogErrors.updateTransactionMessage;
            var idTransaction = "6442dcb6523d52533aeb1ae4";
            _transactionRepositoryMock.Setup(x => x.Update<object>(It.IsAny<Transaction>()))
                .Throws(new Exception("error conection database"));

            // Act
            var result = (await _transactionController.UpdateTransactionPersonDataAsync(idTransaction) as ObjectResult);

            // Assert
            Assert.AreEqual(500, result.StatusCode);
            Assert.AreEqual(espectedReturn, result.Value);
        }

        [Test]
        public async Task Given_transaction_When_UpdateStage_Then_return_NotFound_Async()
        {
            // Arrange
            var idTransaction = string.Empty;
            var espectedReturn = "Transação é obrigatório";
            var id = "";

            // Act
            var result = (await _transactionController.UpdateTransactionPersonDataAsync(idTransaction) as ObjectResult);

            // Assert
            Assert.AreEqual(404, result.StatusCode);
            Assert.AreEqual(espectedReturn, result.Value);
        }

        [Test]
        public async Task Given_idTransaction_When_GetById_Then_return_transaction_Async()
        {
            // Arrange
            var idTransaction = "6442dcb6523d52533aeb1ae4";
            _transactionRepositoryMock.Setup(x => x.GetById(idTransaction))
                .Returns(Task.FromResult(FactoryTransaction.SimpleTransaction() as object));

            // Act
            var result = (await _transactionController.GetByIdTransactionAsync(idTransaction) as OkObjectResult);

            // Assert
            Assert.AreEqual(200, result.StatusCode);
            Assert.IsNotNull(result?.Value);
        }

        [Test]
        public async Task Given_idTransaction_When_GetById_Then_return_status_code_500_Async()
        {
            // Arrange
            var idTransaction = "6442dcb6523d52533aeb1ae4";
            var espectedReturn = MessageLogErrors.getByIdTransactionMessage;
            _transactionRepositoryMock.Setup(x => x.GetById(idTransaction))
                .Throws(new Exception("error conection database"));

            // Act
            var result = (await _transactionController.GetByIdTransactionAsync(idTransaction) as ObjectResult);

            // Assert
            Assert.AreEqual(500, result.StatusCode);
            Assert.AreEqual(espectedReturn, result.Value);
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
            Assert.AreEqual(404, result.StatusCode);
            Assert.AreEqual(espectedReturn, result.Value);
        }

        [Test]
        public async Task Given_transactionTicketData_When_UpdateStage_Then_return_message_updated_Async()
        {
            // Arrange
            var transactionDto = FactoryTransaction.SimpleStageTicketDataDTo();
            var messageReturn = "Transação alterada";
            _transactionRepositoryMock.Setup(x => x.Update<object>(It.IsAny<Transaction>()))
                .Returns(Task.FromResult(messageReturn as object));

            // Act
            var result = (await _transactionController.UpdateTransactionTicketsDataAsync(transactionDto) as OkObjectResult);

            // Assert
            Assert.AreEqual(messageReturn, result?.Value);
        }

        [Test]
        public async Task Given_transactionTicketData_When_UpdateStage_Then_return_status_code_500_Async()
        {
            // Arrange
            var transactionDto = FactoryTransaction.SimpleStageTicketDataDTo();
            var espectedReturn = MessageLogErrors.updateTransactionMessage;
            _transactionRepositoryMock.Setup(x => x.Update<object>(It.IsAny<Transaction>()))
                .Throws(new Exception("error conection database"));

            // Act
            var result = (await _transactionController.UpdateTransactionTicketsDataAsync(transactionDto) as ObjectResult);

            // Assert
            Assert.AreEqual(500, result.StatusCode);
            Assert.AreEqual(espectedReturn, result.Value);
        }

        [Test]
        public async Task Given_transactionTicketData_When_UpdateStage_Then_return_NotFound_Async()
        {
            // Arrange
            var transactionDto = FactoryTransaction.SimpleStageTicketDataDTo();
            transactionDto.Id = string.Empty;
            var espectedReturn = "Transação é obrigatório";

            // Act
            var result = (await _transactionController.UpdateTransactionTicketsDataAsync(transactionDto) as ObjectResult);

            // Assert
            Assert.AreEqual(404, result.StatusCode);
            Assert.AreEqual(espectedReturn, result.Value);
        }

        [Test]
        public async Task Given_transactionPaymentMethod_When_UpdateStage_Then_return_message_updated_Async()
        {
            // Arrange
            var transactionDto = FactoryTransaction.SimpleStagePaymentDataDToCreditCard();
            var messageReturn = "Transação alterada";
            _transactionRepositoryMock.Setup(x => x.Update<object>(It.IsAny<Transaction>()))
                .Returns(Task.FromResult(messageReturn as object));

            // Act
            var result = (await _transactionController.UpdateTransactionPaymentDataAsync(transactionDto) as OkObjectResult);

            // Assert
            Assert.AreEqual(messageReturn, result?.Value);
        }

        [Test]
        public async Task Given_transactionPaymentMethod_When_UpdateStage_Then_return_status_code_500_Async()
        {
            // Arrange
            var transactionDto = FactoryTransaction.SimpleStagePaymentDataDToCreditCard();
            var espectedReturn = MessageLogErrors.updateTransactionMessage;
            _transactionRepositoryMock.Setup(x => x.Update<object>(It.IsAny<Transaction>()))
                .Throws(new Exception("error conection database"));

            // Act
            var result = (await _transactionController.UpdateTransactionPaymentDataAsync(transactionDto) as ObjectResult);

            // Assert
            Assert.AreEqual(500, result.StatusCode);
            Assert.AreEqual(espectedReturn, result.Value);
        }

        [Test]
        public async Task Given_transactionPaymentMethod_When_UpdateStage_Then_return_NotFound_Async()
        {
            // Arrange
            var transactionDto = FactoryTransaction.SimpleStagePaymentDataDToCreditCard();
            transactionDto.Id = string.Empty;
            var espectedReturn = "Transação é obrigatório";

            // Act
            var result = (await _transactionController.UpdateTransactionPaymentDataAsync(transactionDto) as ObjectResult);

            // Assert
            Assert.AreEqual(404, result.StatusCode);
            Assert.AreEqual(espectedReturn, result.Value);
        }

        [Test]
        public async Task Given_transactionPayment_When_Payment_Then_return_message_updated_Async()
        {
            // Arrange
            var transaction = FactoryTransaction.SimpleStagePaymentDataDToCreditCard();
            var messageReturn = "Transação Efetivada";
            _transactionRepositoryMock.Setup(x => x.GetById(transaction.Id))
                .Returns(Task.FromResult(FactoryTransaction.SimpleTransaction() as object));
            _paymentServiceMock.Setup(x => x.Payment(It.IsAny<Transaction>()))
                .Returns(Task.FromResult( new MessageReturn(){ Data = "OK"} ));
            _transactionRepositoryMock.Setup(x => x.Update<object>(It.IsAny<Transaction>()))
                .Returns(Task.FromResult("Transação alterada" as object));

            // Act
            var result = (await _transactionController.PaymentTransactionAsync(transaction) as OkObjectResult);

            // Assert
            Assert.AreEqual(messageReturn, result?.Value);
        }

        [Test]
        public async Task Given_transactionPayment_When_Payment_Then_return_status_code_500_Async()
        {
            // Arrange
            var transaction = FactoryTransaction.SimpleStagePaymentDataDToCreditCard();
            var messageReturn = MessageLogErrors.paymentTransactionMessage;
            _transactionRepositoryMock.Setup(x => x.GetById(transaction.Id))
                .Throws(new Exception("error conection database"));

            // Act
            var result = (await _transactionController.PaymentTransactionAsync(transaction) as ObjectResult);

            // Assert
            Assert.AreEqual(500, result.StatusCode);
            Assert.AreEqual(messageReturn, result.Value);
        }

        [Test]
        public async Task Given_transactionPayment_When_Payment_Then_return_NotFound_Async()
        {
            // Arrange
            var transaction = FactoryTransaction.SimpleStagePaymentDataDToCreditCard();
            var espectedReturn = "Transação é obrigatório";

            // Act
            var result = (await _transactionController.PaymentTransactionAsync(transaction) as ObjectResult);

            // Assert
            Assert.AreEqual(404, result.StatusCode);
            Assert.AreEqual(espectedReturn, result.Value);
        }

        [Test]
        public async Task Given_transactionPayment_When_Payment_Then_return_NotFound_Cielo_Async()
        {
            // Arrange
            var transaction = FactoryTransaction.SimpleStagePaymentDataDToCreditCard();
            var espectedReturn = "Transação é Obrigatório";
            _transactionRepositoryMock.Setup(x => x.GetById(transaction.Id))
                .Returns(Task.FromResult(FactoryTransaction.SimpleTransaction() as object));
            _paymentServiceMock.Setup(x => x.Payment(It.IsAny<Transaction>()))
                .Returns(Task.FromResult( new MessageReturn(){ Message = "Transação é Obrigatório"} ));

            // Act
            var result = (await _transactionController.PaymentTransactionAsync(transaction) as ObjectResult);

            // Assert
            Assert.AreEqual(404, result.StatusCode);
            Assert.AreEqual(espectedReturn, result.Value);
        }

        [Test]
        public async Task Given_idPerson_When_GetByPerson_Then_return_transaction_Async()
        {
            // Arrange
            var idPerson = "6442dcb6523d52533aeb1ae4";
            _transactionRepositoryMock.Setup(x => x.GetByPerson(idPerson))
                .Returns(Task.FromResult(FactoryTransaction.SimpleTransaction() as object));

            // Act
            var result = (await _transactionController.GetByPersonAsync(idPerson) as OkObjectResult);

            // Assert
            Assert.AreEqual(200, result.StatusCode);
            Assert.IsNotNull(result?.Value);
        }

        [Test]
        public async Task Given_idPerson_When_GetByPerson_Then_return_status_code_500_Async()
        {
            // Arrange
            var idTransaction = "6442dcb6523d52533aeb1ae4";
            var espectedReturn = MessageLogErrors.getByPersonTransactionMessage;
            _transactionRepositoryMock.Setup(x => x.GetByPerson(idTransaction))
                .Throws(new Exception("error conection database"));

            // Act
            var result = (await _transactionController.GetByPersonAsync(idTransaction) as ObjectResult);

            // Assert
            Assert.AreEqual(500, result.StatusCode);
            Assert.AreEqual(espectedReturn, result.Value);
        }

        [Test]
        public async Task Given_idPerson_When_GetByPerson_Then_return_NotFound_Async()
        {
            // Arrange
            var idTransaction= string.Empty;
            var espectedReturn = "Usuário é obrigatório";

            // Act
            var result = (await _transactionController.GetByPersonAsync(idTransaction) as ObjectResult);

            // Assert
            Assert.AreEqual(404, result.StatusCode);
            Assert.AreEqual(espectedReturn, result.Value);
        }
    }
}