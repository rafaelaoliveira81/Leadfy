using Application.DTO;
using Domain.Entities;
using Domain.Enuns;

namespace Application;

public class LeadApp : ILeadApp
{
    private readonly ILeadRepo _leadRepo;
    private readonly IOpportunityRepo _opportunityRepo;
    public LeadApp(ILeadRepo leadRepo, IOpportunityRepo opportunityRepo)
    {
        _leadRepo = leadRepo;
        _opportunityRepo = opportunityRepo;
    }
    public async Task<int> AddAsync(LeadRequest request, int idUser)
    {
        await ValidateLeadInformation(request);

        var lead = MapToLeadRequest(request);

        var idLead = await _leadRepo.AddAsync(lead);

        var idOpportunity = await _opportunityRepo.AddAsync(new Opportunity
        {
            LeadId = idLead,
            UserId = idUser,
            Amount = 0,
            Stage = OpportunityStage.NewLead,
            ExpectedCloseDate = null
        });

        return idLead;
    }

    public async Task<LeadResponse> GetByIdAsync(int idLead)
    {
        var lead = await ValidateLeadExistsByIdAsync(idLead);

        return MapToLeadResponse(lead);
    }

    public async Task<LeadPagedResponse> GetAllAsync(bool? status, int pagina, int quantidadePorPagina)
    {
        var lead = await _leadRepo.GetPagedAsync(status, pagina, quantidadePorPagina);

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

        await ValidateLeadInformation(request);

        lead.Name = request.Name;
        lead.Email = request.Email;
        lead.PhoneNumber = request.PhoneNumber;

        await _leadRepo.UpdateAsync(lead);
    }

    public async Task DeleteAsync(int idLead)
    {
        var leadEntity = await ValidateLeadExistsByIdAsync(idLead);

        await _leadRepo.DeleteAsync(leadEntity);
    }

    public async Task DeactivateAsync(int idLead)
    {
        var leadEntity = await ValidateLeadExistsByIdAsync(idLead);

        leadEntity.Deactivate();

        await _leadRepo.UpdateAsync(leadEntity);
    }

    public async Task ActivateAsync(int idLead)
    {
        var leadEntity = await ValidateLeadExistsByIdAsync(idLead);

        leadEntity.Activate();

        await _leadRepo.UpdateAsync(leadEntity);
    }

    #region Métodos auxiliares
    private async Task ValidateLeadInformation(LeadRequest lead)
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
    private async Task<Lead> ValidateLeadExistsByIdAsync(int idLead)
    {
        var leadEntity = await _leadRepo.GetByIdAsync(idLead);

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

    private static Lead MapToLeadRequest(LeadRequest request)
    {
        return new Lead
        {
            Name = request.Name,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber
        };
    }

    private static LeadResponse MapToLeadResponse(Lead lead)
    {
        return new LeadResponse
        {
            ID = lead.Id,
            Name = lead.Name,
            Email = lead.Email,
            PhoneNumber = lead.PhoneNumber,
            IsActive = lead.IsActive
        };
    }

    #endregion
}
