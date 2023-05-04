using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Amg_ingressos_aqui_eventos_api.Model;
using Moq;
using Moq.Protected;

namespace Amg_ingressos_aqui_carrinho_tests.FactoryServices
{
    public class TestHttpClientFactory : IHttpClientFactory
    {
        public HttpClient CreateClient(string name)
        {
            var handlerMock = new Mock<HttpMessageHandler>();
            string jsonString = JsonSerializer.Serialize(new List<Ticket>(){new Ticket() { 
                Id = "6442dcb6523d52533aeb1ae4",
                IdLot = "6442dcb6523d52533aeb1ae4",
                IdUser = null,
                isSold = false,
                Position= null,
                Value= 50
            }});
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(jsonString),
            };
            handlerMock
              .Protected()
              .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
              .ReturnsAsync(response);
            return new HttpClient(handlerMock.Object);
        }

    }
}