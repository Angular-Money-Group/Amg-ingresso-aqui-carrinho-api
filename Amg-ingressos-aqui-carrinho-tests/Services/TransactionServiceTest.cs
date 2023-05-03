using Amg_ingressos_aqui_carrinho_tests.FactoryServices;
using Amg_ingressos_aqui_carrinho_api.Services;
using NUnit.Framework;
using Moq;
using Amg_ingressos_aqui_carrinho_api.Repository.Interfaces;
using Amg_ingressos_aqui_carrinho_api.Model;
using Amg_ingressos_aqui_carrinho_api.Enum;
using Amg_ingressos_aqui_carrinho_api.Infra;
using Amg_ingressos_aqui_carrinho_api.Dtos;

namespace Prime.UnitTests.Services
{
    public class TransactionServiceTest
    {
        private TransactionService _transactionService;
        private Mock<ITransactionRepository> _transactionServiceMock = new Mock<ITransactionRepository>();
        private Mock<ICieloClient> _cieloClienteMock = new Mock<ICieloClient>();
        private Mock<HttpClient> _httpClienteMock = new Mock<HttpClient>();
        private TestHttpClientFactory HttpClientFactory = new TestHttpClientFactory();

        [SetUp]
        public void SetUp()
        {
            _cieloClienteMock.Setup(x => x.CreateClient())
                .Returns(HttpClientFactory.CreateClient());
            _transactionServiceMock = new Mock<ITransactionRepository>();
            _transactionService = new TransactionService(
                _transactionServiceMock.Object,
                _cieloClienteMock.Object);

        }

        [Test]
        public void Given_IdPerson_and_IdEvent_When_save_Then_return_Id_Transaction()
        {
            //Arrange
            TransactionDto transaction = new TransactionDto()
            {
                IdCustomer = "6442dcb6523d52533aeb1ae4"
            };
            var messageReturn = "6442dcb6523d52533aeb1ae4";
            _transactionServiceMock.Setup(x => x.Save<object>(transaction)).Returns(Task.FromResult(messageReturn as object));

            //Act
            var resultMethod = _transactionService.SaveAsync(transaction);

            //Assert
            Assert.AreEqual(messageReturn, resultMethod.Result.Data);
        }

        [Test]
        public void Given_transaction_without_IdPerson_When_save_Then_return_message_miss_Person()
        {
            //Arrange
            TransactionDto transaction = new TransactionDto()
            {
                IdCustomer = ""
            };
            var messageReturn = "Usuário é obrigatório";
            _transactionServiceMock.Setup(x => x.Save<object>(transaction)).Returns(Task.FromResult(messageReturn as object));

            //Act
            var resultMethod = _transactionService.SaveAsync(transaction);

            //Assert
            Assert.AreEqual(messageReturn, resultMethod.Result.Message);
        }

        /*[Test]
        public void Given_transaction_without_IdPayment_When_save_Then_return_message_miss_PaymentMethod()
        {
            //Arrange
            Transaction transaction = new Transaction()
            {
                IdPaymentMethod = "",
                IdPerson = "6442dcb6523d52533aeb1ae4",
                Tax= new decimal(60)
            };
            var messageReturn = "Método de pagamento é obrigatório";
            _transactionServiceMock.Setup(x => x.Save<object>(transaction)).Returns(Task.FromResult(messageReturn as object));

            //Act
            var resultMethod = _transactionService.SaveAsync(transaction);

            //Assert
            Assert.AreEqual(messageReturn, resultMethod.Result.Message);
        }*/

        [Test]
        public void Given_transaction_and_IdPerson_lenth_20_When_save_Then_return_message_miss_Person()
        {
            //Arrange
            TransactionDto transaction = new TransactionDto()
            {
                IdCustomer = "6442dcb6523d52533aeb",
            };
            var messageReturn = "Usuário é obrigatório e está menor que 24 digitos";
            _transactionServiceMock.Setup(x => x.Save<object>(transaction)).Returns(Task.FromResult(messageReturn as object));

            //Act
            var resultMethod = _transactionService.SaveAsync(transaction);

            //Assert
            Assert.AreEqual(messageReturn, resultMethod.Result.Message);
        }

        /*[Test]
        public void Given_transaction_and_IdPayment_lenth_20_When_save_Then_return_message_miss_Payment()
        {
            //Arrange
            Transaction transaction = new Transaction()
            {
                IdPaymentMethod = "6442dcb6523d52533aeb",
                IdPerson = "6442dcb6523d52533aeb1ae4",
                Tax= new decimal(60)
            };
            var messageReturn = "Método de pagamento é obrigatório e está menor que 24 digitos";
            _transactionServiceMock.Setup(x => x.Save<object>(transaction)).Returns(Task.FromResult(messageReturn as object));

            //Act
            var resultMethod = _transactionService.SaveAsync(transaction);

            //Assert
            Assert.AreEqual(messageReturn, resultMethod.Result.Message);
        }*/

