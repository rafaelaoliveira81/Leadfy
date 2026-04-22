using Domain.Enuns;

namespace Domain.Entities;

/// <summary>
/// Representa uma interação registrada no histórico do CRM.
/// Permite armazenar anotações, mudanças de etapa e próximos contatos
/// vinculados a uma entidade do domínio, como uma opportunity.
/// </summary>
public class Interaction
{
    /// <summary>
    /// Identificador único da interação.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Identificador da entidade de negócio dentro do contexto do CRM.
    /// Exemplo: ID da opportunity associada ao histórico.
    /// </summary>
    public int CrmEntityId { get; set; }

    /// <summary>
    /// Tipo da entidade do CRM à qual a interação pertence.
    /// </summary>
    public CrmEntityType CrmEntityType { get; set; }

    /// <summary>
    /// Etapa de origem antes da interação, quando aplicável.
    /// </summary>
    public int? FromStage { get; set; }

    /// <summary>
    /// Etapa de destino após a interação, quando aplicável.
    /// </summary>
    public int? ToStage { get; set; }

    /// <summary>
    /// Descrição textual da interação registrada no histórico.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Data e hora em que a interação ocorreu.
    /// </summary>
    public DateTime InteractionDate { get; set; }

    /// <summary>
    /// Data e hora de criação do registro da interação.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Identificador do usuário responsável pelo registro da interação.
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// Usuário responsável pela criação da interação.
    /// </summary>
    public User User { get; set; }

    /// <summary>
    /// Próxima data de contato prevista, quando existir acompanhamento futuro.
    /// </summary>
    public DateTime? NextContactDate { get; set; }

    /// <summary>
    /// Construtor padrão.
    /// Inicializa a data de criação e a data da interação com o horário atual em UTC.
    /// </summary>
    public Interaction()
    {
        CreatedAt = DateTime.UtcNow;
        InteractionDate = DateTime.UtcNow;
    }
}