using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Model;
using System.Runtime.ConstrainedExecution;

namespace Domain.Test
{
    public class RepositoryTests : IDisposable
    {
        private readonly SqliteConnection _connection;
        private readonly DbContextOptions<Storage> _options;

        public RepositoryTests()
        {
            // 1. Create and open a connection to an SQLite in-memory database
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();

            // 2. Configure the DbContext to use this connection
            _options = new DbContextOptionsBuilder<Storage>()
                .UseSqlite(_connection)
                .Options;

            // 3. Create the database schema before each test runs
            using var context = new Storage(_options);
            context.Database.EnsureCreated();
        }

        public void Dispose()
        {
            // Close the connection, destroying the in-memory database
            _connection.Close();
        }

        [Fact]
        public void PatientRepository_CRUDOperations_ShouldWorkCorrectly()
        {
            // Arrange
            using var context = new Storage(_options);
            var repo = new Repository<Patient>(context);
            var patientId = Guid.NewGuid();
            var patient = new Patient { Id = patientId, FirstName = "John", LastName = "Doe", Birth = new DateTime(1990, 1, 1) };

            // Act 1: CREATE
            repo.Add(patient);
            repo.Save();

            // Assert 1
            Assert.Single(repo.GetAll());

            // Act 2: READ & UPDATE
            var savedPatient = repo.GetById(patientId);
            Assert.NotNull(savedPatient);
            savedPatient.FirstName = "Jane";
            repo.Update(savedPatient);
            repo.Save();

            // Assert 2
            var updatedPatient = repo.GetById(patientId);
            Assert.Equal("Jane", updatedPatient!.FirstName);

            // Act 3: DELETE
            repo.Delete(updatedPatient);
            repo.Save();

            // Assert 3
            Assert.Empty(repo.GetAll());
        }

        [Fact]
        public void Diagnosis_CascadeDelete_WhenPatientIsDeleted_ShouldDeleteDiagnosis()
        {
            // Arrange
            using var context = new Storage(_options);
            var patientRepo = new Repository<Patient>(context);
            var diseaseRepo = new Repository<Disease>(context);
            var diagnosisRepo = new Repository<Diagnosis>(context);

            var patientId = Guid.NewGuid();
            var diseaseId = 1;

            var patient = new Patient { Id = patientId, FirstName = "Test", LastName = "Patient" };
            var disease = new Disease { Id = diseaseId, Description = "Flu" };

            var diagnosis = new Diagnosis
            {
                PatientId = patientId,
                DiseaseId = diseaseId,
                Completion = DateTime.Now
            };

            // Add all entities to the database
            patientRepo.Add(patient);
            diseaseRepo.Add(disease);
            diagnosisRepo.Add(diagnosis);
            context.SaveChanges(); // Using context directly to save all at once

            // Verify they were added
            Assert.Single(diagnosisRepo.GetAll());

            // Act: Delete the patient
            // This should trigger the cascade delete for the diagnosis
            patientRepo.Delete(patient);
            patientRepo.Save();

            // Assert
            var remainingDiagnoses = diagnosisRepo.GetAll().ToList();
            var remainingDiseases = diseaseRepo.GetAll().ToList();

            // Diagnosis should be deleted (Cascade)
            Assert.Empty(remainingDiagnoses);

            // Disease should NOT be deleted (Restrict/No Cascade from Patient)
            Assert.Single(remainingDiseases);
        }

        [Fact]
        public void Check_ToString()
        {
            // Arrange
            using var context = new Storage(_options);
            var patientRepo = new Repository<Patient>(context);
            var diseaseRepo = new Repository<Disease>(context);
            var diagnosisRepo = new Repository<Diagnosis>(context);

            var patientId = Guid.NewGuid();
            var diseaseId = 1;
            var now = DateTime.Now;

            var chkPatient = $"Patient F.M. ({now:yyyy-MM-dd}, id = {patientId})";
            var chkDisease2 = "2. Parent disease > Child disease";
            var chkDiagnosis = $"Patient F.M. - 1. Parent disease ({now:yyyy-MM-dd})";

            var patient = new Patient { 
                Id = patientId, 
                FirstName = "First",
                MiddleName = "Middle",
                LastName = "Patient",
                Birth = now.Date
            };

            var disease1 = new Disease { Id = diseaseId, Description = "Parent disease" };
            var disease2 = new Disease { 
                Id = diseaseId+1, 
                Description = "Child disease", 
                ParentId = diseaseId 
            };

            var diagnosis = new Diagnosis
            {
                PatientId = patientId,
                DiseaseId = diseaseId,
                Completion = now
            };

            // Add all entities to the database
            patientRepo.Add(patient);
            diseaseRepo.Add(disease1);
            diseaseRepo.Add(disease2);
            diagnosisRepo.Add(diagnosis);
            context.SaveChanges(); // Using context directly to save all at once


            Assert.Equal(patient.ToString(), chkPatient);
            Assert.Equal(disease2.ToString(), chkDisease2);
            Assert.Equal(diagnosis.ToString(), chkDiagnosis);
        }

    }
}
