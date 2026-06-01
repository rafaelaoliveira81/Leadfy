public interface IAiService
{
    Task<string?> GetResponseFromModel(string prompt);
}
