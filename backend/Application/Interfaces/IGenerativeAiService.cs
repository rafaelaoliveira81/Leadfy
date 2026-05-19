public interface IGenerativeAiService
{
    Task<string> GenerateAsync(string prompt, string modelName, string apiKey);
}