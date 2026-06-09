using System.Globalization;
using System.Text;
using Domain.Entities;
using System.Text.Json;
using System.Text.Json.Serialization;
using Application.DTO;

namespace Application;

public class OpportunityActionPlanApp : IOpportunityActionPlanApp
{
    private readonly IOpportunityRepo _opportunityRepo;
    private readonly IOpportunityActionPlanRepo _actionPlanRepo;
    private readonly ILeadApp _leadApp;
    private readonly IInteractionRepo _interactionRepo;
    private readonly IPromptApp _promptApp;
    private readonly IAiService _aiService;

    public OpportunityActionPlanApp(
        IOpportunityRepo opportunityRepo,
        IOpportunityActionPlanRepo actionPlanRepo,
        ILeadApp leadApp,
        IInteractionRepo interactionRepo,
        IPromptApp promptApp,
        IAiService aiService)
    {
        _opportunityRepo = opportunityRepo;
        _actionPlanRepo = actionPlanRepo;
        _leadApp = leadApp;
        _interactionRepo = interactionRepo;
        _promptApp = promptApp;
        _aiService = aiService;
    }

    public async Task<OpportunityActionPlanDto> GenerateAsync(int opportunityId, string promptId)
    {
        var opportunity = await GetOpportunityAsync(opportunityId);

        var prompt = await _promptApp.GetByIdAsync(promptId);

        var interactions = (await _interactionRepo.GetLastInteractionsByOpportunityIdAsync(opportunityId)).ToList();

        var lead = await _leadApp.GetByIdAsync(opportunity.LeadId);

        var promptRequest = BuildPrompt(prompt.Content, opportunity, interactions, lead.Name);

        var generatedActionPlan = await _aiService.GetResponseFromModel(promptRequest);

        if (string.IsNullOrWhiteSpace(generatedActionPlan))
            throw new InvalidOperationException("A IA não retornou um plano de ação válido.");

        Console.WriteLine("Resposta bruta da IA:");
        Console.WriteLine(generatedActionPlan);

        try
        {
            var parsedActionPlan = JsonSerializer.Deserialize<ActionPlanServiceResponse>(generatedActionPlan);

            var actionPlanDto = MapToDtoByDto(parsedActionPlan);
            var actionPlan = MapToDtEntity(actionPlanDto);
            actionPlan.OpportunityId = opportunityId;

            var id = await _actionPlanRepo.AddAsync(actionPlan);
            actionPlan.Id = id;

            return MapToDto(actionPlan);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(ex.Message);
        }
    }

    public async Task<IEnumerable<OpportunityActionPlanDto>> GetByOpportunityIdAsync(int opportunityId)
    {
        await GetOpportunityAsync(opportunityId);

        var actionPlans = await _actionPlanRepo.GetByOpportunityIdAsync(opportunityId);

        return actionPlans.Select(ap => MapToDto(ap));
    }

    #region Utils

    private static OpportunityActionPlanDto MapToDto(OpportunityActionPlan actionPlan)
    {
        return new OpportunityActionPlanDto
        {
            Id = actionPlan.Id,
            OpportunityId = actionPlan.OpportunityId,
            Message = actionPlan.Message ?? string.Empty,
            ActionPlan = actionPlan.ActionPlan,
            GeneratedAt = actionPlan.GeneratedAt
        };
    }
    private static OpportunityActionPlanDto MapToDtoByDto(ActionPlanServiceResponse actionPlan)
    {
        return new OpportunityActionPlanDto
        {
            Message = actionPlan.message ?? string.Empty,
            ActionPlan = actionPlan.actionPlan,
            GeneratedAt = DateTime.Now
        };
    }

