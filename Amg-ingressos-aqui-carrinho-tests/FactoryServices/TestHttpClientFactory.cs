using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;

namespace Amg_ingressos_aqui_carrinho_tests.FactoryServices
{
    public class TestHttpClientFactory : IHttpClientFactory
    {
        public HttpClient CreateClient(string name)
        {
            var handlerMock = new Mock<HttpMessageHandler>();
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(@"{ ""id"": 101 }"),
            };
            handlerMock
              .Protected()
              .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
              .ReturnsAsync(response);
            return new HttpClient(handlerMock.Object);


            var messageHandlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);

            messageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .Returns(async (HttpRequestMessage request, CancellationToken token) =>
                {

                    string requestMessageContent = await request.Content.ReadAsStringAsync();

                    HttpResponseMessage response = new HttpResponseMessage
                    {
                        StatusCode = HttpStatusCode.OK,
                        Content = new StringContent(@"{ ""id"": 101 }"),
                    };

                    //...decide what to put in the response after looking at the contents of the request

                    return response;
                })
                .Verifiable();

            var httpClient = new HttpClient(messageHandlerMock.Object);
            return httpClient;
        }

    }
}