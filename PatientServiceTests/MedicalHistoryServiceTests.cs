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
    public class MedicalHistoryServiceTests : IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly IMedicalHistoryService _service;

        public MedicalHistoryServiceTests()
        {
            // Configura o provedor InMemory para simular o banco de dados,
            // utilizando um nome único para isolar os testes.
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _context = new ApplicationDbContext(options);

            // Instancia a implementação do serviço MedicalHistoryService com o contexto configurado.
            _service = new MedicalHistoryService(_context);
        }

        [Fact]
        public async Task CreateMedicalHistoryAsync_ShouldAddHistory()
        {
            // Arrange: Cria um DTO para a criação de um histórico médico (sem o ID).
            var createDto = new CreateMedicalHistoryDto
            {
                PatientId = 1,
                Diagnoses = "Hipertensão",
                Exams = "Raio-X",
                Prescriptions = "Atenolol",
                RecordDate = DateTime.UtcNow
            };

            // Act: Chama o método de criação.
            var createdHistory = await _service.CreateMedicalHistoryAsync(createDto);

            // Assert: Verifica se o histórico foi criado com um ID válido e os dados corretos.
            Assert.NotNull(createdHistory);
            Assert.True(createdHistory.Id > 0);
            Assert.Equal("Hipertensão", createdHistory.Diagnoses);

            // Verifica se o registro foi persistido no contexto.
            var historyFromDb = await _context.MedicalHistories.FindAsync(createdHistory.Id);
            Assert.NotNull(historyFromDb);
            Assert.Equal("Hipertensão", historyFromDb.Diagnoses);
        }

        [Fact]
        public async Task GetMedicalHistoryByPatientAsync_ShouldReturnHistories()
        {
            // Arrange: Insere dois históricos médicos para o mesmo paciente (PatientId = 1)
            _context.MedicalHistories.AddRange(
                new MedicalHistory
                {
                    PatientId = 1,
                    Diagnoses = "Diagnóstico 1",
                    ExamsPerformed = "Exame 1",
                    Prescriptions = "Prescrição 1",
                    RecordDate = DateTime.UtcNow,
                    IsDeleted = false
                },
                new MedicalHistory
                {
                    PatientId = 1,
                    Diagnoses = "Diagnóstico 2",
                    ExamsPerformed = "Exame 2",
                    Prescriptions = "Prescrição 2",
                    RecordDate = DateTime.UtcNow,
                    IsDeleted = false
                }
            );
            await _context.SaveChangesAsync();

            // Act: Chama o método para obter o histórico médico de um paciente.
            var histories = await _service.GetMedicalHistoryByPatientAsync(1);

            // Assert: Verifica se dois históricos são retornados.
            Assert.NotNull(histories);
            Assert.Equal(2, histories.Count());
        }

        [Fact]
        public async Task UpdateMedicalHistoryAsync_ShouldModifyHistoryDetails()
        {
            // Arrange: Insere um histórico médico no banco.
            var history = new MedicalHistory
            {
                PatientId = 2,
                Diagnoses = "Diabetes",
                ExamsPerformed = "Glicemia",
                Prescriptions = "Metformina",
                RecordDate = DateTime.UtcNow,
                IsDeleted = false
            };
            _context.MedicalHistories.Add(history);
            await _context.SaveChangesAsync();

            // Prepara um DTO de atualização (não inclui o Id, pois este virá como parâmetro).
            var updateDto = new UpdateMedicalHistoryDto
            {
                PatientId = history.PatientId,
                Diagnoses = "Diabetes Tipo 2", // Atualização no diagnóstico
                Exams = "Glicemia e Hemoglobina", // Atualização nos exames
                Prescriptions = "Metformina 500mg", // Atualização na prescrição
                RecordDate = history.RecordDate // Mantém a mesma data
            };

            // Act: Atualiza o histórico médico passando o ID como parâmetro.
            var updatedHistory = await _service.UpdateMedicalHistoryAsync(history.Id, updateDto);

            // Assert: Verifica se os dados foram atualizados corretamente.
            Assert.NotNull(updatedHistory);
            Assert.Equal("Diabetes Tipo 2", updatedHistory.Diagnoses);
            Assert.Equal("Metformina 500mg", updatedHistory.Prescriptions);
            Assert.Equal("Glicemia e Hemoglobina", updatedHistory.Exams);
        }

        [Fact]
        public async Task DeleteMedicalHistoryAsync_ShouldMarkHistoryAsDeleted()
        {
            // Arrange: Insere um histórico médico no banco.
            var history = new MedicalHistory
            {
                PatientId = 3,
                Diagnoses = "Asma",
                ExamsPerformed = "Teste de função pulmonar",
                Prescriptions = "Inalador",
                RecordDate = DateTime.UtcNow,
                IsDeleted = false
            };
            _context.MedicalHistories.Add(history);
            await _context.SaveChangesAsync();

            // Act: Chama o método de exclusão (soft delete).
            var result = await _service.DeleteMedicalHistoryAsync(history.Id);

            // Assert: Verifica se o registro foi marcado como excluído.
            Assert.True(result);
            var historyFromDb = await _context.MedicalHistories.FindAsync(history.Id);
            Assert.True(historyFromDb.IsDeleted);
        }

        public void Dispose()
        {
            // Garante que o banco InMemory seja apagado após os testes para evitar interferência.
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}