namespace Models.Request;

/// <summary>
/// Modelo de requisição para registrar uma nova interação no histórico de uma opportunity.
/// </summary>
public class InteractionAdd
{
    /// <summary>
    /// Descrição do que foi tratado na interação.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Identificador do usuário responsável pelo registro da interação.
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// Etapa de origem antes da interação, quando aplicável.
    /// </summary>
    public int? FromStage { get; set; }

    /// <summary>
    /// Etapa de destino após a interação, quando aplicável.
    /// </summary>
    public int? ToStage { get; set; }

    /// <summary>
    /// Data em que a interação ocorreu. Quando não informada, será utilizado o horário atual.
    /// </summary>
    public DateTime? InteractionDate { get; set; }

    /// <summary>
    /// Próxima data prevista para contato, quando houver acompanhamento futuro.
    /// </summary>
    public DateTime? NextContactDate { get; set; }
}