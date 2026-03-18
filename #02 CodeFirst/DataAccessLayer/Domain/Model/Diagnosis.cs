using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model;

// todo : 1.3 Add diagnosis model table/class with composite primary key

[PrimaryKey(nameof(PatientId), nameof(DiseaseId), nameof(Completion))]
public class Diagnosis
{
    [ForeignKey(nameof(Patient))]
    public Guid PatientId { get; set; }

    [ForeignKey(nameof(Disease))]
    public int DiseaseId { get; set; }

    public DateTime Completion { get; set; }

    // todo : 2.3 Add navigation properties of diagnosis with appropriate delete behaviors

    [InverseProperty(nameof(Patient.Diagnoses))]
    [DeleteBehavior(DeleteBehavior.Cascade)] // If you delete a patient, the diagnoses are deleted.
    public virtual Patient Patient { get; set; } = null!;

    [InverseProperty(nameof(Disease.Diagnoses))]
    [DeleteBehavior(DeleteBehavior.Restrict)] // You can't delete a disease while the diagnoses are still present.
    public virtual Disease Disease { get; set; } = null!;

    public override string ToString() 
        => $"{Patient.Name} - {Disease} ({Completion:yyyy-MM-dd})";
}
