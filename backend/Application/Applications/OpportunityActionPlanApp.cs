using System.Globalization;
using System.Text;
using Application.DTOs;
using Domain.Entities;

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
    public async Task<OpportunityActionPlanDto> GenerateAsync(int opportunityId, int promptId)
    {
        var opportunity = await GetOpportunityAsync(opportunityId);

        var prompt = await _promptApp.GetByIdAsync(promptId);

        var interactions = (await _interactionRepo.GetLastInteractionsByOpportunityIdAsync(opportunityId)).ToList();

        var lead = await _leadApp.GetByIdAsync(opportunity.LeadId);

        var promptRequest = BuildPrompt(prompt.Content, opportunity, interactions, lead.Name);

        var generatedActionPlan = await _aiService.GetResponseFromModel(promptRequest);

        if (string.IsNullOrWhiteSpace(generatedActionPlan))
            throw new InvalidOperationException("A IA não retornou um plano de ação válido.");

        var actionPlan = new OpportunityActionPlan
        {
            OpportunityId = opportunityId,
            ActionPlan = generatedActionPlan.Trim(),
            GeneratedAt = DateTime.UtcNow
        };

        var id = await _actionPlanRepo.AddAsync(actionPlan);
        actionPlan.Id = id;

        return MapToDto(actionPlan);
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
        prompt.AppendLine();
        prompt.AppendLine("Instruções de saída:");
        prompt.AppendLine("Com base no contexto da oportunidade e nas interações anteriores, gere uma mensagem comercial personalizada para enviar ao lead pelo WhatsApp.");
        prompt.AppendLine("O objetivo principal da mensagem é converter o lead em cliente.");
        prompt.AppendLine("Sempre conduza a mensagem para fechamento ou avanço claro da negociação.");
        prompt.AppendLine("A mensagem deve:");
        prompt.AppendLine("- Ser natural, persuasiva e profissional;");
        prompt.AppendLine("- Conter o nome do lead e uma saudação agradável;");
        prompt.AppendLine("- Considerar o estágio atual da oportunidade;");
        prompt.AppendLine("- Levar em conta as objeções ou interesses demonstrados nas interações;");
        prompt.AppendLine("- Incentivar uma próxima ação clara (resposta, reunião, fechamento ou follow-up);");
        prompt.AppendLine("- Ser objetiva e pronta para envio;");
        prompt.AppendLine("- Não explicar o raciocínio, retornar apenas a mensagem final.");

        Console.WriteLine("Prompt gerado para IA:");
        Console.WriteLine(prompt.ToString());

        return prompt.ToString();
    }

    #endregion
}