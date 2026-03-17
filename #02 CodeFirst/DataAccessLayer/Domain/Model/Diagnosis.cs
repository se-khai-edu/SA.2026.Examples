using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model;

[PrimaryKey(nameof(PatientId), nameof(DiseaseId), nameof(Completion))]
public class Diagnosis
{
    [ForeignKey(nameof(Patient))]
    public Guid PatientId { get; set; }

    [ForeignKey(nameof(Disease))]
    public int DiseaseId { get; set; }
    
    public DateTime Completion { get; set; }

    [InverseProperty(nameof(Patient.Diagnoses))]
    public virtual Patient Patient { get; set; } = null!;
    [InverseProperty(nameof(Disease.Diagnoses))]
    public virtual Disease Disease { get; set; } = null!;

    public override string ToString() => $"{Patient.Name} - {Disease} ({Completion:yyyy-MM-dd})";
}
