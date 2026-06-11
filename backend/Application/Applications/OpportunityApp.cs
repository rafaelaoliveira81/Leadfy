using Application.DTO;
using Domain.Entities;
using Domain.Enuns;
using Domain.Interface;

namespace Application;

public class OpportunityApp : IOpportunityApp
{
    private readonly IOpportunityRepo _opportunityRepo;
    private readonly ILeadRepo _leadRepo;
    private readonly IProductRepo _productRepo;
    private readonly ITenantProvider _tenantProvider;

    public OpportunityApp(IOpportunityRepo opportunityRepo, ILeadRepo leadRepo, IProductRepo productRepo, ITenantProvider tenantProvider)
    {
        _opportunityRepo = opportunityRepo;
        _leadRepo = leadRepo;
        _productRepo = productRepo;
        _tenantProvider = tenantProvider;
    }
    public async Task<string> AddAsync(OpportunityAdd request, string idUser)
    {
        var tenantId = _tenantProvider.GetRequiredTenantId();

        if (!Guid.TryParse(idUser, out var userGuid))
            throw new ArgumentException("O identificador do usuário é inválido.");

        var opportunity = MapToOpportunity(request, tenantId);

        opportunity.UserId = userGuid;

        await ValidateOpportunityInformation(opportunity);

        return (await _opportunityRepo.CreateAsync(opportunity)).ToString();
    }
    public async Task<OpportunityResponse> GetByIdAsync(string idOpportunity)
    {
        var opportunity = await ValidateOpportunityExistsByIdAsync(idOpportunity);

        return MapToOpportunityResponse(opportunity);
    }
    public async Task<IEnumerable<OpportunityResponse>> GetAllAsync()
    {
        _tenantProvider.GetRequiredTenantId();

        var opportunities = await _opportunityRepo.GetAllAsync();

        return opportunities.Select(MapToOpportunityResponse);
    }
    public async Task<IEnumerable<OpportunityResponse>> GetByStageAsync(int stage)
    {
        ParseOpportunityStage(stage, "A stage informada é inválida.");

        var tenantId = _tenantProvider.GetRequiredTenantId();

        var opportunities = await _opportunityRepo.GetByStageAsync(tenantId, stage);

        return opportunities.Select(MapToOpportunityResponse);
    }
    public async Task UpdateAsync(string idOpportunity, OpportunityUpdate request)
    {
        var opportunityEntity = await ValidateOpportunityExistsByIdAsync(idOpportunity);
        var opportunity = MapToOpportunity(request, idOpportunity, opportunityEntity.TenantId);

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

        await _opportunityRepo.UpdateAsync(opportunityEntity);
    }
    public async Task DeleteAsync(string idOpportunity)
    {
        var opportunityEntity = await ValidateOpportunityExistsByIdAsync(idOpportunity);

        await _opportunityRepo.DeleteAsync(opportunityEntity);
    }

