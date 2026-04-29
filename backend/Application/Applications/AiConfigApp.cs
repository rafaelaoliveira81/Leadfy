using Domain.Entities;

namespace Application;

/// <summary>
/// Serviço de aplicação responsável por orquestrar os casos de uso de configuração de IA.
/// </summary>
public class AiConfigApp : IAiConfigApp
{
    private readonly IAiConfigRepo _repo;
    private readonly ILeadRepo _leadRepo;
    private readonly IApiKeyEncryptionService _encryption;

    /// <summary>
    /// Inicializa uma nova instância de <see cref="AiConfigApp"/>.
    /// </summary>
    public AiConfigApp(IAiConfigRepo repo, ILeadRepo leadRepo, IApiKeyEncryptionService encryption)
    {
        _repo = repo;
        _leadRepo = leadRepo;
        _encryption = encryption;
    }

    /// <summary>
    /// Cria uma nova configuração de IA criptografando a chave de API antes de persistir.
    /// </summary>
    /// <param name="promptTemplate">Template do prompt com placeholders opcionais.</param>
    /// <param name="modelName">Nome do modelo de IA (ex: gpt-4.1).</param>
    /// <param name="plainApiKey">Chave da API do GitHub Models em texto plano.</param>
    /// <returns>ID da configuração criada.</returns>
    /// <exception cref="ArgumentException">Lançada quando os dados são inválidos.</exception>
    public async Task<int> AddAsync(string promptTemplate, string modelName, string plainApiKey)
    {
        ValidateConfigInput(promptTemplate, modelName, plainApiKey);

        var encryptedKey = _encryption.Encrypt(plainApiKey.Trim());
        var config = new AiConfig(promptTemplate.Trim(), modelName.Trim(), encryptedKey);

        return await _repo.AddAsync(config);
    }

    /// <summary>
    /// Retorna uma configuração pelo seu identificador.
    /// </summary>
    /// <param name="id">ID da configuração.</param>
    /// <exception cref="KeyNotFoundException">Lançada quando a configuração não é localizada.</exception>
    public async Task<AiConfig> GetByIdAsync(int id)
    {
        return await ValidateExistsByIdAsync(id);
    }

    /// <summary>
    /// Retorna a configuração ativa mais recente, ou null se nenhuma estiver ativa.
    /// </summary>
    public async Task<AiConfig?> GetActiveAsync()
    {
        return await _repo.GetActiveAsync();
    }

    /// <summary>
    /// Retorna todas as configurações de IA cadastradas.
    /// </summary>
    public async Task<IEnumerable<AiConfig>> GetAllAsync()
    {
        return await _repo.GetAllAsync();
    }

    /// <summary>
    /// Atualiza o template e o modelo de uma configuração existente.
    /// Quando <paramref name="newPlainApiKey"/> for informada, a chave é recriptografada.
    /// </summary>
    /// <param name="id">ID da configuração a ser atualizada.</param>
    /// <param name="promptTemplate">Novo template do prompt.</param>
    /// <param name="modelName">Novo nome do modelo.</param>
    /// <param name="newPlainApiKey">Nova chave de API em texto plano, ou null para manter a existente.</param>
    /// <exception cref="ArgumentException">Lançada quando os dados são inválidos.</exception>
    /// <exception cref="KeyNotFoundException">Lançada quando a configuração não é localizada.</exception>
    public async Task UpdateAsync(int id, string promptTemplate, string modelName, string? newPlainApiKey)
    {
        var config = await ValidateExistsByIdAsync(id);

        ValidateConfigInput(promptTemplate, modelName, newPlainApiKey, requireApiKey: false);

        string? encryptedKey = null;
        if (!string.IsNullOrWhiteSpace(newPlainApiKey))
            encryptedKey = _encryption.Encrypt(newPlainApiKey.Trim());

        config.Update(promptTemplate.Trim(), modelName.Trim(), encryptedKey);

        await _repo.UpdateAsync(config);
    }

    /// <summary>
    /// Remove fisicamente uma configuração de IA.
    /// </summary>
    /// <param name="id">ID da configuração a ser removida.</param>
    /// <exception cref="KeyNotFoundException">Lançada quando a configuração não é localizada.</exception>
    public async Task DeleteAsync(int id)
    {
        var config = await ValidateExistsByIdAsync(id);
        await _repo.DeleteAsync(config);
    }

    /// <summary>
    /// Ativa uma configuração de IA.
    /// </summary>
    /// <param name="id">ID da configuração.</param>
    /// <exception cref="KeyNotFoundException">Lançada quando a configuração não é localizada.</exception>
    public async Task ActivateAsync(int id)
    {
        var config = await ValidateExistsByIdAsync(id);
        config.Activate();
        await _repo.UpdateAsync(config);
    }

    /// <summary>
    /// Desativa uma configuração de IA.
    /// </summary>
    /// <param name="id">ID da configuração.</param>
    /// <exception cref="KeyNotFoundException">Lançada quando a configuração não é localizada.</exception>
    public async Task DeactivateAsync(int id)
    {
        var config = await ValidateExistsByIdAsync(id);
        config.Deactivate();
        await _repo.UpdateAsync(config);
    }

    /// <summary>
    /// Descriptografa a chave de API armazenada. Usar apenas internamente para chamadas ao serviço de IA.
    /// </summary>
    public string DecryptApiKey(string encryptedApiKey)
    {
        return _encryption.Decrypt(encryptedApiKey);
    }

    /// <summary>
    /// Constrói o prompt final substituindo os placeholders pelo dados do lead.
    /// </summary>
    /// <param name="config">Configuração de IA com o template.</param>
    /// <param name="lead">Lead cujos dados serão inseridos no prompt.</param>
    /// <returns>Prompt pronto para envio ao modelo de IA.</returns>
    public string BuildPrompt(AiConfig config, Lead lead)
    {
        return config.PromptTemplate
            .Replace("{{LeadName}}", lead.Name ?? string.Empty)
            .Replace("{{LeadEmail}}", lead.Email ?? string.Empty)
            .Replace("{{LeadPhone}}", lead.PhoneNumber ?? string.Empty);
    }

    #region Métodos auxiliares

    private async Task<AiConfig> ValidateExistsByIdAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("ID da configuração deve ser maior que zero.");

        var config = await _repo.GetByIdAsync(id);

        if (config == null)
            throw new KeyNotFoundException("Configuração de IA não localizada.");

        return config;
    }

    private static void ValidateConfigInput(string promptTemplate, string modelName, string? plainApiKey, bool requireApiKey = true)
    {
        if (string.IsNullOrWhiteSpace(promptTemplate))
            throw new ArgumentException("O template do prompt não pode ser vazio.");

        if (promptTemplate.Length > 2000)
            throw new ArgumentException("O template do prompt não pode exceder 2000 caracteres.");

        if (string.IsNullOrWhiteSpace(modelName))
            throw new ArgumentException("O nome do modelo não pode ser vazio.");

        if (modelName.Length > 100)
            throw new ArgumentException("O nome do modelo não pode exceder 100 caracteres.");

        if (requireApiKey && string.IsNullOrWhiteSpace(plainApiKey))
            throw new ArgumentException("A chave de API do GitHub deve ser informada.");
    }

    #endregion
}
