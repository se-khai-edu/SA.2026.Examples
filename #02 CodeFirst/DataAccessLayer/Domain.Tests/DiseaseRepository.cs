using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Test
{
    public class DiseaseRepositoryTests : IDisposable
    {
        private readonly SqliteConnection _connection;
        private readonly DbContextOptions<Storage> _options;

        public DiseaseRepositoryTests()
        {
            // Setup SQLite In-Memory database
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();

            _options = new DbContextOptionsBuilder<Storage>()
                .UseSqlite(_connection)
                .Options;

            using var context = new Storage(_options);
            context.Database.EnsureCreated();
        }

        public void Dispose()
        {
            _connection.Close();
        }

        [Fact]
        public void Create_ShouldAddDiseaseToDatabase()
        {
            // Arrange
            using var context = new Storage(_options);
            var repo = new Repository<Disease>(context);
            var disease = new Disease { Id = 1, Description = "Viral Infection" };

            // Act
            repo.Add(disease);
            repo.Save();

            // Assert
            using var assertContext = new Storage(_options);
            var savedDisease = assertContext.Diseases.Find(1);

            Assert.NotNull(savedDisease);
            Assert.Equal("Viral Infection", savedDisease.Description);
            Assert.Null(savedDisease.ParentId);
        }

        [Fact]
        public void Read_ShouldReturnDiseaseById()
        {
            // Arrange
            using (var setupContext = new Storage(_options))
            {
                setupContext.Diseases.Add(new Disease { Id = 2, Description = "Bacterial Infection" });
                setupContext.SaveChanges();
            }

            // Act
            using var context = new Storage(_options);
            var repo = new Repository<Disease>(context);
            var result = repo.GetById(2);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Id);
            Assert.Equal("Bacterial Infection", result.Description);
        }

        [Fact]
        public void Update_ShouldModifyExistingDisease()
        {
            // Arrange
            using (var setupContext = new Storage(_options))
            {
                setupContext.Diseases.Add(new Disease { Id = 3, Description = "Old Description" });
                setupContext.SaveChanges();
            }

            // Act
            using (var context = new Storage(_options))
            {
                var repo = new Repository<Disease>(context);
                var diseaseToUpdate = repo.GetById(3);

                diseaseToUpdate!.Description = "Updated Description";
                repo.Update(diseaseToUpdate);
                repo.Save();
            }

            // Assert
            using (var assertContext = new Storage(_options))
            {
                var updatedDisease = assertContext.Diseases.Find(3);
                Assert.Equal("Updated Description", updatedDisease!.Description);
            }
        }

        [Fact]
        public void Delete_ShouldRemoveDisease_WhenNoChildrenExist()
        {
            // Arrange
            using (var setupContext = new Storage(_options))
            {
                setupContext.Diseases.Add(new Disease { Id = 4, Description = "To Be Deleted" });
                setupContext.SaveChanges();
            }

            // Act
            using (var context = new Storage(_options))
            {
                var repo = new Repository<Disease>(context);
                var diseaseToDelete = repo.GetById(4);

                repo.Delete(diseaseToDelete!);
                repo.Save();
            }

            // Assert
            using (var assertContext = new Storage(_options))
            {
                Assert.Null(assertContext.Diseases.Find(4));
            }
        }

        [Fact]
        public void Delete_ParentDisease_WithChildren_ShouldThrowException_DueToRestrict()
        {
            // Arrange
            using (var setupContext = new Storage(_options))
            {
                // Create a parent disease
                var parentDisease = new Disease { Id = 10, Description = "Respiratory Diseases" };

                // Create a child disease that references the parent
                var childDisease = new Disease { Id = 11, Description = "Asthma", ParentId = 10 };

                setupContext.Diseases.Add(parentDisease);
                setupContext.Diseases.Add(childDisease);
                setupContext.SaveChanges();
            }

            // Act & Assert
            using (var context = new Storage(_options))
            {
                var repo = new Repository<Disease>(context);
                var parentToDelete = repo.GetById(10);

                repo.Delete(parentToDelete!);

                // Assert that calling Save() throws a DbUpdateException because of DeleteBehavior.Restrict
                // This proves our database constraint is working perfectly!
                Assert.Throws<DbUpdateException>(() => repo.Save());
            }
        }
    }
}