    #region Utils
    private async Task ValidateOpportunityInformation(Opportunity opportunity)
    {
        if (opportunity == null)
            throw new ArgumentException("Opportunity não pode ser vazia.");

        if (!Enum.IsDefined(typeof(OpportunityStage), opportunity.Stage))
            throw new ArgumentException("A stage da opportunity é inválida.");

        if (opportunity.LeadId == Guid.Empty)
            throw new ArgumentException("O lead vinculado à opportunity deve ser informado.");

        if (opportunity.ProductId.HasValue && opportunity.ProductId.Value == Guid.Empty)
            throw new ArgumentException("Quando informado, o produto vinculado à opportunity deve ser válido.");

        if (opportunity.Amount <= 0)
            throw new ArgumentException("O amount deve ser maior que zero.");

        if (opportunity.ExpectedCloseDate.HasValue && opportunity.ExpectedCloseDate.Value < DateTime.Now.Date)
            throw new ArgumentException("A data esperada de encerramento não pode ser anterior a hoje.");

        await ValidateLeadExistsByIdAsync(opportunity.LeadId);

        if (opportunity.ProductId.HasValue)
            await ValidateProductExistsByIdAsync(opportunity.ProductId.Value);
    }
    private async Task<Opportunity> ValidateOpportunityExistsByIdAsync(string idOpportunity)
    {
        if (!Guid.TryParse(idOpportunity, out var opportunityGuid))
            throw new ArgumentException("O identificador da opportunity é inválido.");

        var opportunityEntity = await _opportunityRepo.GetByIdAsync(opportunityGuid);

        if (opportunityEntity == null)
            throw new KeyNotFoundException("Opportunity não localizada.");

        return opportunityEntity;
    }
    private async Task<Lead> ValidateLeadExistsByIdAsync(Guid idLead)
    {
        var leadEntity = await _leadRepo.GetByIdAsync(idLead);
        if (leadEntity == null)
            throw new KeyNotFoundException("Lead não localizado.");

        return leadEntity;
    }
    private async Task<Product> ValidateProductExistsByIdAsync(Guid idProduct)
    {
        var productEntity = await _productRepo.GetByIdAsync(idProduct);
        if (productEntity == null)
            throw new KeyNotFoundException("Produto não localizado.");

        return productEntity;
    }
    private static Opportunity MapToOpportunity(OpportunityAdd request, Guid tenantId)
    {
        if (!Guid.TryParse(request.LeadId, out var leadId))
            throw new ArgumentException("O identificador do lead é inválido.");

        if (!string.IsNullOrWhiteSpace(request.ProductId) &&
            !Guid.TryParse(request.ProductId, out _))
            throw new ArgumentException("O identificador do produto é inválido.");

        return new Opportunity
        {
            TenantId = tenantId,
            LeadId = leadId,
            ProductId = string.IsNullOrWhiteSpace(request.ProductId)
                ? null
                : Guid.Parse(request.ProductId),
            Stage = (OpportunityStage)request.Stage,
            Amount = request.Amount,
            ExpectedCloseDate = request.ExpectedCloseDate
        };
    }
    private static Opportunity MapToOpportunity(OpportunityUpdate request, string idOpportunity, Guid tenantId)
    {
        if (!Guid.TryParse(idOpportunity, out var opportunityGuid))
            throw new ArgumentException("O identificador da opportunity é inválido.");

        if (!Guid.TryParse(request.LeadId, out var leadId))
            throw new ArgumentException("O identificador do lead é inválido.");

        if (!string.IsNullOrWhiteSpace(request.ProductId) &&
            !Guid.TryParse(request.ProductId, out _))
            throw new ArgumentException("O identificador do produto é inválido.");

        return new Opportunity
        {
            Id = opportunityGuid,
            TenantId = tenantId,
            LeadId = leadId,
            ProductId = string.IsNullOrWhiteSpace(request.ProductId)
                ? null
                : Guid.Parse(request.ProductId),
            Stage = (OpportunityStage)request.Stage,
            Amount = request.Amount,
            ExpectedCloseDate = request.ExpectedCloseDate,
        };
    }
    private static OpportunityResponse MapToOpportunityResponse(Opportunity opportunity)
    {
        return new OpportunityResponse
        {
            Id = opportunity.Id.ToString(),
            LeadId = opportunity.LeadId.ToString(),
            LeadName = opportunity.Lead?.Name,
            PhoneNumber = opportunity.Lead?.PhoneNumber,
            ProductId = opportunity.ProductId?.ToString(),
            ProductName = opportunity.Product?.Name,
            Stage = (int)opportunity.Stage,
            StageName = opportunity.Stage.ToString(),
            Amount = opportunity.Amount,
            SortOrder = opportunity.SortOrder,
            ExpectedCloseDate = opportunity.ExpectedCloseDate,
            CreatedAt = opportunity.CreatedAt,
        };
    }

    #endregion
    public async Task ChangeStageAsync(string idOpportunity, int newStage)
    {
        var stage = ParseOpportunityStage(newStage, "A stage informada é inválida.");

        var opportunityEntity = await ValidateOpportunityExistsByIdAsync(idOpportunity);

        opportunityEntity.Stage = stage;

        await _opportunityRepo.UpdateAsync(opportunityEntity);
    }
    public async Task UpdateSortOrderAsync(IEnumerable<OpportunitySortOrderItem> items)
    {
        if (items == null || !items.Any())
            throw new ArgumentException("A lista de itens não pode ser vazia.");

        foreach (var item in items)
        {
            var opportunityEntity = await ValidateOpportunityExistsByIdAsync(item.Id);
            opportunityEntity.Stage = ParseOpportunityStage(item.Stage, "A stage informada na lista é inválida.");
            opportunityEntity.SortOrder = item.SortOrder;
            await _opportunityRepo.UpdateAsync(opportunityEntity);
        }
    }
    private static OpportunityStage ParseOpportunityStage(int stage, string errorMessage)
    {
        if (!Enum.IsDefined(typeof(OpportunityStage), stage))
            throw new ArgumentException(errorMessage);

        return (OpportunityStage)stage;
    }
}
