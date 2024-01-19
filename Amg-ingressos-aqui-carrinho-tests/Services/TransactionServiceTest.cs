using Amg_ingressos_aqui_carrinho_tests.FactoryServices;
using Amg_ingressos_aqui_carrinho_api.Services;
using NUnit.Framework;
using Moq;
using Amg_ingressos_aqui_carrinho_api.Repository.Interfaces;
using Amg_ingressos_aqui_carrinho_api.Model;
using Amg_ingressos_aqui_carrinho_api.Infra;
using Amg_ingressos_aqui_carrinho_api.Dto;
using Amg_ingressos_aqui_carrinho_api.Services.Interfaces;
using Amg_ingressos_aqui_carrinho_api.Exceptions;
using Amazon.Runtime.Internal.Util;
using Microsoft.Extensions.Logging;

namespace Prime.UnitTests.Services
{
    public class TransactionServiceTest
    {
        private TransactionService _transactionService;
        private readonly Mock<ITransactionRepository> _transactionRepositoryMock = new Mock<ITransactionRepository>();
        private readonly Mock<ITransactionItenRepository> _transactionItenRepositoryMock = new Mock<ITransactionItenRepository>();
        private readonly Mock<ITicketService> _ticketServiceMock = new Mock<ITicketService>();
        private readonly Mock<IPaymentService> _paymentServiceMock = new Mock<IPaymentService>();
        private readonly Mock<INotificationService> _emailService = new Mock<INotificationService>();
        private readonly Mock<ILogger<TransactionService>> _logger = new Mock<ILogger<TransactionService>>();


        [SetUp]
        public void SetUp()
        {

            _transactionService = new TransactionService(
                _transactionRepositoryMock.Object,
                _transactionItenRepositoryMock.Object,
                _ticketServiceMock.Object,
                _paymentServiceMock.Object,
                _emailService.Object,
                _logger.Object
                );
        }

        [Test]
        public void Given_IdPerson_and_TransactionItesn_When_save_Then_return_Id_Transaction()
        {
            //Arrange
            var transactionDto = FactoryTransaction.SimpleTransactionDtoStageConfirm();
            var messageReturn = "6442dcb6523d52533aeb1ae4";
            _transactionRepositoryMock.Setup(x => x.Save<object>(It.IsAny<Transaction>()))
                .Returns(Task.FromResult(messageReturn as object));
            _ticketServiceMock.Setup(x => x.GetTicketsByLotAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(new List<Ticket>()));
            _ticketServiceMock.Setup(x => x.UpdateTicketsAsync(It.IsAny<Ticket>()))
                .Returns(Task.FromResult(true));
            _transactionRepositoryMock.Setup(x => x.Update<object>(It.IsAny<Transaction>()))
                .Returns(Task.FromResult("Transaction alterada" as object));

            //Act
            var result = _transactionService.ProcessSaveAsync(transactionDto);

            //Assert
            Assert.AreEqual(messageReturn, result.Result.Data);
        }

        [Test]
        public void Given_transaction_without_IdTransaction_When_save_Then_return_message_miss_Transaction()
        {
            //Arrange
            TransactionDto transaction = new TransactionDto()
            {
                IdUser = "6442dcb6523d52533aeb1ae4"
            };
            var messageReturn = "Id Transação é obrigatório";
            _transactionRepositoryMock.Setup(x => x.Save<object>(It.IsAny<Transaction>()))
                .Returns(Task.FromResult("" as object));

            //Act
            var result = _transactionService.ProcessSaveAsync(transaction);

            //Assert
            Assert.AreEqual(messageReturn, result.Result.Message);
        }

        [Test]
        public void Given_transaction_without_IdPerson_When_save_Then_return_message_miss_Person()
        {
            //Arrange
            TransactionDto transaction = new TransactionDto()
            {
                IdUser = ""
            };
            var messageReturn = "Usuário é obrigatório";
            _transactionRepositoryMock.Setup(x => x.Save<object>(transaction))
                .Returns(Task.FromResult(messageReturn as object));

            //Act
            var result = _transactionService.ProcessSaveAsync(transaction);

            //Assert
            Assert.AreEqual(messageReturn, result.Result.Message);
        }

