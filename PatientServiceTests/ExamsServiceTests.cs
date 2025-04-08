using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;
using Patient.Application.DTOs;
using Patient.Application.Interfaces;
using Patient.Infrastructure.Models;
using Patient.Infrastructure.Services;
using Xunit;

namespace Patient.Infrastructure.Tests
{
    public class ExamsServiceTests
    {
        [Fact]
        public async Task GetExamsAsync_WithValidCep_ReturnsExamDtoList()
        {
            // Arrange: Cria um objeto simulando a resposta da BrasilAPI para CEP
            var fakeCepResponse = new BRCepResponse
            {
                Cep = "01001000",
                State = "SP",
                City = "São Paulo",
                Neighborhood = "Sé",
                Street = "Praça da Sé",
                Complement = "lado ímpar"
            };

            var jsonResponse = JsonSerializer.Serialize(fakeCepResponse);
            var httpResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(jsonResponse, Encoding.UTF8, "application/json")
            };

            // Configura o HttpMessageHandler para simular a resposta
            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(httpResponse);

            // Cria um HttpClient usando o handler simulado e define uma base address
            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("https://brasilapi.com.br/")
            };

            // Instancia o ExamsService com o HttpClient configurado
            IExamsService examsService = new ExamsService(httpClient);

            // Act: Chama o método passando o CEP como filtro
            var result = await examsService.GetExamsAsync("01001000");

            // Assert: Verifica se a resposta não é nula e tem ao menos um item
            Assert.NotNull(result);
            var examsList = new List<ExamDto>(result);
            Assert.Single(examsList);

            // Valida o conteúdo mapeado
            var exam = examsList[0];
            Assert.Equal("Exame de CEP 01001000", exam.ExamName);
            Assert.Equal("Cidade: São Paulo, Bairro: Sé", exam.Result);
            Assert.True(exam.ExamDate > DateTime.MinValue);
        }

        [Fact]
        public async Task GetExamsAsync_WithErrorResponse_ThrowsHttpRequestException()
        {
            // Arrange: Simula uma resposta com código 403 Forbidden
            var httpResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.Forbidden,
                Content = new StringContent("Forbidden")
            };

            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(httpResponse);

            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("https://brasilapi.com.br/")
            };

            IExamsService examsService = new ExamsService(httpClient);

            // Act & Assert: Verifica se uma exceção é lançada ao receber uma resposta de erro
            await Assert.ThrowsAsync<HttpRequestException>(() => examsService.GetExamsAsync("01001000"));
        }
    }
}