using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using susak.Models;
using Xunit;

namespace TestProject
{


    public class DisciplinaRepositoryTests
    {
        private susakContext CreateInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<susakContext>()
                .UseInMemoryDatabase(databaseName: "TestDb_" + System.Guid.NewGuid())
                .Options;

            return new susakContext(options);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllDiscipline()
        {
            // Arrange
            using var context = CreateInMemoryContext();
            context.Disciplina.AddRange(
                new Disciplina { DisciplinaId = 1, Naziv = "Sprint" },
                new Disciplina { DisciplinaId = 2, Naziv = "Bacanje koplja" }
            );
            await context.SaveChangesAsync();

            var repository = new DisciplinaRepository(context);

            // Act
            var result = await repository.GetAllAsync();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains(result, d => d.Naziv == "Sprint");
            Assert.Contains(result, d => d.Naziv == "Bacanje koplja");
        }
    }

}
