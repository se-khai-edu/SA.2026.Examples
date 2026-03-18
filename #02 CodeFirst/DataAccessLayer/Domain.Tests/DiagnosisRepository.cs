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
    public class DiagnosisRepositoryTests : IDisposable
    {
        private readonly SqliteConnection _connection;
        private readonly DbContextOptions<Storage> _options;

        public DiagnosisRepositoryTests()
        {
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
        public void Create_ShouldAddDiagnosisToDatabase()
        {
            // Arrange
            var patientId = Guid.NewGuid();
            var diseaseId = 1;
            var completionDate = new DateTime(2023, 10, 15);

            using (var setupContext = new Storage(_options))
            {
                // We must first create the related Patient and Disease due to Foreign Key constraints
                setupContext.Patients.Add(new Patient { Id = patientId, FirstName = "Test", LastName = "Patient" });
                setupContext.Diseases.Add(new Disease { Id = diseaseId, Description = "Flu" });
                setupContext.SaveChanges();
            }

            // Act
            using (var context = new Storage(_options))
            {
                var repo = new Repository<Diagnosis>(context);
                var diagnosis = new Diagnosis
                {
                    PatientId = patientId,
                    DiseaseId = diseaseId,
                    Completion = completionDate
                };

                repo.Add(diagnosis);
                repo.Save();
            }

            // Assert
            using (var assertContext = new Storage(_options))
            {
                // Testing the composite key retrieval
                var savedDiagnosis = assertContext.Diagnoses.Find(patientId, diseaseId, completionDate);
                Assert.NotNull(savedDiagnosis);
            }
        }

        [Fact]
        public void Read_ShouldReturnDiagnosisByCompositeKey()
        {
            // Arrange
            var patientId = Guid.NewGuid();
            var diseaseId = 2;
            var completionDate = new DateTime(2023, 11, 20);

            using (var setupContext = new Storage(_options))
            {
                setupContext.Patients.Add(new Patient { Id = patientId, FirstName = "Alice", LastName = "Wonder" });
                setupContext.Diseases.Add(new Disease { Id = diseaseId, Description = "Allergy" });
                setupContext.Diagnoses.Add(new Diagnosis { PatientId = patientId, DiseaseId = diseaseId, Completion = completionDate });
                setupContext.SaveChanges();
            }

            // Act
            using (var context = new Storage(_options))
            {
                var repo = new Repository<Diagnosis>(context);

                // Passing all three parts of the composite key to params object[]
                var result = repo.GetById(patientId, diseaseId, completionDate);

                // Assert
                Assert.NotNull(result);
                Assert.Equal(patientId, result.PatientId);
                Assert.Equal(diseaseId, result.DiseaseId);
            }
        }

        [Fact]
        public void Delete_ShouldRemoveDiagnosisDirectly()
        {
            // Arrange
            var patientId = Guid.NewGuid();
            var diseaseId = 3;
            var completionDate = DateTime.Now;

            using (var setupContext = new Storage(_options))
            {
                setupContext.Patients.Add(new Patient { Id = patientId, FirstName = "Bob", LastName = "Builder" });
                setupContext.Diseases.Add(new Disease { Id = diseaseId, Description = "Cold" });
                setupContext.Diagnoses.Add(new Diagnosis { PatientId = patientId, DiseaseId = diseaseId, Completion = completionDate });
                setupContext.SaveChanges();
            }

            // Act
            using (var context = new Storage(_options))
            {
                var repo = new Repository<Diagnosis>(context);
                var diagnosisToDelete = repo.GetById(patientId, diseaseId, completionDate);

                repo.Delete(diagnosisToDelete!);
                repo.Save();
            }

            // Assert
            using (var assertContext = new Storage(_options))
            {
                var deletedDiagnosis = assertContext.Diagnoses.Find(patientId, diseaseId, completionDate);
                Assert.Null(deletedDiagnosis);
            }
        }

        [Fact]
        public void CascadeDelete_WhenPatientIsDeleted_ShouldDeleteRelatedDiagnoses()
        {
            // Arrange
            var patientId = Guid.NewGuid();
            var diseaseId = 4;
            var completionDate = DateTime.Now;

            using (var setupContext = new Storage(_options))
            {
                setupContext.Patients.Add(new Patient { Id = patientId, FirstName = "Charlie", LastName = "Chaplin" });
                setupContext.Diseases.Add(new Disease { Id = diseaseId, Description = "Fever" });
                setupContext.Diagnoses.Add(new Diagnosis { PatientId = patientId, DiseaseId = diseaseId, Completion = completionDate });
                setupContext.SaveChanges();
            }

            // Act
            using (var context = new Storage(_options))
            {
                var patientRepo = new Repository<Patient>(context);
                var patientToDelete = patientRepo.GetById(patientId);

                // Deleting the Patient should trigger Cascade Delete for Diagnosis
                patientRepo.Delete(patientToDelete!);
                patientRepo.Save();
            }

            // Assert
            using (var assertContext = new Storage(_options))
            {
                var remainingDiagnosis = assertContext.Diagnoses.Find(patientId, diseaseId, completionDate);

                // The diagnosis must not exist in the database anymore
                Assert.Null(remainingDiagnosis);
            }
        }
    }
}
