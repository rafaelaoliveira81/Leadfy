namespace Domain.Entities;

public class Lead
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; private set; }

    /// <summary>
    /// Construtor padrão.
    /// Inicializa o lead como ativo e define a data de criação.
    /// </summary>
    public Lead()
    {
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Desativa o lead no sistema.
    /// </summary>
    public void Deactivate()
    {
        IsActive = false;
    }

    /// <summary>
    /// Ativa o lead no sistema.
    /// </summary>
    public void Activate()
    {
        IsActive = true;
    }
}