        [Test]
        public void Given_transaction_and_IdPerson_lenth_20_When_save_Then_return_message_miss_Person()
        {
            //Arrange
            TransactionDto transaction = new TransactionDto()
            {
                IdUser = "6442dcb6523d52533aeb",
            };
            var messageReturn = "Usuário é obrigatório e está menor que 24 digitos";
            _transactionRepositoryMock.Setup(x => x.Save<object>(transaction))
                .Returns(Task.FromResult(messageReturn as object));

            //Act
            var result = _transactionService.ProcessSaveAsync(transaction);

            //Assert
            Assert.AreEqual(messageReturn, result.Result.Message);
        }

        [Test]
        public void Given_Transaction_When_save_Then_return_message_internal_error()
        {
            //Arrange
            var transactionDto = FactoryTransaction.SimpleTransactionDtoStageConfirm();
            var messageReturn = "erro ao conectar com base de dados";
            _transactionRepositoryMock.Setup(x => x.Save<object>(It.IsAny<Transaction>()))
                .Throws(new Exception(messageReturn));

            //Act
            var result = _transactionService.ProcessSaveAsync(transactionDto);

            //Assert
            Assert.IsNotEmpty(result.Exception.Message);
        }

        [Test]
        public void Given_TransactionItens_When_saveTransactionItens_Then_return_not_have_tickets()
        {
            //Arrange
            var transactionDto = FactoryTransaction.SimpleTransactionDtoStageConfirm();
            var messageReturn = "Número de ingressos inválido";
            _transactionRepositoryMock.Setup(x => x.Save<object>(It.IsAny<Transaction>()))
                .Returns(Task.FromResult("6442dcb6523d52533aeb1ae4" as object));
            _ticketServiceMock.Setup(x => x.GetTicketsByLotAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(new List<Ticket>()));

            //Act
            var result = _transactionService.ProcessSaveAsync(transactionDto);

            //Assert
            Assert.AreEqual(messageReturn, result.Result.Message);
        }

        [Test]
        public void Given_transaction_with_tickets_without_ticketPrice_to_insert_When_SaveTransactionIten_Then_return_message_miss_valueTicket()
        {
            //Arrange
            var transactionDto = FactoryTransaction.SimpleTransactionDtoStageConfirm();
            var messageReturn = "Valor do Ingresso inválido.";
            _transactionRepositoryMock.Setup(x => x.Save<object>(It.IsAny<Transaction>()))
                .Returns(Task.FromResult("6442dcb6523d52533aeb1ae4" as object));
            _ticketServiceMock.Setup(x => x.GetTicketsByLotAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(new List<Ticket>()));

            //Act
            var result = _transactionService.ProcessSaveAsync(transactionDto);

            //Assert
            Assert.AreEqual(messageReturn, result.Result.Message);
        }

        [Test]
        public void Given_transaction_When_UpdateTransaction_Then_return_Ok()
        {
            //Arrange
            var transaction = FactoryTransaction.SimpleTransactionPersonData();
            
                       var messageReturn = "Transação alterada";
            _transactionRepositoryMock.Setup(x => x.Update<object>(It.IsAny<Transaction>()))
                .Returns(Task.FromResult(messageReturn as object));

            //Act
            var result = _transactionService.UpdateAsync(transaction);

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
            _transactionRepositoryMock.Setup(x => x.Update<object>(It.IsAny<Transaction>()))
                .Returns(Task.FromResult(messageReturn as object));

            //Act
            var result = _transactionService.UpdateAsync(transaction);

            //Assert
            Assert.AreEqual(messageReturn, result.Result.Message);
        }

        [Test]
        public void Given_transaction_When_UpdateTransaction_Then_return_Miss_TransactionId_in_Db()
        {
            //Arrange
            var transaction = FactoryTransaction.SimpleTransactionPersonData();
            var messageReturn = "Transação não encontrada";
            _transactionRepositoryMock.Setup(x => x.Update<object>(It.IsAny<Transaction>()))
                .Throws(new UpdateTransactionException(messageReturn));

            //Act
            var result = _transactionService.UpdateAsync(transaction);

            //Assert
            Assert.AreEqual(messageReturn, result.Result.Message);
        }

        [Test]
        public void Given_transaction_When_UpdateTransaction_Then_return_internal_error()
        {
            //Arrange
            var transaction = FactoryTransaction.SimpleTransactionPersonData();
            _transactionRepositoryMock.Setup(x => x.Update<object>(It.IsAny<Transaction>()))
                .Throws(new Exception("erro ao conectar na base de dados"));

            //Act
            var result = _transactionService.UpdateAsync(transaction);

            //Assert
            Assert.IsNotEmpty(result.Exception.Message);
        }

