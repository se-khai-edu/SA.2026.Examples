using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model;

[PrimaryKey(nameof(Id))]
public class Disease
{
    public int Id { get; set; }
    [Required]
    public string Description { get; set; } = string.Empty;
    [ForeignKey(nameof(ParentDisease))]
    public int? ParentId { get; set; }  // Self-referencing foreign key

    [DeleteBehavior(DeleteBehavior.Restrict)]
    [InverseProperty(nameof(SubDiseases))]
    public Disease? ParentDisease { get; set; }  // Navigation property for the parent disease
    [InverseProperty(nameof(ParentDisease))]
    public ICollection<Disease> SubDiseases { get; set; } = [];  // Navigation property for child diseases

    [InverseProperty(nameof(Diagnosis.Disease))]
    public virtual ICollection<Diagnosis> Diagnoses { get; set; } = [];  // Navigation property for diagnoses

    public string FullDescription => ParentId.HasValue 
        ? $"{ParentDisease?.FullDescription} > {Description}" 
        : Description;

    public override string ToString() => $"{Id}. {FullDescription})";
}
