using GymBro_App.DTO;
using System.Text.Json;
using System.Net.Http.Headers;
using System.Text;

namespace GymBro_App.Services;

public class Choice
{
    public object logprobs { get; set; }
    public string finish_reason { get; set; }
    public string native_finish_reason { get; set; }
    public int index { get; set; }
    public Message message { get; set; }
}

public class Message
{
    public string role { get; set; }
    public string content { get; set; }
    public object refusal { get; set; }
    public object reasoning { get; set; }
}

public class AiResponse
{
    public string id { get; set; }
    public string provider { get; set; }
    public string model { get; set; }
    public string @object { get; set; }
    public int created { get; set; }
    public List<Choice> choices { get; set; }
    public Usage usage { get; set; }
}

public class Usage
{
    public int prompt_tokens { get; set; }
    public int completion_tokens { get; set; }
    public int total_tokens { get; set; }
}



public class AiService : IAiService
{
    readonly HttpClient _httpClient;
    readonly ILogger<AiService> _logger;

    private const string MODEL = "deepseek/deepseek-chat-v3-0324:free";

    private async Task<string> BuildPrompt(string query)
    {
        var prompt = new StringBuilder();
        prompt.AppendLine("Give me 5 meal suggestions.");
        prompt.AppendLine("These suggestions should be a list of what the meals are called with no elaboration.");
        prompt.AppendLine("There should be nothing else in your response except the list.");
        prompt.AppendLine("Also try to list meals that contain one or more of the following ingredients:");
        prompt.AppendLine(query);
        return prompt.ToString();
    }

    public AiService(HttpClient httpClient, ILogger<AiService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<string> GetResponse(string query)
    {
        var payload = new
        {
            model = "deepseek/deepseek-chat-v3-0324:free",
            messages = new[]
            {
                new
                {
                    role = "user",
                    content = BuildPrompt(query).Result
                }
            }
        };

        var json = JsonSerializer.Serialize(payload);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("", content);

        JsonSerializerOptions options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        try{
            var result = await JsonSerializer.DeserializeAsync<AiResponse>(await response.Content.ReadAsStreamAsync(), options);
            result = result ?? new AiResponse();
            return result.choices.FirstOrDefault()?.message?.content ?? "No response from AI";
        } catch {
            return "No response from AI";
        }
    }

}