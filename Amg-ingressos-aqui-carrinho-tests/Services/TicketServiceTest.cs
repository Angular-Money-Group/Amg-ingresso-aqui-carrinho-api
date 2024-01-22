using Amg_ingressos_aqui_carrinho_api.Services;
using Amg_ingressos_aqui_carrinho_api.Infra;
using Moq;
using NUnit.Framework;
using Microsoft.Extensions.Logging;

namespace Amg_ingressos_aqui_carrinho_tests.Services
{
    public class TicketServiceTest
    {
        private TicketService _ticketService;
        private readonly Mock<ILogger<TicketService>> _loggerMock = new Mock<ILogger<TicketService>>();

        [SetUp]
        public void SetUp()
        {
            _ticketService = new TicketService(_loggerMock.Object);
        }
    }
}