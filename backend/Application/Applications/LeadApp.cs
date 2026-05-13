using Domain.Entities;
using System.Text.RegularExpressions;

namespace Application;

public class LeadApp : ILeadApp
{
    private readonly ILeadRepo _leadRepo;
    public LeadApp(ILeadRepo leadRepo)
    {
        _leadRepo = leadRepo;
    }
    public async Task<int> AddAsync(Lead lead)
    {
        await ValidateLeadInformation(lead);

        return await _leadRepo.AddAsync(lead);
    }
    public async Task<Lead> GetByIdAsync(int idLead)
    {
        return await ValidateLeadExistsByIdAsync(idLead);
    }
    public async Task<IEnumerable<Lead>> GetByNameContainingAsync(string nameLead)
    {
        if (string.IsNullOrWhiteSpace(nameLead))
            throw new ArgumentException("Nome do lead não pode ser vazio.");

        nameLead = nameLead.Trim();

        var leadEntity = await _leadRepo.GetByNameContainingAsync(nameLead);

        if (leadEntity == null || !leadEntity.Any())
            throw new KeyNotFoundException("Lead não localizado.");

        return leadEntity;
    }
    public async Task<IEnumerable<Lead>> GetAllAsync()
    {
        return await _leadRepo.GetAllAsync();
    }
    public async Task<IEnumerable<Lead>> GetAllByStatusAsync(bool statusLead)
    {
        return await _leadRepo.GetAllByStatusAsync(statusLead);
    }
    public async Task UpdateAsync(Lead lead)
    {
        var leadEntity = await ValidateLeadExistsByIdAsync(lead.Id);

        await ValidateLeadInformation(lead);

        leadEntity.Name = lead.Name;
        leadEntity.Email = lead.Email;
        leadEntity.PhoneNumber = lead.PhoneNumber;
        leadEntity.IsActive = lead.IsActive;

        await _leadRepo.UpdateAsync(leadEntity);
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
    private async Task ValidateLeadInformation(Lead lead)
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

    #endregion
}
