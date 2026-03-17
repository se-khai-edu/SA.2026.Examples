namespace Model;

public class Diagnosis
{
    public Guid PatientId { get; set; }
    public int DiseaseId { get; set; }
    public DateTime Completion { get; set; }
}
