namespace Model;

public class Patient
{
    public Guid Id { get; set; }
    public string LastName { get; set; } = string.Empty;
    public string MiddleName { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public DateTime Birth { get; set; }
}
