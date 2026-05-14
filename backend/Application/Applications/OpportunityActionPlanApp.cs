using System.Globalization;
using System.Text;
using Domain.Entities;
using Domain.Enuns;
using Domain.Interfaces;
using Dominio.Enums;
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
        if (opportunityId <= 0)
            throw new ArgumentException("A opportunity informada é inválida.");

        if (configId <= 0)
            throw new ArgumentException("A configuração de IA informada é inválida.");

        var opportunity = await _opportunityRepo.GetByIdAsync(opportunityId);

        if (opportunity == null)
            throw new KeyNotFoundException("Opportunity não localizada.");

        var config = await _aiConfigApp.GetByIdAsync(configId);

        if (!config.IsActive)
            throw new ArgumentException("A configuração de IA informada está inativa.");

        var interactions = (await _interactionRepo.GetByOpportunityIdAsync(opportunityId))
            .Take(3)
            .ToList();

        var prompt = BuildPrompt(config, opportunity, interactions);
        var plainApiKey = _aiConfigApp.DecryptApiKey(config.ApiKeyHash);

        var modelName = config.Model.GetDescription();

        var generatedActionPlan = await _generativeAiService.GenerateAsync(prompt, modelName, plainApiKey);

        if (string.IsNullOrWhiteSpace(generatedActionPlan))
            throw new InvalidOperationException("A IA não retornou um plano de ação válido.");

        opportunity.UpdateActionPlan(generatedActionPlan.Trim());

        await _opportunityRepo.UpdateAsync(opportunity);

        return opportunity;
    }

    private static string BuildPrompt(AiConfig config, Opportunity opportunity, IEnumerable<Interaction> interactions)
    {
        var culture = new CultureInfo("pt-BR");
        var lead = opportunity.Lead;
        var product = opportunity.Product;
        var interactionList = interactions.ToList();

        var promptTemplate = config.PromptTemplate
            .Replace("{{LeadName}}", lead?.Name ?? string.Empty)
            .Replace("{{LeadEmail}}", lead?.Email ?? string.Empty)
            .Replace("{{LeadPhone}}", lead?.PhoneNumber ?? string.Empty)
            .Replace("{{OpportunityTitle}}", opportunity.Title ?? string.Empty)
            .Replace("{{OpportunityAmount}}", opportunity.Amount.ToString("C", culture))
            .Replace("{{OpportunityStage}}", opportunity.Stage.ToString())
            .Replace("{{OpportunityExpectedCloseDate}}", opportunity.ExpectedCloseDate.ToString("dd/MM/yyyy", culture))
            .Replace("{{ProductName}}", product?.Name ?? string.Empty);

        var prompt = new StringBuilder();
        prompt.AppendLine(promptTemplate.Trim());
        prompt.AppendLine();
        prompt.AppendLine("Contexto da oportunidade:");
        prompt.AppendLine($"- Titulo: {opportunity.Title}");
        prompt.AppendLine($"- Stage atual: {opportunity.Stage}");
        prompt.AppendLine($"- Valor: {opportunity.Amount.ToString("C", culture)}");
        prompt.AppendLine($"- Fechamento previsto: {opportunity.ExpectedCloseDate.ToString("dd/MM/yyyy", culture)}");
        prompt.AppendLine($"- Produto: {product?.Name ?? "Nao informado"}");
        prompt.AppendLine();
        prompt.AppendLine("Dados do lead vinculado:");
        prompt.AppendLine($"- Nome: {lead?.Name ?? "Nao informado"}");
        prompt.AppendLine($"- Email: {lead?.Email ?? "Nao informado"}");
        prompt.AppendLine($"- Telefone: {lead?.PhoneNumber ?? "Nao informado"}");
        prompt.AppendLine();
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
                prompt.AppendLine($"  Usuario: {interaction.User?.Name ?? "Nao informado"}");
                prompt.AppendLine($"  Stage origem: {GetStageName(interaction.FromStage)}");
                prompt.AppendLine($"  Stage destino: {GetStageName(interaction.ToStage)}");
                prompt.AppendLine($"  Proximo contato: {FormatNextContactDate(interaction.NextContactDate, culture)}");
            }
        }

        prompt.AppendLine();
        prompt.AppendLine("Gere um plano de acao pratico, objetivo e acionavel para os proximos passos do processo comercial.");

        return prompt.ToString();
    }

    private static string GetStageName(int? stage)
    {
        if (!stage.HasValue || !Enum.IsDefined(typeof(OpportunityStage), stage.Value))
            return "Nao informado";

        return ((OpportunityStage)stage.Value).ToString();
    }

    private static string FormatNextContactDate(DateTime? nextContactDate, CultureInfo culture)
    {
        return nextContactDate.HasValue
            ? nextContactDate.Value.ToString("dd/MM/yyyy HH:mm", culture)
            : "Nao informado";
    }
}