using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace tests
{
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
            new Disciplina { DisciplinaId = 1, Naziv = "Karate" },
            new Disciplina { DisciplinaId = 2, Naziv = "Judo" }
        };

            _repositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(data);

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains(result, d => d.Naziv == "Karate");
            Assert.Contains(result, d => d.Naziv == "Judo");
        }

        [Fact]
        public async Task GetAllAsync_WithSearch_ReturnsFilteredDisciplines()
        {
            // Arrange
            var data = new List<Disciplina>
        {
            new Disciplina { DisciplinaId = 1, Naziv = "Karate" },
            new Disciplina { DisciplinaId = 2, Naziv = "Judo" },
            new Disciplina { DisciplinaId = 3, Naziv = "Kickboxing" }
        };

            _repositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(data);

            // Act
            var result = await _service.GetAllAsync("kar");

            // Assert
            Assert.Single(result);
            Assert.Equal("Karate", result.First().Naziv);
        }

        [Fact]
        public async Task GetAllAsync_SearchIsCaseInsensitive()
        {
            // Arrange
            var data = new List<Disciplina>
        {
            new Disciplina { DisciplinaId = 1, Naziv = "Karate" },
            new Disciplina { DisciplinaId = 2, Naziv = "Judo" }
        };

            _repositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(data);

            // Act
            var result = await _service.GetAllAsync("KaRa");

            // Assert
            Assert.Single(result);
            Assert.Equal("Karate", result.First().Naziv);
        }
    }
}
