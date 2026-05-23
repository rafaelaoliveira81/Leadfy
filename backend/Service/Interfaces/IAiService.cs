public interface IAiService
{
    Task<string?> GetResponseFromModel(string prompt, string modelName, string apiKey);
}
