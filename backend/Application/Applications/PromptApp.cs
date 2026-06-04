using Application.DTO;
using Domain.Entities;
using System.Text;


namespace Application;

public class PromptApp : IPromptApp
{
    private readonly IPromptRepo _promptRepo;
    private readonly IAiService _aiService;
    public PromptApp(IPromptRepo promptRepo, IAiService aiService)
    {
        _promptRepo = promptRepo;
        _aiService = aiService;
    }
    public async Task<int> AddAsync(PromptRequest request)
    {
        ValidatePromptInformation(request);

        var prompt = MapToPromptRequest(request);

        return await _promptRepo.AddAsync(prompt);
    }

    public async Task<PromptResponse> GetByIdAsync(int idPrompt)
    {
        var prompt = await ValidatePromptExistsByIdAsync(idPrompt);

        return MapToPromptResponse(prompt);
    }

    public async Task<PromptPagedResponse> GetAllAsync(bool? status, int pagina, int quantidadePorPagina)
    {
        var prompt = await _promptRepo.GetPagedAsync(status, pagina, quantidadePorPagina);

        var response = prompt.Dados.Select(MapToPromptResponse).ToList();
        return new PromptPagedResponse
        {
            TotalRegistros = prompt.TotalRegistros,
            Dados = response
        };
    }

    public async Task UpdateAsync(PromptRequest request)
    {
        var prompt = await ValidatePromptExistsByIdAsync(request.Id);

        ValidatePromptInformation(request);

        prompt.Title = request.Title;
        prompt.Content = request.Content;

        await _promptRepo.UpdateAsync(prompt);
    }

    public async Task DeleteAsync(int idPrompt)
    {
        var promptEntity = await ValidatePromptExistsByIdAsync(idPrompt);

        await _promptRepo.DeleteAsync(promptEntity);
    }

    public async Task DeactivateAsync(int idPrompt)
    {
        var promptEntity = await ValidatePromptExistsByIdAsync(idPrompt);

        promptEntity.Deactivate();

        await _promptRepo.UpdateAsync(promptEntity);
    }

    public async Task ActivateAsync(int idPrompt)
    {
        var promptEntity = await ValidatePromptExistsByIdAsync(idPrompt);

        promptEntity.Activate();

        await _promptRepo.UpdateAsync(promptEntity);
    }

    public async Task<PromptOptimizeDTO> OptimizePromptAsync(PromptOptimizeDTO prompt)
    {
        if (string.IsNullOrWhiteSpace(prompt.Title))
            throw new ArgumentException("Titulo não pode ser vazio.");

        if (string.IsNullOrWhiteSpace(prompt.Content))
            throw new ArgumentException("Prompt não pode ser vazio.");

        var promptRequest = BuildPrompt(prompt);

        var response = await _aiService.GetResponseFromModel(promptRequest);

        return new PromptOptimizeDTO { Content = response };
    }

    #region Utils
    private void ValidatePromptInformation(PromptRequest prompt)
    {
        if (prompt == null)
            throw new ArgumentException("Prompt não pode ser vazio.");

        if (string.IsNullOrWhiteSpace(prompt.Title))
            throw new ArgumentException("O titulo do prompt deve ser informado.");

        if (prompt.Title.Length > 150)
            throw new ArgumentException("O titulo do prompt não pode exceder 150 caracteres.");

        if (string.IsNullOrWhiteSpace(prompt.Content))
            throw new ArgumentException("O prompt deve ser informado.");

        if (prompt.Title.Length > 1000)
            throw new ArgumentException("O prompt não pode exceder 1000 caracteres.");
    }

    private async Task<Prompt> ValidatePromptExistsByIdAsync(int idPrompt)
    {
        var prompt = await _promptRepo.GetByIdAsync(idPrompt);

        if (prompt == null)
            throw new KeyNotFoundException("Prompt não localizado.");

        return prompt;
    }

    private static Prompt MapToPromptRequest(PromptRequest request)
    {
        return new Prompt
        {
            Title = request.Title,
            Content = request.Content
        };
    }

    private static PromptResponse MapToPromptResponse(Prompt prompt)
    {
        return new PromptResponse
        {
            Id = prompt.Id,
            Title = prompt.Title,
            Content = prompt.Content,
            IsActive = prompt.IsActive
        };
    }

    private string BuildPrompt(PromptOptimizeDTO userPrompt)
    {
        var prompt = new StringBuilder();
        prompt.AppendLine("Você é um especialista em engenharia de prompts para IA aplicada a vendas, CRM e conversão de leads.");
        prompt.AppendLine();
        prompt.AppendLine("Sua tarefa é melhorar, otimizar e reestruturar o prompt enviado pelo usuário, mantendo a intenção original, mas tornando-o significativamente mais claro, específico, interpretável e eficiente para um modelo de IA.");
        prompt.AppendLine();
        prompt.AppendLine("Contexto importante:");
        prompt.AppendLine("O prompt otimizado será utilizado posteriormente junto com dados dinâmicos de uma oportunidade comercial:");
        prompt.AppendLine();
        prompt.AppendLine("Regras para otimização:");
        prompt.AppendLine("1. Preserve a intenção principal do usuário.");
        prompt.AppendLine("2. Reescreva o prompt para aumentar:");
        prompt.AppendLine("   - clareza;");
        prompt.AppendLine("   - precisão;");
        prompt.AppendLine("   - objetividade;");
        prompt.AppendLine("   - contexto operacional;");
        prompt.AppendLine("   - qualidade das instruções para IA.");
        prompt.AppendLine("3. Elimine ambiguidades, redundâncias, inconsistências ou instruções vagas.");
        prompt.AppendLine("4. Transforme pedidos genéricos em orientações mais acionáveis e mensuráveis.");
        prompt.AppendLine("5. Sempre que fizer sentido, fortaleça orientações relacionadas a:");
        prompt.AppendLine("   - personalização;");
        prompt.AppendLine("   - persuasão comercial;");
        prompt.AppendLine("   - adaptação ao estágio da oportunidade;");
        prompt.AppendLine("   - consideração das interações anteriores;");
        prompt.AppendLine("   - condução para próximo passo ou fechamento;");
        prompt.AppendLine("   - tom profissional, natural e humano.");
        prompt.AppendLine("6. Não invente requisitos completamente novos que mudem a intenção original.");
        prompt.AppendLine("7. Produza um prompt pronto para uso em produção.");
        prompt.AppendLine("8. Retorne somente o prompt melhorado, sem explicações, comentários, análise ou justificativas.");
        prompt.AppendLine("9. O prompt final deve ter no máximo 1000 caracteres.");
        prompt.AppendLine();
        prompt.AppendLine("Prompt enviado pelo usuário:");
        prompt.AppendLine($"Título: {userPrompt.Title}");
        prompt.AppendLine($"Conteúdo: {userPrompt.Content}");

        return prompt.ToString();
    }

    #endregion
}
