using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using susak.Controllers;
using susak.Models;
using Xunit;

namespace susak.IntegrationTests
{
    public class DisciplinaControllerIntegrationTests
    {
        private readonly susakContext _context;
        private readonly IDisciplinaRepository _repository;
        private readonly IDisciplinaService _service;
        private readonly DisciplinaController _controller;

        public DisciplinaControllerIntegrationTests()
        {
            // InMemory baza
            var options = new DbContextOptionsBuilder<susakContext>()
                .UseInMemoryDatabase(databaseName: "DisciplinaTestDb")
                .Options;

            _context = new susakContext(options);

            // Seed podaci
            _context.Disciplina.Add(new Disciplina
            {
                DisciplinaId = 1,
                Naziv = "Test sport",
                Opis = "Test disciplina opis"
            });
            _context.SaveChanges();

            // Repository -> Service -> Controller
            _repository = new DisciplinaRepository(_context);
            _service = new DisciplinaService(_repository);
            _controller = new DisciplinaController(_service);
        }

        [Fact]
        public async Task Index_ReturnsViewWithModel()
        {
            // Act
            var result = await _controller.Index(null);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Disciplina>>(viewResult.Model);

            Assert.Single(model);
        }
    }
}
