using System.Text.Json;

namespace APICatalogo.Models;

public class ErrorDetails
{
    public int StatusCode { get; set; }
    public String? Message { get; set; }
    public string? Trace { get; set; }
    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}

