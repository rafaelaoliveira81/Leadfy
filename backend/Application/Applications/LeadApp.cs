using Domain.Entities;
using System.Text.RegularExpressions;

namespace Application;

/// <summary>
/// Serviço de aplicação responsável por orquestrar os casos de uso relacionados a leads.
/// </summary>
/// <remarks>
/// Esta classe pertence à camada de Application.
/// Sua responsabilidade é validar entradas, aplicar regras de fluxo,
/// coordenar chamadas ao domínio e persistir alterações por meio do repositório.
/// </remarks>
public class LeadApp : ILeadApp
{
    /// <summary>
    /// Repositório responsável pelo acesso e persistência dos leads.
    /// </summary>
    private readonly ILeadRepo _leadRepo;

    /// <summary>
    /// Inicializa uma nova instância de <see cref="LeadApp"/>.
    /// </summary>
    /// <param name="leadRepo">Repositório de leads.</param>
    public LeadApp(ILeadRepo leadRepo)
    {
        _leadRepo = leadRepo;
    }

    /// <summary>
    /// Adiciona um novo lead ao sistema.
    /// </summary>
    /// <param name="lead">Entidade de lead a ser cadastrada.</param>
    /// <returns>Retorna o identificador do lead criado.</returns>
    /// <exception cref="ArgumentException">
    /// Lançada quando os dados do lead são inválidos.
    /// </exception>
    public async Task<int> AddAsync(Lead lead)
    {
        await ValidateLeadInformation(lead);

        return await _leadRepo.AddAsync(lead);
    }

    /// <summary>
    /// Obtém um lead pelo seu identificador.
    /// </summary>
    /// <param name="idLead">ID do lead.</param>
    /// <returns>Lead encontrado.</returns>
    /// <exception cref="KeyNotFoundException">
    /// Lançada quando o lead não é localizado.
    /// </exception>
    public async Task<Lead> GetByIdAsync(int idLead)
    {
        return await ValidateLeadExistsByIdAsync(idLead);
    }

    /// <summary>
    /// Busca leads cujo nome contenha o valor informado.
    /// </summary>
    /// <param name="nameLead">
    /// Texto utilizado para filtrar os leads pelo nome.
    /// Não pode ser nulo, vazio ou composto apenas por espaços.
    /// </param>
    /// <returns>
    /// Uma coleção de leads que possuem o nome contendo o valor informado.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Lançada quando o parâmetro <paramref name="nameLead"/> é nulo ou inválido.
    /// </exception>
    /// <exception cref="KeyNotFoundException">
    /// Lançada quando nenhum lead é encontrado para o critério informado.
    /// </exception>
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

    /// <summary>
    /// Obtém todos os leads cadastrados.
    /// </summary>
    /// <returns>Coleção com todos os leads.</returns>
    public async Task<IEnumerable<Lead>> GetAllAsync()
    {
        return await _leadRepo.GetAllAsync();
    }

    /// <summary>
    /// Obtém todos os leads filtrando pelo status.
    /// </summary>
    /// <param name="statusLead">
    /// Status desejado para o filtro (true = ativo, false = inativo).
    /// </param>
    /// <returns>Coleção de leads com o status informado.</returns>
    public async Task<IEnumerable<Lead>> GetAllByStatusAsync(bool statusLead)
    {
        return await _leadRepo.GetAllByStatusAsync(statusLead);
    }

    /// <summary>
    /// Atualiza os dados de um lead existente.
    /// </summary>
    /// <param name="lead">Lead com os dados atualizados.</param>
    /// <exception cref="ArgumentException">
    /// Lançada quando os dados do lead são inválidos.
    /// </exception>
    /// <exception cref="KeyNotFoundException">
    /// Lançada quando o lead a ser atualizado não é localizado.
    /// </exception>
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

    /// <summary>
    /// Remove um lead do sistema.
    /// </summary>
    /// <param name="idLead">ID do lead a ser removido.</param>
    /// <exception cref="KeyNotFoundException">
    /// Lançada quando o lead não é localizado.
    /// </exception>
    /// <remarks>
    /// Este método realiza remoção física do registro.
    /// </remarks>
    public async Task DeleteAsync(int idLead)
    {
        var leadEntity = await ValidateLeadExistsByIdAsync(idLead);

        await _leadRepo.DeleteAsync(leadEntity);
    }

    /// <summary>
    /// Desativa um lead no sistema.
    /// </summary>
    /// <param name="idLead">ID do lead a ser desativado.</param>
    /// <exception cref="KeyNotFoundException">
    /// Lançada quando o lead não é localizado.
    /// </exception>
    public async Task DeactivateAsync(int idLead)
    {
        var leadEntity = await ValidateLeadExistsByIdAsync(idLead);

        leadEntity.Deactivate();

        await _leadRepo.UpdateAsync(leadEntity);
    }

    /// <summary>
    /// Ativa um lead no sistema.
    /// </summary>
    /// <param name="idLead">ID do lead a ser ativado.</param>
    /// <exception cref="KeyNotFoundException">
    /// Lançada quando o lead não é localizado.
    /// </exception>
    public async Task ActivateAsync(int idLead)
    {
        var leadEntity = await ValidateLeadExistsByIdAsync(idLead);

        leadEntity.Activate();

        await _leadRepo.UpdateAsync(leadEntity);
    }

    #region Métodos auxiliares

    /// <summary>
    /// Valida as regras básicas e de negócio do lead.
    /// </summary>
    /// <param name="lead">Lead a ser validado.</param>
    /// <exception cref="ArgumentException">
    /// Lançada quando dados obrigatórios são inválidos.
    /// </exception>
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
    /// Valida se um email possui formato válido.
    /// </summary>
    /// <param name="email">Email a ser validado.</param>
    /// <returns>True se o email é válido, false caso contrário.</returns>
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
