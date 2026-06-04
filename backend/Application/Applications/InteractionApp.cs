using Application.DTO;
using Domain.Entities;
using Domain.Enuns;

namespace Application;

public class InteractionApp : IInteractionApp
{
    private readonly IInteractionRepo _interactionRepo;
    private readonly IOpportunityRepo _opportunityRepo;
    private readonly IUserRepo _userRepo;
    public InteractionApp(IInteractionRepo interactionRepo, IOpportunityRepo opportunityRepo, IUserRepo userRepo)
    {
        _interactionRepo = interactionRepo;
        _opportunityRepo = opportunityRepo;
        _userRepo = userRepo;
    }
    public async Task<int> AddToOpportunityAsync(int opportunityId, InteractionAdd interactionRequest)
    {
        var interaction = MapToEntity(interactionRequest);

        await ValidateOpportunityExistsAsync(opportunityId);
        await ValidateInteractionAsync(interaction);

        interaction.OpportunityId = opportunityId;
        interaction.CreatedAt = DateTime.Now;

        if (interaction.InteractionDate == default)
            interaction.InteractionDate = DateTime.Now;

        return await _interactionRepo.AddAsync(interaction);
    }
    public async Task<InteractionResponse> GetByIdAsync(int idInteraction)
    {
        var interactionEntity = await GetEntityByIdAsync(idInteraction);

        return MapToResponse(interactionEntity);
    }
    public async Task<IEnumerable<InteractionResponse>> GetByOpportunityIdAsync(int opportunityId)
    {
        await ValidateOpportunityExistsAsync(opportunityId);

        var interactions = await _interactionRepo.GetAllByOpportunityIdAsync(opportunityId);

        return interactions.Select(MapToResponse);
    }
    public async Task DeleteAsync(int idInteraction)
    {
        var interactionEntity = await GetEntityByIdAsync(idInteraction);
        await _interactionRepo.DeleteAsync(interactionEntity);
    }

    #region Utils
    private async Task ValidateInteractionAsync(Interaction interaction)
    {
        if (interaction == null)
            throw new ArgumentException("A interação não pode ser vazia.");

        if (string.IsNullOrWhiteSpace(interaction.Description))
            throw new ArgumentException("A descrição da interação deve ser informada.");

        interaction.Description = interaction.Description.Trim();

        if (interaction.Description.Length > 1000)
            throw new ArgumentException("A descrição da interação não pode exceder 1000 caracteres.");

        if (interaction.UserId <= 0)
            throw new ArgumentException("O usuário da interação deve ser informado.");

        if (interaction.FromStage.HasValue && !Enum.IsDefined(typeof(OpportunityStage), interaction.FromStage.Value))
            throw new ArgumentException("A stage de origem da interação é inválida.");

        if (interaction.ToStage.HasValue && !Enum.IsDefined(typeof(OpportunityStage), interaction.ToStage.Value))
            throw new ArgumentException("A stage de destino da interação é inválida.");

        if (interaction.NextContactDate.HasValue && interaction.NextContactDate.Value < interaction.InteractionDate)
            throw new ArgumentException("A próxima data de contato não pode ser anterior à data da interação.");

        var userEntity = await _userRepo.GetByIdAsync(interaction.UserId);

        if (userEntity == null)
            throw new KeyNotFoundException("Usuário não localizado.");
    }
    private async Task<Opportunity> ValidateOpportunityExistsAsync(int opportunityId)
    {
        if (opportunityId <= 0)
            throw new ArgumentException("A opportunity informada é inválida.");

        var opportunityEntity = await _opportunityRepo.GetByIdAsync(opportunityId);

        if (opportunityEntity == null)
            throw new KeyNotFoundException("Opportunity não localizada.");

        return opportunityEntity;
    }

    private async Task<Interaction> GetEntityByIdAsync(int idInteraction)
    {
        if (idInteraction <= 0)
            throw new ArgumentException("O identificador da interação é inválido.");

        var interactionEntity = await _interactionRepo.GetByIdAsync(idInteraction);

        if (interactionEntity == null)
            throw new KeyNotFoundException("Interação não localizada.");

        await ValidateOpportunityExistsAsync(interactionEntity.OpportunityId);

        return interactionEntity;
    }

    private static Interaction MapToEntity(InteractionAdd request)
    {
        if (request == null)
            return null;

        return new Interaction
        {
            Description = request.Description,
            UserId = request.UserId,
            FromStage = request.FromStage,
            ToStage = request.ToStage,
            InteractionDate = request.InteractionDate ?? DateTime.Now,
            NextContactDate = request.NextContactDate
        };
    }

    private static InteractionResponse MapToResponse(Interaction interaction)
    {
        return new InteractionResponse
        {
            Id = interaction.Id,
            OpportunityId = interaction.OpportunityId,
            FromStage = interaction.FromStage,
            FromStageName = interaction.FromStage.HasValue
                ? ((OpportunityStage)interaction.FromStage.Value).ToString()
                : null,
            ToStage = interaction.ToStage,
            ToStageName = interaction.ToStage.HasValue
                ? ((OpportunityStage)interaction.ToStage.Value).ToString()
                : null,
            Description = interaction.Description,
            InteractionDate = interaction.InteractionDate,
            CreatedAt = interaction.CreatedAt,
            UserId = interaction.UserId,
            UserName = interaction.User?.Name,
            NextContactDate = interaction.NextContactDate
        };
    }
    #endregion
}