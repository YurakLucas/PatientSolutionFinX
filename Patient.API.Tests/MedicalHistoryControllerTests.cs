using Xunit;
using Moq;
using Patient.Application.DTOs;
using Patient.Application.Interfaces;
using Patient.API.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Patient.API.Tests
{
    public class MedicalHistoryControllerTests
    {
        [Fact]
        public async Task GetMedicalHistoryByPatient_ShouldReturnOk_WithHistories()
        {
            // Arrange
            var mockService = new Mock<IMedicalHistoryService>();
            int patientId = 1;
            var histories = new List<MedicalHistoryDto>
            {
                new MedicalHistoryDto
                {
                    Id = 1,
                    PatientId = patientId,
                    Diagnoses = "Hipertensão",
                    Exams = "Raio-X",
                    Prescriptions = "Atenolol",
                    RecordDate = DateTime.UtcNow
                },
                new MedicalHistoryDto
                {
                    Id = 2,
                    PatientId = patientId,
                    Diagnoses = "Diabetes",
                    Exams = "Glicemia",
                    Prescriptions = "Metformina",
                    RecordDate = DateTime.UtcNow
                }
            };

            mockService.Setup(s => s.GetMedicalHistoryByPatientAsync(patientId)).ReturnsAsync(histories);
            var controller = new MedicalHistoryController(mockService.Object);

            // Act
            var result = await controller.GetHistoryByPatient(patientId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedHistories = Assert.IsAssignableFrom<IEnumerable<MedicalHistoryDto>>(okResult.Value);
            Assert.Equal(2, returnedHistories.Count());
        }

        [Fact]
        public async Task CreateMedicalHistory_InvalidData_ShouldReturnBadRequest()
        {
            // Arrange
            var mockService = new Mock<IMedicalHistoryService>();
            var controller = new MedicalHistoryController(mockService.Object);

            // Cria um objeto nulo ou inválido
            CreateMedicalHistoryDto historyDto = null;

            // Act
            var result = await controller.Create(historyDto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}