    private static OpportunityActionPlan MapToDtEntity(OpportunityActionPlanDto actionPlan)
    {
        return new OpportunityActionPlan
        {
            Id = actionPlan.Id,
            OpportunityId = actionPlan.OpportunityId,
            Message = actionPlan.Message ?? string.Empty,
            ActionPlan = actionPlan.ActionPlan,
            GeneratedAt = actionPlan.GeneratedAt
        };
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

    private string BuildPrompt(string promptTemplate, Opportunity opportunity, IEnumerable<Interaction> interactions, string leadName)
    {
        var culture = new CultureInfo("pt-BR");
        var product = opportunity.Product;
        var interactionList = interactions.ToList();

        var prompt = new StringBuilder();

        prompt.AppendLine(promptTemplate.Trim());
        prompt.AppendLine();

        prompt.AppendLine("Contexto da oportunidade:");
        prompt.AppendLine($"- Stage atual: {opportunity.Stage}");
        prompt.AppendLine($"- Valor: {opportunity.Amount.ToString("C", culture) ?? "Nao informado"}");
        prompt.AppendLine($"- Fechamento previsto: {opportunity.ExpectedCloseDate?.ToString("dd/MM/yyyy", culture) ?? "Nao informado"}");
        prompt.AppendLine($"- Produto: {product?.Name ?? "Nao informado"}");
        prompt.AppendLine();

        prompt.AppendLine("Dados do lead vinculado:");
        prompt.AppendLine($"- Nome: {leadName}");
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
        prompt.AppendLine("Instruções de saída:");
        prompt.AppendLine("Com base no contexto da oportunidade e nas interações anteriores, gere uma resposta em JSON válido.");
        prompt.AppendLine("O objetivo principal é maximizar as chances de converter o lead em cliente.");
        prompt.AppendLine();
        prompt.AppendLine("O JSON deve conter exatamente os seguintes campos:");
        prompt.AppendLine();
        prompt.AppendLine("{");
        prompt.AppendLine("  \"actionPlan\": \"texto\",");
        prompt.AppendLine("  \"message\": \"texto\"");
        prompt.AppendLine("}");
        prompt.AppendLine();

        prompt.AppendLine("Regras para cada campo:");
        prompt.AppendLine();

        prompt.AppendLine("1. plano_acao:");
        prompt.AppendLine("- Deve conter orientações práticas, objetivas e estratégicas para ajudar na conversão do lead;");
        prompt.AppendLine("- Considerar o estágio atual da oportunidade;");
        prompt.AppendLine("- Levar em conta histórico, interesses, objeções ou sinais demonstrados nas interações;");
        prompt.AppendLine("- Indicar abordagem recomendada, gatilhos de venda, próximos passos e estratégia de follow-up;");
        prompt.AppendLine("- Focar em como aumentar a probabilidade de fechamento ou avanço da negociação;");
        prompt.AppendLine("- Não escrever uma mensagem pronta para o cliente neste campo.");

        prompt.AppendLine();

        prompt.AppendLine("2. mensagem:");
        prompt.AppendLine("- Deve ser uma mensagem comercial personalizada pronta para envio via WhatsApp;");
        prompt.AppendLine("- Ser natural, persuasiva, profissional e objetiva;");
        prompt.AppendLine("- Conter o nome do lead e uma saudação agradável;");
        prompt.AppendLine("- Considerar o estágio atual da oportunidade;");
        prompt.AppendLine("- Levar em conta objeções, interesses ou contexto das interações;");
        prompt.AppendLine("- Incentivar uma próxima ação clara (resposta, reunião, fechamento ou follow-up);");
        prompt.AppendLine("- Conduzir a conversa para avanço ou fechamento da negociação;");
        prompt.AppendLine("- Retornar apenas a mensagem final pronta para envio.");

        prompt.AppendLine();
        prompt.AppendLine("Importante:");
        prompt.AppendLine("- Retorne apenas JSON válido.");
        prompt.AppendLine("- Não use markdown.");
        prompt.AppendLine("- Não explique o raciocínio.");
        prompt.AppendLine("- Não adicione campos extras.");

        return prompt.ToString();
    }

    #endregion
}

internal sealed class OpportunityActionPlanContent
{
    [JsonPropertyName("plano_acao")]
    public string ActionPlan { get; set; }

    [JsonPropertyName("mensagem")]
    public string Message { get; set; }
}