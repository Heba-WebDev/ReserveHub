using System.Text.Json;

namespace Entities.ErrorModel;

public class ErrorDetails
{
    public int StatusCode { get; set; }
    public string? Message { get; set; }
    // This makes it easy to log or return error details in a JSON format
    public override string ToString() => JsonSerializer.Serialize(this);
}