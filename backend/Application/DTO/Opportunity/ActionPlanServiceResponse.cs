using System.Text.Json.Serialization;

namespace Application.DTO;

public class ActionPlanServiceResponse
{
    public string actionPlan { get; set; }
    public string message { get; set; }
}