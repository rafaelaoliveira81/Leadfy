using Domain.Entities;
using Domain.Enuns;

namespace Application;

/// <summary>
/// Serviço de aplicação responsável por orquestrar os casos de uso relacionados a interações.
/// </summary>
/// <remarks>
/// Esta classe pertence à camada de Application.
/// Sua responsabilidade é validar entradas, aplicar regras de fluxo,
/// coordenar chamadas ao domínio e persistir alterações por meio do repositório.
/// </remarks>
public class InteractionApp : IInteractionApp
{
    /// <summary>
    /// Repositório responsável pela persistência das interações.
    /// </summary>
    private readonly IInteractionRepo _interactionRepo;

    /// <summary>
    /// Repositório utilizado para validar a existência da opportunity vinculada.
    /// </summary>
    private readonly IOpportunityRepo _opportunityRepo;

    /// <summary>
    /// Repositório utilizado para validar o usuário responsável pela interação.
    /// </summary>
    private readonly IUserRepo _userRepo;

    /// <summary>
    /// Inicializa uma nova instância de <see cref="InteractionApp"/>.
    /// </summary>
    /// <param name="interactionRepo">Repositório de interações.</param>
    /// <param name="opportunityRepo">Repositório de opportunities.</param>
    /// <param name="userRepo">Repositório de usuários.</param>
    public InteractionApp(IInteractionRepo interactionRepo, IOpportunityRepo opportunityRepo, IUserRepo userRepo)
    {
        _interactionRepo = interactionRepo;
        _opportunityRepo = opportunityRepo;
        _userRepo = userRepo;
    }

    /// <summary>
    /// Adiciona uma interação ao histórico de uma opportunity.
    /// </summary>
    /// <param name="opportunityId">ID da opportunity.</param>
    /// <param name="interaction">Entidade da interação a ser registrada.</param>
    /// <returns>Retorna o identificador da interação criada.</returns>
    /// <exception cref="ArgumentException">
    /// Lançada quando os dados da interação ou da opportunity são inválidos.
    /// </exception>
    /// <exception cref="KeyNotFoundException">
    /// Lançada quando a opportunity ou o usuário não são localizados.
    /// </exception>
    public async Task<int> AddToOpportunityAsync(int opportunityId, Interaction interaction)
    {
        await ValidateOpportunityExistsAsync(opportunityId);
        await ValidateInteractionAsync(interaction);

        interaction.CrmEntityId = opportunityId;
        interaction.CrmEntityType = CrmEntityType.Opportunity;
        interaction.CreatedAt = DateTime.UtcNow;

        if (interaction.InteractionDate == default)
            interaction.InteractionDate = DateTime.UtcNow;

        return await _interactionRepo.AddAsync(interaction);
    }

    /// <summary>
    /// Obtém uma interação pelo seu identificador.
    /// </summary>
    /// <param name="idInteraction">ID da interação.</param>
    /// <returns>Interação encontrada.</returns>
    /// <exception cref="ArgumentException">
    /// Lançada quando o identificador informado é inválido.
    /// </exception>
    /// <exception cref="KeyNotFoundException">
    /// Lançada quando a interação não é localizada.
    /// </exception>
    public async Task<Interaction> GetByIdAsync(int idInteraction)
    {
        if (idInteraction <= 0)
            throw new ArgumentException("O identificador da interação é inválido.");

        var interactionEntity = await _interactionRepo.GetByIdAsync(idInteraction);

        if (interactionEntity == null)
            throw new KeyNotFoundException("Interação não localizada.");

        if (interactionEntity.CrmEntityType == CrmEntityType.Opportunity)
            await ValidateOpportunityExistsAsync(interactionEntity.CrmEntityId);

        return interactionEntity;
    }

    /// <summary>
    /// Obtém todas as interações vinculadas a uma opportunity.
    /// </summary>
    /// <param name="opportunityId">ID da opportunity.</param>
    /// <returns>Coleção de interações da opportunity.</returns>
    /// <exception cref="ArgumentException">
    /// Lançada quando o identificador da opportunity é inválido.
    /// </exception>
    /// <exception cref="KeyNotFoundException">
    /// Lançada quando a opportunity não é localizada.
    /// </exception>
    public async Task<IEnumerable<Interaction>> GetByOpportunityIdAsync(int opportunityId)
    {
        await ValidateOpportunityExistsAsync(opportunityId);

        return await _interactionRepo.GetByOpportunityIdAsync(opportunityId);
    }

    /// <summary>
    /// Remove uma interação do sistema.
    /// </summary>
    /// <param name="idInteraction">ID da interação a ser removida.</param>
    /// <exception cref="ArgumentException">
    /// Lançada quando o identificador informado é inválido.
    /// </exception>
    /// <exception cref="KeyNotFoundException">
    /// Lançada quando a interação não é localizada.
    /// </exception>
    public async Task DeleteAsync(int idInteraction)
    {
        var interactionEntity = await GetByIdAsync(idInteraction);
        await _interactionRepo.DeleteAsync(interactionEntity);
    }

    /// <summary>
    /// Valida as regras básicas e de negócio da interação.
    /// </summary>
    /// <param name="interaction">Interação a ser validada.</param>
    /// <exception cref="ArgumentException">
    /// Lançada quando descrição, usuário, stages ou datas são inválidos.
    /// </exception>
    /// <exception cref="KeyNotFoundException">
    /// Lançada quando o usuário da interação não é encontrado.
    /// </exception>
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

    /// <summary>
    /// Valida se existe uma opportunity com o ID informado.
    /// </summary>
    /// <param name="opportunityId">ID da opportunity.</param>
    /// <returns>Opportunity encontrada.</returns>
    /// <exception cref="ArgumentException">
    /// Lançada quando o identificador informado é inválido.
    /// </exception>
    /// <exception cref="KeyNotFoundException">
    /// Lançada quando a opportunity não é localizada.
    /// </exception>
    private async Task<Opportunity> ValidateOpportunityExistsAsync(int opportunityId)
    {
        if (opportunityId <= 0)
            throw new ArgumentException("A opportunity informada é inválida.");

        var opportunityEntity = await _opportunityRepo.GetByIdAsync(opportunityId);

        if (opportunityEntity == null)
            throw new KeyNotFoundException("Opportunity não localizada.");

        return opportunityEntity;
    }
}