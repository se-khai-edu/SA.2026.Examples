namespace Model;

public class Disease
{
    public int Id { get; set; }
    public string Description { get; set; } = string.Empty;
    public int? ParentId { get; set; }  // Self-referencing foreign key
}
