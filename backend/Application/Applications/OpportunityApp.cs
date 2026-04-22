using Domain.Entities;
using Domain.Enuns;

namespace Application;

/// <summary>
/// Serviço de aplicação responsável por orquestrar os casos de uso relacionados a opportunities.
/// </summary>
/// <remarks>
/// Esta classe pertence à camada de Application.
/// Sua responsabilidade é validar entradas, aplicar regras de fluxo,
/// coordenar chamadas ao domínio e persistir alterações por meio do repositório.
/// </remarks>
public class OpportunityApp : IOpportunityApp
{
    /// <summary>
    /// Repositório responsável pelo acesso e persistência das opportunities.
    /// </summary>
    private readonly IOpportunityRepo _opportunityRepo;
    private readonly ILeadRepo _leadRepo;
    private readonly IOwerRepo _owerRepo;
    private readonly IProductRepo _productRepo;

    /// <summary>
    /// Inicializa uma nova instância de <see cref="OpportunityApp"/>.
    /// </summary>
    /// <param name="opportunityRepo">Repositório de opportunities.</param>
    /// <param name="leadRepo">Repositório de leads.</param>
    /// <param name="owerRepo">Repositório de owners.</param>
    /// <param name="productRepo">Repositório de produtos.</param>
    public OpportunityApp(IOpportunityRepo opportunityRepo, ILeadRepo leadRepo, IOwerRepo owerRepo, IProductRepo productRepo)
    {
        _opportunityRepo = opportunityRepo;
        _leadRepo = leadRepo;
        _owerRepo = owerRepo;
        _productRepo = productRepo;
    }

    /// <summary>
    /// Adiciona uma nova opportunity ao sistema.
    /// </summary>
    /// <param name="opportunity">Entidade de opportunity a ser cadastrada.</param>
    /// <returns>Retorna o identificador da opportunity criada.</returns>
    /// <exception cref="ArgumentException">
    /// Lançada quando os dados da opportunity são inválidos.
    /// </exception>
    /// <exception cref="KeyNotFoundException">
    /// Lançada quando alguma entidade vinculada não é localizada.
    /// </exception>
    public async Task<int> AddAsync(Opportunity opportunity)
    {
        await ValidateOpportunityInformation(opportunity);

        return await _opportunityRepo.AddAsync(opportunity);
    }

    /// <summary>
    /// Obtém uma opportunity pelo seu identificador.
    /// </summary>
    /// <param name="idOpportunity">ID da opportunity.</param>
    /// <returns>Opportunity encontrada.</returns>
    /// <exception cref="KeyNotFoundException">
    /// Lançada quando a opportunity não é localizada.
    /// </exception>
    public async Task<Opportunity> GetByIdAsync(int idOpportunity)
    {
        return await ValidateOpportunityExistsByIdAsync(idOpportunity);
    }

    /// <summary>
    /// Busca opportunities cujo título contenha o valor informado.
    /// </summary>
    /// <param name="titleOpportunity">
    /// Texto utilizado para filtrar as opportunities pelo título.
    /// Não pode ser nulo, vazio ou composto apenas por espaços.
    /// </param>
    /// <returns>
    /// Uma coleção de opportunities que possuem o título contendo o valor informado.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Lançada quando o parâmetro <paramref name="titleOpportunity"/> é nulo ou inválido.
    /// </exception>
    /// <exception cref="KeyNotFoundException">
    /// Lançada quando nenhuma opportunity é encontrada para o critério informado.
    /// </exception>
    public async Task<IEnumerable<Opportunity>> GetByTitleContainingAsync(string titleOpportunity)
    {
        if (string.IsNullOrWhiteSpace(titleOpportunity))
            throw new ArgumentException("Título da opportunity não pode ser vazio.");

        titleOpportunity = titleOpportunity.Trim();

        var opportunityEntity = await _opportunityRepo.GetByTitleContainingAsync(titleOpportunity);

        if (opportunityEntity == null || !opportunityEntity.Any())
            throw new KeyNotFoundException("Opportunity não localizada.");

        return opportunityEntity;
    }

    /// <summary>
    /// Obtém todas as opportunities cadastradas.
    /// </summary>
    /// <returns>Coleção com todas as opportunities.</returns>
    public async Task<IEnumerable<Opportunity>> GetAllAsync()
    {
        return await _opportunityRepo.GetAllAsync();
    }

