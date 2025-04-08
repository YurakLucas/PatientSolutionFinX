using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Patient.Application.DTOs;
using Patient.Application.Interfaces;
using Patient.Infrastructure.Models;

namespace Patient.Infrastructure.Services
{
    public class ExamsService : IExamsService
    {
        private readonly HttpClient _httpClient;

        public ExamsService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<ExamDto>> GetExamsAsync(string filter)
        {
            // BrasilAPI para buscar dados de CEP.
            // O parâmetro filter pode ser interpretado como um CEP.
            // Exemplo: "01001000" (CEP de São Paulo)
            string cep = filter;
            string url = $"https://brasilapi.com.br/api/cep/v2/{cep}";

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var jsonString = await response.Content.ReadAsStringAsync();

            var cepResult = JsonSerializer.Deserialize<BRCepResponse>(jsonString, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            // Mapeia os dados retornados da API para ExamDto
            // Dados do endereço para simular informações de "exame"
            var exam = new ExamDto
            {
                ExamName = $"Exame de CEP {cepResult.Cep}",
                Result = $"Cidade: {cepResult.City}, Bairro: {cepResult.Neighborhood}",
                ExamDate = DateTime.UtcNow
            };

            return new List<ExamDto> { exam };
        }
    }
}