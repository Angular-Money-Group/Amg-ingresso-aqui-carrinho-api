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
using Amg_ingressos_aqui_carrinho_api.Model.Querys;

namespace Amg_ingressos_aqui_carrinho_tests.Controllers
{
    public class TransactionControllerTest
    {
        private TransactionController _transactionController;
        private readonly Mock<ITransactionRepository> _transactionRepositoryMock = new Mock<ITransactionRepository>();
        private readonly Mock<ITransactionItenService> _transactionItenServiceMock = new Mock<ITransactionItenService>();
        private readonly Mock<ILogger<TransactionController>> _loggerMock = new Mock<ILogger<TransactionController>>();
        private readonly Mock<ITicketService> _ticketServiceMock = new Mock<ITicketService>();
        private readonly Mock<IPaymentService> _paymentServiceMock = new Mock<IPaymentService>();
        private readonly Mock<INotificationService> _emailServiceMock = new Mock<INotificationService>();
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

        /*[Test]
        public async Task Given_transaction_CreditCard_When_Save_Then_return_message_created_Async()
        {
            // Arrange
            var transactionDto = FactoryTransaction.SimpleTransactionDtoStageConfirm();
            var messageReturn = "6442dcb6523d52533aeb1ae4";
            _transactionRepositoryMock.Setup(x => x.Save<object>(It.IsAny<Transaction>()))
                .Returns(Task.FromResult(messageReturn as object));
            _ticketServiceMock.Setup(x => x.GetTicketsByLotAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(new List<Ticket>()));
            _ticketServiceMock.Setup(x => x.UpdateTicketsAsync(It.IsAny<Ticket>()))
                .Returns(Task.FromResult(true));

            // Act
            var result = await _transactionController.SaveTransactionAsync(transactionDto) as OkObjectResult;

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
            transactionSave.IdUser= string.Empty;
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
            var simpleListTransaction = FactoryTransaction.SimpleListTransactionQueryStageConfirm();
            _transactionRepositoryMock.Setup(x => x.Update<object>(It.IsAny<Transaction>()))
                .Returns(Task.FromResult(messageReturn as object));
            _transactionRepositoryMock.Setup(x => x.GetById(idTransaction))
                .Returns(Task.FromResult(simpleListTransaction as object));

            // Act
            var result = await _transactionController.UpdateTransactionPersonDataAsync(idTransaction) as OkObjectResult;

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
            var espectedReturn = "Id Transação é Obrigatório";
            var id = "";
            var simpleListTransaction = FactoryTransaction.SimpleListTransactionQueryStagePersonData();
            _transactionRepositoryMock.Setup(x => x.GetById(idTransaction))
                .Returns(Task.FromResult(simpleListTransaction as object));

            // Act
            var result = (await _transactionController.UpdateTransactionPersonDataAsync(idTransaction) as ObjectResult);

            // Assert
            Assert.AreEqual(404, result.StatusCode);
            Assert.AreEqual(espectedReturn, result.Value);
        }*/

