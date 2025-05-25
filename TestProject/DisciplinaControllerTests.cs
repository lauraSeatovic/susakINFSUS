using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Moq;
using susak.Controllers;
using susak.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TestProject
{

    public class DisciplinaControllerTests
    {
        [Fact]
        public async Task Index_ReturnsViewResult_WithListOfDiscipline()
        {
            // Arrange
            var mockService = new Mock<IDisciplinaService>();
            mockService.Setup(service => service.GetAllAsync(null))
                       .ReturnsAsync(GetTestDiscipline());

            var controller = new DisciplinaController(mockService.Object);

            // Act
            var result = await controller.Index(null);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<Disciplina>>(viewResult.Model);
            Assert.Equal(2, model.Count);
        }

        private List<Disciplina> GetTestDiscipline()
        {
            return new List<Disciplina>
        {
            new Disciplina { DisciplinaId = 1, Naziv = "Trcanje 50 m" },
            new Disciplina { DisciplinaId = 2, Naziv = "Brzo hodanje" }
        };
        }
    }

}
