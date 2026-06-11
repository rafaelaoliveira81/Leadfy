using Application.DTO;
using Domain.Entities;
using Domain.Enuns;
using Domain.Interface;

namespace Application;

public class LeadApp : ILeadApp
{
    private readonly ILeadRepo _leadRepo;
    private readonly IOpportunityRepo _opportunityRepo;
    private readonly ITenantProvider _tenantProvider;

    public LeadApp(ILeadRepo leadRepo, IOpportunityRepo opportunityRepo, ITenantProvider tenantProvider)
    {
        _leadRepo = leadRepo;
        _opportunityRepo = opportunityRepo;
        _tenantProvider = tenantProvider;
    }
    public async Task<string> AddAsync(LeadRequest request, string idUser)
    {
        ValidateLeadInformation(request);

        var tenantId = _tenantProvider.GetRequiredTenantId();

        if (!Guid.TryParse(idUser, out var userGuid))
            throw new ArgumentException("O identificador do usuário é inválido.");

        var lead = MapToLeadRequest(request, tenantId);

        var idLead = await _leadRepo.CreateAsync(lead);

        await _opportunityRepo.CreateAsync(new Opportunity
        {
            TenantId = tenantId,
            LeadId = idLead,
            UserId = userGuid,
            Amount = 0,
            Stage = OpportunityStage.NewLead,
            ExpectedCloseDate = null
        });

        return idLead.ToString();
    }

    public async Task<LeadResponse> GetByIdAsync(string idLead)
    {
        var lead = await ValidateLeadExistsByIdAsync(idLead);

        return MapToLeadResponse(lead);
    }

    public async Task<LeadPagedResponse> GetAllAsync(bool? status, int pagina, int quantidadePorPagina)
    {
        var tenantId = _tenantProvider.GetRequiredTenantId();

        var lead = await _leadRepo.GetPagedAsync(tenantId, status, pagina, quantidadePorPagina);

        var response = lead.Dados.Select(MapToLeadResponse).ToList();
        return new LeadPagedResponse
        {
            TotalRegistros = lead.TotalRegistros,
            Dados = response
        };
    }

    public async Task UpdateAsync(LeadRequest request)
    {
        var lead = await ValidateLeadExistsByIdAsync(request.Id);

        ValidateLeadInformation(request);

        lead.Name = request.Name;
        lead.Email = request.Email;
        lead.PhoneNumber = request.PhoneNumber;

        await _leadRepo.UpdateAsync(lead);
    }

    public async Task DeleteAsync(string idLead)
    {
        var leadEntity = await ValidateLeadExistsByIdAsync(idLead);

        await _leadRepo.DeleteAsync(leadEntity);
    }

    public async Task DeactivateAsync(string idLead)
    {
        var leadEntity = await ValidateLeadExistsByIdAsync(idLead);

        leadEntity.Deactivate();

        await _leadRepo.UpdateAsync(leadEntity);
    }

    public async Task ActivateAsync(string idLead)
    {
        var leadEntity = await ValidateLeadExistsByIdAsync(idLead);

        leadEntity.Activate();

        await _leadRepo.UpdateAsync(leadEntity);
    }

    #region Utils
    private void ValidateLeadInformation(LeadRequest lead)
    {
        if (lead == null)
            throw new ArgumentException("Lead não pode ser vazio.");

        if (string.IsNullOrWhiteSpace(lead.Name))
            throw new ArgumentException("O nome do lead deve ser informado.");

        if (lead.Name.Length > 150)
            throw new ArgumentException("O nome do lead não pode exceder 150 caracteres.");

        if (!string.IsNullOrWhiteSpace(lead.Email))
        {
            if (lead.Email.Length > 254)
                throw new ArgumentException("O email do lead não pode exceder 254 caracteres.");

            if (!IsValidEmail(lead.Email))
                throw new ArgumentException("O formato do email é inválido.");
        }

        if (!string.IsNullOrWhiteSpace(lead.PhoneNumber))
        {
            if (lead.PhoneNumber.Length > 20)
                throw new ArgumentException("O telefone do lead não pode exceder 20 caracteres.");
        }
    }
    private async Task<Lead> ValidateLeadExistsByIdAsync(string idLead)
    {
        if (!Guid.TryParse(idLead, out var leadGuid))
            throw new ArgumentException("O identificador do lead é inválido.");

        var leadEntity = await _leadRepo.GetByIdAsync(leadGuid);

        if (leadEntity == null)
            throw new KeyNotFoundException("Lead não localizado.");

        return leadEntity;
    }
    private bool IsValidEmail(string email)
    {
        try
        {
            var address = new System.Net.Mail.MailAddress(email);
            return address.Address == email;
        }
        catch
        {
            return false;
        }
    }
    private static Lead MapToLeadRequest(LeadRequest request, Guid tenantId)
    {
        return new Lead
        {
            TenantId = tenantId,
            Name = request.Name,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber
        };
    }

    private static LeadResponse MapToLeadResponse(Lead lead)
    {
        return new LeadResponse
        {
            Id = lead.Id.ToString(),
            Name = lead.Name,
            Email = lead.Email,
            PhoneNumber = lead.PhoneNumber,
            IsActive = lead.IsActive
        };
    }

    #endregion
}