    /// <summary>
    /// Obtém todas as opportunities filtrando pelo status.
    /// </summary>
    /// <param name="statusOpportunity">
    /// Status desejado para o filtro (true = ativo, false = inativo).
    /// </param>
    /// <returns>Coleção de opportunities com o status informado.</returns>
    public async Task<IEnumerable<Opportunity>> GetAllByStatusAsync(bool statusOpportunity)
    {
        return await _opportunityRepo.GetAllByStatusAsync(statusOpportunity);
    }

    /// <summary>
    /// Obtém todas as opportunities associadas a um lead específico.
    /// </summary>
    /// <param name="leadId">ID do lead.</param>
    /// <returns>Coleção de opportunities do lead.</returns>
    public async Task<IEnumerable<Opportunity>> GetByLeadIdAsync(int leadId)
    {
        return await _opportunityRepo.GetByLeadIdAsync(leadId);
    }

    /// <summary>
    /// Obtém todas as opportunities associadas a um owner específico.
    /// </summary>
    /// <param name="ownerId">ID do owner.</param>
    /// <returns>Coleção de opportunities do owner.</returns>
    public async Task<IEnumerable<Opportunity>> GetByOwnerIdAsync(int ownerId)
    {
        return await _opportunityRepo.GetByOwnerIdAsync(ownerId);
    }

    /// <summary>
    /// Atualiza os dados de uma opportunity existente.
    /// </summary>
    /// <param name="opportunity">Opportunity com os dados atualizados.</param>
    /// <exception cref="ArgumentException">
    /// Lançada quando os dados da opportunity são inválidos ou o stage é inválido para atualização.
    /// </exception>
    /// <exception cref="KeyNotFoundException">
    /// Lançada quando a opportunity a ser atualizada não é localizada.
    /// </exception>
    public async Task UpdateAsync(Opportunity opportunity)
    {
        var opportunityEntity = await ValidateOpportunityExistsByIdAsync(opportunity.ID);

        await ValidateOpportunityInformation(opportunity);

        // Validação: Amount não pode ser atualizado após ProposalSent.
        if (opportunityEntity.Stage >= OpportunityStage.ProposalSent &&
            opportunityEntity.Amount != opportunity.Amount)
            throw new ArgumentException("Amount não pode ser atualizado após a stage ProposalSent.");

        opportunityEntity.Title = opportunity.Title;
        opportunityEntity.LeadId = opportunity.LeadId;
        opportunityEntity.OwnerId = opportunity.OwnerId;
        opportunityEntity.ProductId = opportunity.ProductId;
        opportunityEntity.Stage = opportunity.Stage;
        if (opportunityEntity.Stage < OpportunityStage.ProposalSent)
            opportunityEntity.Amount = opportunity.Amount;
        opportunityEntity.ExpectedCloseDate = opportunity.ExpectedCloseDate;
        opportunityEntity.IsActive = opportunity.IsActive;

        await _opportunityRepo.UpdateAsync(opportunityEntity);
    }

    /// <summary>
    /// Remove uma opportunity do sistema.
    /// </summary>
    /// <param name="idOpportunity">ID da opportunity a ser removida.</param>
    /// <exception cref="KeyNotFoundException">
    /// Lançada quando a opportunity não é localizada.
    /// </exception>
    /// <remarks>
    /// Este método realiza remoção física do registro.
    /// </remarks>
    public async Task DeleteAsync(int idOpportunity)
    {
        var opportunityEntity = await ValidateOpportunityExistsByIdAsync(idOpportunity);

        await _opportunityRepo.DeleteAsync(opportunityEntity);
    }

    /// <summary>
    /// Desativa uma opportunity no sistema.
    /// </summary>
    /// <param name="idOpportunity">ID da opportunity a ser desativada.</param>
    /// <exception cref="KeyNotFoundException">
    /// Lançada quando a opportunity não é localizada.
    /// </exception>
    public async Task DeactivateAsync(int idOpportunity)
    {
        var opportunityEntity = await ValidateOpportunityExistsByIdAsync(idOpportunity);

        opportunityEntity.Deactivate();

        await _opportunityRepo.UpdateAsync(opportunityEntity);
    }

    /// <summary>
    /// Ativa uma opportunity no sistema.
    /// </summary>
    /// <param name="idOpportunity">ID da opportunity a ser ativada.</param>
    /// <exception cref="KeyNotFoundException">
    /// Lançada quando a opportunity não é localizada.
    /// </exception>
    public async Task ActivateAsync(int idOpportunity)
    {
        var opportunityEntity = await ValidateOpportunityExistsByIdAsync(idOpportunity);

        opportunityEntity.Activate();

        await _opportunityRepo.UpdateAsync(opportunityEntity);
    }

    #region Métodos auxiliares

