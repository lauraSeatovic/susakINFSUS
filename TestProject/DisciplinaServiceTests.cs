using Xunit;

namespace TestProject
{
    using Xunit;
    using Moq;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Linq;
    using susak.Models;

    public class DisciplinaServiceTests
    {
        private readonly Mock<IDisciplinaRepository> _repositoryMock;
        private readonly DisciplinaService _service;

        public DisciplinaServiceTests()
        {
            _repositoryMock = new Mock<IDisciplinaRepository>();
            _service = new DisciplinaService(_repositoryMock.Object);
        }

        [Fact]
        public async Task GetAllAsync_NoSearch_ReturnsAllDisciplines()
        {
            // Arrange
            var data = new List<Disciplina>
        {
            new Disciplina { DisciplinaId = 1, Naziv = "Trcanje" },
            new Disciplina { DisciplinaId = 2, Naziv = "Sprint" }
        };

            _repositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(data);

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains(result, d => d.Naziv == "Trcanje");
            Assert.Contains(result, d => d.Naziv == "Sprint");
        }

        [Fact]
        public async Task GetAllAsync_WithSearch_ReturnsFilteredDisciplines()
        {
            // Arrange
            var data = new List<Disciplina>
        {
            new Disciplina { DisciplinaId = 1, Naziv = "Trcanje" },
            new Disciplina { DisciplinaId = 2, Naziv = "Sprint" },
            new Disciplina { DisciplinaId = 3, Naziv = "Bacanje koplja" }
        };

            _repositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(data);

            // Act
            var result = await _service.GetAllAsync("trc");

            // Assert
            Assert.Single(result);
            Assert.Equal("Trcanje", result.First().Naziv);
        }

        [Fact]
        public async Task GetAllAsync_SearchIsCaseInsensitive()
        {
            // Arrange
            var data = new List<Disciplina>
        {
            new Disciplina { DisciplinaId = 1, Naziv = "Trcanje" },
            new Disciplina { DisciplinaId = 2, Naziv = "Sprint" }
        };

            _repositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(data);

            // Act
            var result = await _service.GetAllAsync("TrCaN");

            // Assert
            Assert.Single(result);
            Assert.Equal("Trcanje", result.First().Naziv);
        }
    }

}