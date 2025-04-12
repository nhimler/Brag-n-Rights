namespace GymBro_App.Models;

using System.Text.Json.Serialization;

public class FitbitActivityResponse
{
    [JsonPropertyName("summary")]
    public Summary Summary { get; set; }
}

public class Summary
{
    [JsonPropertyName("steps")]
    public int Steps { get; set; }
}


public class TokenExpiredException : Exception
{
    public TokenExpiredException(string message) : base(message) { }
}
