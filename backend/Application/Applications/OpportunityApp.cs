using Domain.Entities;
using Domain.Enuns;

namespace Application;

public class OpportunityApp : IOpportunityApp
{
    private readonly IOpportunityRepo _opportunityRepo;
    private readonly ILeadRepo _leadRepo;
    private readonly IProductRepo _productRepo;
    public OpportunityApp(IOpportunityRepo opportunityRepo, ILeadRepo leadRepo, IProductRepo productRepo)
    {
        _opportunityRepo = opportunityRepo;
        _leadRepo = leadRepo;
        _productRepo = productRepo;
    }
    public async Task<int> AddAsync(Opportunity opportunity)
    {
        await ValidateOpportunityInformation(opportunity);

        return await _opportunityRepo.AddAsync(opportunity);
    }
    public async Task<Opportunity> GetByIdAsync(int idOpportunity)
    {
        return await ValidateOpportunityExistsByIdAsync(idOpportunity);
    }
    public async Task<IEnumerable<Opportunity>> GetAllAsync()
    {
        return await _opportunityRepo.GetAllAsync();
    }
    public async Task<IEnumerable<Opportunity>> GetAllByStatusAsync(bool statusOpportunity)
    {
        return await _opportunityRepo.GetAllByStatusAsync(statusOpportunity);
    }
    public async Task<IEnumerable<Opportunity>> GetByLeadIdAsync(int leadId)
    {
        return await _opportunityRepo.GetByLeadIdAsync(leadId);
    }
    public async Task UpdateAsync(Opportunity opportunity)
    {
        var opportunityEntity = await ValidateOpportunityExistsByIdAsync(opportunity.ID);

        await ValidateOpportunityInformation(opportunity);

        if (opportunityEntity.Stage >= OpportunityStage.ProposalSent &&
            opportunityEntity.Amount != opportunity.Amount)
            throw new ArgumentException("Amount não pode ser atualizado após a stage ProposalSent.");

        opportunityEntity.LeadId = opportunity.LeadId;
        opportunityEntity.ProductId = opportunity.ProductId;
        opportunityEntity.Stage = opportunity.Stage;
        if (opportunityEntity.Stage < OpportunityStage.ProposalSent)
            opportunityEntity.Amount = opportunity.Amount;
        opportunityEntity.ExpectedCloseDate = opportunity.ExpectedCloseDate;
        opportunityEntity.IsActive = opportunity.IsActive;

        await _opportunityRepo.UpdateAsync(opportunityEntity);
    }
    public async Task DeleteAsync(int idOpportunity)
    {
        var opportunityEntity = await ValidateOpportunityExistsByIdAsync(idOpportunity);

        await _opportunityRepo.DeleteAsync(opportunityEntity);
    }
    public async Task DeactivateAsync(int idOpportunity)
    {
        var opportunityEntity = await ValidateOpportunityExistsByIdAsync(idOpportunity);

        opportunityEntity.Deactivate();

        await _opportunityRepo.UpdateAsync(opportunityEntity);
    }
    public async Task ActivateAsync(int idOpportunity)
    {
        var opportunityEntity = await ValidateOpportunityExistsByIdAsync(idOpportunity);

        opportunityEntity.Activate();

        await _opportunityRepo.UpdateAsync(opportunityEntity);
    }

    #region Métodos auxiliares
    private async Task ValidateOpportunityInformation(Opportunity opportunity)
    {
        if (opportunity == null)
            throw new ArgumentException("Opportunity não pode ser vazia.");

        if (!Enum.IsDefined(typeof(OpportunityStage), opportunity.Stage))
            throw new ArgumentException("A stage da opportunity é inválida.");

        if (opportunity.LeadId <= 0)
            throw new ArgumentException("O lead vinculado à opportunity deve ser informado.");

        if (opportunity.ProductId <= 0)
            throw new ArgumentException("O produto vinculado à opportunity deve ser informado.");

        if (opportunity.Amount <= 0)
            throw new ArgumentException("O amount deve ser maior que zero.");

        if (opportunity.ExpectedCloseDate < DateTime.UtcNow.Date)
            throw new ArgumentException("A data esperada de encerramento não pode ser anterior a hoje.");

        await ValidateLeadExistsByIdAsync(opportunity.LeadId);

        await ValidateProductExistsByIdAsync(opportunity.ProductId);
    }
    private async Task<Opportunity> ValidateOpportunityExistsByIdAsync(int idOpportunity)
    {
        var opportunityEntity = await _opportunityRepo.GetByIdAsync(idOpportunity);

        if (opportunityEntity == null)
            throw new KeyNotFoundException("Opportunity não localizada.");

        return opportunityEntity;
    }
    private async Task<Lead> ValidateLeadExistsByIdAsync(int idLead)
    {
        var leadEntity = await _leadRepo.GetByIdAsync(idLead);
        if (leadEntity == null)
            throw new KeyNotFoundException("Lead não localizado.");

        return leadEntity;
    }
    private async Task<Product> ValidateProductExistsByIdAsync(int idProduct)
    {
        var productEntity = await _productRepo.GetByIdAsync(idProduct);
        if (productEntity == null)
            throw new KeyNotFoundException("Produto não localizado.");

        return productEntity;
    }

    #endregion
    public async Task ChangeStageAsync(int idOpportunity, OpportunityStage newStage)
    {
        if (!Enum.IsDefined(typeof(OpportunityStage), newStage))
            throw new ArgumentException("A stage informada é inválida.");

        var opportunityEntity = await ValidateOpportunityExistsByIdAsync(idOpportunity);

        opportunityEntity.Stage = newStage;

        await _opportunityRepo.UpdateAsync(opportunityEntity);
    }
    public async Task UpdateSortOrderAsync(IEnumerable<(int id, OpportunityStage stage, int sortOrder)> items)
    {
        if (items == null || !items.Any())
            throw new ArgumentException("A lista de itens não pode ser vazia.");

        foreach (var item in items)
        {
            var opportunityEntity = await ValidateOpportunityExistsByIdAsync(item.id);
            opportunityEntity.Stage = item.stage;
            opportunityEntity.SortOrder = item.sortOrder;
            await _opportunityRepo.UpdateAsync(opportunityEntity);
        }
    }
}