        [Test]
        public async Task Given_idTransaction_When_GetById_Then_return_transaction_Async()
        {
            // Arrange
            var idTransaction = "6442dcb6523d52533aeb1ae4";
            _transactionRepositoryMock.Setup(x => x.GetById<GetTransactionEventData>(idTransaction))
                .Returns(Task.FromResult(new GetTransactionEventData()));

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
            _transactionRepositoryMock.Setup(x => x.GetById<GetTransactionEventData>(idTransaction))
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

        /*[Test]
        public async Task Given_transactionTicketData_When_UpdateStage_Then_return_message_updated_Async()
        {
            // Arrange
            var transactionDto = FactoryTransaction.SimpleStageTicketDataDTo();
            var idTransaction = "6442dcb6523d52533aeb1ae4";
            var messageReturn = "Transação alterada";
            var simpleListTransaction = FactoryTransaction.SimpleListTransactionQueryStagePersonData();
            _transactionRepositoryMock.Setup(x => x.GetById<GetTransactionEventData>(idTransaction))
                .Returns(Task.FromResult(new GetTransactionEventData()));
            _transactionRepositoryMock.Setup(x => x.Update<object>(It.IsAny<Transaction>()))
                .Returns(Task.FromResult(messageReturn as object));

            // Act
            var result = (await _transactionController
                .UpdateTransactionTicketsDataAsync(idTransaction,transactionDto) as OkObjectResult);

            // Assert
            Assert.AreEqual(messageReturn, result?.Value);
        }

        [Test]
        public async Task Given_transactionTicketData_When_UpdateStage_Then_return_status_code_500_Async()
        {
            // Arrange
            var transactionDto = FactoryTransaction.SimpleStageTicketDataDTo();
            var idTransaction = "6442dcb6523d52533aeb1ae4";
            var espectedReturn = MessageLogErrors.updateTransactionMessage;
            _transactionRepositoryMock.Setup(x => x.Update<object>(It.IsAny<Transaction>()))
                .Throws(new Exception("error conection database"));

            // Act
            var result = (await _transactionController
                .UpdateTransactionTicketsDataAsync(idTransaction,transactionDto) as ObjectResult);

            // Assert
            Assert.AreEqual(500, result.StatusCode);
            Assert.AreEqual(espectedReturn, result.Value);
        }

        [Test]
        public async Task Given_transactionTicketData_When_UpdateStage_Then_return_NotFound_Async()
        {
            // Arrange
            var transactionDto = FactoryTransaction.SimpleStageTicketDataDTo();
            var idTransaction = string.Empty;
            var espectedReturn = "Id Transação é Obrigatório";
            

            // Act
            var result = (await _transactionController
                .UpdateTransactionTicketsDataAsync(idTransaction,transactionDto) as ObjectResult);

            // Assert
            Assert.AreEqual(404, result.StatusCode);
            Assert.AreEqual(espectedReturn, result.Value);
        }

        [Test]
        public async Task Given_transactionPaymentMethod_When_UpdateStage_Then_return_message_updated_Async()
        {
            // Arrange
            var transactionDto = FactoryTransaction.SimpleStagePaymentDataDToCreditCard();
            var idTransaction = "6442dcb6523d52533aeb1ae4";
            var messageReturn = "Transação alterada";
            var simpleListTransaction = FactoryTransaction.SimpleListTransactionQueryStageTicketData();
            _transactionRepositoryMock.Setup(x => x.GetById(idTransaction))
                .Returns(Task.FromResult(simpleListTransaction as object));
            _transactionRepositoryMock.Setup(x => x.Update<object>(It.IsAny<Transaction>()))
                .Returns(Task.FromResult(messageReturn as object));
                

            // Act
            var result = (await _transactionController
                .UpdateTransactionPaymentDataAsync(idTransaction,transactionDto) as OkObjectResult);

            // Assert
            Assert.AreEqual(messageReturn, result?.Value);
        }

        [Test]
        public async Task Given_transactionPaymentMethod_When_UpdateStage_Then_return_status_code_500_Async()
        {
            // Arrange
            var transactionDto = FactoryTransaction.SimpleStagePaymentDataDToCreditCard();
            var idTransaction = "6442dcb6523d52533aeb1ae4";
            var espectedReturn = MessageLogErrors.updateTransactionMessage;
            _transactionRepositoryMock.Setup(x => x.Update<object>(It.IsAny<Transaction>()))
                .Throws(new Exception("error conection database"));

            // Act
            var result = (await _transactionController
                .UpdateTransactionPaymentDataAsync(idTransaction,transactionDto) as ObjectResult);

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
            var espectedReturn = "Id Transação é Obrigatório";

            // Act
            var result = (await _transactionController
                .UpdateTransactionPaymentDataAsync(string.Empty,transactionDto) as ObjectResult);

            // Assert
            Assert.AreEqual(404, result.StatusCode);
            Assert.AreEqual(espectedReturn, result.Value);
        }

        [Test]
        public async Task Given_transactionPayment_When_Payment_Then_return_message_updated_Async()
        {
            // Arrange
            var idTransaction = "6442dcb6523d52533aeb1ae4";
            var messageReturn = "Transação Efetivada";
            var simpleListTransaction = FactoryTransaction.SimpleListTransactionQueryStagePaymentData();
            _transactionRepositoryMock.Setup(x => x.GetById(idTransaction))
                .Returns(Task.FromResult(simpleListTransaction as object));
            _paymentServiceMock.Setup(x => x.Payment(It.IsAny<Transaction>()))
                .Returns(Task.FromResult( new MessageReturn(){ Data = "OK"} ));
            _transactionRepositoryMock.Setup(x => x.Update<object>(It.IsAny<Transaction>()))
                .Returns(Task.FromResult("Transação alterada" as object));

            // Act
            var result = await _transactionController.PaymentTransactionAsync(idTransaction) as OkObjectResult;

            // Assert
            Assert.AreEqual(messageReturn, result?.Value);
        }

        [Test]
        public async Task Given_transactionPayment_When_Payment_Then_return_status_code_500_Async()
        {
            // Arrange
            var idTransaction = "6442dcb6523d52533aeb1ae4";
            var messageReturn = MessageLogErrors.paymentTransactionMessage;
            _transactionRepositoryMock.Setup(x => x.GetById(idTransaction))
                .Throws(new Exception("error conection database"));

            // Act
            var result = await _transactionController.PaymentTransactionAsync(idTransaction) as ObjectResult;

            // Assert
            Assert.AreEqual(500, result?.StatusCode);
            Assert.AreEqual(messageReturn, result?.Value);
        }

        [Test]
        public async Task Given_transactionPayment_When_Payment_Then_return_NotFound_Async()
        {
            // Arrange
            var idTransaction = "";
            var espectedReturn = "Id Transação é Obrigatório";
            var simpleListTransaction = FactoryTransaction.SimpleListTransactionQueryStagePaymentData();
            _transactionRepositoryMock.Setup(x => x.GetById(idTransaction))
                .Returns(Task.FromResult(simpleListTransaction as object));

            // Act
            var result = await _transactionController.PaymentTransactionAsync(idTransaction) as ObjectResult;

            // Assert
            Assert.AreEqual(404, result?.StatusCode);
            Assert.AreEqual(espectedReturn, result?.Value);
        }

        [Test]
        public async Task Given_transactionPayment_When_Payment_Then_return_NotFound_Cielo_Async()
        {
            // Arrange
            var idTransaction = "6442dcb6523d52533aeb1ae4";
            var espectedReturn = "Transação é Obrigatório";
            var simpleListTransaction = FactoryTransaction.SimpleListTransactionQueryStagePaymentData();
            _transactionRepositoryMock.Setup(x => x.GetById(idTransaction))
                .Returns(Task.FromResult(simpleListTransaction as object));
            _paymentServiceMock.Setup(x => x.Payment(It.IsAny<Transaction>()))
                .Returns(Task.FromResult( new MessageReturn(){ Message = "Transação é Obrigatório"} ));

            // Act
            var result = await _transactionController.PaymentTransactionAsync(idTransaction) as ObjectResult;

            // Assert
            Assert.AreEqual(404, result.StatusCode);
            Assert.AreEqual(espectedReturn, result.Value);
        }*/

        [Test]
        public async Task Given_idPerson_When_GetByPerson_Then_return_NotFound_Async()
        {
            // Arrange
            var idTransaction= string.Empty;
            var espectedReturn = "Usuário é obrigatório";

            // Act
            var result = (await _transactionController.GetByUserActivesAsync(idTransaction) as ObjectResult);

            // Assert
            Assert.AreEqual(404, result.StatusCode);
            Assert.AreEqual(espectedReturn, result.Value);
        }
    }
}