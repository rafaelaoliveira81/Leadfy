namespace Domain.Entities;

/// <summary>
/// Join entity for the N:N relationship between UserGroup and Permission.
/// </summary>
public class UserGroupPermission
{
    public int ID { get; set; }
    public int UserGroupId { get; set; }
    public int PermissionId { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }

    public UserGroup UserGroup { get; set; }
    public Permission Permission { get; set; }

    public UserGroupPermission()
    {
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
    }

    public void Deactivate() => IsActive = false;
    public void Activate() => IsActive = true;
}
