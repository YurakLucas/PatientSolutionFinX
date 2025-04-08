using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Patient.Application.DTOs;
using Xunit;

namespace Patient.API.IntegrationTests
{
    public class PatientsControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public PatientsControllerIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetAllPatients_ReturnsOk_WithListOfPatients()
        {
            // 1. Autenticação: Obter o token JWT via endpoint de login.
            var loginRequest = new
            {
                Username = "admin",
                Password = "password123"
            };

            var loginResponse = await _client.PostAsJsonAsync("/api/auth/login", loginRequest);
            loginResponse.EnsureSuccessStatusCode();

            var loginContent = await loginResponse.Content.ReadAsStringAsync();
            using var jsonDoc = JsonDocument.Parse(loginContent);
            var token = jsonDoc.RootElement.GetProperty("token").GetString();
            Assert.False(string.IsNullOrWhiteSpace(token));

            // Configura o header de autorização com o token obtido
            _client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var createPatient = new
            {
                Name = "Test Patient",
                CPF = "12345678901",
                DateOfBirth = "1980-01-01T00:00:00",
                Contact = "555-0000"
            };

            var createResponse = await _client.PostAsJsonAsync("/api/patients", createPatient);
            createResponse.EnsureSuccessStatusCode();

            // 3. Chama o endpoint GET /api/patients para obter a lista de pacientes
            var response = await _client.GetAsync("/api/patients");
            response.EnsureSuccessStatusCode();

            // Deserializa a resposta para uma coleção de PatientDto
            var patients = await response.Content.ReadFromJsonAsync<IEnumerable<PatientDto>>();
            Assert.NotNull(patients);
            // Agora deve haver pelo menos um paciente
            Assert.True(patients.Any(), "Expected at least one patient in the result.");
        }
    }
}