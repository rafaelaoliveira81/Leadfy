namespace Domain.Entities;

/// <summary>
/// Represents a user group used for role-based authorization.
/// A user belongs to one group; a group holds many permissions.
/// </summary>
public class UserGroup
{
    public int ID { get; set; }
    public string Name { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }

    public ICollection<User> Users { get; set; }
    public ICollection<UserGroupPermission> UserGroupPermissions { get; set; }

    public UserGroup()
    {
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
        Users = new List<User>();
        UserGroupPermissions = new List<UserGroupPermission>();
    }

    public void Deactivate() => IsActive = false;
    public void Activate() => IsActive = true;
}
