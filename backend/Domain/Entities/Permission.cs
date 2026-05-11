namespace Domain.Entities;

/// <summary>
/// Represents a system permission that can be assigned to user groups.
/// </summary>
public class Permission
{
    public int ID { get; set; }
    public string Name { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }

    public ICollection<UserGroupPermission> UserGroupPermissions { get; set; }

    public Permission()
    {
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
        UserGroupPermissions = new List<UserGroupPermission>();
    }

    public void Deactivate() => IsActive = false;
    public void Activate() => IsActive = true;
}
