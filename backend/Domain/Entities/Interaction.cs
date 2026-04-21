using Domain.Enuns;

namespace Domain.Entities;

public class Interaction
{
    public int Id { get; set; }

    // Id do registro no contexto do CRM
    // Ex: OpportunityId, PostSaleId, etc.
    public int CrmEntityId { get; set; }

    // Tipo da entidade do CRM
    public CrmEntityType CrmEntityType { get; set; }

    // Status/etapa de origem e destino
    public int? FromStage { get; set; }
    public int? ToStage { get; set; }

    public string Description { get; set; }

    public DateTime InteractionDate { get; set; }
    public DateTime CreatedAt { get; set; }

    public int UserId { get; set; }
    public User User { get; set; }

    public DateTime? NextContactDate { get; set; }
}

// exemplo de uso:

// var interaction = new Interaction
// {
//     CrmEntityId = 123,
//     CrmEntityType = CrmEntityType.Opportunity,
//     FromStage = (int)OpportunityStage.Qualified,
//     ToStage = (int)OpportunityStage.ProposalSent,
//     Description = "Oportunidade avançou para proposta enviada.",
//     InteractionDate = DateTime.UtcNow,
//     CreatedAt = DateTime.UtcNow,
//     UserId = 10
// };

// var interaction = new Interaction
// {
//     CrmEntityId = 45,
//     CrmEntityType = CrmEntityType.PostSale,
//     FromStage = (int)PostSaleStage.Activated,
//     ToStage = (int)PostSaleStage.InFollowUp,
//     Description = "Cliente entrou em acompanhamento pós ativação.",
//     InteractionDate = DateTime.UtcNow,
//     CreatedAt = DateTime.UtcNow,
//     UserId = 10,
//     NextContactDate = DateTime.UtcNow.AddDays(7)
// };