        [Test]
        public void Given_idtransaction_When_GetByIdTransaction_Then_return_Ok()
        {
            //Arrange
            var idTransaction = "6442dcb6523d52533aeb1ae4";
            _transactionRepositoryMock.Setup(x => x.GetById(idTransaction))
                .Returns(Task.FromResult(FactoryTransaction.SimpleTransaction() as object));

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
            _transactionRepositoryMock.Setup(x => x.GetById(idTransaction))
                .Returns(Task.FromResult(FactoryTransaction.SimpleTransaction() as object));

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
            _transactionRepositoryMock.Setup(x => x.GetById(idTransaction))
                .Throws(new GetException(messageReturn));

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
             _transactionRepositoryMock.Setup(x => x.GetById(idTransaction))
                .Throws(new Exception("erro ao conectar na base de dados"));

            //Act
            var result = _transactionService.GetByIdAsync(idTransaction);

            //Assert
            Assert.IsNotEmpty(result.Exception.Message);
        }

        [Test]
        public void Given_Transaction_When_Payment_Then_return_Ok()
        {
            //Arrange
            var messageReturn = "Transação Efetivada";
            var transaction = FactoryTransaction.SimpleTransaction();
            _paymentServiceMock.Setup(x => x.Payment(transaction))
                .Returns(Task.FromResult(new MessageReturn(){Data="Transação Efetivada"}));
            _transactionRepositoryMock.Setup(x => x.Update<object>(It.IsAny<Transaction>()))
                .Returns(Task.FromResult("Transação alterada" as object));

            //Act
            var result = _transactionService.Payment(transaction);

            //Assert
            Assert.AreEqual(messageReturn, result.Result.Data);
        }

        [Test]
        public void Given_Transaction_without_idTransaction_When_Payment_Then_return_miss_transactionId()
        {
            //Arrange
            var messageReturn = "Transação é obrigatório";
            var transaction = FactoryTransaction.SimpleTransaction();
            transaction.Id = string.Empty;

            //Act
            var result = _transactionService.Payment(transaction);

            //Assert
            Assert.AreEqual(messageReturn, result.Result.Message);
        }

        [Test]
        public void Given_Transaction_When_Payment_Then_return_error_service_payment()
        {
            //Arrange
            var messageReturn = "Transação não efetivada";
            var transaction = FactoryTransaction.SimpleTransaction();
            _paymentServiceMock.Setup(x => x.Payment(transaction))
                .Returns(Task.FromResult(new MessageReturn(){Message="Transação não efetivada"}));

            //Act
            var result = _transactionService.Payment(transaction);

            //Assert
            Assert.AreEqual(messageReturn, result.Result.Message);
        }

        [Test]
        public void Given_Transaction_When_Payment_Then_return_internal_error()
        {
            //Arrange
            var transaction = FactoryTransaction.SimpleTransaction();
             _paymentServiceMock.Setup(x => x.Payment(transaction))
                .Throws(new Exception("erro ao conectar com servidor cielo"));

            //Act
            var result = _transactionService.Payment(transaction);

            //Assert
            Assert.IsNotEmpty(result.Exception.Message);
        }

        [Test]
        public void Given_idPerson_When_GetByPerson_Then_return_internal_error()
        {
            //Arrange
            var idTransaction = "6442dcb6523d52533aeb1ae4";
             _transactionRepositoryMock.Setup(x => x.GetById(idTransaction))
                .Throws(new Exception("erro ao conectar na base de dados"));

            //Act
            var result = _transactionService.GetByIdAsync(idTransaction);

            //Assert
            Assert.IsNotEmpty(result.Exception.Message);
        }

        [Test]
        public void Given_Transaction_When_finished_Then_return_Ok()
        {
            //Arrange
            var messageReturn = "Transação Efetivada";
            var idTransaction = "6442dcb6523d52533aeb1ae4";
            var transaction = FactoryTransaction.SimpleTransaction();
            _ticketServiceMock.Setup(x => x.GetTicketByIdDataUserAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(new TicketUserDataDto()));
            _transactionRepositoryMock.Setup(x => x.GetById(idTransaction))
                .Returns(Task.FromResult(FactoryTransaction.SimpleTransaction() as object));
            _transactionRepositoryMock.Setup(x => x.Update<object>(It.IsAny<Transaction>()))
                .Returns(Task.FromResult("Transação alterada" as object));

            //Act
            var result = _transactionService.FinishedTransactionAsync(transaction);

            //Assert
            Assert.AreEqual(messageReturn, result.Result.Data);
        }
    }
}
