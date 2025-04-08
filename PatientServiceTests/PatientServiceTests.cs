using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Patient.Application.DTOs;
using Patient.Application.Interfaces;
using Patient.Domain.Entities;
using Patient.Infrastructure.Data;
using Patient.Infrastructure.Services;
using Xunit;

namespace Patient.Infrastructure.Tests
{
    public class PatientServiceTests : IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly IPatientService _patientService;

        public PatientServiceTests()
        {
            // Configura o provedor InMemory para simular o banco de dados com um nome único a cada teste.
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _context = new ApplicationDbContext(options);

            // Instancia a implementação do serviço PatientService com o contexto configurado.
            _patientService = new PatientService(_context);
        }

        [Fact]
        public async Task CreatePatientAsync_ShouldReturnCreatedPatient_WithGeneratedId()
        {
            // Arrange: Cria um DTO para a criação de um paciente sem o ID (usamos o CreatePatientDto)
            var createDto = new CreatePatientDto
            {
                Name = "John Doe",
                CPF = "12345678901",
                DateOfBirth = new DateTime(1980, 1, 1),
                Contact = "555-1234"
            };

            // Act: Cria o paciente utilizando o serviço
            var createdPatient = await _patientService.CreatePatientAsync(createDto);

            // Assert: Verifica se o paciente foi criado e se um ID foi atribuído
            Assert.NotNull(createdPatient);
            Assert.True(createdPatient.Id > 0);
            Assert.Equal("John Doe", createdPatient.Name);

            // Valida se o paciente está presente no banco
            var patientFromDb = await _context.Patients.FindAsync(createdPatient.Id);
            Assert.NotNull(patientFromDb);
            Assert.Equal("John Doe", patientFromDb.Name);
        }

        [Fact]
        public async Task GetAllPatientsAsync_ShouldReturnAllPatients()
        {
            // Arrange: Adiciona dois pacientes diretamente no contexto
            _context.Patients.AddRange(
                new Domain.Entities.Patient { Name = "Alice", CPF = "11111111111", DateOfBirth = new DateTime(1985, 6, 15), Contact = "555-1111" },
                new Domain.Entities.Patient { Name = "Bob", CPF = "22222222222", DateOfBirth = new DateTime(1990, 8, 25), Contact = "555-2222" }
            );
            await _context.SaveChangesAsync();

            // Act: Recupera a lista de pacientes via serviço
            var patients = await _patientService.GetAllPatientsAsync();

            // Assert: Verifica se a quantidade de pacientes retornada é 2
            Assert.NotNull(patients);
            Assert.Equal(2, patients.Count());
        }

        [Fact]
        public async Task GetPatientByIdAsync_ShouldReturnPatient_WhenExists()
        {
            // Arrange: Cria um paciente e adiciona no banco
            var patient = new Domain.Entities.Patient { Name = "Charlie", CPF = "33333333333", DateOfBirth = new DateTime(1975, 3, 10), Contact = "555-3333" };
            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();

            // Act: Recupera o paciente pelo ID utilizando o serviço
            var patientDto = await _patientService.GetPatientByIdAsync(patient.Id);

            // Assert: Verifica se o paciente retornado não é nulo e os dados batem com o inserido
            Assert.NotNull(patientDto);
            Assert.Equal("Charlie", patientDto.Name);
        }

        [Fact]
        public async Task UpdatePatientAsync_ShouldModifyPatientDetails()
        {
            // Arrange: Cria e salva um paciente
            var patient = new Domain.Entities.Patient { Name = "David", CPF = "44444444444", DateOfBirth = new DateTime(1995, 12, 12), Contact = "555-4444" };
            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();

            // Prepara os dados de atualização via UpdatePatientDto (os dados de entrada não incluem o ID)
            var updateDto = new UpdatePatientDto
            {
                Name = "David Updated",
                CPF = "44444444444",
                DateOfBirth = patient.DateOfBirth,
                Contact = "555-9999"
            };

            // Act: Atualiza o paciente, passando o ID via parâmetro (rota) e o DTO de atualização
            var updatedPatient = await _patientService.UpdatePatientAsync(patient.Id, updateDto);

            // Assert: Verifica se os dados foram atualizados corretamente
            Assert.NotNull(updatedPatient);
            Assert.Equal("David Updated", updatedPatient.Name);
            Assert.Equal("555-9999", updatedPatient.Contact);
        }

        [Fact]
        public async Task DeletePatientAsync_ShouldRemovePatient()
        {
            // Arrange: Cria e salva um paciente
            var patient = new Domain.Entities.Patient { Name = "Eve", CPF = "55555555555", DateOfBirth = new DateTime(2000, 1, 1), Contact = "555-5555" };
            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();

            // Act: Chama o método de exclusão
            var deleteResult = await _patientService.DeletePatientAsync(patient.Id);

            // Assert: Verifica se o paciente foi removido
            Assert.True(deleteResult);
            var deletedPatient = await _context.Patients.FindAsync(patient.Id);
            Assert.Null(deletedPatient);
        }

        public void Dispose()
        {
            // Apaga o banco de dados em memória para garantir testes isolados
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}