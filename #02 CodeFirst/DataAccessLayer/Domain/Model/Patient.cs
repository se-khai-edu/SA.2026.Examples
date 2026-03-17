using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model;

// todo : 1.2 Add patient model table/class

[PrimaryKey(nameof(Id))]
public class Patient
{
    public Guid Id { get; set; }
    [Required]
    public string LastName { get; set; } = string.Empty;
    [Required]
    public string MiddleName { get; set; } = string.Empty;
    [Required]
    public string FirstName { get; set; } = string.Empty;
    [Required]
    public DateTime Birth { get; set; }

// todo : 2.2 Add navigation properties of patients 

    [InverseProperty(nameof(Diagnosis.Patient))]
    public virtual ICollection<Diagnosis> Diagnoses { get; set; } = [];

    public string Name => $"{LastName ?? "Unknown"} {FirstLetter(FirstName)}.{FirstLetter(MiddleName)}.";
    private string? FirstLetter(string s) => 
        string.IsNullOrEmpty(s) ? "_" : s.Substring(0, 1).ToUpper();

    public override string ToString() => $"{Name} ({Birth:yyyy-MM-dd}, id = {Id})";
}
