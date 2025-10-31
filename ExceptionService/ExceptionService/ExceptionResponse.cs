namespace ExceptionService;

public class ExceptionResponse
{
    public int StatusCode { get; init; }
    public string Error { get; init; } = string.Empty;
    public string Message { get; init; } = string.Empty;
    public string TraceId { get; init; } = string.Empty;
}