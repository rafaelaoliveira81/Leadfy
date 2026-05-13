namespace Models.Request;

public class OwnerUpdate
{
    public string Name { get; set; }
    public int UserID { get; set; }
    public bool IsActive { get; set; }
}