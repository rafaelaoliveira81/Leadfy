namespace Domain.Entities;

public class Ower
{
    public int ID { get; set; }
    public string Name { get; set; }
    public User User { get; set; }
    public int UserID { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Construtor padrão.
    /// Inicializa o responsável como ativo e define a data de criação.
    /// </summary>
    public Ower()
    {
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Desativa o responsável no sistema.
    /// </summary>
    public void Deactivate()
    {
        IsActive = false;
    }

    /// <summary>
    /// Ativa o responsável no sistema.
    /// </summary>
    public void Activate()
    {
        IsActive = true;
    }
}