using Domain.Entities;
using Application.DTO;
using Dominio.Enums;

namespace Application;

public class AiConfigApp : IAiConfigApp
{
    private readonly IAiConfigRepo _aiConfigRepo;
    private readonly ILeadRepo _leadRepo;
    private readonly IApiKeyEncryptionService _encryption;

    public AiConfigApp(IAiConfigRepo aiConfigRepo, ILeadRepo leadRepo, IApiKeyEncryptionService encryption)
    {
        _aiConfigRepo = aiConfigRepo;
        _leadRepo = leadRepo;
        _encryption = encryption;
    }

    public async Task<int> AddAsync(AiConfigRequest request)
    {
        ValidateConfigInput(request);

        var encryptedKey = _encryption.Encrypt(request.ApiKey.Trim());
        var config = new AiConfig(request.Title, request.PromptTemplate.Trim(), (AiModelsEnum)request.Model, encryptedKey);

        return await _aiConfigRepo.AddAsync(config);
    }

    public async Task<AiConfig> GetByIdAsync(int id)
    {
        return await ValidateExistsByIdAsync(id);
    }

    public async Task<AiConfig?> GetActiveAsync()
    {
        return await _aiConfigRepo.GetActiveAsync();
    }

    public async Task<IEnumerable<AiConfig>> GetAllAsync()
    {
        return await _aiConfigRepo.GetAllAsync();
    }

    public async Task UpdateAsync(int id, AiConfigRequest request)
    {
        var config = await ValidateExistsByIdAsync(id);

        ValidateConfigInput(request, requireApiKey: false);

        if (!string.IsNullOrWhiteSpace(request.ApiKey))
            config.ApiKeyHash = _encryption.Encrypt(request.ApiKey.Trim());

        config.Model = (AiModelsEnum)request.Model;
        config.PromptTemplate = request.PromptTemplate.Trim();

        await _aiConfigRepo.UpdateAsync(config);
    }

    public async Task DeleteAsync(int id)
    {
        var config = await ValidateExistsByIdAsync(id);
        await _aiConfigRepo.DeleteAsync(config);
    }

    public async Task ActivateAsync(int id)
    {
        var config = await ValidateExistsByIdAsync(id);
        config.Activate();
        await _aiConfigRepo.UpdateAsync(config);
    }

    public async Task DeactivateAsync(int id)
    {
        var config = await ValidateExistsByIdAsync(id);
        config.Deactivate();
        await _aiConfigRepo.UpdateAsync(config);
    }

    public string DecryptApiKey(string encryptedApiKey)
    {
        return _encryption.Decrypt(encryptedApiKey);
    }

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

        var config = await _aiConfigRepo.GetByIdAsync(id);

        if (config == null)
            throw new KeyNotFoundException("Configuração de IA não localizada.");

        return config;
    }

    private static void ValidateConfigInput(AiConfigRequest request, bool requireApiKey = true)
    {
        if (string.IsNullOrWhiteSpace(request.Title))
            throw new ArgumentException("O título não pode ser vazio.");

        if (string.IsNullOrWhiteSpace(request.PromptTemplate))
            throw new ArgumentException("O template do prompt não pode ser vazio.");

        if (request.PromptTemplate.Length > 2000)
            throw new ArgumentException("O template do prompt não pode exceder 2000 caracteres.");

        if (!Enum.IsDefined(typeof(AiModelsEnum), request.Model))
            throw new ArgumentException("O nome do modelo não pode ser vazio.");

        if (requireApiKey && string.IsNullOrWhiteSpace(request.ApiKey))
            throw new ArgumentException("A chave de API do GitHub deve ser informada.");
    }

    #endregion
}
