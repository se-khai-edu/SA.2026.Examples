using System;
using System.Collections.Generic;

namespace BlazorHospital.Models;

public partial class Patient
{
    public string Id { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string MiddleName { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string Birth { get; set; } = null!;

    public virtual ICollection<Diagnosis> Diagnoses { get; set; } = new List<Diagnosis>();
}
