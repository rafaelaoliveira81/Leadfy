namespace Models.Response;

/// <summary>
/// Modelo de resposta utilizado para retornar dados de uma interação.
/// </summary>
public class InteractionResponse
{
    /// <summary>
    /// Identificador único da interação.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Identificador da opportunity associada à interação.
    /// </summary>
    public int OpportunityId { get; set; }

    /// <summary>
    /// Código numérico da etapa de origem, quando informado.
    /// </summary>
    public int? FromStage { get; set; }

    /// <summary>
    /// Nome da etapa de origem, quando informado.
    /// </summary>
    public string FromStageName { get; set; }

    /// <summary>
    /// Código numérico da etapa de destino, quando informado.
    /// </summary>
    public int? ToStage { get; set; }

    /// <summary>
    /// Nome da etapa de destino, quando informado.
    /// </summary>
    public string ToStageName { get; set; }

    /// <summary>
    /// Descrição textual da interação.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Data e hora em que a interação aconteceu.
    /// </summary>
    public DateTime InteractionDate { get; set; }

    /// <summary>
    /// Data e hora de criação do registro da interação.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Identificador do usuário responsável pelo registro.
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// Nome do usuário responsável pelo registro.
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// Próxima data prevista para contato, quando existente.
    /// </summary>
    public DateTime? NextContactDate { get; set; }
}