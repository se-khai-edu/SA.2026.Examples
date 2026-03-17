using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model;

// todo : 1.0 Install Entity Framework Core Tools
// Tools -> NuGet Package Manager -> Package Manager Console
//      Install-Package Microsoft.EntityFrameworkCore.Sqlite -Version 9.0.2
//      Install-Package Microsoft.EntityFrameworkCore.Tools -Version 9.0.2
//      Install-Package Microsoft.EntityFrameworkCore.Design -Version 9.0.2
//

// todo : 1.1 Add disease model table/class

[PrimaryKey(nameof(Id))]
public class Disease
{
    public int Id { get; set; }

    [Required]
    public string Description { get; set; } = string.Empty;

    [ForeignKey(nameof(ParentDisease))]
    public int? ParentId { get; set; } // Self-FK

    // todo : 2.1 Add navigation properties of diseases with appropriate delete behaviors

    [DeleteBehavior(DeleteBehavior.Restrict)] // Cascade delete behavior
    [InverseProperty(nameof(SubDiseases))]
    public virtual Disease? ParentDisease { get; set; }

    [InverseProperty(nameof(ParentDisease))]
    public virtual ICollection<Disease> SubDiseases { get; set; } = [];

    [InverseProperty(nameof(Diagnosis.Disease))]
    public virtual ICollection<Diagnosis> Diagnoses { get; set; } = [];

    public string FullDescription => ParentId.HasValue 
        ? $"{ParentDisease?.FullDescription} > {Description}" 
        : Description;

    public override string ToString() => $"{Id}. {FullDescription})";
}