    /// <summary>
    /// Valida as regras básicas e de negócio da opportunity.
    /// </summary>
    /// <param name="opportunity">Opportunity a ser validada.</param>
    /// <exception cref="ArgumentException">
    /// Lançada quando dados obrigatórios são inválidos ou regras de negócio são violadas.
    /// </exception>
    /// <exception cref="KeyNotFoundException">
    /// Lançada quando entidades vinculadas não são encontradas.
    /// </exception>
    private async Task ValidateOpportunityInformation(Opportunity opportunity)
    {
        if (opportunity == null)
            throw new ArgumentException("Opportunity não pode ser vazia.");

        if (string.IsNullOrWhiteSpace(opportunity.Title))
            throw new ArgumentException("O título da opportunity deve ser informado.");

        if (opportunity.Title.Length > 150)
            throw new ArgumentException("O título da opportunity não pode exceder 150 caracteres.");

        if (!Enum.IsDefined(typeof(OpportunityStage), opportunity.Stage))
            throw new ArgumentException("A stage da opportunity é inválida.");

        if (opportunity.LeadId <= 0)
            throw new ArgumentException("O lead vinculado à opportunity deve ser informado.");

        // Owner é obrigatório apenas quando Stage != NewLead.
        if (opportunity.Stage != OpportunityStage.NewLead && opportunity.OwnerId <= 0)
            throw new ArgumentException("O owner deve ser informado quando a stage não é NewLead.");

        if (opportunity.ProductId <= 0)
            throw new ArgumentException("O produto vinculado à opportunity deve ser informado.");

        if (opportunity.Amount <= 0)
            throw new ArgumentException("O amount deve ser maior que zero.");

        if (opportunity.ExpectedCloseDate < DateTime.UtcNow.Date)
            throw new ArgumentException("A data esperada de encerramento não pode ser anterior a hoje.");

        // Validar que as entidades existem
        await ValidateLeadExistsByIdAsync(opportunity.LeadId);

        if (opportunity.OwnerId.HasValue && opportunity.OwnerId > 0)
            await ValidateOwnerExistsByIdAsync(opportunity.OwnerId.Value);

        await ValidateProductExistsByIdAsync(opportunity.ProductId);
    }

    /// <summary>
    /// Valida se existe uma opportunity com o ID informado.
    /// </summary>
    /// <param name="idOpportunity">ID da opportunity.</param>
    /// <returns>Opportunity encontrada.</returns>
    /// <exception cref="KeyNotFoundException">
    /// Lançada quando a opportunity não é localizada.
    /// </exception>
    private async Task<Opportunity> ValidateOpportunityExistsByIdAsync(int idOpportunity)
    {
        var opportunityEntity = await _opportunityRepo.GetByIdAsync(idOpportunity);

        if (opportunityEntity == null)
            throw new KeyNotFoundException("Opportunity não localizada.");

        return opportunityEntity;
    }

    /// <summary>
    /// Valida se existe um lead com o ID informado.
    /// </summary>
    /// <param name="idLead">ID do lead.</param>
    /// <returns>Lead encontrado.</returns>
    /// <exception cref="KeyNotFoundException">
    /// Lançada quando o lead não é localizado.
    /// </exception>
    private async Task<Lead> ValidateLeadExistsByIdAsync(int idLead)
    {
        var leadEntity = await _leadRepo.GetByIdAsync(idLead);
        if (leadEntity == null)
            throw new KeyNotFoundException("Lead não localizado.");

        return leadEntity;
    }

    /// <summary>
    /// Valida se existe um owner com o ID informado.
    /// </summary>
    /// <param name="idOwner">ID do owner.</param>
    /// <returns>Owner encontrado.</returns>
    /// <exception cref="KeyNotFoundException">
    /// Lançada quando o owner não é localizado.
    /// </exception>
    private async Task<Ower> ValidateOwnerExistsByIdAsync(int idOwner)
    {
        var owerEntity = await _owerRepo.GetByIdAsync(idOwner);
        if (owerEntity == null)
            throw new KeyNotFoundException("Owner não localizado.");

        return owerEntity;
    }

    /// <summary>
    /// Valida se existe um produto com o ID informado.
    /// </summary>
    /// <param name="idProduct">ID do produto.</param>
    /// <returns>Produto encontrado.</returns>
    /// <exception cref="KeyNotFoundException">
    /// Lançada quando o produto não é localizado.
    /// </exception>
    private async Task<Product> ValidateProductExistsByIdAsync(int idProduct)
    {
        var productEntity = await _productRepo.GetByIdAsync(idProduct);
        if (productEntity == null)
            throw new KeyNotFoundException("Produto não localizado.");

        return productEntity;
    }

    #endregion
}
