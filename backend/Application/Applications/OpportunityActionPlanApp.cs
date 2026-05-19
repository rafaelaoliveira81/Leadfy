using System.Globalization;
using System.Text;
using Domain.Entities;
using Application.Extensions;

namespace Application;

public class OpportunityActionPlanApp : IOpportunityActionPlanApp
{
    private readonly IOpportunityRepo _opportunityRepo;
    private readonly IInteractionRepo _interactionRepo;
    private readonly IAiConfigApp _aiConfigApp;
    private readonly IGenerativeAiService _generativeAiService;

    public OpportunityActionPlanApp(
        IOpportunityRepo opportunityRepo,
        IInteractionRepo interactionRepo,
        IAiConfigApp aiConfigApp,
        IGenerativeAiService generativeAiService)
    {
        _opportunityRepo = opportunityRepo;
        _interactionRepo = interactionRepo;
        _aiConfigApp = aiConfigApp;
        _generativeAiService = generativeAiService;
    }
    public async Task<Opportunity> GenerateAsync(int opportunityId, int configId)
    {
        var opportunity = await GetOpportunityAsync(opportunityId);

        var config = await GetAiConfig(configId);

        var interactions = (await _interactionRepo.GetLastInteractionsByOpportunityIdAsync(opportunityId)).ToList();

        var prompt = BuildPrompt(config, opportunity, interactions);

        var plainApiKey = _aiConfigApp.DecryptApiKey(config.ApiKeyHash);

        var modelName = config.Model.GetDescription();

        var generatedActionPlan = await _generativeAiService.GenerateAsync(prompt, modelName, plainApiKey);

        if (string.IsNullOrWhiteSpace(generatedActionPlan))
            throw new InvalidOperationException("A IA não retornou um plano de ação válido.");

        opportunity.ActionPlan = generatedActionPlan.Trim();
        opportunity.ActionPlanGeneratedAt = DateTime.UtcNow;

        await _opportunityRepo.UpdateAsync(opportunity);

        return opportunity;
    }

    private async Task<AiConfig> GetAiConfig(int configId)
    {
        if (configId <= 0)
            throw new ArgumentException("A configuração de IA informada é inválida.");

        var config = await _aiConfigApp.GetByIdAsync(configId);

        if (!config.IsActive)
            throw new ArgumentException("A configuração de IA informada está inativa.");
        return config;
    }

    private async Task<Opportunity> GetOpportunityAsync(int opportunityId)
    {
        if (opportunityId <= 0)
            throw new ArgumentException("A opportunity informada é inválida.");

        var opportunity = await _opportunityRepo.GetByIdAsync(opportunityId);

        if (opportunity == null)
            throw new KeyNotFoundException("Opportunity não localizada.");
        return opportunity;
    }

    private static string BuildPrompt(AiConfig config, Opportunity opportunity, IEnumerable<Interaction> interactions)
    {
        var culture = new CultureInfo("pt-BR");
        var lead = opportunity.Lead;
        var product = opportunity.Product;
        var interactionList = interactions.ToList();

        var promptTemplate = config.PromptTemplate;

        var prompt = new StringBuilder();
        prompt.AppendLine(promptTemplate.Trim());
        prompt.AppendLine();
        prompt.AppendLine("Contexto da oportunidade:");
        prompt.AppendLine($"- Stage atual: {opportunity.Stage}");
        prompt.AppendLine($"- Valor: {opportunity.Amount.ToString("C", culture)}");
        prompt.AppendLine($"- Fechamento previsto: {opportunity.ExpectedCloseDate.ToString("dd/MM/yyyy", culture)}");
        prompt.AppendLine($"- Produto: {product?.Name ?? "Nao informado"}");
        prompt.AppendLine();
        prompt.AppendLine("Dados do lead vinculado:");
        prompt.AppendLine($"- Nome: {lead?.Name ?? "Nao informado"}");
        prompt.AppendLine();
        prompt.AppendLine("Data Atual: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm", culture));
        prompt.AppendLine("Ultimas 3 interacoes registradas:");

        if (!interactionList.Any())
        {
            prompt.AppendLine("- Nenhuma interacao registrada para esta opportunity.");
        }
        else
        {
            foreach (var interaction in interactionList)
            {
                prompt.AppendLine($"- Data: {interaction.InteractionDate.ToString("dd/MM/yyyy HH:mm", culture)}");
                prompt.AppendLine($"  Descricao: {interaction.Description}");
            }
        }

        prompt.AppendLine();
        prompt.AppendLine();
        prompt.AppendLine("Instruções de saída:");
        prompt.AppendLine("Com base no contexto da oportunidade e nas interações anteriores, gere uma mensagem comercial personalizada para enviar ao lead pelo WhatsApp.");
        prompt.AppendLine("O objetivo principal da mensagem é converter o lead em cliente.");
        prompt.AppendLine("Sempre conduza a mensagem para fechamento ou avanço claro da negociação.");
        prompt.AppendLine("A mensagem deve:");
        prompt.AppendLine("- Ser natural, persuasiva e profissional;");
        prompt.AppendLine("- Considerar o estágio atual da oportunidade;");
        prompt.AppendLine("- Levar em conta as objeções ou interesses demonstrados nas interações;");
        prompt.AppendLine("- Incentivar uma próxima ação clara (resposta, reunião, fechamento ou follow-up);");
        prompt.AppendLine("- Ser objetiva e pronta para envio;");
        prompt.AppendLine("- Não explicar o raciocínio, retornar apenas a mensagem final.");

        return prompt.ToString();
    }
}