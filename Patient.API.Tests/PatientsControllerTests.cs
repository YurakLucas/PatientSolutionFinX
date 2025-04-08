using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Patient.API.Controllers;
using Patient.Application.DTOs;
using Patient.Application.Interfaces;
using Xunit;

namespace Patient.API.Tests
{
    public class PatientsControllerTests
    {
        [Fact]
        public async Task GetAll_ShouldReturnOkResult_WithListOfPatients()
        {
            // Arrange
            var mockService = new Mock<IPatientService>();
            var patients = new List<PatientDto>
            {
                new PatientDto { Id = 1, Name = "Alice", CPF = "11111111111", DateOfBirth = new DateTime(1985, 6, 15), Contact = "555-1111" },
                new PatientDto { Id = 2, Name = "Bob", CPF = "22222222222", DateOfBirth = new DateTime(1990, 8, 25), Contact = "555-2222" }
            };
            mockService.Setup(s => s.GetAllPatientsAsync())
                       .ReturnsAsync(patients);

            var controller = new PatientsController(mockService.Object);

            // Act
            var result = await controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedPatients = Assert.IsAssignableFrom<IEnumerable<PatientDto>>(okResult.Value);
            Assert.Equal(2, returnedPatients.Count());
        }

        [Fact]
        public async Task GetById_WithInvalidId_ShouldReturnNotFound()
        {
            // Arrange
            var mockService = new Mock<IPatientService>();
            mockService.Setup(s => s.GetPatientByIdAsync(It.IsAny<int>()))
                       .ReturnsAsync((PatientDto)null);
            var controller = new PatientsController(mockService.Object);

            // Act
            var result = await controller.GetById(999);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetById_WithValidId_ShouldReturnPatient()
        {
            // Arrange
            var mockService = new Mock<IPatientService>();
            var patient = new PatientDto { Id = 1, Name = "Charlie", CPF = "33333333333", DateOfBirth = new DateTime(1975, 3, 10), Contact = "555-3333" };
            mockService.Setup(s => s.GetPatientByIdAsync(patient.Id))
                       .ReturnsAsync(patient);
            var controller = new PatientsController(mockService.Object);

            // Act
            var result = await controller.GetById(patient.Id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedPatient = Assert.IsType<PatientDto>(okResult.Value);
            Assert.Equal("Charlie", returnedPatient.Name);
        }

        [Fact]
        public async Task CreatePatient_WithValidData_ShouldReturnCreatedPatient()
        {
            // Arrange
            var mockService = new Mock<IPatientService>();
            var createDto = new CreatePatientDto
            {
                Name = "David",
                CPF = "44444444444",
                DateOfBirth = new DateTime(1995, 12, 12),
                Contact = "555-4444"
            };
            // Simulamos a criação do paciente, atribuindo um ID gerado
            var createdPatient = new PatientDto
            {
                Id = 3,
                Name = createDto.Name,
                CPF = createDto.CPF,
                DateOfBirth = createDto.DateOfBirth,
                Contact = createDto.Contact
            };
            mockService.Setup(s => s.CreatePatientAsync(createDto))
                       .ReturnsAsync(createdPatient);
            var controller = new PatientsController(mockService.Object);

            // Act
            var result = await controller.Create(createDto);

            // Assert
            var createdAtResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnedPatient = Assert.IsType<PatientDto>(createdAtResult.Value);
            Assert.Equal(3, returnedPatient.Id);
            Assert.Equal("David", returnedPatient.Name);
        }

        [Fact]
        public async Task UpdatePatient_WithValidData_ShouldReturnUpdatedPatient()
        {
            // Arrange
            var mockService = new Mock<IPatientService>();
            int patientId = 4;
            var updateDto = new UpdatePatientDto
            {
                Name = "Eva Updated",
                CPF = "55555555555",
                DateOfBirth = new DateTime(2000, 1, 1),
                Contact = "555-9999"
            };

            var updatedPatient = new PatientDto
            {
                Id = patientId,
                Name = updateDto.Name,
                CPF = updateDto.CPF,
                DateOfBirth = updateDto.DateOfBirth,
                Contact = updateDto.Contact
            };

            mockService.Setup(s => s.UpdatePatientAsync(patientId, updateDto))
                       .ReturnsAsync(updatedPatient);

            var controller = new PatientsController(mockService.Object);

            // Act
            var result = await controller.Update(patientId, updateDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedPatient = Assert.IsType<PatientDto>(okResult.Value);
            Assert.Equal("Eva Updated", returnedPatient.Name);
            Assert.Equal("555-9999", returnedPatient.Contact);
        }

        [Fact]
        public async Task DeletePatient_WithValidId_ShouldReturnNoContent()
        {
            // Arrange
            var mockService = new Mock<IPatientService>();
            int patientId = 5;
            mockService.Setup(s => s.DeletePatientAsync(patientId))
                       .ReturnsAsync(true);
            var controller = new PatientsController(mockService.Object);

            // Act
            var result = await controller.Delete(patientId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeletePatient_WithInvalidId_ShouldReturnNotFound()
        {
            // Arrange
            var mockService = new Mock<IPatientService>();
            int patientId = 999;
            mockService.Setup(s => s.DeletePatientAsync(patientId))
                       .ReturnsAsync(false);
            var controller = new PatientsController(mockService.Object);

            // Act
            var result = await controller.Delete(patientId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}