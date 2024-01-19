using Amg_ingressos_aqui_carrinho_api.Services;
using Amg_ingressos_aqui_carrinho_api.Infra;
using Moq;
using NUnit.Framework;

namespace Amg_ingressos_aqui_carrinho_tests.Services
{
    public class TicketServiceTest
    {
        private TicketService _ticketService;
        private Mock<ITransactionGatewayClient> _cieloClienteMock = new Mock<ITransactionGatewayClient>();
        private Mock<HttpClient> _httpClienteMock = new Mock<HttpClient>();
        private FactoryServices.TestHttpClientFactory HttpClientFactory = new FactoryServices.TestHttpClientFactory();

        [SetUp]
        public void SetUp()
        {
            /*_cieloClienteMock.Setup(x => x.CreateClient())
                .Returns(HttpClientFactory.CreateClient());*/
            //_ticketService = new TicketService(_cieloClienteMock.Object);

        }
    }
}