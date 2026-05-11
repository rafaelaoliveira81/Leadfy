namespace Models.Shared;

public class ErrorResponse
{
    public string Message { get; set; }
    public int StatusCode { get; set; }
    public DateTime Timestamp { get; set; }
    public string StackTrace { get; set; }
    public string InnerException { get; set; }

    public ErrorResponse()
    {
        Timestamp = DateTime.UtcNow;
    }
}
