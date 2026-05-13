namespace Domain.Entities;

public class Ower
{
    public int ID { get; set; }
    public string Name { get; set; }
    public User User { get; set; }
    public int UserID { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public Ower()
    {
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
    }
    public void Deactivate()
    {
        IsActive = false;
    }
    public void Activate()
    {
        IsActive = true;
    }
}