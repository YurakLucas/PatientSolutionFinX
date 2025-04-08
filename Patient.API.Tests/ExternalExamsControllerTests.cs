using Xunit;
using Moq;
using Patient.Application.DTOs;
using Patient.Application.Interfaces;
using Patient.API.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace Patient.API.Tests
{
    public class ExternalExamsControllerTests
    {
        [Fact]
        public async Task GetExams_WithValidFilter_ShouldReturnOkResult()
        {
            // Arrange
            var mockExamsService = new Mock<IExamsService>();
            string filter = "sangue";
            var exams = new List<ExamDto>
            {
                new ExamDto { ExamName = "Hemograma", Result = "Normal", ExamDate = System.DateTime.UtcNow },
                new ExamDto { ExamName = "Glicemia", Result = "Elevada", ExamDate = System.DateTime.UtcNow }
            };

            mockExamsService.Setup(s => s.GetExamsAsync(filter)).ReturnsAsync(exams);
            var controller = new ExternalExamsController(mockExamsService.Object);

            // Act
            var result = await controller.GetExams(filter);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedExams = Assert.IsAssignableFrom<IEnumerable<ExamDto>>(okResult.Value);
            Assert.Equal(2, returnedExams.Count());
        }
    }
}