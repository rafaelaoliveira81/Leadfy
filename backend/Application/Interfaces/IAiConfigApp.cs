using Domain.Entities;

namespace Application;

/// <summary>
/// Contrato dos casos de uso para configuração de IA.
/// </summary>
public interface IAiConfigApp
{
    /// <summary>Cria uma nova configuração criptografando a <paramref name="plainApiKey"/> antes de persistir.</summary>
    Task<int> AddAsync(string promptTemplate, string modelName, string plainApiKey);

    /// <summary>Retorna uma configuração pelo ID.</summary>
    Task<AiConfig> GetByIdAsync(int id);

    /// <summary>Retorna a configuração ativa mais recente, ou null se nenhuma existir.</summary>
    Task<AiConfig?> GetActiveAsync();

    /// <summary>Retorna todas as configurações cadastradas.</summary>
    Task<IEnumerable<AiConfig>> GetAllAsync();

    /// <summary>
    /// Atualiza template e modelo. Quando <paramref name="newPlainApiKey"/> for informada,
    /// ela substitui a chave existente (criptografada antes de salvar).
    /// </summary>
    Task UpdateAsync(int id, string promptTemplate, string modelName, string? newPlainApiKey);

    /// <summary>Remove fisicamente uma configuração.</summary>
    Task DeleteAsync(int id);

    /// <summary>Ativa uma configuração.</summary>
    Task ActivateAsync(int id);

    /// <summary>Desativa uma configuração.</summary>
    Task DeactivateAsync(int id);

    /// <summary>
    /// Descriptografa a chave de API armazenada e retorna em texto plano.
    /// Usar apenas internamente (servidor → serviço de IA).
    /// </summary>
    string DecryptApiKey(string encryptedApiKey);

    /// <summary>
    /// Substitui os placeholders do template com os dados do lead.
    /// Suporta: {{LeadName}}, {{LeadEmail}}, {{LeadPhone}}.
    /// </summary>
    string BuildPrompt(AiConfig config, Lead lead);
}
