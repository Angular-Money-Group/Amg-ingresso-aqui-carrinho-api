using Amg_ingressos_aqui_carrinho_tests.FactoryServices;
using Amg_ingressos_aqui_carrinho_api.Services;
using NUnit.Framework;
using Moq;
using Amg_ingressos_aqui_carrinho_api.Repository.Interfaces;
using Amg_ingressos_aqui_carrinho_api.Model;
using Amg_ingressos_aqui_carrinho_api.Enum;

namespace Prime.UnitTests.Services
{
    public class TransactionServiceTest
    {
        private TransactionService _transactionService;
        private Mock<ITransactionRepository> _transactionServiceMock = new Mock<ITransactionRepository>();

        [SetUp]
        public void SetUp()
        {
            _transactionServiceMock = new Mock<ITransactionRepository>();
            _transactionService = new TransactionService(_transactionServiceMock.Object);
        }

        [Test]
        public void Given_IdPerson_and_IdEvent_When_save_Then_return_Id_Transaction()
        {
            //Arrange
            Transaction transaction = new Transaction()
            {
                IdEvent = "6442dcb6523d52533aeb1ae4",
                IdPerson = "6442dcb6523d52533aeb1ae4"
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
            Transaction transaction = new Transaction()
            {
                IdEvent = "6442dcb6523d52533aeb1ae4",
                IdPerson = ""
            };
            var messageReturn = "Usuário é obrigatório";
            _transactionServiceMock.Setup(x => x.Save<object>(transaction)).Returns(Task.FromResult(messageReturn as object));

            //Act
            var resultMethod = _transactionService.SaveAsync(transaction);

            //Assert
            Assert.AreEqual(messageReturn, resultMethod.Result.Message);
        }

        [Test]
        public void Given_transaction_without_IdEvent_When_save_Then_return_message_miss_Event()
        {
            //Arrange
            Transaction transaction = new Transaction()
            {
                IdEvent = "",
                IdPerson = "6442dcb6523d52533aeb1ae4"
            };
            var messageReturn = "Evento é obrigatório";
            _transactionServiceMock.Setup(x => x.Save<object>(transaction)).Returns(Task.FromResult(messageReturn as object));

            //Act
            var resultMethod = _transactionService.SaveAsync(transaction);

            //Assert
            Assert.AreEqual(messageReturn, resultMethod.Result.Message);
        }

        [Test]
        public void Given_transaction_and_IdPerson_lenth_20_When_save_Then_return_message_miss_Person()
        {
            //Arrange
            Transaction transaction = new Transaction()
            {
                IdEvent = "6442dcb6523d52533aeb1ae4",
                IdPerson = "6442dcb6523d52533aeb"
            };
            var messageReturn = "Usuário é obrigatório e está menor que 24 digitos";
            _transactionServiceMock.Setup(x => x.Save<object>(transaction)).Returns(Task.FromResult(messageReturn as object));

            //Act
            var resultMethod = _transactionService.SaveAsync(transaction);

            //Assert
            Assert.AreEqual(messageReturn, resultMethod.Result.Message);
        }

        [Test]
        public void Given_transaction_and_IdEvent_lenth_20_When_save_Then_return_message_miss_Event()
        {
            //Arrange
            Transaction transaction = new Transaction()
            {
                IdEvent = "6442dcb6523d52533aeb",
                IdPerson = "6442dcb6523d52533aeb1ae4"
            };
            var messageReturn = "Evento é obrigatório e está menor que 24 digitos";
            _transactionServiceMock.Setup(x => x.Save<object>(transaction)).Returns(Task.FromResult(messageReturn as object));

            //Act
            var resultMethod = _transactionService.SaveAsync(transaction);

            //Assert
            Assert.AreEqual(messageReturn, resultMethod.Result.Message);
        }

        [Test]
        public void Given_IdPerson_and_IdEvent_When_save_Then_return_message_internal_error()
        {
            //Arrange
            Transaction transaction = new Transaction()
            {
                IdEvent = "6442dcb6523d52533aeb1ae4",
                IdPerson = "6442dcb6523d52533aeb1ae4"
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
            Transaction transaction = new Transaction()
            {
                Id = "6442dcb6523d52533aeb1ae4",
                TransactionItens = new List<TransactionIten>(){
                    new TransactionIten(){
                        AmoutTicket = 2,
                        HalfPrice = false,
                        IdLote = "6442dcb6523d52533aeb1ae4",
                        IdVariante= "6442dcb6523d52533aeb1ae4",
                        Tax= new decimal(60),
                        TicketPrice = new decimal(360)
                    }
                }
            };

            var messageReturn = "Tikets Criados";
            _transactionServiceMock.Setup(x => x.SaveTransactionIten<object>(transaction)).Returns(Task.FromResult(messageReturn as object));

            //Act
            var resultMethod = _transactionService.SaveTransactionItenAsync(transaction);

            //Assert
            Assert.AreEqual(messageReturn, resultMethod.Result.Data);
        }

        [Test]
        public void Given_transaction_with_tickets_without_amount_to_insert_When_SaveTransactionIten_Then_return_message_miss_amount()
        {
            //Arrange
            Transaction transaction = new Transaction()
            {
                Id = "6442dcb6523d52533aeb1ae4",
                TransactionItens = new List<TransactionIten>(){
                    new TransactionIten(){
                        AmoutTicket = 0,
                        HalfPrice = false,
                        IdLote = "6442dcb6523d52533aeb1ae4",
                        IdVariante= "6442dcb6523d52533aeb1ae4",
                        Tax= new decimal(60),
                        TicketPrice = new decimal(360)
                    }
                }
            };

            var messageReturn = "Quantidade de Ingresso é obrigatório";
            _transactionServiceMock.Setup(x => x.SaveTransactionIten<object>(transaction)).Returns(Task.FromResult(messageReturn as object));

            //Act
            var resultMethod = _transactionService.SaveTransactionItenAsync(transaction);

            //Assert
            Assert.AreEqual(messageReturn, resultMethod.Result.Message);
        }

        [Test]
        public void Given_transaction_with_tickets_without_ticketPrice_to_insert_When_SaveTransactionIten_Then_return_message_miss_valueTicket()
        {
            //Arrange
            Transaction transaction = new Transaction()
            {
                Id = "6442dcb6523d52533aeb1ae4",
                TransactionItens = new List<TransactionIten>(){
                    new TransactionIten(){
                        AmoutTicket = 5,
                        HalfPrice = false,
                        IdLote = "6442dcb6523d52533aeb1ae4",
                        IdVariante= "6442dcb6523d52533aeb1ae4",
                        Tax= new decimal(60),
                        TicketPrice = new decimal(0)
                    }
                }
            };

            var messageReturn = "Valor do Ingresso é obrigatório";
            _transactionServiceMock.Setup(x => x.SaveTransactionIten<object>(transaction)).Returns(Task.FromResult(messageReturn as object));

            //Act
            var resultMethod = _transactionService.SaveTransactionItenAsync(transaction);

            //Assert
            Assert.AreEqual(messageReturn, resultMethod.Result.Message);
        }

        [Test]
        public void Given_transaction_with_tickets_without_idVariant_to_insert_When_SaveTransactionIten_Then_return_message_miss_IdVariante()
        {
            //Arrange
            Transaction transaction = new Transaction()
            {
                Id = "6442dcb6523d52533aeb1ae4",
                TransactionItens = new List<TransactionIten>(){
                    new TransactionIten(){
                        AmoutTicket = 5,
                        HalfPrice = false,
                        IdLote = "6442dcb6523d52533aeb1ae4",
                        IdVariante= string.Empty,
                        Tax= new decimal(60),
                        TicketPrice = new decimal(0)
                    }
                }
            };

            var messageReturn = "Variante é obrigatório";
            _transactionServiceMock.Setup(x => x.SaveTransactionIten<object>(transaction)).Returns(Task.FromResult(messageReturn as object));

            //Act
            var resultMethod = _transactionService.SaveTransactionItenAsync(transaction);

            //Assert
            Assert.AreEqual(messageReturn, resultMethod.Result.Message);
        }

        [Test]
        public void Given_transaction_with_tickets_without_idLote_to_insert_When_SaveTransactionIten_Then_return_message_miss_IdLote()
        {
            //Arrange
            Transaction transaction = new Transaction()
            {
                Id = "6442dcb6523d52533aeb1ae4",
                TransactionItens = new List<TransactionIten>(){
                    new TransactionIten(){
                        AmoutTicket = 5,
                        HalfPrice = false,
                        IdLote = string.Empty,
                        IdVariante= "6442dcb6523d52533aeb1ae4",
                        Tax= new decimal(60),
                        TicketPrice = new decimal(0)
                    }
                }
            };

            var messageReturn = "Lote é obrigatório";
            _transactionServiceMock.Setup(x => x.SaveTransactionIten<object>(transaction)).Returns(Task.FromResult(messageReturn as object));

            //Act
            var resultMethod = _transactionService.SaveTransactionItenAsync(transaction);

            //Assert
            Assert.AreEqual(messageReturn, resultMethod.Result.Message);
        }

        [Test]
        public void Given_transaction_When_SaveTransactionIten_Then_return_message_internal_error()
        {
            //Arrange
            Transaction transaction = new Transaction()
            {
                Id = "6442dcb6523d52533aeb1ae4",
                TransactionItens = new List<TransactionIten>(){
                    new TransactionIten(){
                        AmoutTicket = 2,
                        HalfPrice = false,
                        IdLote = "6442dcb6523d52533aeb1ae4",
                        IdVariante= "6442dcb6523d52533aeb1ae4",
                        Tax= new decimal(60),
                        TicketPrice = new decimal(360)
                    }
                }
            };

            var messageReturn = "erro ao conectar com base de dados";
            _transactionServiceMock.Setup(x => x.SaveTransactionIten<object>(transaction.TransactionItens.FirstOrDefault()))
                .Throws(new Exception(messageReturn));

            //Act
            var ex = Assert.ThrowsAsync<Exception>(() => _transactionService.SaveTransactionItenAsync(transaction));

            //Assert
            Assert.That(ex?.Message, Is.EqualTo(messageReturn));
        }
    }
}