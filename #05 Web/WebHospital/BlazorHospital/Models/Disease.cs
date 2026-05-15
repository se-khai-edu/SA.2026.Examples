using System;
using System.Collections.Generic;

namespace BlazorHospital.Models;

public partial class Disease
{
    public int Id { get; set; }

    public string Description { get; set; } = null!;

    public int? ParentId { get; set; }

    public virtual ICollection<Diagnosis> Diagnoses { get; set; } = new List<Diagnosis>();

    public virtual ICollection<Disease> InverseParent { get; set; } = new List<Disease>();

    public virtual Disease? Parent { get; set; }
}