        [Test]
        public void Given_IdPerson_When_save_Then_return_message_internal_error()
        {
            //Arrange
            TransactionDto transaction = new TransactionDto()
            {
                IdCustomer = "6442dcb6523d52533aeb",
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
        public void Given_transaction_with_tickets_to_insert_When_SaveTransactionIten_Then_return_message_OK()
        {
            //Arrange
            TransactionDto transaction = new TransactionDto()
            {
                IdCustomer = "6442dcb6523d52533aeb1ae4",
                TransactionItensDto = new List<TransactionItenDto>(){
                    new TransactionItenDto(){
                        HalfPrice = false,
                        AmountTicket=10,
                        IdLot ="6442dcb6523d52533aeb1ae4"
                    }
                }
            };

            var messageReturn = "6442dcb6523d52533aeb1ae4";
            _transactionServiceMock.Setup(x => x.Save<object>(transaction)).Returns(Task.FromResult(messageReturn as object));
            _transactionServiceMock.Setup(x => x.SaveTransactionIten<object>(transaction)).Returns(Task.FromResult(messageReturn as object));

            //Act
            var resultMethod = _transactionService.SaveTransactionItenAsync(
                    transaction.IdCustomer,
                    messageReturn,
                    transaction.TransactionItensDto);

            //Assert
            Assert.AreEqual(messageReturn, resultMethod.Result.Data);
        }

        [Test]
        public void Given_transaction_with_tickets_without_ticketPrice_to_insert_When_SaveTransactionIten_Then_return_message_miss_valueTicket()
        {
            //Arrange
            TransactionDto transaction = new TransactionDto()
            {
                IdCustomer = "6442dcb6523d52533aeb1ae4",
                TransactionItensDto = new List<TransactionItenDto>(){
                    new TransactionItenDto(){
                        HalfPrice = false,
                        AmountTicket=10,
                        IdLot ="6442dcb6523d52533aeb1ae4"
                    }
                }
            };

            var messageReturn = "Valor do Ingresso é obrigatório";
            _transactionServiceMock.Setup(x => x.SaveTransactionIten<object>(transaction)).Returns(Task.FromResult(messageReturn as object));

            //Act
            var resultMethod = _transactionService.SaveTransactionItenAsync(
                    transaction.IdCustomer,
                    messageReturn,
                    transaction.TransactionItensDto);

            //Assert
            Assert.AreEqual(messageReturn, resultMethod.Result.Message);
        }

        [Test]
        public void Given_transaction_with_tickets_without_IdTicket_to_insert_When_SaveTransactionIten_Then_return_message_miss_IdTicket()
        {
            //Arrange
            TransactionDto transaction = new TransactionDto()
            {
                IdCustomer = "6442dcb6523d52533aeb1ae4",
                TransactionItensDto = new List<TransactionItenDto>(){
                    new TransactionItenDto(){
                        HalfPrice = false,
                        AmountTicket=10,
                        IdLot ="6442dcb6523d52533aeb1ae4"
                    }
                }
            };

            var messageReturn = "Ticket é obrigatório";
            _transactionServiceMock.Setup(x => x.SaveTransactionIten<object>(transaction)).Returns(Task.FromResult(messageReturn as object));

            //Act
            var resultMethod = _transactionService.SaveTransactionItenAsync(
                    transaction.IdCustomer,
                    messageReturn,
                    transaction.TransactionItensDto);

            //Assert
            Assert.AreEqual(messageReturn, resultMethod.Result.Message);
        }

        [Test]
        public void Given_transaction_with_tickets_without_Tax_to_insert_When_SaveTransactionIten_Then_return_message_miss_IdTransaction()
        {
            //Arrange
            TransactionDto transaction = new TransactionDto()
            {
                IdCustomer = "6442dcb6523d52533aeb1ae4",
                TransactionItensDto = new List<TransactionItenDto>(){
                    new TransactionItenDto(){
                        HalfPrice = false,
                        AmountTicket=10,
                        IdLot ="6442dcb6523d52533aeb1ae4"
                    }
                }
            };

            var messageReturn = "Transação é obrigatório";
            _transactionServiceMock.Setup(x => x.SaveTransactionIten<object>(transaction)).Returns(Task.FromResult(messageReturn as object));

            //Act
            var resultMethod = _transactionService.SaveTransactionItenAsync(
                    transaction.IdCustomer,
                    messageReturn,
                    transaction.TransactionItensDto);

            //Assert
            Assert.AreEqual(messageReturn, resultMethod.Result.Message);
        }

        [Test]
        public void Given_transaction_When_SaveTransactionIten_Then_return_message_internal_error()
        {
            //Arrange
            TransactionDto transaction = new TransactionDto()
            {
                IdCustomer = "6442dcb6523d52533aeb1ae4",
                TransactionItensDto = new List<TransactionItenDto>(){
                    new TransactionItenDto(){
                        HalfPrice = false,
                        AmountTicket=10,
                        IdLot ="6442dcb6523d52533aeb1ae4"
                    }
                }
            };

            var messageReturn = "erro ao conectar com base de dados";
            _transactionServiceMock.Setup(x => x.SaveTransactionIten<object>(It.IsAny<TransactionIten>))
                .Throws(new Exception(messageReturn));

            //Act
            var resultMethod = _transactionService.SaveTransactionItenAsync(
                    transaction.IdCustomer,
                    messageReturn,
                    transaction.TransactionItensDto);

            //Assert
            //Assert.That(ex?.Message, Is.EqualTo(messageReturn));
        }

        [Test]
        public void Given_transaction_with_payment_When_SendCielo_Then_return_message_OK()
        {
            //Arrange
            Transaction transaction = new Transaction()
            {
                Id = "6442dcb6523d52533aeb1ae4",
                Tax = new decimal(10),
                TransactionItens = new List<TransactionIten>(){
                    new TransactionIten(){
                        IdTicket ="6442dcb6523d52533aeb1ae4",
                        HalfPrice = false,
                        TicketPrice = new decimal(360)
                    }
                }
            };

            var messageReturn = "Ticket criado";

            //Act
            var resultMethod = _transactionService.Payment(transaction).Result;

            //Assert
            Assert.AreEqual(messageReturn, resultMethod.Data);
        }
    }
}