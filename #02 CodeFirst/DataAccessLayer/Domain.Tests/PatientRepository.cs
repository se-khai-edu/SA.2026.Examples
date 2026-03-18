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
    public class PatientRepository : IDisposable
    {
        private readonly SqliteConnection _connection;
        private readonly DbContextOptions<Storage> _options;

        public PatientRepository()
        {
            // Setup SQLite In-Memory database for this test class
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();

            _options = new DbContextOptionsBuilder<Storage>()
                .UseSqlite(_connection)
                .Options;

            // Ensure schema is created before each test runs
            using var context = new Storage(_options);
            context.Database.EnsureCreated();
        }

        public void Dispose()
        {
            // Clean up the in-memory database
            _connection.Close();
        }

        [Fact]
        public void Create_ShouldAddPatientToDatabase()
        {
            // Arrange
            using var context = new Storage(_options);
            var repo = new Repository<Patient>(context);
            var patientId = Guid.NewGuid();
            var patient = new Patient
            {
                Id = patientId,
                FirstName = "John",
                LastName = "Doe",
                Birth = new DateTime(1985, 5, 20)
            };

            // Act
            repo.Add(patient);
            repo.Save();

            // Assert
            // Using a new context to verify it was actually saved to the DB, not just cached
            using var assertContext = new Storage(_options);
            var savedPatient = assertContext.Patients.FirstOrDefault(p => p.Id == patientId);

            Assert.NotNull(savedPatient);
            Assert.Equal("John", savedPatient.FirstName);
            Assert.Equal("Doe", savedPatient.LastName);
        }

        [Fact]
        public void Read_ShouldReturnPatientById()
        {
            // Arrange
            using var setupContext = new Storage(_options);
            var patientId = Guid.NewGuid();
            setupContext.Patients.Add(new Patient { Id = patientId, FirstName = "Jane", LastName = "Smith" });
            setupContext.SaveChanges();

            // Act
            using var context = new Storage(_options);
            var repo = new Repository<Patient>(context);
            var result = repo.GetById(patientId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(patientId, result.Id);
            Assert.Equal("Jane", result.FirstName);
        }

        [Fact]
        public void Update_ShouldModifyExistingPatient()
        {
            // Arrange
            var patientId = Guid.NewGuid();
            using (var setupContext = new Storage(_options))
            {
                setupContext.Patients.Add(new Patient { Id = patientId, FirstName = "OldName", LastName = "Test" });
                setupContext.SaveChanges();
            }

            // Act
            using (var context = new Storage(_options))
            {
                var repo = new Repository<Patient>(context);
                var patientToUpdate = repo.GetById(patientId);

                // Change the property
                patientToUpdate!.FirstName = "NewName";
                repo.Update(patientToUpdate);
                repo.Save();
            }

            // Assert
            using (var assertContext = new Storage(_options))
            {
                var updatedPatient = assertContext.Patients.Find(patientId);
                Assert.NotNull(updatedPatient);
                Assert.Equal("NewName", updatedPatient.FirstName);
            }
        }

        [Fact]
        public void Delete_ShouldRemovePatientFromDatabase()
        {
            // Arrange
            var patientId = Guid.NewGuid();
            using (var setupContext = new Storage(_options))
            {
                setupContext.Patients.Add(new Patient { Id = patientId, FirstName = "Mark", LastName = "Twain" });
                setupContext.SaveChanges();
            }

            // Act
            using (var context = new Storage(_options))
            {
                var repo = new Repository<Patient>(context);
                var patientToDelete = repo.GetById(patientId);

                repo.Delete(patientToDelete!);
                repo.Save();
            }

            // Assert
            using (var assertContext = new Storage(_options))
            {
                var deletedPatient = assertContext.Patients.Find(patientId);
                Assert.Null(deletedPatient);
            }
        }
    }
}
