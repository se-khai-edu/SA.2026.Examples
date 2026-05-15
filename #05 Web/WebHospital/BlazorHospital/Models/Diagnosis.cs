using System;
using System.Collections.Generic;

namespace BlazorHospital.Models;

public partial class Diagnosis
{
    public string PatientId { get; set; } = null!;

    public int DiseaseId { get; set; }

    public string Completion { get; set; } = null!;

    public virtual Disease Disease { get; set; } = null!;

    public virtual Patient Patient { get; set; } = null!;
}
